using Application.ViewModels.UnitViewModels;
using Applications.Commons;
using Applications.Interfaces;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Moq;
using System.Net;

namespace Applications.Tests.Services.UnitServices
{
    public class UnitServiceTest : SetupTest
    {
        private readonly IUnitServices _unitService;
        public UnitServiceTest()
        {
            _unitService = new Applications.Services.UnitServices(_unitOfWorkMock.Object, _mapperConfig);
        }

        [Fact]
        public async Task GetAllUnits_ShouldReturnCorrectData()
        {
            //arrange
            var MockData = new Pagination<Unit>
            {
                Items = _fixture.Build<Unit>()
                                .Without(x => x.Practices)
                                .Without(x => x.Lectures)
                                .Without(x => x.Assignments)
                                .Without(x => x.Quizzs)
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
            var expected = _mapperConfig.Map<Pagination<UnitViewModel>>(MockData);
            _unitOfWorkMock.Setup(x => x.UnitRepository.ToPagination(0, 10)).ReturnsAsync(MockData);
            _unitOfWorkMock.Setup(x => x.UserRepository.GetEntitiesByIdsAsync(It.IsAny<List<Guid?>>())).ReturnsAsync(user);
            //act
            var result = await _unitService.GetAllUnits();
            //assert
            _unitOfWorkMock.Verify(x => x.UnitRepository.ToPagination(0, 10), Times.Once());
        }

        [Fact]
        public async Task GetUnitById_ShouldReturnCorrectData()
        {
            //arrange
            var MockData = _fixture.Build<Unit>()
                                       .Without(x => x.Practices)
                                       .Without(x => x.Lectures)
                                       .Without(x => x.Assignments)
                                       .Without(x => x.Quizzs)
                                       .Without(x => x.ModuleUnits)
                                       .Create();
            var expected = _mapperConfig.Map<UnitViewModel>(MockData);
            var createBy = new User { Email = "mock@example.com" };
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(MockData.CreatedBy)).ReturnsAsync(createBy);
            expected.CreatedBy = createBy.Email;
            _unitOfWorkMock.Setup(x => x.UnitRepository.GetByIdAsync(MockData.Id))
                           .ReturnsAsync(MockData);
            //act
            var result = await _unitService.GetUnitById(MockData.Id);
            //assert
            _unitOfWorkMock.Verify(x => x.UnitRepository.GetByIdAsync(MockData.Id), Times.Once());
        }

        [Fact]
        public async Task CreateUnit_ShouldReturnCorrectData_WhenSuccessSaved()
        {
            //arrange
            var mocks = _fixture.Build<CreateUnitViewModel>()
                                .Create();
            _unitOfWorkMock.Setup(x => x.UnitRepository.AddAsync(It.IsAny<Unit>()))
                           .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            //act
            var result = await _unitService.CreateUnitAsync(mocks);
            //assert
            _unitOfWorkMock.Verify(x => x.UnitRepository.AddAsync(It.IsAny<Unit>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
        }

        [Fact]
        public async Task CreateUnit_ShouldReturnNull_WhenFailedSave()
        {
            //arrange
            var mocks = _fixture.Build<CreateUnitViewModel>()
                                .Create();
            _unitOfWorkMock.Setup(x => x.UnitRepository.AddAsync(It.IsAny<Unit>()))
                           .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            //act
            var result = await _unitService.CreateUnitAsync(mocks);
            //assert
            _unitOfWorkMock.Verify(x => x.UnitRepository.AddAsync(It.IsAny<Unit>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
            Assert.Equal(HttpStatusCode.BadRequest.ToString(), result.Status);
            Assert.Equal("Created Failed", result.Message);
        }

        [Fact]
        public async Task UpdateUnit_ShouldReturnCorrectData_WhenSuccessSaved()
        {
            //arrange
            var unitObj = _fixture.Build<Unit>()
                                    .Without(x => x.Practices)
                                    .Without(x => x.Lectures)
                                    .Without(x => x.Assignments)
                                    .Without(x => x.Quizzs)
                                    .Without(x => x.ModuleUnits)
                                   .Create();
            _unitOfWorkMock.Setup(x => x.UnitRepository.GetByIdAsync(unitObj.Id))
                           .ReturnsAsync(unitObj);
            var updateDataMock = _fixture.Build<CreateUnitViewModel>()
                                         .Create();
            //act
            await _unitService.UpdateUnitAsync(unitObj.Id, updateDataMock);
            var result = _mapperConfig.Map<CreateUnitViewModel>(unitObj);
            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CreateUnitViewModel>();
            result.UnitName.Should().Be(updateDataMock.UnitName);
            // add more property ...
            _unitOfWorkMock.Verify(x => x.UnitRepository.Update(unitObj), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateUnit_ShouldReturnNull_WhenFailedSave()
        {
            //arrange
            var UnitId = Guid.NewGuid();
            _unitOfWorkMock.Setup(x => x.UnitRepository.GetByIdAsync(UnitId))
                           .ReturnsAsync(null as Unit);
            var updateDataMock = _fixture.Build<CreateUnitViewModel>()
                                         .Create();
            //act
            var result = await _unitService.UpdateUnitAsync(UnitId, updateDataMock);
            //assert
            _unitOfWorkMock.Verify(x => x.UnitRepository.Update(It.IsAny<Unit>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Never);
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetDisableUnits_ShouldReturnCorrectData()
        {
            //arrange
            var MockData = new Pagination<Unit>
            {
                Items = _fixture.Build<Unit>()
                                .Without(x => x.Practices)
                                .Without(x => x.Lectures)
                                .Without(x => x.Assignments)
                                .Without(x => x.Quizzs)
                                .Without(x => x.ModuleUnits)
                                .With(x => x.Status, Domain.Enum.StatusEnum.Status.Disable)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,  
                TotalItemsCount = 30,
            };
            var units = _mapperConfig.Map<Pagination<Unit>>(MockData);
            _unitOfWorkMock.Setup(x => x.UnitRepository.GetDisableUnits(0, 10)).ReturnsAsync(MockData);
            var expected = _mapperConfig.Map<Pagination<UnitViewModel>>(units);
            //act
            var result = await _unitService.GetDisableUnitsAsync();
            //assert
            _unitOfWorkMock.Verify(x => x.UnitRepository.GetDisableUnits(0, 10), Times.Once());
        }

        [Fact]
        public async Task GetEnableUnits_ShouldReturnCorrectData()
        {
            //arrange
            var MockData = new Pagination<Unit>
            {
                Items = _fixture.Build<Unit>()
                                .Without(x => x.Practices)
                                .Without(x => x.Lectures)
                                .Without(x => x.Assignments)
                                .Without(x => x.Quizzs)
                                .Without(x => x.ModuleUnits)
                                .With(x => x.Status, Domain.Enum.StatusEnum.Status.Enable)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };
            var units = _mapperConfig.Map<Pagination<Unit>>(MockData);
            _unitOfWorkMock.Setup(x => x.UnitRepository.GetEnableUnits(0, 10)).ReturnsAsync(MockData);
            var expected = _mapperConfig.Map<Pagination<UnitViewModel>>(units);
            //act
            var result = await _unitService.GetEnableUnitsAsync();
            //assert
            _unitOfWorkMock.Verify(x => x.UnitRepository.GetEnableUnits(0, 10), Times.Once());
        }

        [Fact]
        public async Task GetUnitByModuleId_ShouldReturnCorrectData()
        {
            //arrange
            var moduleId = Guid.NewGuid();
            var mockData = new Pagination<Unit>
            {
                Items = _fixture.Build<Unit>()
                                .Without(x => x.Practices)
                                .Without(x => x.Lectures)
                                .Without(x => x.Assignments)
                                .Without(x => x.Quizzs)
                                .Without(x => x.ModuleUnits)
                                .With(x => x.Id, moduleId)
                .CreateMany(100)
                .ToList(),
                PageIndex = 0,
                PageSize = 100,
                TotalItemsCount = 100
            };
            var unit = _mapperConfig.Map<Pagination<Unit>>(mockData);
            _unitOfWorkMock.Setup(x => x.UnitRepository.ViewAllUnitByModuleIdAsync(moduleId, 0, 10)).ReturnsAsync(mockData);
            var expected = _mapperConfig.Map<Pagination<Unit>>(unit);
            //act
            var result = await _unitService.GetUnitByModuleIdAsync(moduleId);
            //assert
            _unitOfWorkMock.Verify(x => x.UnitRepository.ViewAllUnitByModuleIdAsync(moduleId, 0, 10), Times.Once());
        }

        [Fact]
        public async Task GetUnitByName_ShouldReturnCorrectData()
        {
            //arrange
            var MockData = new Pagination<Unit>
            {
                Items = _fixture.Build<Unit>()
                                .Without(x => x.Practices)
                                .Without(x => x.Lectures)
                                .Without(x => x.Assignments)
                                .Without(x => x.Quizzs)
                                .Without(x => x.ModuleUnits)
                                .With(x => x.UnitName, "Mock")
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };
            var units = _mapperConfig.Map<Pagination<Unit>>(MockData);
            _unitOfWorkMock.Setup(x => x.UnitRepository.GetUnitByNameAsync("Mock", 0, 10)).ReturnsAsync(MockData);
            var expected = _mapperConfig.Map<Pagination<UnitViewModel>>(units);
            //act
            var result = await _unitService.GetUnitByNameAsync("Mock");
            //assert
            _unitOfWorkMock.Verify(x => x.UnitRepository.GetUnitByNameAsync("Mock", 0, 10), Times.Once());
        }
    }
}
