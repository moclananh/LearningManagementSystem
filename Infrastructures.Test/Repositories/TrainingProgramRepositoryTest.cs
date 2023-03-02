using Applications.Repositories;
using AutoFixture;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Repositories;

namespace Infrastructures.Tests.Repositories
{
    public class TrainingProgramRepositoryTest : SetupTest
    {
        private readonly ITrainingProgramRepository _trainingProgramRepository;

        public TrainingProgramRepositoryTest()
        {
            _trainingProgramRepository = new TrainingProgramRepository(
                _dbContext,
                _currentTimeMock.Object,
                _claimServiceMock.Object
                );
        }
        [Fact]
        public async Task TrainingProgramRepository_GetTrainingProgramEnable_ShouldReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<TrainingProgram>()
                           .Without(t => t.TrainingProgramSyllabi)
                           .Without(t => t.ClassTrainingPrograms)
                           .With(t => t.Status, Domain.Enum.StatusEnum.Status.Enable)
                           .CreateMany(30)
                           .ToList();
            await _dbContext.TrainingPrograms.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(t => t.Status == Domain.Enum.StatusEnum.Status.Enable)
                                   .OrderByDescending(t => t.CreationDate)
                                   .Take(10)
                                   .ToList();
            //act
            var resultPaging = await _trainingProgramRepository.GetTrainingProgramEnable();
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
        public async Task TrainingProgramRepository_GetTrainingProgramDisable_ShouldReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<TrainingProgram>()
                           .Without(t => t.TrainingProgramSyllabi)
                           .Without(t => t.ClassTrainingPrograms)
                           .With(t => t.Status, Domain.Enum.StatusEnum.Status.Disable)
                           .CreateMany(30)
                           .ToList();
            await _dbContext.TrainingPrograms.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(t => t.Status == Domain.Enum.StatusEnum.Status.Disable)
                                   .OrderByDescending(t => t.CreationDate)
                                   .Take(10)
                                   .ToList();
            //act
            var resultPaging = await _trainingProgramRepository.GetTrainingProgramDisable();
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
        public async Task TrainingProgramRepository_GetTrainingProgramByClassId_ShouldReturnCorrectData()
        {
            //arrange
            var trainingProgramMockData = _fixture.Build<TrainingProgram>()
                .Without(x => x.TrainingProgramSyllabi)
                .Without(x => x.ClassTrainingPrograms)
                .CreateMany(10)
                .ToList();
            var classMockData = _fixture.Build<Class>()
                .Without(x => x.ClassTrainingPrograms)
                .Without(x => x.AuditPlans)
                .Without(x => x.ClassUsers)
                .Without(x => x.Attendences)
                .Without(x => x.AbsentRequests)
                .Create();

            await _dbContext.TrainingPrograms.AddRangeAsync(trainingProgramMockData);
            await _dbContext.Classes.AddAsync(classMockData);
            await _dbContext.SaveChangesAsync();
            var dataList = new List<ClassTrainingProgram>();
            foreach (var item in trainingProgramMockData)
            {
                var data = new ClassTrainingProgram
                {
                    Class = classMockData,
                    TrainingProgram = item
                };
                dataList.Add(data);
            }
            await _dbContext.ClassTrainingProgram.AddRangeAsync(dataList);
            await _dbContext.SaveChangesAsync();

            var expected = _dbContext.ClassTrainingProgram.Where(x => x.ClassId.Equals(classMockData.Id)).Select(x => x.TrainingProgram).ToList();
            //act
            var resultPaging = await _trainingProgramRepository.GetTrainingProgramByClassId(classMockData.Id);
            var result = resultPaging.Items;
            //assert
            resultPaging.Previous.Should().BeFalse();
            resultPaging.Next.Should().BeFalse();
            resultPaging.Items.Count.Should().Be(10);
            resultPaging.TotalItemsCount.Should().Be(10);
            resultPaging.TotalPagesCount.Should().Be(1);
            resultPaging.PageIndex.Should().Be(0);
            resultPaging.PageSize.Should().Be(10);
            result.Should().BeEquivalentTo(expected, op => op.Excluding(x => x.ClassTrainingPrograms));
        }
    }
}
