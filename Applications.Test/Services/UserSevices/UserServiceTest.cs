using Applications.Commons;
using Applications.Interfaces;
using Applications.Services;
using Applications.Utils;
using Applications.ViewModels.Response;
using Applications.ViewModels.TokenViewModels;
using Applications.ViewModels.UserViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Moq;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Applications.Tests.Services.UserSevices;

public class UserServiceTest : SetupTest
{
    private readonly IUserService _userService;

    public UserServiceTest()
    {
        _userService = new UserService(_unitOfWorkMock.Object, _mapperConfig, _tokenServiceMock.Object,_claimServiceMock.Object);
    }
    
    [Fact]
    public async Task GetUserById_ShouldReturnCorrectData()
    {
        //arrange
        var mock = _fixture.Build<User>()
            .Without(x => x.AbsentRequests)
            .Without(x => x.Attendences)
            .Without(x => x.UserAuditPlans)
            .Without(x => x.ClassUsers)
            .Create();
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mock);
        var expected = _mapperConfig.Map<UserViewModel>(mock);
        //act
        var result = _userService.GetUserById(mock.Id);
        //assert
        result.Result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task SearchUserByName_shouldReturnCorectData()
    {
        //arrage
        var userMockData = new Pagination<User>
        {
            Items = _fixture.Build<User>()
                .Without(x => x.AbsentRequests)
                .Without(x => x.Attendences)
                .Without(x => x.UserAuditPlans)
                .Without(x => x.ClassUsers)
                .With(x => x.firstName, "mock")
                .With(x => x.lastName, "mock")
                .CreateMany(30)
                .ToList(),

            PageIndex = 0,
            PageSize = 10,
            TotalItemsCount = 30  
        };
        _unitOfWorkMock.Setup(u => u.UserRepository.SearchUserByName("mock",0,10)).ReturnsAsync(userMockData);
        var expected = _mapperConfig.Map<Pagination<UserViewModel>>(userMockData);

        //act
        var result = await _userService.SearchUserByName("mock", 0, 10);

        //assert
        _unitOfWorkMock.Verify(u => u.UserRepository.SearchUserByName("mock", 0, 10));
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetAllUsers_shouldBeReturnData()
    {
        //arrage
        var userMockData = new Pagination<User>
        {
            PageIndex = 0,
            PageSize = 10,
            TotalItemsCount = 30,
            Items = _fixture.Build<User>()
                .Without(x => x.AbsentRequests)
                .Without(x => x.Attendences)
                .Without(x => x.UserAuditPlans)
                .Without(x => x.ClassUsers)
                .CreateMany(30)
                .ToList(),
        };
        _unitOfWorkMock.Setup(u => u.UserRepository.ToPagination(0, 10)).ReturnsAsync(userMockData);
        var expected = _mapperConfig.Map<Pagination<UserViewModel>>(userMockData);
        //act
        var result = await _userService.GetAllUsers(0,10);
        //assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetUsersByRole_ShouldBeReturnCorrectData()
    {
        //arrage
        var userMockData = new Pagination<User>
        {
            PageIndex = 0,
            PageSize = 10,
            TotalItemsCount = 30,
            Items = _fixture.Build<User>()
                .Without(x => x.AbsentRequests)
                .Without(x => x.Attendences)
                .Without(x => x.UserAuditPlans)
                .Without(x => x.ClassUsers)
                .With(x => x.Role, Domain.Enum.RoleEnum.Role.SuperAdmin)
                .CreateMany(30)
                .ToList(),
        };
        _unitOfWorkMock.Setup(u => u.UserRepository.GetUsersByRole(0,0,10)).ReturnsAsync(userMockData);
        var expected = _mapperConfig.Map<Pagination<UserViewModel>>(userMockData);

        //act
        var result = await _userService.GetUsersByRole(0, 0, 10);

        //assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task AddUser_ShouldReturnCorrectData_WhenSuccessSaved()
    {
        //arrage
        var userViewModelMock = _fixture.Build<CreateUserViewModel>()
            .Create();
        //_unitOfWorkMock.Setup(u => u.UserRepository.GetUserByEmail(userViewModelMock.Email)).Returns(null);
        _unitOfWorkMock.Setup(u => u.UserRepository.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangeAsync()).ReturnsAsync(1);

        //act
        var result = await _userService.AddUser(userViewModelMock);

        //assert
        _unitOfWorkMock.Verify(s => s.UserRepository.AddAsync(It.IsAny<User>()), Times.Once());
        _unitOfWorkMock.Verify(s => s.SaveChangeAsync(), Times.Once());
    }

    [Fact]
    public async Task AddUser_ShouldReturnCorrectData_WhenSuccessFaild()
    {
        //arrage
        var userViewModelMock = _fixture.Build<CreateUserViewModel>()
            .Create();
        //_unitOfWorkMock.Setup(u => u.UserRepository.GetUserByEmail(userViewModelMock.Email)).Returns(null);
        _unitOfWorkMock.Setup(u => u.UserRepository.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangeAsync()).ReturnsAsync(0);

        //act
        var result = await _userService.AddUser(userViewModelMock);

        //assert
        _unitOfWorkMock.Verify(s => s.UserRepository.AddAsync(It.IsAny<User>()), Times.Once());
        _unitOfWorkMock.Verify(s => s.SaveChangeAsync(), Times.Once());
    }

    [Fact]
    public async Task AddUser_ShouldReturnBabRequest()
    {
        //arrage
        var userViewModelMock = _fixture.Build<CreateUserViewModel>()
            .Create();
        var user = _fixture.Build<User>()
                .Without(x => x.AbsentRequests)
                .Without(x => x.Attendences)
                .Without(x => x.UserAuditPlans)
                .Without(x => x.ClassUsers)
                .Create();
        
        _unitOfWorkMock.Setup(u => u.UserRepository.GetUserByEmail(userViewModelMock.Email));

        //act
        var result = await _userService.AddUser(userViewModelMock);

        //assert
        result.Should().NotBeNull();

    }

    [Fact]
    public async Task FilterUser_ShouldBeReturnCorrectData()
    {
        //arrage
        var filterMock = _fixture.Build<FilterUserRequest>()
            .Create();
        var userMockData = new Pagination<User>
        {
            PageIndex = 0,
            PageSize = 10,
            TotalItemsCount = 30,
            Items = _fixture.Build<User>()
                .Without(x => x.AbsentRequests)
                .Without(x => x.Attendences)
                .Without(x => x.UserAuditPlans)
                .Without(x => x.ClassUsers)
                .With(x => x.Role, Domain.Enum.RoleEnum.Role.SuperAdmin)
                .CreateMany(30)
                .ToList(),
        };
        _unitOfWorkMock.Setup(u => u.UserRepository.FilterUser(filterMock,0,10)).ReturnsAsync(userMockData);
        var expected = _mapperConfig.Map<Pagination<UserViewModel>>(userMockData);
        
        //act
        var result = await _userService.FilterUser(filterMock,0,10);

        //assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Login_ShouldBeReturnCorrectData()
    {
        //arrage
        var userMock = new UserLoginViewModel
        {
            Email = "mock",
            Password = "12345"
        };
        var expectedResponse = new Response(HttpStatusCode.OK, "authorized",It.IsAny<LoginResult>());

        var user = _fixture.Build<User>()
                .Without(x => x.AbsentRequests)
                .Without(x => x.Attendences)
                .Without(x => x.UserAuditPlans)
                .Without(x => x.ClassUsers)
                .With(x => x.Email, userMock.Email)
                .With(x => x.Password,StringUtils.Hash(userMock.Password))
                .Create();
        var tokenMock = _fixture.Build<TokenModel>().Create();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.UserRepository.GetUserByEmail(userMock.Email)).ReturnsAsync(user);
        _tokenServiceMock.Setup(service => service.GetToken(user.Email)).ReturnsAsync(tokenMock);

        //act
        var result = await _userService.Login(userMock);

        //assert
        Assert.Equal(expectedResponse.Status, result.Status);
        Assert.IsType<LoginResult>(result.Result);
    }

    [Fact]
    public async Task Login_WithInvalidEmail_ReturnsBadRequestResponse()
    {
        //arrage
        var userMock = new UserLoginViewModel
        {
            Email = "mock",
            Password = "12345"
        };
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.UserRepository.GetUserByEmail(userMock.Email)).ReturnsAsync((User)null);
        var expectedResponse = new Response(HttpStatusCode.BadRequest, "Invalid Email");

        //act
        var result = await _userService.Login(userMock);

        //assert
        Assert.Equal(expectedResponse.Status, result.Status);
        Assert.Equal(expectedResponse.Message, result.Message);
        Assert.Null(result.Result);
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ReturnsBadRequestResponse()
    {
        //arrage
        var userMock = new UserLoginViewModel
        {
            Email = "mock",
            Password = "12345"
        };
        var user = _fixture.Build<User>()
                .Without(x => x.AbsentRequests)
                .Without(x => x.Attendences)
                .Without(x => x.UserAuditPlans)
                .Without(x => x.ClassUsers)
                .With(x => x.Email, userMock.Email)
                .With(x => x.Password, StringUtils.Hash("123"))
                .Create();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.UserRepository.GetUserByEmail(userMock.Email)).ReturnsAsync(user);
        var expectedResponse = new Response(HttpStatusCode.BadRequest, "Invalid Password");

        //act
        var result = await _userService.Login(userMock);

        //assert
        Assert.Equal(expectedResponse.Status, result.Status);
        Assert.Equal(expectedResponse.Message, result.Message);
        Assert.Null(result.Result);
    }

   
}