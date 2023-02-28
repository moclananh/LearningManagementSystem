using AutoFixture;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Repositories;

namespace Infrastructures.Tests.Repositories
{
    public class TrainingProgramSyllabiRepositoryTests : SetupTest
    {
        private readonly TrainingProgramSyllabiRepository _trainingProgramSyllabiRepository;

        public TrainingProgramSyllabiRepositoryTests()
        {
            _trainingProgramSyllabiRepository = new TrainingProgramSyllabiRepository(
                _dbContext,
                _currentTimeMock.Object,
                _claimServiceMock.Object
                );
        }

        [Fact]
        public async Task TrainingProgramSyllabiRepository_GetTrainingProgramSyllabus_ShouldReturnCorrectData()
        {
            //arrange
            var syllabusMockData = _fixture.Build<Syllabus>()
                                   .Without(s => s.TrainingProgramSyllabi)
                                   .Without(s => s.SyllabusModules)
                                   .Without(s => s.SyllabusOutputStandards)
                                   .Create();
            var trainingProgramMockData = _fixture.Build<TrainingProgram>()
                                         .Without(s => s.ClassTrainingPrograms)
                                         .Without(s => s.TrainingProgramSyllabi)
                                         .Create();
            var mockData = new TrainingProgramSyllabus()
            {
                Syllabus = syllabusMockData,
                TrainingProgram = trainingProgramMockData
            };
            await _dbContext.AddAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var listMock = await _trainingProgramSyllabiRepository.GetAllAsync();
            var expected = listMock[0];

            //act
            var result = await _trainingProgramSyllabiRepository.GetTrainingProgramSyllabus(syllabusMockData.Id, trainingProgramMockData.Id);

            //assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
