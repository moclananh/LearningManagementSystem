using Applications.Repositories;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Repositories;

namespace Infrastructures.Tests.Repositories
{
    public class QuizzRepositoryTests : SetupTest
    {
        private readonly QuizzRepository _quizzRepository;
        public QuizzRepositoryTests()
        {
            _quizzRepository = new QuizzRepository(
            _dbContext,
            _currentTimeMock.Object,
            _claimServiceMock.Object
            );
        }

        [Fact]
        public async Task QuizzRepository_GetQuizzByName_ShoulReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Quizz>()
                .Without(x => x.QuizzQuestions)
                .Without(x => x.Unit)
                .With(x => x.QuizzName, "Mock")
                .CreateMany(30)
                .ToList();
            await _dbContext.Quizzs.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(x => x.QuizzName.Contains("Mock"))
                                    .OrderByDescending(x => x.CreationDate)
                                    .Take(10)
                                    .ToList();
            //act
            var resultPaging = await _quizzRepository.GetQuizzByName("Mock");
            var result = resultPaging.Items;
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
        public async Task QuizzRepository_GetEnableQuizzes_ShoulReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Quizz>()
                .Without(x => x.QuizzQuestions)
                .Without(x => x.Unit)
                .With(x => x.Status, Domain.Enum.StatusEnum.Status.Enable)
                .CreateMany(30)
                .ToList();
            await _dbContext.Quizzs.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Take(10)
                                    .ToList();
            //act
            var resultPaging = await _quizzRepository.GetEnableQuizzes();
            var result = resultPaging.Items;
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
        public async Task QuizzRepository_GetDiableQuizzes_ShoulReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Quizz>()
                .Without(x => x.QuizzQuestions)
                .Without(x => x.Unit)
                .With(x => x.Status, Domain.Enum.StatusEnum.Status.Disable)
                .CreateMany(30)
                .ToList();
            await _dbContext.Quizzs.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Take(10)
                                    .ToList();
            //act
            var resultPaging = await _quizzRepository.GetDisableQuizzes();
            var result = resultPaging.Items;
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
        public async Task QuizzRepository_GetQuizzByUnitId_ShouldReturnCorrectData()
        {
            //arrange
            var i = Guid.NewGuid();
            var Mock = _fixture.Build<Quizz>()
                                .Without(x => x.QuizzQuestions)
                                .Without(x => x.Unit)
                                .With(x => x.UnitId, i)
                                .CreateMany(30)
                                .ToList();
            await _dbContext.AddRangeAsync(Mock);
            await _dbContext.SaveChangesAsync();
            var expected = Mock.Where(x => x.UnitId.Equals(i))
                                        .OrderByDescending(x => x.CreationDate)
                                        .Take(10)
                                        .ToList();
            //act
            var resultPaging = await _quizzRepository.GetQuizzByUnitIdAsync(i);
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
    }
}

