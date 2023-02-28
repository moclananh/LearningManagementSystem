using Applications.ViewModels.AssignmentQuestionViewModels;
using Applications.ViewModels.AssignmentViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;

namespace Infrastructures.Tests.Mappers.AssignmentQuestionMapper
{
    public class AssignmentQuestionMapper : SetupTest
    {
        [Fact]
        public void TestViewMapper()
        {
            //arrange
            var assignmentQuestionMock = _fixture.Build<AssignmentQuestion>()
                                .Without(x => x.Assignment)
                                .Create();
            //act
            var result = _mapperConfig.Map<AssignmentQuestionViewModel>(assignmentQuestionMock);
            //assert
            result.Question.Should().Be(assignmentQuestionMock.Question.ToString());
        }
    }
}
