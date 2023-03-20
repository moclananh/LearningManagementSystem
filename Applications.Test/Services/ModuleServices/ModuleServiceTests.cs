using Applications.Commons;
using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.ModuleUnitViewModels;
using Applications.ViewModels.ModuleViewModels;
using Applications.ViewModels.Response;
using AutoFixture;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Tests;
using FluentAssertions;
using Moq;
using System.Net;

namespace Applications.Tests.Services.ModuleServices
{
    public class ModuleServiceTests : SetupTest
    {
        private readonly IModuleService _moduleService;

        public ModuleServiceTests()
        {
            _moduleService = new ModuleService(_unitOfWorkMock.Object, _mapperConfig);
        }
        [Fact]
        public async Task GetAllModules_ShouldReturnCorrectData()
        {
            //arrange
            var mockData = new Pagination<Module>
            {
                Items = _fixture.Build<Module>()
                                .Without(x => x.AuditPlan)
                                .Without(x => x.SyllabusModules)
                                .Without(x => x.ModuleUnits)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30
            };
            var user = _fixture.Build<User>()
                               .Without(x => x.UserAuditPlans)
                               .Without(x => x.AbsentRequests)
                               .Without(x => x.ClassUsers)
                               .Without(x => x.Attendences)
                               .CreateMany(3)
                               .ToList();
            var expected = _mapperConfig.Map<Pagination<ModuleViewModels>>(mockData);
            _unitOfWorkMock.Setup(x => x.ModuleRepository.ToPagination(0, 10)).ReturnsAsync(mockData);
            _unitOfWorkMock.Setup(x => x.UserRepository.GetEntitiesByIdsAsync(It.IsAny<List<Guid?>>())).ReturnsAsync(user);
            //act
            var result = await _moduleService.GetAllModules();
            //assert
            _unitOfWorkMock.Verify(x => x.ModuleRepository.ToPagination(0, 10), Times.Once());
        }

        [Fact]
        public async Task CreateModule_ShouldReturnCorrectData_WhenSccessSaved()
        {
            //arrange
            var mockData = _fixture.Build<CreateModuleViewModel>().Create();

            _unitOfWorkMock.Setup(x => x.ModuleRepository.AddAsync(It.IsAny<Module>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            //act
            var result = await _moduleService.CreateModule(mockData);
            //assert
            _unitOfWorkMock.Verify(x => x.ModuleRepository.AddAsync(It.IsAny<Module>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
        }

        [Fact]
        public async Task CreateModule_ShouldReturnNull_WhenFailSaved()
        {
            //arrange
            var mockData = _fixture.Build<CreateModuleViewModel>().Create();

            _unitOfWorkMock.Setup(x => x.ModuleRepository.AddAsync(It.IsAny<Module>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            //act 
            var result = await _moduleService.CreateModule(mockData);
            //assert
            _unitOfWorkMock.Verify(x => x.ModuleRepository.AddAsync(It.IsAny<Module>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateModule_ShouldReturnCorrectData_WhenSuccessSaved()
        {
            //arrange
            var mockData = _fixture.Build<Module>()
                                   .Without(x => x.AuditPlan)
                                   .Without(x => x.ModuleUnits)
                                   .Without(x => x.SyllabusModules)
                                   .Create();
            var updateDataMock = _fixture.Build<UpdateModuleViewModel>()
                                         .Create();
            _unitOfWorkMock.Setup(x => x.ModuleRepository.GetByIdAsync(It.IsAny<Guid>()))
                          .ReturnsAsync(mockData);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            //act
            await _moduleService.UpdateModule(mockData.Id, updateDataMock);
            var result = _mapperConfig.Map<UpdateModuleViewModel>(mockData);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UpdateModuleViewModel>();
            result.ModuleName.Should().Be(updateDataMock.ModuleName);
            // add more property ...
            _unitOfWorkMock.Verify(x => x.ModuleRepository.Update(mockData), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateModule_ShouldReturnNull_WhenFailSaved()
        {
            //arrange
            var mockData = _fixture.Build<Module>()
                                   .Without(x => x.AuditPlan)
                                   .Without(x => x.ModuleUnits)
                                   .Without(x => x.SyllabusModules)
                                   .Create();
            _unitOfWorkMock.Setup(x => x.ModuleRepository.GetByIdAsync(It.IsAny<Guid>()))
                           .ReturnsAsync(mockData);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            var updateDataMock = _fixture.Build<UpdateModuleViewModel>()
                                         .Create();
            //act
            var result = await _moduleService.UpdateModule(mockData.Id, updateDataMock);
            //assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateModule_ShouldReturnNull_WhenNotFoundModule()
        {
            //arrange
            var moduleMockData = _fixture.Build<Module>()
                                         .Without(x => x.AuditPlan)
                                         .Without(x => x.ModuleUnits)
                                         .Without(x => x.SyllabusModules)
                                         .Create();
            _unitOfWorkMock.Setup(x => x.ModuleRepository.GetByIdAsync(It.IsAny<Guid>()))
                           .ReturnsAsync(moduleMockData);
            var updateDataMock = _fixture.Build<UpdateModuleViewModel>().Create();
            //act
            var result = await _moduleService.UpdateModule(moduleMockData.Id, updateDataMock);
            //assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetModuleById_ShouldReturnCorrectData()
        {
            //arrange
            var moduleMockData = _fixture.Build<Module>()
                                         .Without(x => x.AuditPlan)
                                         .Without(x => x.ModuleUnits)
                                         .Without(x => x.SyllabusModules)
                                         .Create();
            var expected = _mapperConfig.Map<ModuleViewModels>(moduleMockData);
            var createBy = new User { Email = "mock@example.com" };
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(moduleMockData.CreatedBy)).ReturnsAsync(createBy);
            expected.CreatedBy = createBy.Email;
            _unitOfWorkMock.Setup(x => x.ModuleRepository.GetByIdAsync(moduleMockData.Id))
                           .ReturnsAsync(moduleMockData);
            //act
            var result = await _moduleService.GetModuleById(moduleMockData.Id);
            //assert
            _unitOfWorkMock.Verify(x => x.ModuleRepository.GetByIdAsync(moduleMockData.Id), Times.Once());
        }

        [Fact]
        public async Task GetModulesBySyllabusId_ShouldReturnCorrectData()
        {
            //arrange
            var syllabusId = Guid.NewGuid();
            var moduleMockData = new Pagination<Module>
            {
                Items = _fixture.Build<Module>()
                                .Without(x => x.AuditPlan)
                                .Without(x => x.ModuleUnits)
                                .Without(x => x.SyllabusModules)
                                .With(x => x.Id, syllabusId)
                                .CreateMany(100)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 10,
            };
            var module = _mapperConfig.Map<Pagination<Module>>(moduleMockData);
            _unitOfWorkMock.Setup(x => x.ModuleRepository.GetModulesBySyllabusId(syllabusId, 0, 10)).ReturnsAsync(moduleMockData);
            var expected = _mapperConfig.Map<Pagination<Module>>(module);
            //act
            var result = await _moduleService.GetModulesBySyllabusId(syllabusId);
            //assert
            _unitOfWorkMock.Verify(x => x.ModuleRepository.GetModulesBySyllabusId(syllabusId, 0, 10), Times.Once());
        }

        [Fact]
        public async Task GetModulesByName_ShouldReturnCorrectData()
        {
            //arrange
            var moduleMockData = new Pagination<Module>
            {
                Items = _fixture.Build<Module>()
                                .Without(x => x.AuditPlan)
                                .Without(x => x.ModuleUnits)
                                .Without(x => x.SyllabusModules)
                                .With(x => x.ModuleName, "Mock")
                                .CreateMany(10)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 10,
            };
            var expected = _mapperConfig.Map<Pagination<Module>>(moduleMockData);
            _unitOfWorkMock.Setup(x => x.ModuleRepository.GetModuleByName("Mock", 0, 10))
                           .ReturnsAsync(moduleMockData);
            var expectedResult = _mapperConfig.Map<Pagination<ModuleViewModels>>(expected);
            //act
            var result = await _moduleService.GetModulesByName("Mock");
            //assert
            _unitOfWorkMock.Verify(x => x.ModuleRepository.GetModuleByName("Mock", 0, 10), Times.Once());
        }

        [Fact]
        public async Task GetDisableModules_ShouldReturnCorrectData()
        {
            var moduleMockData = new Pagination<Module>
            {
                Items = _fixture.Build<Module>()
                                .Without(x => x.AuditPlan)
                                .Without(x => x.ModuleUnits)
                                .Without(x => x.SyllabusModules)
                                .CreateMany(10)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 10,
            };
            var expected = _mapperConfig.Map<Pagination<Module>>(moduleMockData);
            _unitOfWorkMock.Setup(x => x.ModuleRepository.GetDisableModules(0, 10))
                           .ReturnsAsync(moduleMockData);
            var expectedResult = _mapperConfig.Map<Pagination<ModuleViewModels>>(expected);
            //act
            var result = await _moduleService.GetDisableModules();
            //assert
            _unitOfWorkMock.Verify(x => x.ModuleRepository.GetDisableModules(0, 10), Times.Once());
        }

        [Fact]
        public async Task GetEnableModules_ShouldReturnCorrectData()
        {
            var moduleMockData = new Pagination<Module>
            {
                Items = _fixture.Build<Module>()
                                .Without(x => x.AuditPlan)
                                .Without(x => x.ModuleUnits)
                                .Without(x => x.SyllabusModules)
                                .CreateMany(10)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 10,
            };
            var expected = _mapperConfig.Map<Pagination<Module>>(moduleMockData);
            _unitOfWorkMock.Setup(x => x.ModuleRepository.GetEnableModules(0, 10))
                           .ReturnsAsync(moduleMockData);
            var expectedResult = _mapperConfig.Map<Pagination<ModuleViewModels>>(expected);
            //act
            var result = await _moduleService.GetEnableModules();
            //assert
            _unitOfWorkMock.Verify(x => x.ModuleRepository.GetEnableModules(0, 10), Times.Once());
        }

        [Fact]
        public async Task AddUnitToModule_ShouldReturnCorrectData()
        {
            //arrange
            var unitMockData = _fixture.Build<Unit>()
                                       .Without(x => x.Practices)
                                       .Without(x => x.Lectures)
                                       .Without(x => x.Assignments)
                                       .Without(x => x.Quizzs)
                                       .Without(x => x.ModuleUnits)
                                       .Create();
            var moduleMockData = _fixture.Build<Module>()
                                         .Without(x => x.AuditPlan)
                                         .Without(x => x.ModuleUnits)
                                         .Without(x => x.SyllabusModules)
                                         .Create();
            var mockData = new ModuleUnit()
            {
                Unit = unitMockData,
                Module = moduleMockData
            };
            _unitOfWorkMock.Setup(x => x.ModuleRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(moduleMockData);
            _unitOfWorkMock.Setup(x => x.UnitRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(unitMockData);
            _unitOfWorkMock.Setup(x => x.ModuleUnitRepository.AddAsync(It.IsAny<ModuleUnit>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            var expected = _mapperConfig.Map<CreateModuleUnitViewModel>(mockData);
            //act
            var result = await _moduleService.AddUnitToModule(moduleMockData.Id, unitMockData.Id);
            //assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task AddUnitToModule_ShouldReturnNull_WhenNotFoundModule()
        {
            //arrange
            var unitMockData = _fixture.Build<Unit>()
                                       .Without(x => x.Practices)
                                       .Without(x => x.Lectures)
                                       .Without(x => x.Assignments)
                                       .Without(x => x.Quizzs)
                                       .Without(x => x.ModuleUnits)
                                       .Create();
            var moduleMockData = _fixture.Build<Module>()
                                         .Without(x => x.AuditPlan)
                                         .Without(x => x.ModuleUnits)
                                         .Without(x => x.SyllabusModules)
                                         .Create();
            var mockData = new ModuleUnit()
            {
                Unit = unitMockData,
                Module = moduleMockData
            };
            _unitOfWorkMock.Setup(x => x.ModuleRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(moduleMockData);
            _unitOfWorkMock.Setup(x => x.UnitRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(unitMockData);
            _unitOfWorkMock.Setup(x => x.ModuleUnitRepository.AddAsync(It.IsAny<ModuleUnit>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            var expected = _mapperConfig.Map<CreateModuleUnitViewModel>(mockData);
            //act
            var result = await _moduleService.AddUnitToModule(moduleMockData.Id, unitMockData.Id);
            //assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddMultipleUnitToModule_ShouldReturnCorrectData()
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
            await _dbContext.AddRangeAsync(unitMockData);
            await _dbContext.AddAsync(moduleMockData);
            await _dbContext.SaveChangesAsync();
            var moduleUnitMockData = new List<ModuleUnit>();
            List<Guid> UnitListId = new List<Guid>();
            foreach (var item in unitMockData)
            {
                var mockData = new ModuleUnit()
                {
                    Unit = item,
                    Module = moduleMockData
                };
                moduleUnitMockData.Add(mockData);
                UnitListId.Add(item.Id);
            }

            await _dbContext.AddRangeAsync(moduleUnitMockData);
            await _dbContext.SaveChangesAsync();
            _unitOfWorkMock.Setup(x => x.ModuleRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(moduleMockData);
            foreach (var item in unitMockData)
            {
                _unitOfWorkMock.Setup(x => x.UnitRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(item);
            }
            _unitOfWorkMock.Setup(x => x.ModuleUnitRepository.AddRangeAsync(It.IsAny<List<ModuleUnit>>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            var expectedlist = _mapperConfig.Map<List<CreateModuleUnitViewModel>>(moduleUnitMockData);
            var expected = new Response(HttpStatusCode.OK, "Output Standards Added Successfully", expectedlist);
            //act
            var result = await _moduleService.AddMultipleUnitToModule(moduleMockData.Id, UnitListId);
            //assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task AddMultipleUnitToModule_ShouldReturnNull_WhenNotFoundModule()
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
            await _dbContext.AddRangeAsync(unitMockData);
            await _dbContext.AddAsync(moduleMockData);
            await _dbContext.SaveChangesAsync();
            var moduleUnitMockData = new List<ModuleUnit>();
            List<Guid> UnitListId = new List<Guid>();
            foreach (var item in unitMockData)
            {
                var mockData = new ModuleUnit()
                {
                    Unit = item,
                    Module = moduleMockData
                };
                moduleUnitMockData.Add(mockData);
                UnitListId.Add(item.Id);
            }

            await _dbContext.AddRangeAsync(moduleUnitMockData);
            await _dbContext.SaveChangesAsync();
            _unitOfWorkMock.Setup(x => x.ModuleRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(moduleMockData);
            foreach (var item in unitMockData)
            {
                _unitOfWorkMock.Setup(x => x.UnitRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(item);
            }
            _unitOfWorkMock.Setup(x => x.ModuleUnitRepository.AddRangeAsync(It.IsAny<List<ModuleUnit>>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            var expected = new Response(HttpStatusCode.NotFound, "Module Not Found");
            //act
            var result = await _moduleService.AddMultipleUnitToModule(moduleMockData.Id, UnitListId);
            //assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task RemoveUnitToModule_ShouldReturnCorrectData()
        {
            //arrange 
            var unitMockData = _fixture.Build<Unit>()
                                       .Without(x => x.Practices)
                                       .Without(x => x.Lectures)
                                       .Without(x => x.Assignments)
                                       .Without(x => x.Quizzs)
                                       .Without(x => x.ModuleUnits)
                                       .Create();
            var moduleMockData = _fixture.Build<Module>()
                                         .Without(x => x.AuditPlan)
                                         .Without(x => x.ModuleUnits)
                                         .Without(x => x.SyllabusModules)
                                         .Create();
            var mockData = new ModuleUnit()
            {
                ModuleId = moduleMockData.Id,
                Module = moduleMockData,
                UnitId = unitMockData.Id,
                Unit = unitMockData
            };
            _unitOfWorkMock.Setup(x => x.ModuleUnitRepository.GetModuleUnit(moduleMockData.Id, unitMockData.Id)).ReturnsAsync(mockData);
            _unitOfWorkMock.Setup(x => x.ModuleUnitRepository.SoftRemove(It.IsAny<ModuleUnit>()));
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            var expected = _mapperConfig.Map<CreateModuleUnitViewModel>(mockData);
            //act
            var result = await _moduleService.RemoveUnitToModule(moduleMockData.Id, unitMockData.Id);
            //assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task RemoveUnitToModule_ShouldReturnNull_WhenNotFoundModule()
        {
            //arrange 
            var unitMockData = _fixture.Build<Unit>()
                                       .Without(x => x.Practices)
                                       .Without(x => x.Lectures)
                                       .Without(x => x.Assignments)
                                       .Without(x => x.Quizzs)
                                       .Without(x => x.ModuleUnits)
                                       .Create();
            var moduleMockData = _fixture.Build<Module>()
                                         .Without(x => x.AuditPlan)
                                         .Without(x => x.ModuleUnits)
                                         .Without(x => x.SyllabusModules)
                                         .Create();
            var mockData = new ModuleUnit()
            {
                ModuleId = moduleMockData.Id,
                Module = moduleMockData,
                UnitId = unitMockData.Id,
                Unit = unitMockData
            };
            _unitOfWorkMock.Setup(x => x.ModuleUnitRepository.GetModuleUnit(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(() => null);
            _unitOfWorkMock.Setup(x => x.ModuleUnitRepository.SoftRemove(It.IsAny<ModuleUnit>()));
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            //act
            var result = await _moduleService.RemoveUnitToModule(moduleMockData.Id, unitMockData.Id);
            //assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task RemoveUnitToModule_ShouldReturnNull_WhenSavedFail()
        {
            //arrange 
            var unitMockData = _fixture.Build<Unit>()
                                       .Without(x => x.Practices)
                                       .Without(x => x.Lectures)
                                       .Without(x => x.Assignments)
                                       .Without(x => x.Quizzs)
                                       .Without(x => x.ModuleUnits)
                                       .Create();
            var moduleMockData = _fixture.Build<Module>()
                                         .Without(x => x.AuditPlan)
                                         .Without(x => x.ModuleUnits)
                                         .Without(x => x.SyllabusModules)
                                         .Create();
            var mockData = new ModuleUnit()
            {
                ModuleId = moduleMockData.Id,
                Module = moduleMockData,
                UnitId = unitMockData.Id,
                Unit = unitMockData
            };
            _unitOfWorkMock.Setup(x => x.ModuleUnitRepository.GetModuleUnit(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(mockData);
            _unitOfWorkMock.Setup(x => x.ModuleUnitRepository.SoftRemove(It.IsAny<ModuleUnit>()));
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            var expected = _mapperConfig.Map<CreateModuleUnitViewModel>(mockData);
            //act
            var result = await _moduleService.RemoveUnitToModule(moduleMockData.Id, unitMockData.Id);
            //assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task RemoveUnitToModule_ShouldReturnNull_WhenSaveChangedFailed()
        {
            //arrange 
            var unitMockData = _fixture.Build<Unit>()
                                       .Without(x => x.Practices)
                                       .Without(x => x.Lectures)
                                       .Without(x => x.Assignments)
                                       .Without(x => x.Quizzs)
                                       .Without(x => x.ModuleUnits)
                                       .Create();
            var moduleMockData = _fixture.Build<Module>()
                                         .Without(x => x.AuditPlan)
                                         .Without(x => x.ModuleUnits)
                                         .Without(x => x.SyllabusModules)
                                         .Create();
            var mockData = new ModuleUnit()
            {
                ModuleId = moduleMockData.Id,
                Module = moduleMockData,
                UnitId = unitMockData.Id,
                Unit = unitMockData
            };
            _unitOfWorkMock.Setup(x => x.ModuleUnitRepository.GetModuleUnit(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(() => null);
            _unitOfWorkMock.Setup(x => x.ModuleUnitRepository.SoftRemove(It.IsAny<ModuleUnit>()));
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            var expected = _mapperConfig.Map<CreateModuleUnitViewModel>(mockData);
            //act
            var result = await _moduleService.RemoveUnitToModule(moduleMockData.Id, unitMockData.Id);
            //assert
            result.Should().BeNull();
        }
    }
}
