using Applications.ViewModels.AssignmentQuestionViewModels;
using Applications.ViewModels.AuditResultViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Microsoft.SqlServer.Server;

namespace Infrastructures.Tests.Mappers.AuditResultMapper
{
    public class AuditResultMapper : SetupTest
    {
        [Fact]
        public void TestViewMapper()
        {
            //arrange
            var auditResultMock = _fixture.Build<AuditResult>()
                                .Without(x => x.AuditPlan)
                                .Create();
            //act
            var result = _mapperConfig.Map<AuditResultViewModel>(auditResultMock);
            //assert
            result.Score.Should().Be(auditResultMock.Score.ToString());
        }

        [Fact]
        public void TestUpdateMapper()
        {
            //arrange
            var auditResultMock = _fixture.Build<AuditResult>()
                                .Without(x => x.AuditPlan)
                                .Create();
            //act
            var result = _mapperConfig.Map<UpdateAuditResultViewModel>(auditResultMock);
            //assert
            result.Score.Should().Be(auditResultMock.Score.ToString());
        }

    }
}
