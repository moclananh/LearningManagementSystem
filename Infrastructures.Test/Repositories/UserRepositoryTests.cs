using Applications.Repositories;
using AutoFixture;
using DocumentFormat.OpenXml.Drawing;
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
            item.Role = Role.SuperAdmin;
        }
        _dbContext.UpdateRange(mockData);
        await _dbContext.SaveChangesAsync();
        var expected = mockData.Where(x => x.Role == Role.SuperAdmin)
            .OrderByDescending(x => x.CreationDate)
            .Take(10)
            .ToList();

        //act
        var resultPaging = await _userRepository.GetUsersByRole(Role.SuperAdmin);
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
    [Fact]
    public async Task UserRepository_GetUserByClassId_ShouldReturnCorrectData()
    {
        //arrage
        var mockDataUser = _fixture.Build<User>()
            .Without(x => x.AbsentRequests)
            .Without(x => x.Attendences)
            .Without(x => x.UserAuditPlans)
            .Without(x => x.ClassUsers)
            .CreateMany(10)
            .ToList();
        var mockDataClass = _fixture.Build<Class>()
            .Without(x => x.AuditPlans)
            .Without(x => x.AbsentRequests)
            .Without(x => x.Attendences)
            .Without(x => x.ClassTrainingPrograms)
            .Without(x => x.ClassUsers)
            .Create();
        await _dbContext.Users.AddRangeAsync(mockDataUser);
        await _dbContext.Classes.AddAsync(mockDataClass);
        await _dbContext.SaveChangesAsync();
        var list = new List<ClassUser>();
        foreach(var user in mockDataUser)
        {
            var data = new ClassUser
            {
                Class = mockDataClass,
                User = user
            };
            list.Add(data);
        }
        await _dbContext.ClassUser.AddRangeAsync(list);
        await _dbContext.SaveChangesAsync();
        
        var expected = _dbContext.ClassUser.Where(x => x.ClassId.Equals(mockDataClass.Id)).Select(x => x.User).ToList();

        //act
        var resultPaging = await _userRepository.GetUserByClassId(mockDataClass.Id);
        var result = resultPaging.Items;
        //assert
        resultPaging.Previous.Should().BeFalse();
        resultPaging.Next.Should().BeFalse();
        resultPaging.Items.Count.Should().Be(10);
        resultPaging.TotalItemsCount.Should().Be(10);
        resultPaging.TotalPagesCount.Should().Be(1);
        resultPaging.PageIndex.Should().Be(0);
        resultPaging.PageSize.Should().Be(10);
        result.Should().BeEquivalentTo(expected, op => op.Excluding(x => x.ClassUsers));
    }

    [Fact]
    public async Task UserRepository_SearchUserByName_ShouldReturnCorrectData()
    {
        // arrage
        var mockData = _fixture.Build<User>()
            .Without(x => x.AbsentRequests)
            .Without(x => x.Attendences)
            .Without(x => x.ClassUsers)
            .Without(x => x.UserAuditPlans)
            .With(x => x.firstName,"Mock")
            .CreateMany(30)
            .ToList();
        await _dbContext.Users.AddRangeAsync(mockData);
        await _dbContext.SaveChangesAsync();
        var expected = mockData.Where(x => x.firstName.Contains("Mock") || x.lastName.Contains("Mock"))
            .OrderByDescending(x => x.CreationDate)
            .Take(10)
            .ToList();
        //act
        var resultPaging = await _userRepository.SearchUserByName("Mock");
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
