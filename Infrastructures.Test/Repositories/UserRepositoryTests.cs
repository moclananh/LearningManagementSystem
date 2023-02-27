using Applications.Repositories;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Repositories;


namespace Infrastructures.Tests.Repositories;

public class UserRepositoryTests : SetupTest
{
    private readonly IUserRepository _userRepository;
    public UserRepositoryTests()
    {
        _userRepository = new UserRepository(
            _dbContext,
            _currentTimeMock.Object,
            _claimServiceMock.Object
            );
    }

    [Fact]
    public async Task UserRepository_GetUserByEmail_ShouldReturnCorrectData()
    {
        // arrange
        var mockData = _fixture.Build<User>()
                        .Without(x => x.AbsentRequests)
                        .Without(x => x.Attendences)
                        .Without(x => x.ClassUsers)
                        .Without(x => x.UserAuditPlans)
                        .Create();
        await _dbContext.Users.AddRangeAsync(mockData);
        await _dbContext.SaveChangesAsync();

        //act
        var result = await _userRepository.GetUserByEmail(mockData.Email);
        //assert
        result.Should().BeEquivalentTo(mockData);
        

    

    }
}
