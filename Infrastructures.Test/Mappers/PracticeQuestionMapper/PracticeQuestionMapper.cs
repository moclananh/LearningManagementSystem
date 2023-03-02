using Applications.ViewModels.PracticeQuestionViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;

namespace Infrastructures.Tests.Mappers.PracticeQuestionMapper
{
    public class PracticeQuestionMapper : SetupTest
    {
        [Fact]
        public void TestViewMapper()
        {
            //arrange
            var practicetQuestionMock = _fixture.Build<PracticeQuestion>()
                                .Without(x => x.Practice)
                                .Create();
            //act
            var result = _mapperConfig.Map<PracticeQuestionViewModel>(practicetQuestionMock);
            //assert
            result.Question.Should().Be(practicetQuestionMock.Question.ToString());
        }   
    }
}
