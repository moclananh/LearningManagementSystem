using Applications.Repositories;
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
            var i = Guid.NewGuid();
            var quizzQuestionMock = _fixture.Build<QuizzQuestion>()
                                .Without(x => x.Quizz)
                                .With(x => x.QuizzId, i)
                                .CreateMany(30)
                                .ToList();
            await _dbContext.AddRangeAsync(quizzQuestionMock);
            await _dbContext.SaveChangesAsync();
            _dbContext.UpdateRange(quizzQuestionMock);
            await _dbContext.SaveChangesAsync();
            var expected = quizzQuestionMock.Where(x => x.QuizzId.Equals(i))
                                        .OrderByDescending(x => x.CreationDate)
                                        .Take(10)
                                        .ToList();
            //act
            var resultPaging = await _quizzQuestionRepository.GetQuizzQuestionListByQuizzId(i);
        }
        [Fact]
        public async Task GetQuizzQuestionListByCreationDate_ShouldReturnCorrectData()
        {
            //arrange
            var startDate = new DateTime();
            var endDate = new DateTime();
            var id = Guid.NewGuid();
            var quizzQuestionMockdata = _fixture.Build<QuizzQuestion>()
                                                .Without(x => x.Quizz)
                                                .CreateMany(30)
                                                .ToList();
            await _dbContext.AddRangeAsync(quizzQuestionMockdata);
            await _dbContext.SaveChangesAsync();
            var expected = quizzQuestionMockdata.Where(x => x.Id == id && (x.CreationDate >= startDate && x.CreationDate <= endDate)).ToList();
            //act
            var resultPaging = await _quizzQuestionRepository.GetQuizzQuestionListByCreationDate(startDate, endDate, id);
            var result = resultPaging.ToList();
            //assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
