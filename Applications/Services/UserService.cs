using Applications.Interfaces;
using Applications.ViewModels.Response;
using Applications.ViewModels.UserViewModels;
using AutoMapper;
using System.Net;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using Domain.Enum.StatusEnum;
using Domain.Enum.RoleEnum;
using Applications.ViewModels.SyllabusViewModels;
using Applications.Commons;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using MimeKit.Cryptography;

namespace Applications.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    public UserService(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    // Change Password
    public async Task<Response> ChangePassword(Guid id,ChangePasswordViewModel changePassword)
    {
        var user = (await _unitOfWork.UserRepository.Find(x => x.Id == id)).FirstOrDefault();
        if (user == null) return new Response(HttpStatusCode.BadRequest, "forbidden exception!");
        if (user.Password != changePassword.OldPassword) return new Response(HttpStatusCode.BadRequest, "Wrong password");
        if (string.Compare(changePassword.NewPassword, changePassword.ConfirmPassword) != 0)
        {
            return new Response(HttpStatusCode.BadRequest, "the new password and confirm password does not match!");
        }
        user.Password = changePassword.NewPassword;
        _unitOfWork.UserRepository.Update(user);
        bool isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
        if (isSuccess) return new Response(HttpStatusCode.OK, "Change Success!");
        return new Response(HttpStatusCode.BadRequest, "Not Success");
    }
    
        
    // Get All Users 
    public async Task<Pagination<UserViewModel>> GetAllUsers(int pageIndex = 0, int pageSize = 10)
    {
        var users = await _unitOfWork.UserRepository.ToPagination(pageIndex, pageSize);
        return _mapper.Map<Pagination<UserViewModel>>(users);
    }


    public async Task<Pagination<UserViewModel>> GetUserByClassId(Guid ClassId, int pageIndex = 0, int pageSize = 10)
    {
        var Users = await _unitOfWork.UserRepository.GetUserByClassId(ClassId, pageIndex, pageSize);
        return _mapper.Map<Pagination<UserViewModel>>(Users); ;
    }


    //Get User By ID
    public async Task<UserViewModel> GetUserById(Guid id) => _mapper.Map<UserViewModel>(await _unitOfWork.UserRepository.GetByIdAsync(id));

    // Get User By role
    public async Task<Pagination<UserViewModel>> GetUsersByRole(Role role, int pageIndex = 0, int pageSize = 10) 
    {
        var users = await _unitOfWork.UserRepository.GetUsersByRole(role,pageIndex,pageSize);
        return _mapper.Map<Pagination<UserViewModel>>(users);
    }

    // Login
    public async Task<Response> Login(UserLoginViewModel userLoginViewModel)
    {
        var user = await _unitOfWork.UserRepository.GetUserByEmail(userLoginViewModel.Email);

        if (user == null) return new Response(HttpStatusCode.BadRequest, "Invalid Email");
        if (user.Password != userLoginViewModel.Password) return new Response(HttpStatusCode.BadRequest, "Invalid Password");
        var token = await _tokenService.GetToken(user.Email);
        if (string.IsNullOrEmpty(token)) return new Response(HttpStatusCode.Unauthorized, "Invalid password or username");

        LoginResult loginResult = new LoginResult
        {
            Email = user.Email,
            Password = user.Password,
            DOB = user.DOB,
            Gender = user.Gender,
            Role = user.Role,
            Token = token
        };
        return new Response(HttpStatusCode.OK, "authorized", loginResult);
    }

    // Update
    public async Task<Response> UpdateUser(Guid id, UpdateUserViewModel updateUserViewModel)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        if (user is null) return new Response(HttpStatusCode.NotFound, "Not Found this user");

        _mapper.Map(updateUserViewModel, user);
        _unitOfWork.UserRepository.Update(user);

        bool isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
        if (isSuccess) return new Response(HttpStatusCode.OK, "Update Success!", _mapper.Map<UserViewModel>(user));
        return new Response(HttpStatusCode.BadRequest, "Not Success");
    }

    // UploadFile users
    public async Task<Response> UploadFileExcel(IFormFile formFile, CancellationToken cancellationToken)
    {
        if (formFile == null || formFile.Length <= 0) return new Response(HttpStatusCode.Conflict, "formfile is empty");

        if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase)) return new Response(HttpStatusCode.Conflict, "Not Support file extension");

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
                        if (worksheet.Cells[row,1].Value is null)
                        {
                            break;
                        }
                        var emailEntity = _unitOfWork.UserRepository.GetUserByEmail(worksheet.Cells[row, 3].Value.ToString().Trim());
                        if (emailEntity != null) return new Response(HttpStatusCode.BadRequest, "Email repeat with accounts in the system");

                        User user = new User();
                        user.firstName = worksheet.Cells[row, 1].Value.ToString().Trim();
                        user.lastName = worksheet.Cells[row, 2].Value.ToString().Trim();
                        user.Email = worksheet.Cells[row, 3].Value.ToString().Trim();
                        user.DOB = DateTime.Parse(worksheet.Cells[row, 4].Value.ToString());
                        var gender = true;
                        if (worksheet.Cells[row, 5].Value.ToString().Trim().Contains("Female")) gender = false;
                        user.Gender = gender;
                        user.Role = (Role)Enum.Parse(typeof(Role), worksheet.Cells[row, 6].Value.ToString());
                        user.Image = string.Empty;
                        user.Level = string.Empty;
                        user.Password = "12345";
                        user.Status = Status.Enable;
                        list.Add(user);
                    }
                }
                catch(Exception ex)
                {
                    return new Response(HttpStatusCode.BadRequest, "data may be empty row. please check again your excel file!!!");
                }
            }
        }
        await _unitOfWork.UserRepository.AddRangeAsync(list);
        await _unitOfWork.SaveChangeAsync();
        return new Response(HttpStatusCode.OK, "ok");

    }
}
