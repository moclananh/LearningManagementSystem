using Applications.Commons;
using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.ModuleViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Moq;

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
                                .CreateMany(100)
                                .ToList(),
                PageIndex = 0,
                PageSize = 100,
                TotalItemsCount = 100
            };
            var expectedResult = _mapperConfig.Map<Pagination<Module>>(mockData);

            _unitOfWorkMock.Setup(x => x.ModuleRepository.ToPagination(0, 10)).ReturnsAsync(mockData);
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
            _unitOfWorkMock.Setup(x => x.ModuleRepository.GetByIdAsync(mockData.Id))
                           .ReturnsAsync(mockData);
            var updateDataMock = _fixture.Build<UpdateModuleViewModel>().Create();
            //act
            await _moduleService.UpdateModule(mockData.Id, updateDataMock);
            var result = _mapperConfig.Map<UpdateModuleViewModel>(mockData);
            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UpdateModuleViewModel>();
            result.ModuleName.Should().Be(mockData.ModuleName);
            _unitOfWorkMock.Verify(x => x.ModuleRepository.Update(mockData), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
        }

        [Fact]
        public async Task UpdateModule_ShouldReturnNull_WhenFailSaved()
        {
            //arrange
            var moduleId = Guid.NewGuid();
            _unitOfWorkMock.Setup(x => x.ModuleRepository.GetByIdAsync(moduleId))
                           .ReturnsAsync(null as Module);
            var updateDataMock = _fixture.Build<UpdateModuleViewModel>().Create();
            //act
            var result = await _moduleService.UpdateModule(moduleId, updateDataMock);
            //assert
            result.Should().BeNull();
            _unitOfWorkMock.Verify(x => x.ModuleRepository.Update(It.IsAny<Module>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Never());
        }
    }
}
