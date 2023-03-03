using Applications.Repositories;
using AutoFixture;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Repositories;

namespace Infrastructures.Tests.Repositories
{
    public class ModuleRepositoryTests : SetupTest
    {
        private readonly IModuleRepository _moduleRepository;
        public ModuleRepositoryTests()
        {
            _moduleRepository = new ModuleRepository(
                _dbContext,
                _currentTimeMock.Object,
                _claimServiceMock.Object
                );
        }        

        [Fact]
        public async Task ModuleRepository_GetModuleByName_ShouldReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Module>()
                            .Without(x => x.ModuleUnits)
                            .Without(x => x.AuditPlan)
                            .Without(x => x.SyllabusModules)
                            .With(x => x.ModuleName, "Mock")
                            .CreateMany(30)
                            .ToList();
            await _dbContext.Modules.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(x => x.ModuleName.Contains("Mock"))
                                    .OrderByDescending(x => x.CreationDate)
                                    .Take(10)
                                    .ToList();
            //act
            var resultPaging = await _moduleRepository.GetModuleByName("Mock");
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
        public async Task ModuleRepository_GetEnableModules_ShouldReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Module>()
                                   .Without(x => x.ModuleUnits)
                                   .Without(x => x.AuditPlan)
                                   .Without(x => x.SyllabusModules)
                                   .With(x => x.Status, Domain.Enum.StatusEnum.Status.Enable)
                                   .CreateMany(30)
                                   .ToList();
            await _dbContext.Modules.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Take(10)
                                    .ToList();
            //act
            var resultPaging = await _moduleRepository.GetEnableModules();
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
        public async Task ModuleRepository_GetDisableModules_ShouldReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Module>()
                                   .Without(x => x.ModuleUnits)
                                   .Without(x => x.AuditPlan)
                                   .Without(x => x.SyllabusModules)
                                   .With(x => x.Status, Domain.Enum.StatusEnum.Status.Disable)
                                   .CreateMany(30)
                                   .ToList();
            await _dbContext.Modules.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Take(10)
                                    .ToList();
            //act
            var resultPaging = await _moduleRepository.GetDisableModules();
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
        public async Task ModuleRepository_GetModulesBySyllabusId_ShouldReturnCorrectData()
        {
            //arrange
            var moduleMockData = _fixture.Build<Module>()
                                         .Without(x => x.AuditPlan)
                                         .Without(x => x.ModuleUnits)
                                         .Without(x => x.SyllabusModules)
                                         .CreateMany(10)
                                         .ToList();
            var syllabusMockData = _fixture.Build<Syllabus>()
                                           .Without(x => x.TrainingProgramSyllabi)
                                           .Without(x => x.SyllabusOutputStandards)
                                           .Without(x => x.SyllabusModules)
                                           .Create();
            await _dbContext.Modules.AddRangeAsync(moduleMockData);
            await _dbContext.Syllabi.AddAsync(syllabusMockData);
            await _dbContext.SaveChangesAsync();
            var dataList = new List<SyllabusModule>();
            foreach(var item in moduleMockData)
            {
                var data = new SyllabusModule
                {
                    Syllabus = syllabusMockData,
                    Module = item
                };
                dataList.Add(data);
            }
            await _dbContext.SyllabusModule.AddRangeAsync(dataList);
            await _dbContext.SaveChangesAsync();
            var expected = _dbContext.SyllabusModule.Where(x => x.SyllabusId.Equals(syllabusMockData.Id))
                                                    .Select(x => x.Module).ToList();
            //act
            var resultPaging = await _moduleRepository.GetModulesBySyllabusId(syllabusMockData.Id);
            var result = resultPaging.Items;
            //assert
            resultPaging.Previous.Should().BeFalse();
            resultPaging.Next.Should().BeFalse();
            resultPaging.Items.Count.Should().Be(10);
            resultPaging.TotalItemsCount.Should().Be(10);
            resultPaging.TotalPagesCount.Should().Be(1);
            resultPaging.PageIndex.Should().Be(0);
            resultPaging.PageSize.Should().Be(10);
            result.Should().BeEquivalentTo(expected, op => op.Excluding(x => x.SyllabusModules));
        }
    }
}
