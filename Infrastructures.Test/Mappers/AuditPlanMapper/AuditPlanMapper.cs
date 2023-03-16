using Applications.ViewModels.AuditPlanViewModel;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;

namespace Infrastructures.Tests.Mappers.AuditPlanMapper
{
    public class AuditPlanMapper : SetupTest
    {
        [Fact]
        public void TestViewMapper()
        {
            //arrange
            var auditplanMock = _fixture.Build<AuditPlan>()
                                .Without(x => x.AuditResults)
                                .Without(x => x.AuditQuestions)
                                .Without(x => x.UserAuditPlans)
                                .Without(x => x.Module)
                                .Without(x => x.Class)
                                .Create();
            //act
            var result = _mapperConfig.Map<AuditPlanViewModel>(auditplanMock);
            //assert
            result.AuditPlanName.Should().Be(auditplanMock.AuditPlanName.ToString());
        }

        [Fact]
        public void TestUpdateMapper()
        {
            //arrange
            var auditplanMock = _fixture.Build<AuditPlan>()
                                .Without(x => x.AuditResults)
                                .Without(x => x.AuditQuestions)
                                .Without(x => x.UserAuditPlans)
                                .Without(x => x.Module)
                                .Without(x => x.Class)
                                .Create();
            //act
            var result = _mapperConfig.Map<UpdateAuditPlanViewModel>(auditplanMock);
            //assert
            result.AuditPlanName.Should().Be(auditplanMock.AuditPlanName.ToString());
        }
    }
}
