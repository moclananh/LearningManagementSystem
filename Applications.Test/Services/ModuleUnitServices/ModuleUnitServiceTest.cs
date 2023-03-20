using Applications.Commons;
using Applications.Interfaces;
using Applications.Services;
using AutoFixture;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Tests;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Applications.Tests.Services.ModuleUnitServices
{
    public class ModuleUnitServiceTest : SetupTest
    {
        private readonly ModuleUnitService _moduleUnitService;

        public ModuleUnitServiceTest()
        {
            _moduleUnitService = new ModuleUnitService(_unitOfWorkMock.Object, _mapperConfig);
        }

        [Fact]
        public async Task GetAllModuleUnit_ShouldReturnCorrectData()
        {
            //arrange
            var unitMockData = _fixture.Build<Unit>()
                                       .Without(x => x.Practices)
                                       .Without(x => x.Lectures)
                                       .Without(x => x.Assignments)
                                       .Without(x => x.Quizzs)
                                       .Without(x => x.ModuleUnits)
                                       .CreateMany(30)
                                       .ToList();
            var moduleMockData = _fixture.Build<Module>()
                                         .Without(x => x.AuditPlan)
                                         .Without(x => x.ModuleUnits)
                                         .Without(x => x.SyllabusModules)
                                         .Create();
            var user = _fixture.Build<User>()
                               .Without(x => x.UserAuditPlans)
                               .Without(x => x.AbsentRequests)
                               .Without(x => x.ClassUsers)
                               .Without(x => x.Attendences)
                               .CreateMany(3)
                               .ToList();
            await _dbContext.Units.AddRangeAsync(unitMockData);
            await _dbContext.Modules.AddAsync(moduleMockData);
            await _dbContext.SaveChangesAsync();
            var MockData = new List<ModuleUnit>();
            foreach (var item in unitMockData)
            {
                var data = new ModuleUnit
                {
                    Unit = item,
                    Module = moduleMockData
                };
                MockData.Add(data);
            }
            var itemCount = await _dbContext.ModuleUnit.CountAsync();
            var items = await _dbContext.ModuleUnit.OrderByDescending(x => x.CreationDate)
                                                      .Take(10)
                                                      .AsNoTracking()
                                                      .ToListAsync();
            var moduleUnits = new Pagination<ModuleUnit>()
            {
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = itemCount,
                Items = items,
            };
            var expected = _mapperConfig.Map<Pagination<ModuleUnit>>(moduleUnits);
            _unitOfWorkMock.Setup(x => x.ModuleUnitRepository.ToPagination(0, 10)).ReturnsAsync(moduleUnits);
            _unitOfWorkMock.Setup(x => x.UserRepository.GetEntitiesByIdsAsync(It.IsAny<List<Guid?>>())).ReturnsAsync(user);
            //act
            var result = await _moduleUnitService.GetAllModuleUnitsAsync();
            //assert
            _unitOfWorkMock.Verify(x => x.ModuleUnitRepository.ToPagination(0, 10), Times.Once());
        }
    }
}
