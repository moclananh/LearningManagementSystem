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
            _unitOfWorkMock.Setup(x => x.PracticeRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(practiceMocks);
            var expected = _mapperConfig.Map<PracticeViewModel>(practiceMocks);
            //act
            var result = _practiceService.GetPracticeById(practiceMocks.Id);
            //assert
            result.Result.Should().BeEquivalentTo(expected);
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
            var expected = _mapperConfig.Map<Pagination<Practice>>(MockData);
            _unitOfWorkMock.Setup(x => x.PracticeRepository.ToPagination(0, 10)).ReturnsAsync(MockData);
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
    }
}
