using Applications.Repositories;
using AutoFixture;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Enum.RoleEnum;
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

    [Fact]
    public async Task UserRepository_GetUserByRole_ShouldReturnCorrectData()
    {
        // arrage
        var mockData = _fixture.Build<User>()
            .Without(x => x.AbsentRequests)
            .Without(x => x.Attendences)
            .Without(x => x.ClassUsers)
            .Without(x => x.UserAuditPlans)
            .CreateMany(30)
            .ToList();
        await _dbContext.Users.AddRangeAsync(mockData);
        await _dbContext.SaveChangesAsync();
        foreach(var item in mockData)
        {
            item.Role = Role.Admin;
        }
        _dbContext.UpdateRange(mockData);
        await _dbContext.SaveChangesAsync();
        var expected = mockData.Where(x => x.Role == Role.Admin)
            .OrderByDescending(x => x.CreationDate)
            .Take(10)
            .ToList();

        //act
        var resultPaging = await _userRepository.GetUsersByRole(Role.Admin);
        var result = resultPaging.Items;

        //assert
        resultPaging.Previous.Should().BeFalse();
        resultPaging.Next.Should().BeTrue();
        resultPaging.Items.Count.Should().Be(10);
        resultPaging.TotalItemsCount.Should().Be(30);
        resultPaging.TotalPagesCount.Should().Be(3);
        resultPaging.PageIndex.Should().Be(0);
        resultPaging.PageSize.Should().Be(10);
        result.Should().BeEquivalentTo(expected);
    }
    
}
