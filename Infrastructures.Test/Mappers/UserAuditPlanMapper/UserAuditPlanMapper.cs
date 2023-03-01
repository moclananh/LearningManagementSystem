using Applications.ViewModels.UserAuditPlanViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Tests;

namespace Infrastructures.Tests.Mappers.UserAuditPlanMapper
{
    public class UserAuditPlanMapper : SetupTest
    {
        [Fact]
        public void TestViewMapper()
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
            //act
            var result = _mapperConfig.Map<UserAuditPlanViewModel>(mockData);
            //assert
            result.Equals(mockData);
        }

        [Fact]
        public void TestCreateViewMapper()
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
            //act
            var result = _mapperConfig.Map<CreateUserAuditPlanViewModel>(mockData);
            //assert
            result.Equals(mockData);
        }
    }
}
