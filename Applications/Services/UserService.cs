using System.IdentityModel.Tokens.Jwt;
using Applications.Interfaces;
using Applications.ViewModels.Response;
using Applications.ViewModels.UserViewModels;
using AutoMapper;
using System.Net;
using System.Text;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using Domain.Enum.StatusEnum;
using Domain.Enum.RoleEnum;
using Applications.Commons;
using Applications.Utils;
using Applications.ViewModels.TokenViewModels;
using Domain.Enum.LevelEnum;
using Microsoft.IdentityModel.Tokens;


namespace Applications.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly IClaimService _claimService;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService, IClaimService claimService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _tokenService = tokenService;
        _claimService = claimService;
    }

    // Change Password
    public async Task<Response> ChangePassword(Guid id, ChangePasswordViewModel changePassword)
    {
        if (!_claimService.GetCurrentUserId.Equals(id))
            return new Response(HttpStatusCode.Forbidden,
                $"you are not login with this account, please login first !!!");
        var user = (await _unitOfWork.UserRepository.Find(x => x.Id == id)).FirstOrDefault();
        if (user == null) return new Response(HttpStatusCode.BadRequest, "forbidden exception!");
        if (!StringUtils.Verify(changePassword.OldPassword, user.Password))
            return new Response(HttpStatusCode.BadRequest, "Wrong password");
        if (string.CompareOrdinal(changePassword.NewPassword, changePassword.ConfirmPassword) != 0)
        {
            return new Response(HttpStatusCode.BadRequest, "the new password and confirm password does not match!");
        }

        user.Password = StringUtils.Hash(changePassword.NewPassword);
        _unitOfWork.UserRepository.Update(user);
        var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
        return isSuccess
            ? new Response(HttpStatusCode.OK, "Change Success!")
            : new Response(HttpStatusCode.BadRequest, "Not Success");
    }

    // Reset-Password
    public async Task<Response> ResetPassword(ResetPasswordRequest request)
    {
        var user = await _unitOfWork.UserRepository.GetUserByPasswordResetToken(request.Token);
        if (user == null || user.ResetTokenExpires < DateTime.Now)
            return new Response(HttpStatusCode.BadRequest, "Invalid Token");
        if (string.CompareOrdinal(request.Password, request.ConfirmPassword) != 0)
        {
            return new Response(HttpStatusCode.BadRequest, "the password and confirm password does not match!");
        }

        user.Password = StringUtils.Hash(request.Password);
        user.PasswordResetToken = null;
        user.ResetTokenExpires = null;
        _unitOfWork.UserRepository.Update(user);
        var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
        return isSuccess
            ? new Response(HttpStatusCode.OK, "Change Success!")
            : new Response(HttpStatusCode.BadRequest, "Not Success");
    }

    // Update Image User
    public async Task<Response> UpdateImage(Guid id, string image)
    {
        if (!_claimService.GetCurrentUserId.Equals(id))
            return new Response(HttpStatusCode.Forbidden,
                $"you are not login with this account, please login first !!!");
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        if (user is null) return new Response(HttpStatusCode.BadRequest, "notfound this user");
        user.Image = image;
        _unitOfWork.UserRepository.Update(user);
        var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
        return isSuccess
            ? new Response(HttpStatusCode.OK, "update success", new { user.Id, user.Email, user.Image })
            : new Response(HttpStatusCode.BadRequest, "not success");
    }

    // Verify token login 
    public async Task<Response> VerifyToken(TokenRequest token)
    {
        var user = _unitOfWork.UserRepository.query().FirstOrDefault(u => u.Token == token.Token);
        if (user == null) return new Response(HttpStatusCode.PaymentRequired, "Invalid Token");
        user.Token = null;
        _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.SaveChangeAsync();
        return new Response(HttpStatusCode.OK, "success!!");
    }

    // Refresh token
    public async Task<Response> GetRefreshToken(TokenModel oldTokenModel)
    {
        try
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("123124321213124322")), // đoạn này đang ma giáo 
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };
            // 1
            var tokenInVerification = jwtTokenHandler.ValidateToken(oldTokenModel.AccessToken,tokenValidationParameters, out var validatedToken);
            // 2
            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                if (!result)
                {
                    return new Response(HttpStatusCode.Conflict, "Invalid Token");
                }
            }
            //3
            var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
            if (expireDate > DateTime.UtcNow)
            {
                return new Response(HttpStatusCode.Conflict, "Access token has not yet expired");
            }
            //4
            var storeToken = (await _unitOfWork.RefreshTokenRepository.Find(x => x.Token == oldTokenModel.RefreshToken)).FirstOrDefault();
            if (storeToken == null)
            {
                return new Response(HttpStatusCode.Conflict, "Refresh token does not exist");
            }
            //5
            if (storeToken.IsUsed)
            {
                return new Response(HttpStatusCode.Conflict, "Refresh token has been used");
            }
            //6
            if (storeToken.IsRevoked)
            {
                return new Response(HttpStatusCode.Conflict, "Refresh token has been revoked");
            }

            storeToken.IsRevoked = true;
            storeToken.IsUsed = true;
            _unitOfWork.RefreshTokenRepository.Update(storeToken);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (!isSuccess) return new Response(HttpStatusCode.Conflict, "update faild");
            var user = await _unitOfWork.UserRepository.GetByIdAsync(storeToken.UserId);
            var token = await _tokenService.GetToken(user.Email);
            return new Response(HttpStatusCode.OK, "success!",token);
        }
        catch (Exception e)
        {
            return new Response(HttpStatusCode.BadRequest, "Something went wrong");
        }
    }
    private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
    {
        var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();
        return dateTimeInterval;
    }

    // Search Users by Name
    public async Task<Pagination<UserViewModel>> SearchUserByName(string name, int pageIndex = 0, int pageSize = 10)
    {
        var users = await _unitOfWork.UserRepository.SearchUserByName(name, pageIndex, pageSize);
        return _mapper.Map<Pagination<UserViewModel>>(users);
    }

    // Filter User
    public async Task<Pagination<UserViewModel>> FilterUser(FilterUserRequest filterUserRequest, int pageNumber = 0,
        int pageSize = 10)
    {
        var user = await _unitOfWork.UserRepository.FilterUser(filterUserRequest, pageNumber, pageSize);
        return _mapper.Map<Pagination<UserViewModel>>(user);
    }

    // add user
    public async Task<Response> AddUser(CreateUserViewModel createUserViewModel)
    {
        var entity = await _unitOfWork.UserRepository.GetUserByEmail(createUserViewModel.Email);
        if (entity is not null)
            return new Response(HttpStatusCode.BadRequest,
                $"The account with email {createUserViewModel.Email} already exists in the system");
        var user = new User()
        {
            firstName = createUserViewModel.firstName,
            lastName = createUserViewModel.lastName,
            Email = createUserViewModel.Email,
            DOB = createUserViewModel.DOB,
            Gender = createUserViewModel.Gender,
            Role = createUserViewModel.Role,
            Level = createUserViewModel.Level,
            OverallStatus = createUserViewModel.OverallStatus,
            Password = StringUtils.Hash("12345"),
            Status = Status.Enable,
            Image = "https://i.pinimg.com/originals/0d/43/d7/0d43d7f06ecf44b7259548edb5f35da6.png",
        };
        await _unitOfWork.UserRepository.AddAsync(user);
        var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
        return !isSuccess
            ? new Response(HttpStatusCode.Conflict, "Create user failed!!")
            : new Response(HttpStatusCode.OK, "ok");
    }


    // Get All Users 
    public async Task<Pagination<UserViewModel>> GetAllUsers(int pageIndex = 0, int pageSize = 10)
    {
        var users = await _unitOfWork.UserRepository.ToPagination(pageIndex, pageSize);
        return _mapper.Map<Pagination<UserViewModel>>(users);
    }

    //get user by classid
    public async Task<Response> GetUserByClassId(Guid classId, int pageIndex = 0, int pageSize = 10)
    {
        var users = await _unitOfWork.UserRepository.GetUserByClassId(classId, pageIndex, pageSize);
        return !users.Items.Any()
            ? new Response(HttpStatusCode.NoContent, "not found")
            : new Response(HttpStatusCode.OK, "ok", _mapper.Map<Pagination<UserViewModel>>(users));
    }


    //Get User By ID
    public async Task<UserViewModel> GetUserById(Guid id) =>
        _mapper.Map<UserViewModel>(await _unitOfWork.UserRepository.GetByIdAsync(id));

    // Get User By role
    public async Task<Pagination<UserViewModel>> GetUsersByRole(Role role, int pageIndex = 0, int pageSize = 10)
    {
        var users = await _unitOfWork.UserRepository.GetUsersByRole(role, pageIndex, pageSize);
        return _mapper.Map<Pagination<UserViewModel>>(users);
    }

    // Login
    public async Task<Response> Login(UserLoginViewModel userLoginViewModel)
    {
        var user = await _unitOfWork.UserRepository.GetUserByEmail(userLoginViewModel.Email);

        if (user == null) return new Response(HttpStatusCode.BadRequest, "Invalid Email");
        if (!StringUtils.Verify(userLoginViewModel.Password, user.Password))
            return new Response(HttpStatusCode.BadRequest, "Invalid Password");
        var token = await _tokenService.GetToken(user.Email);
        user.Token = token.AccessToken;
        
        _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.SaveChangeAsync();

        var loginResult = new LoginResult
        {
            ID = user.Id,
            firstName = user.firstName,
            lastName = user.lastName,
            Email = user.Email,
            Image = user.Image,
            Role = user.Role.ToString(),
            AccessToken = token.AccessToken,
            RefreshToken = token.RefreshToken
        };
        return new Response(HttpStatusCode.OK, "authorized", loginResult);
    }

    // Update
    public async Task<Response> UpdateUser(Guid id, UpdateUserViewModel updateUserViewModel)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        if (user is null) return new Response(HttpStatusCode.NoContent, "Not Found this user");

        _mapper.Map(updateUserViewModel, user);
        _unitOfWork.UserRepository.Update(user);

        var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
        return isSuccess
            ? new Response(HttpStatusCode.OK, "Update Success!", _mapper.Map<UserViewModel>(user))
            : new Response(HttpStatusCode.BadRequest, "Not Success");
    }

    // UploadFile users
    public async Task<Response> UploadFileExcel(IFormFile formFile, CancellationToken cancellationToken)
    {
        if (formFile == null || formFile.Length <= 0) return new Response(HttpStatusCode.Conflict, "formfile is empty");

        if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            return new Response(HttpStatusCode.Conflict, "Not Support file extension");

        var list = new List<User>();

        using (var stream = new MemoryStream())
        {
            await formFile.CopyToAsync(stream, cancellationToken);

            using (var package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;
                try
                {
                    for (int row = 2; row <= rowCount; row++)
                    {
                        if (worksheet.Cells[row, 1].Value is null)
                        {
                            break;
                        }

                        var emailEntity =
                            await _unitOfWork.UserRepository.GetUserByEmail(worksheet.Cells[row, 3].Value.ToString()
                                .Trim());
                        if (emailEntity != null)
                            return new Response(HttpStatusCode.BadRequest, "Email repeat with accounts in the system");

                        User user = new User();
                        user.firstName = worksheet.Cells[row, 1].Value.ToString().Trim();
                        user.lastName = worksheet.Cells[row, 2].Value.ToString().Trim();
                        user.Email = worksheet.Cells[row, 3].Value.ToString().Trim();
                        user.DOB = DateTime.Parse(worksheet.Cells[row, 4].Value.ToString());
                        user.Gender =
                            worksheet.Cells[row, 5].Value.ToString().Trim().ToLower().Contains("Female".ToLower())
                                ? false
                                : true;
                        user.Role = (Role)Enum.Parse(typeof(Role), worksheet.Cells[row, 6].Value.ToString());
                        user.OverallStatus =
                            user.Role == Role.SuperAdmin ? OverallStatus.Active : OverallStatus.OffClass;
                        user.Image = "https://i.pinimg.com/originals/0d/43/d7/0d43d7f06ecf44b7259548edb5f35da6.png";
                        user.Level = (Level)Enum.Parse(typeof(Level), worksheet.Cells[row, 7].Value.ToString());
                        user.Password = StringUtils.Hash("12345");
                        user.Status = Status.Enable;
                        list.Add(user);
                    }
                }
                catch (Exception ex)
                {
                    return new Response(HttpStatusCode.BadRequest,
                        "data may be empty row. please check again your excel file!!!");
                }
            }
        }

        await _unitOfWork.UserRepository.AddRangeAsync(list);
        await _unitOfWork.SaveChangeAsync();
        return new Response(HttpStatusCode.OK, "ok");
    }
}