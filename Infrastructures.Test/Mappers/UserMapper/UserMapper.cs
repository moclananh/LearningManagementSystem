using Applications.ViewModels.UserViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;


namespace Infrastructures.Tests.Mappers.UserMapper;

public class UserMapper : SetupTest
{
    [Fact]
    public void TestMapper()
    {
        //arrange
        var userMock = _fixture.Build<User>()
                        .Without(x => x.AbsentRequests)
                        .Without(x => x.Attendences)
                        .Without(x => x.ClassUsers)
                        .Without(x => x.UserAuditPlans)
                        .Create();
        //act
        var result = _mapperConfig.Map<UserViewModel>(userMock);

        //assert
        result.ID.Should().Be(userMock.Id.ToString());
    }
}
