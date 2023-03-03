using Application.ViewModels.TrainingProgramModels;
using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.TrainingProgramModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Moq;

namespace Applications.Tests.Services.TrainingProgramServices
{
    public class TrainingProgramTest : SetupTest
    {
        private readonly ITrainingProgramService _trainingProgramService;
        public TrainingProgramTest()
        {
            _trainingProgramService = new Applications.Services.TrainingProgramService(_unitOfWorkMock.Object, _mapperConfig);
        }

        [Fact]
        public async Task ViewAllTrainingProgramAsync_ShouldReturnCorrectData()
        {
            //arrange
            var mockData = new Pagination<TrainingProgram>
            {
                Items = _fixture.Build<TrainingProgram>()
                .Without(x => x.ClassTrainingPrograms)
                .Without(x => x.TrainingProgramSyllabi)
                .CreateMany(100)
                .ToList(),
                PageIndex = 0,
                PageSize = 100,
                TotalItemsCount = 100
            };
            var expectedResult = _mapperConfig.Map<Pagination<TrainingProgram>>(mockData);

            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.ToPagination(0, 10)).ReturnsAsync(mockData);

            //act
            var result = await _trainingProgramService.ViewAllTrainingProgramAsync();

            //assert
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepository.ToPagination(0, 10), Times.Once());
        }
        [Fact]
        public async Task CreateTrainingProgramAsync_ShouldReturnNull_WhenFailedSave()
        {
            //arrange
            var mockData = _fixture.Build<CreateTrainingProgramViewModel>()
                                .Create();
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.AddAsync(It.IsAny<TrainingProgram>()))
                           .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            //act
            var result = await _trainingProgramService.CreateTrainingProgramAsync(mockData);
            //assert
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepository.AddAsync(It.IsAny<TrainingProgram>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
            result.Should().BeNull();
        }
        [Fact]
        public async Task CreateTrainingProgramAsync_ShouldReturnCorrentData_WhenSuccessSaved()
        {
            //arrange
            var mockData = _fixture.Build<CreateTrainingProgramViewModel>().Create();

            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.AddAsync(It.IsAny<TrainingProgram>())).Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            //act
            var result = await _trainingProgramService.CreateTrainingProgramAsync(mockData);

            //assert
            _unitOfWorkMock.Verify(
                x => x.TrainingProgramRepository.AddAsync(It.IsAny<TrainingProgram>()), Times.Once());

            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
        }

        [Fact]
        public async Task UpdateTrainingProgramAsync_ShouldReturnCorrectData_WhenSuccessSaved()
        {
            //arrange
            var trainingProgramObj = _fixture.Build<TrainingProgram>()
                                   .Without(x => x.TrainingProgramSyllabi)
                                   .Without(x => x.ClassTrainingPrograms)
                                   .Create();
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetByIdAsync(trainingProgramObj.Id)).ReturnsAsync(trainingProgramObj);
            var updateDataMock = _fixture.Build<UpdateTrainingProgramViewModel>().Create();

            //act
            await _trainingProgramService.UpdateTrainingProgramAsync(trainingProgramObj.Id, updateDataMock);
            var result = _mapperConfig.Map<UpdateTrainingProgramViewModel>(trainingProgramObj);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UpdateTrainingProgramViewModel>();
            result.TrainingProgramName.Should().Be(updateDataMock.TrainingProgramName);
            // add more property ...
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepository.Update(trainingProgramObj), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once);
        }
        [Fact]
        public async Task UpdateTrainingProgramAsync_ShouldReturnNull_WhenFailedSave()
        {
            //arrange
            var TrainingProgramId = Guid.NewGuid();
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetByIdAsync(TrainingProgramId)).ReturnsAsync(null as TrainingProgram);
            var updateDataMock = _fixture.Build<UpdateTrainingProgramViewModel>().Create();

            //act
            var result = await _trainingProgramService.UpdateTrainingProgramAsync(TrainingProgramId, updateDataMock);

            //assert
            result.Should().BeNull();
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepository.Update(It.IsAny<TrainingProgram>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Never);
        }

        [Fact]
        public async Task ViewTrainingProgramEnableAsync_ShouldReturnCorrectData()
        {
            //arrange
            var mockData = new Pagination<TrainingProgram>
            {
                Items = _fixture.Build<TrainingProgram>()
                .Without(x => x.ClassTrainingPrograms)
                .Without(x => x.TrainingProgramSyllabi)
                .CreateMany(100)
                .ToList(),
                PageIndex = 0,
                PageSize = 100,
                TotalItemsCount = 100
            };
            var trainingprograms = _mapperConfig.Map<Pagination<TrainingProgram>>(mockData);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetTrainingProgramEnable(0, 10)).ReturnsAsync(mockData);
            var expected = _mapperConfig.Map<Pagination<TrainingProgramViewModel>>(trainingprograms);
            //act
            var result = await _trainingProgramService.ViewTrainingProgramEnableAsync();
            //assert
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepository.GetTrainingProgramEnable(0, 10), Times.Once());
        }

        [Fact]
        public async Task ViewTrainingProgramDisableAsync_ShouldReturnCorrectData()
        {
            //arrange
            var mockData = new Pagination<TrainingProgram>
            {
                Items = _fixture.Build<TrainingProgram>()
                .Without(x => x.ClassTrainingPrograms)
                .Without(x => x.TrainingProgramSyllabi)
                .CreateMany(100)
                .ToList(),
                PageIndex = 0,
                PageSize = 100,
                TotalItemsCount = 100
            };
            var trainingprograms = _mapperConfig.Map<Pagination<TrainingProgram>>(mockData);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetTrainingProgramDisable(0, 10)).ReturnsAsync(mockData);
            var expected = _mapperConfig.Map<Pagination<TrainingProgramViewModel>>(trainingprograms);
            //act
            var result = await _trainingProgramService.ViewTrainingProgramDisableAsync();
            //assert
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepository.GetTrainingProgramDisable(0, 10), Times.Once());
        }

        [Fact]
        public async Task GetTrainingProgramById_ShouldReturnCorrectData()
        {
            //arrange
            var trainingProgramObj = _fixture.Build<TrainingProgram>()
                                   .Without(x => x.TrainingProgramSyllabi)
                                   .Without(x => x.ClassTrainingPrograms)
                                   .Create();
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetByIdAsync(trainingProgramObj.Id))
                           .ReturnsAsync(trainingProgramObj);
            //act
            var result = await _trainingProgramService.GetTrainingProgramById(trainingProgramObj.Id);
            //assert
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepository.GetByIdAsync(trainingProgramObj.Id), Times.Once());
        }

        [Fact]
        public async Task GetTrainingProgramByClassId_ShouldReturnCorrectData()
        {
            //arrange
            var classId = Guid.NewGuid();
            var mockData = new Pagination<TrainingProgram>
            {
                Items = _fixture.Build<TrainingProgram>()
                .Without(x => x.ClassTrainingPrograms)
                .Without(x => x.TrainingProgramSyllabi)
                .With(x => x.Id, classId)
                .CreateMany(100)
                .ToList(),
                PageIndex = 0,
                PageSize = 100,
                TotalItemsCount = 100
            };
            var trainingprograms = _mapperConfig.Map<Pagination<TrainingProgram>>(mockData);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetTrainingProgramByClassId(classId, 0, 10)).ReturnsAsync(mockData);
            var expected = _mapperConfig.Map<Pagination<TrainingProgramViewModel>>(trainingprograms);
            //act
            var result = await _trainingProgramService.GetTrainingProgramByClassId(classId);
            //assert
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepository.GetTrainingProgramByClassId(classId, 0, 10), Times.Once());
        }
    }
}
