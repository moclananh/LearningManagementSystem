using Applications.Repositories;
using AutoFixture;
using Domain.Entities;
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
                            .CreateMany(30)
                            .ToList();
            await _dbContext.Modules.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            int i = 0;
            foreach (var item in mockData)
            {
                item.ModuleName = $"Mock{i}";
                i++;
            }
            _dbContext.UpdateRange(mockData);
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
                                   .CreateMany(30)
                                   .ToList();
            await _dbContext.Modules.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            foreach (var item in mockData)
            {
                item.Status = Domain.Enum.StatusEnum.Status.Enable;
            }
            _dbContext.UpdateRange(mockData);
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
                                   .CreateMany(30)
                                   .ToList();
            await _dbContext.Modules.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            foreach (var item in mockData)
            {
                item.Status = Domain.Enum.StatusEnum.Status.Disable;
            }
            _dbContext.UpdateRange(mockData);
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
    }
}
