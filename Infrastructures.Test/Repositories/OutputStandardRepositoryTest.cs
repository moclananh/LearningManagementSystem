using AutoFixture;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Repositories;

namespace Infrastructures.Tests.Repositories
{
    public class OutputStandardRepositoryTest : SetupTest
    {
        private readonly OutputStandardRepository _outputStandardRepository;
        public OutputStandardRepositoryTest()
        {
            _outputStandardRepository = new OutputStandardRepository(
            _dbContext,
            _currentTimeMock.Object,
            _claimServiceMock.Object
            );
        }
        [Fact]
        public async Task OutputStandardRepository_GetOutputStandardBySyllabusId_ShouldReturnCorrectData()
        {
            //arrange
            var outputStandardsMockData = _fixture.Build<OutputStandard>()
                                .Without(x => x.SyllabusOutputStandards)
                                .CreateMany(30);
            var syllabusMockData = _fixture.Build<Syllabus>()
                                   .Without(s => s.TrainingProgramSyllabi)
                                   .Without(s => s.SyllabusModules)
                                   .Without(s => s.SyllabusOutputStandards)
                                   .Create();
            await _dbContext.AddRangeAsync(outputStandardsMockData);
            await _dbContext.AddRangeAsync(syllabusMockData);
            await _dbContext.SaveChangesAsync();
            var dataList = new List<SyllabusOutputStandard>();
            foreach (var item in outputStandardsMockData)
            {
                var data = new SyllabusOutputStandard()
                {
                    Syllabus = syllabusMockData,
                    OutputStandard = item,
                };
                dataList.Add(data);
            }
            _dbContext.SyllabusOutputStandard.AddRange(dataList);
            await _dbContext.SaveChangesAsync();
            var expected = _dbContext.SyllabusOutputStandard.Where(x => x.SyllabusId.Equals(syllabusMockData.Id))
                                        .Select(x => x.OutputStandard)
                                        .OrderByDescending(x => x.CreationDate)
                                        .Take(10)
                                        .ToList();
            //act
            var resultPaging = await _outputStandardRepository.GetOutputStandardBySyllabusIdAsync(syllabusMockData.Id);
            var result = resultPaging.Items.ToList();
            //assert
            resultPaging.Previous.Should().BeFalse();
            resultPaging.Next.Should().BeTrue();
            resultPaging.Items.Count.Should().Be(10);
            resultPaging.TotalItemsCount.Should().Be(30);
            resultPaging.TotalPagesCount.Should().Be(3);
            resultPaging.PageIndex.Should().Be(0);
            resultPaging.PageSize.Should().Be(10);
            result.Should().BeEquivalentTo(expected, op => op.Excluding(x => x.SyllabusOutputStandards));
        }
    }
}
