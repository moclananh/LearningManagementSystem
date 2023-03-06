using Applications.IRepositories;
using Applications.Repositories;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Repositories;

namespace Infrastructures.Tests.Repositories
{
    public class PracticeRepositoryTest: SetupTest
    {
        private readonly PracticeRepository _practiceRepository;
        public PracticeRepositoryTest()
        {
            _practiceRepository = new PracticeRepository(
            _dbContext,
            _currentTimeMock.Object,
            _claimServiceMock.Object
            );
        }
        [Fact]
        public async Task PracticeRepository_GetPracticeByName_ShoulReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Practice>()
                .Without(x => x.PracticeQuestions)
                .Without(x => x.Unit)
                .With(x => x.PracticeName, "Mock")
                .CreateMany(30)
                .ToList();
            await _dbContext.Practices.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(x => x.PracticeName.Contains("Mock"))
                                    .OrderByDescending(x => x.CreationDate)
                                    .Take(10)
                                    .ToList();
            //act
            var resultPaging = await _practiceRepository.GetPracticeByName("Mock");
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
        public async Task PracticetRepository_GetPracticeByUnitId_ShouldReturnCorrectData()
        {
            var i = Guid.NewGuid();
            var practiceMock = _fixture.Build<Practice>()
                                .Without(x => x.Unit)
                                .Without(x => x.PracticeQuestions)
                                .With(x => x.UnitId, i)
                                .CreateMany(30)
                                .ToList();
            await _dbContext.AddRangeAsync(practiceMock);
            await _dbContext.SaveChangesAsync();
            var expected = practiceMock.Where(x => x.UnitId.Equals(i))
                                        .OrderByDescending(x => x.CreationDate)
                                        .Take(10)
                                        .ToList();
            //act
            var resultPaging = await _practiceRepository.GetPracticeByUnitId(i);
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
        public async Task PracticetRepository_GetEnablePractices_ShoulReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Practice>()
                .Without(x => x.PracticeQuestions)
                .Without(x => x.Unit)
                .With(x => x.Status, Domain.Enum.StatusEnum.Status.Enable)
                .CreateMany(30)
                .ToList();
            await _dbContext.Practices.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Take(10)
                                    .ToList();
            //act
            var resultPaging = await _practiceRepository.GetEnablePractices();
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
        public async Task PracticetRepository_GetDisablePractices_ShoulReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Practice>()
                .Without(x => x.PracticeQuestions)
                .Without(x => x.Unit)
                .With(x => x.Status, Domain.Enum.StatusEnum.Status.Disable)
                .CreateMany(30)
                .ToList();
            await _dbContext.Practices.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Take(10)
                                    .ToList();
            //act
            var resultPaging = await _practiceRepository.GetDisablePractices();
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
    }
}
