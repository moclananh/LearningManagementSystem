﻿using Applications.Interfaces;
using Applications.ViewModels.Response;
using Applications.ViewModels.UserViewModels;
using AutoMapper;
using System.Net;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using Domain.Enum.StatusEnum;
using Domain.Enum.RoleEnum;
using Applications.Interfaces;

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

    // Get All Users 
    public async Task<List<UserViewModel>> GetAllUsers() => _mapper.Map<List<UserViewModel>>(await _unitOfWork.UserRepository.GetAllAsync());


    //Get User By ID
    public async Task<UserViewModel> GetUserById(Guid id) => _mapper.Map<UserViewModel>(await _unitOfWork.UserRepository.GetByIdAsync(id));

    // Get User By role
    public async Task<List<UserViewModel>> GetUsersByRole(Role role) => _mapper.Map<List<UserViewModel>>(await _unitOfWork.UserRepository.Find(x => x.Role == role));

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

    // UploadFile
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

                for (int row = 2; row <= rowCount; row++)
                {
                    list.Add(new User
                    {
                        firstName = worksheet.Cells[row, 1].Value.ToString().Trim(),
                        lastName = worksheet.Cells[row, 2].Value.ToString().Trim(),
                        Email = worksheet.Cells[row, 3].Value.ToString().Trim(),
                        DOB = DateTime.Parse(worksheet.Cells[row, 4].Value.ToString()),
                        Gender = bool.Parse(worksheet.Cells[row, 5].Value.ToString().Trim()),
                        Role = (Role)Enum.Parse(typeof(Role), worksheet.Cells[row, 6].Value.ToString()),
                        Image = string.Empty,
                        Level = string.Empty,
                        Password = "12345",
                        Status = Status.Enable,
                    });
                }
            }
        }
        await _unitOfWork.UserRepository.AddRangeAsync(list);
        await _unitOfWork.SaveChangeAsync();
        return new Response(HttpStatusCode.OK, "ok");

    }
}