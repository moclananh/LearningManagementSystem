using Applications.Repositories;
using AutoFixture;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Repositories;

namespace Infrastructures.Tests.Repositories
{
    public class UserAuditPlanRepositoryTest :SetupTest
    {
        private readonly IUserAuditPlanRepository _userAuditPlanRepository;
        public UserAuditPlanRepositoryTest() {
            _userAuditPlanRepository = new UserAuditPlanRepository(_dbContext,
                _currentTimeMock.Object,
                _claimServiceMock.Object);
        }

        [Fact]
        public async Task UserAuditPlanRepository_GetUserAuditPlanProgram_ShouldReturnCorrectData()
        {
            //arrange
            var userMockData = _fixture.Build<User>()
                                        .Without(x => x.AbsentRequests)
                                        .Without(x => x.Attendences)
                                        .Without(x => x.UserAuditPlans)
                                        .Without(x => x.ClassUsers)
                                        .Create();
            var auditPLanMockData = _fixture.Build<AuditPlan>()
                                                  .Without(x => x.Module)
                                                  .Without(x => x.AuditResults)
                                                  .Without(x => x.AuditQuestions)
                                                  .Without(x => x.UserAuditPlans)
                                                  .Without(x => x.Class)
                                                  .Create();
            var mockData = new UserAuditPlan()
            {
                User = userMockData,
                AuditPlan = auditPLanMockData
            };
            await _dbContext.AddAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var listMock = await _userAuditPlanRepository.GetAllAsync();
            var expected = listMock[0];
            //act
            var result = await _userAuditPlanRepository.GetUserAuditPlan(auditPLanMockData.Id, userMockData.Id);
            //assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
