using Applications.Commons;
using Applications.Interfaces;
using Applications.Services;
using AutoFixture;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Tests;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Applications.Tests.Services.SyllabusTrainingProgramServices
{
    public class SyllabusTrainingProgramServiceTest : SetupTest
    {
        private readonly ISyllabusTrainingProgramService _syllabusTrainingProgramService;
        public SyllabusTrainingProgramServiceTest()
        {
            _syllabusTrainingProgramService = new SyllabusTrainingProgramService(_unitOfWorkMock.Object, _mapperConfig);
        }

        [Fact]
        public async Task GetAllSyllabusTrainingProgram_ShouldReturnCorrectData()
        {
            //arrange
            var syllabusMockData = _fixture.Build<Syllabus>()
                                        .Without(s => s.SyllabusModules)
                                        .Without(s => s.SyllabusOutputStandards)
                                        .Without(s => s.TrainingProgramSyllabi)
                                        .CreateMany(30)
                                        .ToList();
            var trainingProgramMockData = _fixture.Build<TrainingProgram>()
                                       .Without(x => x.TrainingProgramSyllabi)
                                       .Without(x => x.ClassTrainingPrograms)
                                       .Create();
            await _dbContext.Syllabi.AddRangeAsync(syllabusMockData);
            await _dbContext.TrainingPrograms.AddAsync(trainingProgramMockData);
            await _dbContext.SaveChangesAsync();
            var MockData = new List<TrainingProgramSyllabus>();
            foreach (var item in syllabusMockData)
            {
                var data = new TrainingProgramSyllabus
                {
                    Syllabus = item,
                    TrainingProgram = trainingProgramMockData
                };
                MockData.Add(data);
            }
            var itemCount = await _dbContext.TrainingProgramSyllabi.CountAsync();
            var items = await _dbContext.TrainingProgramSyllabi.OrderByDescending(x => x.CreationDate)
                                                  .Take(10)
                                                  .AsNoTracking()
            .ToListAsync();
            var resultset = new Pagination<TrainingProgramSyllabus>()
            {
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = itemCount,
                Items = items,
            };
            _unitOfWorkMock.Setup(x => x.TrainingProgramSyllabiRepository.ToPagination(0, 10)).ReturnsAsync(resultset);
            //act
            var result = await _syllabusTrainingProgramService.GetAllSyllabusTrainingPrograms();
            //assert
            _unitOfWorkMock.Verify(x => x.TrainingProgramSyllabiRepository.ToPagination(0, 10), Times.Once());
        }
    }
}
