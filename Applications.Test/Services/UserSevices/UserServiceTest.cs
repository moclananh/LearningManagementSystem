using Applications.Commons;
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

}