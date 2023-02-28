using Applications.ViewModels.AssignmentViewModels;
using Applications.ViewModels.AuditPlanViewModel;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;

namespace Infrastructures.Tests.Mappers.AssignmentMapper
{
    public class AssignmentMapper : SetupTest
    {
        [Fact]
        public void TestViewMapper()
        {
            //arrange
            var assignmentMock = _fixture.Build<Assignment>()
                                .Without(x => x.Unit)
                                .Without(x => x.AssignmentQuestions)
                                .Create();
            //act
            var result = _mapperConfig.Map<AssignmentViewModel>(assignmentMock);
            //assert
            result.AssignmentName.Should().Be(assignmentMock.AssignmentName.ToString());
        }

        [Fact]
        public void TestCreateViewMapper()
        {
            //arrange
            var assignmentMock = _fixture.Build<Assignment>()
                                .Without(x => x.Unit)
                                .Without(x => x.AssignmentQuestions)
                                .Create();
            //act
            var result = _mapperConfig.Map<CreateAssignmentViewModel>(assignmentMock);
            //assert
            result.AssignmentName.Should().Be(assignmentMock.AssignmentName.ToString());
        }

        [Fact]
        public void TestUpdateViewMapper()
        {
            //arrange
            var assignmentMock = _fixture.Build<Assignment>()
                                .Without(x => x.Unit)
                                .Without(x => x.AssignmentQuestions)
                                .Create();
            //act
            var result = _mapperConfig.Map<UpdateAssignmentViewModel>(assignmentMock);
            //assert
            result.AssignmentName.Should().Be(assignmentMock.AssignmentName.ToString());
        }
    }
}
