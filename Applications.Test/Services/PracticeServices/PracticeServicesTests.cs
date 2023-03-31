using Application.ViewModels.UnitViewModels;
using Applications.Commons;
using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.PracticeViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Moq;


namespace Applications.Tests.Services.PracticeServices
{
    public class PracticeServicesTests : SetupTest
    {
        private readonly IPracticeService _practiceService;
        public PracticeServicesTests()
        {
            _practiceService = new PracticeService(_unitOfWorkMock.Object, _mapperConfig);
        }

        [Fact]
        public async Task GetPracticeById_ShouldReturnCorrectData()
        {
            var practiceMocks = _fixture.Build<Practice>()
                                .Without(x => x.Unit)
                                .Without(x => x.PracticeQuestions)
                                .Create();
            var expected = _mapperConfig.Map<PracticeViewModel>(practiceMocks);
            var createBy = new User { Email = "mock@example.com" };
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(practiceMocks.CreatedBy)).ReturnsAsync(createBy);
            expected.CreatedBy = createBy.Email;
            _unitOfWorkMock.Setup(x => x.PracticeRepository.GetByIdAsync(practiceMocks.Id))
                           .ReturnsAsync(practiceMocks);
            //act
            var result = await _practiceService.GetPracticeById(practiceMocks.Id);
            //assert
            _unitOfWorkMock.Verify(x => x.PracticeRepository.GetByIdAsync(practiceMocks.Id), Times.Once());
        }

        [Fact]
        public async Task GetPracticeByName_ShouldReturnCorrectData()
        {
            //arrange
            var MockData = new Pagination<Practice>
            {
                Items = _fixture.Build<Practice>()
                                .Without(x => x.PracticeQuestions)
                                .Without(x => x.Unit)
                                .With(x => x.PracticeName, "Mock")
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };
            var practice = _mapperConfig.Map<Pagination<Practice>>(MockData);
            _unitOfWorkMock.Setup(x => x.PracticeRepository.GetPracticeByName("Mock", 0, 10)).ReturnsAsync(MockData);
            var expected = _mapperConfig.Map<Pagination<PracticeViewModel>>(practice);
            //act
            var result = await _practiceService.GetPracticeByName("Mock");
            //assert
            _unitOfWorkMock.Verify(x => x.PracticeRepository.GetPracticeByName("Mock", 0, 10), Times.Once());
        }

        [Fact]
        public async Task GetAllPractice_ShouldReturnCorrectData()
        {
            //arrange
            var MockData = new Pagination<Practice>
            {
                Items = _fixture.Build<Practice>()
                                .Without(x => x.PracticeQuestions)
                                .Without(x => x.Unit)
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
            var expected = _mapperConfig.Map<Pagination<PracticeViewModel>>(MockData);
            _unitOfWorkMock.Setup(x => x.PracticeRepository.ToPagination(0, 10)).ReturnsAsync(MockData);
            _unitOfWorkMock.Setup(x => x.UserRepository.GetEntitiesByIdsAsync(It.IsAny<List<Guid?>>())).ReturnsAsync(user);
            //act
            var result = await _practiceService.GetAllPractice();
            //assert
            _unitOfWorkMock.Verify(x => x.PracticeRepository.ToPagination(0, 10), Times.Once());
        }

        [Fact]
        public async Task CreatePractice_ShouldReturnCorrectData_WhenSuccessSaved()
        {
            //arrange
            var mocks = _fixture.Build<CreatePracticeViewModel>()
                                .Create();
            _unitOfWorkMock.Setup(x => x.PracticeRepository.AddAsync(It.IsAny<Practice>()))
                           .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            //act
            var result = await _practiceService.CreatePracticeAsync(mocks);
            //assert
            _unitOfWorkMock.Verify(x => x.PracticeRepository.AddAsync(It.IsAny<Practice>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
        }

        [Fact]
        public async Task CreatPractice_ShouldReturnNull_WhenFailedSave()
        {
            //arrange
            var mocks = _fixture.Build<CreatePracticeViewModel>()
                                .Create();
            _unitOfWorkMock.Setup(x => x.PracticeRepository.AddAsync(It.IsAny<Practice>()))
                           .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            //act
            var result = await _practiceService.CreatePracticeAsync(mocks);
            //assert
            _unitOfWorkMock.Verify(x => x.PracticeRepository.AddAsync(It.IsAny<Practice>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
            result.Should().BeNull();
        }
        [Fact]
        public async Task UpdatePractice_ShouldReturnCorrectData_WhenSuccessSaved()
        {
            //arrange
            var PracticeObj = _fixture.Build<Practice>()
                                   .Without(x => x.Unit)
                                   .Without(x => x.UnitId)
                                   .Without(x => x.Status)
                                   .Without(x => x.PracticeQuestions)
                                   .Create();
            var updateDataMock = _fixture.Build<UpdatePracticeViewModel>()
                                         .Create();
            _unitOfWorkMock.Setup(x => x.PracticeRepository.GetByIdAsync(It.IsAny<Guid>()))
                          .ReturnsAsync(PracticeObj);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            //act
            await _practiceService.UpdatePractice(PracticeObj.Id, updateDataMock);
            var result = _mapperConfig.Map<UpdatePracticeViewModel>(PracticeObj);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UpdatePracticeViewModel>();
            result.PracticeName.Should().Be(updateDataMock.PracticeName);
            // add more property ...
            _unitOfWorkMock.Verify(x => x.PracticeRepository.Update(PracticeObj), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdatePractice_ShouldReturnNull_WhenNotFoundPractice()
        {
            //arrange
            var PracticeObj = _fixture.Build<Practice>()
                                   .Without(x => x.Unit)
                                   .Without(x => x.UnitId)
                                   .Without(x => x.Status)
                                   .Without(x => x.PracticeQuestions)
                                   .Create();
            _unitOfWorkMock.Setup(x => x.PracticeRepository.GetByIdAsync(It.IsAny<Guid>()))
                           .ReturnsAsync(PracticeObj);
            var updateDataMock = _fixture.Build<UpdatePracticeViewModel>()
                                         .Create();
            //act
            var result = await _practiceService.UpdatePractice(PracticeObj.Id, updateDataMock);
            //assert
            result.Should().BeNull();
        }
        [Fact]
        public async Task UpdatePractice_ShouldReturnNull_WhenFailedSaveChange()
        {
            //arrange
            var PracticeObj = _fixture.Build<Practice>()
                                   .Without(x => x.Unit)
                                   .Without(x => x.UnitId)
                                   .Without(x => x.Status)
                                   .Without(x => x.PracticeQuestions)
                                   .Create();
            _unitOfWorkMock.Setup(x => x.PracticeRepository.GetByIdAsync(It.IsAny<Guid>()))
                           .ReturnsAsync(PracticeObj);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            var updateDataMock = _fixture.Build<UpdatePracticeViewModel>()
                                         .Create();
            //act
            var result = await _practiceService.UpdatePractice(PracticeObj.Id, updateDataMock);
            //assert
            result.Should().BeNull();
        }
        [Fact]
        public async Task GetPracticeByUnitId_ShouldReturnCorrectData()
        {
            //arrange
            var id = Guid.NewGuid();
            var practiceMock = new Pagination<Practice>()
            {
                Items = _fixture.Build<Practice>()
                                .Without(x => x.Unit)
                                .Without(x => x.PracticeQuestions)
                                .With(x => x.UnitId, id)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };

            var practice = _mapperConfig.Map<Pagination<Practice>>(practiceMock);
            _unitOfWorkMock.Setup(x => x.PracticeRepository.GetPracticeByUnitId(id, 0, 10)).ReturnsAsync(practiceMock);
            var expected = _mapperConfig.Map<Pagination<PracticeViewModel>>(practice);
            //act
            var result = await _practiceService.GetPracticeByUnitId(id);
            //assert
            _unitOfWorkMock.Verify(x => x.PracticeRepository.GetPracticeByUnitId(id, 0, 10), Times.Once());
        }
        [Fact]
        public async Task GetEnablePractice_ShouldReturnCorrectData()
        {
            //arrange
            var MockData = new Pagination<Practice>
            {
                Items = _fixture.Build<Practice>()
                                .Without(x => x.Unit)
                                .Without(x => x.UnitId)
                                .Without(x => x.Status)
                                .Without(x => x.PracticeQuestions)
                                .With(x => x.Status, Domain.Enum.StatusEnum.Status.Enable)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };
            var practice = _mapperConfig.Map<Pagination<Practice>>(MockData);
            _unitOfWorkMock.Setup(x => x.PracticeRepository.GetEnablePractices(0, 10)).ReturnsAsync(MockData);
            var expected = _mapperConfig.Map<Pagination<PracticeViewModel>>(practice);
            //act
            var result = await _practiceService.GetEnablePractice();
            //assert
            _unitOfWorkMock.Verify(x => x.PracticeRepository.GetEnablePractices(0, 10), Times.Once());
        }
        [Fact]
        public async Task GetDisableUnits_ShouldReturnCorrectData()
        {
            //arrange
            var MockData = new Pagination<Practice>
            {
                Items = _fixture.Build<Practice>()
                                 .Without(x => x.Unit)
                                .Without(x => x.UnitId)
                                .Without(x => x.Status)
                                .Without(x => x.PracticeQuestions)
                                .With(x => x.Status, Domain.Enum.StatusEnum.Status.Disable)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };
            var practice = _mapperConfig.Map<Pagination<Practice>>(MockData);
            _unitOfWorkMock.Setup(x => x.PracticeRepository.GetDisablePractices(0, 10)).ReturnsAsync(MockData);
            var expected = _mapperConfig.Map<Pagination<PracticeViewModel>>(practice);
            //act
            var result = await _practiceService.GetDisablePractice();
            //assert
            _unitOfWorkMock.Verify(x => x.PracticeRepository.GetDisablePractices(0, 10), Times.Once());
        }
    }
}

