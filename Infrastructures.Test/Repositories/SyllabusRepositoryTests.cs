using Applications.Repositories;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Repositories;

namespace Infrastructures.Tests.Repositories
{
    public class SyllabusRepositoryTests : SetupTest
    {
        private readonly ISyllabusRepository _syllabusRepository;
        public SyllabusRepositoryTests()
        {
            _syllabusRepository = new SyllabusRepository(
                _dbContext,
                _currentTimeMock.Object,
                _claimServiceMock.Object
                );
        }

        [Fact]
        public async Task SyllabusRepository_GetEnableSyllabus_ShouldReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Syllabus>()
                           .Without(s => s.TrainingProgramSyllabi)
                           .Without(s => s.SyllabusOutputStandards)
                           .Without(s => s.SyllabusModules)
                           .CreateMany(30)
                           .ToList();
            await _dbContext.Syllabi.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            foreach (var item in mockData)
            {
                item.Status = Domain.Enum.StatusEnum.Status.Enable;
            }
            _dbContext.UpdateRange(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(s => s.Status == Domain.Enum.StatusEnum.Status.Enable)
                                   .OrderByDescending(s => s.CreationDate)
                                   .Take(10)
                                   .ToList();

            //act
            var resultPaging = await _syllabusRepository.GetEnableSyllabus();
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
        public async Task SyllabusRepository_GetDisableSyllabus_ShouldReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Syllabus>()
                           .Without(s => s.TrainingProgramSyllabi)
                           .Without(s => s.SyllabusOutputStandards)
                           .Without(s => s.SyllabusModules)
                           .CreateMany(30)
                           .ToList();
            await _dbContext.Syllabi.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            foreach (var item in mockData)
            {
                item.Status = Domain.Enum.StatusEnum.Status.Disable;
            }
            _dbContext.UpdateRange(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(s => s.Status == Domain.Enum.StatusEnum.Status.Disable)
                                   .OrderByDescending(s => s.CreationDate)
                                   .Take(10)
                                   .ToList();

            //act
            var resultPaging = await _syllabusRepository.GetDisableSyllabus();
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
        public async Task SyllabusRepository_GetSyllabusByName_ShouldReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Syllabus>()
                           .Without(s => s.TrainingProgramSyllabi)
                           .Without(s => s.SyllabusOutputStandards)
                           .Without(s => s.SyllabusModules)
                           .CreateMany(30)
                           .ToList();
            await _dbContext.Syllabi.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            int i = 0;
            foreach (var item in mockData)
            {
                item.SyllabusName = $"Mock{i}";
                i++;
            }
            _dbContext.UpdateRange(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(s => s.SyllabusName!.Contains("Mock"))
                                   .OrderByDescending(s => s.CreationDate)
                                   .Take(10)
                                   .ToList();

            //act
            var resultPaging = await _syllabusRepository.GetSyllabusByName("Mock");
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
