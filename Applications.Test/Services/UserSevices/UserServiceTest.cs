using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.UserViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;

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
}