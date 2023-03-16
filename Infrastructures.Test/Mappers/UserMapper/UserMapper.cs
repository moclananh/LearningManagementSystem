using Applications.ViewModels.UserViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Org.BouncyCastle.Asn1.Pkcs;


namespace Infrastructures.Tests.Mappers.UserMapper;

public class UserMapper : SetupTest
{
    [Fact]
    public void TestViewMapper()
    {
        
        //arrange
        var userMock = _fixture.Build<User>()
                        .Without(x => x.AbsentRequests)
                        .Without(x => x.Attendences)
                        .Without(x => x.ClassUsers)
                        .Without(x => x.UserAuditPlans)
                        .Create();

        var userMock2 = _fixture.Build<User>()
                        .Without(x => x.AbsentRequests)
                        .Without(x => x.Attendences)
                        .Without(x => x.ClassUsers)
                        .Without(x => x.UserAuditPlans)
                        .With(x => x.CreatedBy, userMock.Id)
                        .Create();
        _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(userMock.Id));

        //act
        var result = _mapperConfig.Map<UserViewModel>(userMock2);

        //assert
        result.ID.Should().Be(userMock2.Id.ToString());
    }

}
