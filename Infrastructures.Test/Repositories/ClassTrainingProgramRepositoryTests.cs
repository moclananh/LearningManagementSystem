using Applications.Repositories;
using AutoFixture;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Repositories;

namespace Infrastructures.Tests.Repositories
{
    public class ClassTrainingProgramRepositoryTests : SetupTest
    {
        private readonly IClassTrainingProgramRepository _classTrainingProgramRepository;
        public ClassTrainingProgramRepositoryTests()
        {
            _classTrainingProgramRepository = new ClassTrainingProgramRepository
            (
                _dbContext,
                _currentTimeMock.Object,
                _claimServiceMock.Object
            );
        }
        [Fact]
        public async Task ClassTrainingProgramRepository_GetClassTrainingProgram_ShouldReturnCorrectData()
        {
            //arrange
            var classMockData = _fixture.Build<Class>()
                                        .Without(x => x.AbsentRequests)
                                        .Without(x => x.Attendences)
                                        .Without(x => x.AuditPlans)
                                        .Without(x => x.ClassUsers)
                                        .Without(x => x.ClassTrainingPrograms)
                                        .Create();
            var trainingProgramMockData = _fixture.Build<TrainingProgram>()
                                                  .Without(x => x.ClassTrainingPrograms)
                                                  .Without(x => x.TrainingProgramSyllabi)
                                                  .Create();
            var mockData = new ClassTrainingProgram()
            {
                Class = classMockData,
                TrainingProgram = trainingProgramMockData
            };
            await _dbContext.AddAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var listMock = await _classTrainingProgramRepository.GetAllAsync();
            var expected = listMock[0];
            //act
            var result = await _classTrainingProgramRepository.GetClassTrainingProgram(classMockData.Id, trainingProgramMockData.Id);
            //assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
