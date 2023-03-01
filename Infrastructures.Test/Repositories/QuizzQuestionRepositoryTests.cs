using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Repositories;

namespace Infrastructures.Tests.Repositories
{
    public class QuizzQuestionRepositoryTests : SetupTest
    {
        private QuizzQuestionRepository _quizzQuestionRepository;
        public QuizzQuestionRepositoryTests()
        {
            _quizzQuestionRepository = new QuizzQuestionRepository(
                _dbContext,
                _currentTimeMock.Object,
                _claimServiceMock.Object);
        }
        [Fact]
        public async Task QuizzQuestionRepository_GetQuestionByQuizzId_ShoulReturnCorrectData()
        {
            //arrange
            var quizzMockData = _fixture.Build<Quizz>()
                                        .Without(x => x.Unit)
                                        .Without(x => x.QuizzQuestions)
                                        .Create();
            var quizzQuestionMockData = _fixture.Build<QuizzQuestion>()
                                                .Without(x => x.QuizzId)
                                                .Without(x => x.Quizz)
                                                .CreateMany(10)
                                                .ToList();
            var questionList = new List<QuizzQuestion>();
            foreach (var item in quizzQuestionMockData)
            {
                var question = new QuizzQuestion
                {
                    QuizzId = quizzMockData.Id,
                    Question = item.Question,
                    Answer = item.Answer,
                    Note = item.Note
                };
                questionList.Add(question);
            }
            await _dbContext.AddAsync(quizzMockData);
            await _dbContext.AddRangeAsync(quizzQuestionMockData);
            await _dbContext.AddRangeAsync(questionList);
            await _dbContext.SaveChangesAsync();
            List<QuizzQuestion> expected = _dbContext.QuizzQuestions.Where(x => x.QuizzId == quizzMockData.Id).ToList();
            ////act
            var resultPaging = await _quizzQuestionRepository.GetQuestionByQuizzId(quizzMockData.Id);
            var result = resultPaging.Items;
            //assert
            resultPaging.Previous.Should().BeFalse();
            resultPaging.Next.Should().BeFalse();
            resultPaging.Items.Count.Should().Be(10);
            resultPaging.TotalItemsCount.Should().Be(10);
            resultPaging.TotalPagesCount.Should().Be(1);
            resultPaging.PageIndex.Should().Be(0);
            resultPaging.PageSize.Should().Be(10);
            result.Should().BeEquivalentTo(expected, op => op.Excluding(x => x.Quizz));
        }
    }
}
