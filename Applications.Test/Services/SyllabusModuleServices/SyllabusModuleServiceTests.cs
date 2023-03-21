using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.Response;
using Applications.ViewModels.SyllabusModuleViewModel;
using AutoFixture;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Tests;
using FluentAssertions;
using Moq;
using System.Net;

namespace Applications.Tests.Services.SyllabusModuleServices
{
    public class SyllabusModuleServiceTests : SetupTest
    {
        private readonly ISyllabusModuleService _syllabusModuleService;
        public SyllabusModuleServiceTests()
        {
            _syllabusModuleService = new SyllabusModuleService(_unitOfWorkMock.Object, _mapperConfig);
        }

        [Fact]
        public async Task AddMultiModulesToSyllabus_ShouldReturnCorrectData()
        {
            //arrange
            var syllabusMockData = _fixture.Build<Syllabus>()
                                       .Without(x => x.TrainingProgramSyllabi)
                                       .Without(x => x.SyllabusOutputStandards)
                                       .Without(x => x.SyllabusModules)
                                       .Create();
            var moduleMockData = _fixture.Build<Module>()
                                         .Without(x => x.AuditPlan)
                                         .Without(x => x.ModuleUnits)
                                         .Without(x => x.SyllabusModules)
                                         .CreateMany(30)
                                         .ToList();
            await _dbContext.AddRangeAsync(moduleMockData);
            await _dbContext.AddAsync(syllabusMockData);
            await _dbContext.SaveChangesAsync();
            var syllabusModuleMockData = new List<SyllabusModule>();
            List<Guid> ModulesListId = new List<Guid>();
            foreach (var item in moduleMockData)
            {
                var mockData = new SyllabusModule()
                {
                    Syllabus = syllabusMockData,
                    Module = item
                };
                syllabusModuleMockData.Add(mockData);
                ModulesListId.Add(item.Id);
            }

            await _dbContext.AddRangeAsync(syllabusModuleMockData);
            await _dbContext.SaveChangesAsync();
            _unitOfWorkMock.Setup(x => x.SyllabusRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(syllabusMockData);
            foreach (var item in moduleMockData)
            {
                _unitOfWorkMock.Setup(x => x.ModuleRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(item);
            }
            _unitOfWorkMock.Setup(x => x.SyllabusModuleRepository.AddRangeAsync(It.IsAny<List<SyllabusModule>>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            var expectedlist = _mapperConfig.Map<List<SyllabusModuleViewModel>>(syllabusModuleMockData);
            var expected = new Response(HttpStatusCode.OK, "Syllabus Module Added Successfully", expectedlist);
            //act
            var result = await _syllabusModuleService.AddMultiModulesToSyllabus(syllabusMockData.Id, ModulesListId);
            //assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
