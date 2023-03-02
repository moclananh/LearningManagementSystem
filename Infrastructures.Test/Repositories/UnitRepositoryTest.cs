using AutoFixture;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Repositories;

namespace Infrastructures.Tests.Repositories
{
    public class UnitRepositoryTest : SetupTest
    {
        private readonly UnitRepository _unitRepository;
        public UnitRepositoryTest()
        {
            _unitRepository = new UnitRepository(
            _dbContext,
            _currentTimeMock.Object,
            _claimServiceMock.Object
            );
        }

        [Fact]
        public async Task UnitRepository_GetUnitByName_ShoulReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Unit>()
                .Without(x => x.Practices)
                .Without(x => x.Lectures)
                .Without(x => x.Assignments)
                .Without(x => x.Quizzs)
                .Without(x => x.ModuleUnits)
                .With(x => x.UnitName, "Mock")
                .CreateMany(30)
                .ToList();
            await _dbContext.Units.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(x => x.UnitName.Contains("Mock"))
                                    .OrderByDescending(x => x.CreationDate)
                                    .Take(10)
                                    .ToList();
            //act
            var resultPaging = await _unitRepository.GetUnitByNameAsync("Mock");
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
        public async Task UnitRepository_GetEnableUnites_ShoulReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Unit>()
                .Without(x => x.Practices)
                .Without(x => x.Lectures)
                .Without(x => x.Assignments)
                .Without(x => x.Quizzs)
                .Without(x => x.ModuleUnits)
                .With(x => x.Status, Domain.Enum.StatusEnum.Status.Enable)
                .CreateMany(30)
                .ToList();
            await _dbContext.Units.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Take(10)
                                    .ToList();
            //act
            var resultPaging = await _unitRepository.GetEnableUnits();
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
        public async Task UnitRepository_GetDisableUnites_ShoulReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Unit>()
                .Without(x => x.Practices)
                .Without(x => x.Lectures)
                .Without(x => x.Assignments)
                .Without(x => x.Quizzs)
                .Without(x => x.ModuleUnits)
                .With(x => x.Status, Domain.Enum.StatusEnum.Status.Disable)
                .CreateMany(30)
                .ToList();
            await _dbContext.Units.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Take(10)
                                    .ToList();
            //act
            var resultPaging = await _unitRepository.GetDisableUnits();
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
        public async Task UnitRepository_GetUnitByModuleId_ShouldReturnCorrectData()
        {
            //arrange
            var unitMockData = _fixture.Build<Unit>()
                .Without(x => x.Practices)
                .Without(x => x.Lectures)
                .Without(x => x.Assignments)
                .Without(x => x.Quizzs)
                .Without(x => x.ModuleUnits)
                .CreateMany(10)
                .ToList();
            var moduleMockData = _fixture.Build<Module>()
                .Without(x => x.AuditPlan)
                .Without(x => x.ModuleUnits)
                .Without(x => x.SyllabusModules)
                .Create();

            await _dbContext.Units.AddRangeAsync(unitMockData);
            await _dbContext.Modules.AddAsync(moduleMockData);
            await _dbContext.SaveChangesAsync();
            var dataList = new List<ModuleUnit>();
            foreach (var item in unitMockData)
            {
                var data = new ModuleUnit
                {
                    Module = moduleMockData,
                    Unit = item
                };
                dataList.Add(data);
            }
            await _dbContext.ModuleUnit.AddRangeAsync(dataList);
            await _dbContext.SaveChangesAsync();

            var expected = _dbContext.ModuleUnit.Where(x => x.ModuleId.Equals(moduleMockData.Id)).Select(x => x.Unit).ToList();
            //act
            var resultPaging = await _unitRepository.ViewAllUnitByModuleIdAsync(moduleMockData.Id);
            var result = resultPaging.Items;
            //assert
            resultPaging.Previous.Should().BeFalse();
            resultPaging.Next.Should().BeFalse();
            resultPaging.Items.Count.Should().Be(10);
            resultPaging.TotalItemsCount.Should().Be(10);
            resultPaging.TotalPagesCount.Should().Be(1);
            resultPaging.PageIndex.Should().Be(0);
            resultPaging.PageSize.Should().Be(10);
            result.Should().BeEquivalentTo(expected, op => op.Excluding(x => x.ModuleUnits));
        }
    }
}
