using Application.Repositories;
using AutoFixture;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Tests;
using FluentAssertions;
using Infrastructure.Repositories;

namespace Infrastructures.Tests.Repositories
{
    public class ClassUserRepositoryTests : SetupTest
    {
        private readonly IClassUserRepository _classUserRepository;
        public ClassUserRepositoryTests()
        {
            _classUserRepository = new ClassUserRepository(_dbContext,
                _currentTimeMock.Object,
                _claimServiceMock.Object);
        }

        [Fact]
        public async Task ClassUserRepository_GetClassUserProgram_ShouldReturnCorrectData()
        {
            //arrange
            var userMockData = _fixture.Build<User>()
                                        .Without(x => x.AbsentRequests)
                                        .Without(x => x.Attendences)
                                        .Without(x => x.UserAuditPlans)
                                        .Without(x => x.ClassUsers)
                                        .Create();
            var classMockData = _fixture.Build<Class>()
                                          .Without(x => x.AbsentRequests)
                                          .Without(x => x.Attendences)
                                          .Without(x => x.AuditPlans)
                                          .Without(x => x.ClassUsers)
                                          .Without(x => x.ClassTrainingPrograms)
                                          .Create();
            var mockData = new ClassUser()
            {
                User = userMockData,
                Class = classMockData
            };
            await _dbContext.AddAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var listMock = await _classUserRepository.GetAllAsync();
            var expected = listMock[0];
            //act
            var result = await _classUserRepository.GetClassUser(classMockData.Id, userMockData.Id);
            //assert
            result.Should().BeEquivalentTo(expected);
        }

    }
}
