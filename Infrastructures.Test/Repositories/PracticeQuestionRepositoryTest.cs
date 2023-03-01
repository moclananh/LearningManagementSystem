using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Repositories;


namespace Infrastructures.Tests.Repositories
{
    public class PracticeQuestionRepositoryTest : SetupTest
    {
        private readonly PracticeQuestionRepository _practiceQuestionRepository;
        public PracticeQuestionRepositoryTest()
        {
            _practiceQuestionRepository = new PracticeQuestionRepository(_dbContext,
                _currentTimeMock.Object,
                _claimServiceMock.Object);
        }

        [Fact]
        public async Task PracticeQuestionRepository_GetPracticeQuestionById_ShouldReturnCorrectData()
        {
            //arrange
            var i = Guid.NewGuid();
            var practiceQuestionMock = _fixture.Build<PracticeQuestion>()
                                .Without(x => x.Practice)
                                .With(x => x.PracticeId, i)
                                .CreateMany(30)
                                .ToList();
            await _dbContext.AddRangeAsync(practiceQuestionMock);
            await _dbContext.SaveChangesAsync();
            var expected = practiceQuestionMock.Where(x => x.PracticeId.Equals(i))
                                        .OrderByDescending(x => x.CreationDate)
                                        .Take(10)
                                        .ToList();
            //act
            var resultPaging = await _practiceQuestionRepository.GetAllPracticeQuestionById(i);
            var result = resultPaging.Items.ToList();
            //assert
            resultPaging.Previous.Should().BeFalse();
            resultPaging.Next.Should().BeTrue();
            resultPaging.Items.Count.Should().Be(10);
            resultPaging.TotalItemsCount.Should().Be(30);
            resultPaging.TotalPagesCount.Should().Be(3);
            resultPaging.PageIndex.Should().Be(0);
            resultPaging.PageSize.Should().Be(10);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task PracticeQuestionRepository_GetPracticeQuestionListById_ShouldReturnCorrectData()
        {
            //arrange
            var i = Guid.NewGuid();
            var practiceQuestionMock = _fixture.Build<PracticeQuestion>()
                                .Without(x => x.Practice)
                                .With(x => x.PracticeId, i)
                                .CreateMany(30)
                                .ToList();
            await _dbContext.AddRangeAsync(practiceQuestionMock);
            await _dbContext.SaveChangesAsync();
            var expected = practiceQuestionMock.Where(x => x.PracticeId.Equals(i))
                                        .OrderByDescending(x => x.CreationDate)
                                        .Take(10)
                                        .ToList();
            //act
            var resultPaging = await _practiceQuestionRepository.GetAllPracticeQuestionByPracticeId(i);
        }
    }
}
