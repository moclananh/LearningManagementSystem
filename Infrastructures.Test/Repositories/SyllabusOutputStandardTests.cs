using AutoFixture;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Repositories;

namespace Infrastructures.Tests.Repositories
{
    public class SyllabusOutputStandardTests : SetupTest
    {
        private readonly SyllabusOutputStandardRepository _syllabusOutputStandardRepository;

        public SyllabusOutputStandardTests()
        {
            _syllabusOutputStandardRepository = new SyllabusOutputStandardRepository(
                _dbContext,
                _currentTimeMock.Object,
                _claimServiceMock.Object
                );
        }

        [Fact]
        public async Task SyllabusOutputStandardRepository_GetSyllabusOutputStandard_ShouldReturnCorrectData()
        {
            //arrange
            var syllabusMockData = _fixture.Build<Syllabus>()
                                   .Without(s => s.TrainingProgramSyllabi)
                                   .Without(s => s.SyllabusModules)
                                   .Without(s => s.SyllabusOutputStandards)
                                   .Create();
            var outputStandardMockData = _fixture.Build<OutputStandard>()
                                         .Without(s => s.SyllabusOutputStandards)
                                         .Create();
            var mockData = new SyllabusOutputStandard()
            {
                Syllabus = syllabusMockData,
                OutputStandard = outputStandardMockData
            };
            await _dbContext.AddAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var listMock = await _syllabusOutputStandardRepository.GetAllAsync();
            var expected = listMock[0];

            //act
            var result = await _syllabusOutputStandardRepository.GetSyllabusOutputStandard(syllabusMockData.Id, outputStandardMockData.Id);

            //assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
