using Applications.Commons;
using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.LectureViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Moq;



namespace Applications.Tests.Services.LectureServices
{
    public class LectureServiceTests : SetupTest
    {
        private readonly ILectureService _lectureService;
        public LectureServiceTests()
        {
            _lectureService = new LectureServies(_unitOfWorkMock.Object, _mapperConfig);
        }
        [Fact]
        public async Task GetAllLectures_ShouldReturnCorrectData()
        {
            //arrange
            var mockData = new Pagination<Lecture>
            {
                Items = _fixture.Build<Lecture>().Without(x => x.Unit).CreateMany(100).ToList(),
                PageIndex = 0,
                PageSize = 100,
                TotalItemsCount = 100
            };
            var expectedResult = _mapperConfig.Map<Pagination<Lecture>>(mockData);

            _unitOfWorkMock.Setup(x => x.LectureRepository.ToPagination(0, 10)).ReturnsAsync(mockData);

            //act
            var result = await _lectureService.GetAllLectures();

            //assert
            _unitOfWorkMock.Verify(x => x.LectureRepository.ToPagination(0, 10), Times.Once());
        }

        [Fact]
        public async Task CreateLecture_ShouldReturnCorrentData_WhenSuccessSaved()
        {
            //arrange
            var mockData = _fixture.Build<CreateLectureViewModel>().Create();

            _unitOfWorkMock.Setup(x => x.LectureRepository.AddAsync(It.IsAny<Lecture>())).Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            //act
            var result = await _lectureService.CreateLecture(mockData);

            //assert
            _unitOfWorkMock.Verify(
                x => x.LectureRepository.AddAsync(It.IsAny<Lecture>()), Times.Once());

            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
        }

        [Fact]
        public async Task CreateLecture_ShouldReturnNull_WhenFailedSave()
        {
            //arrange
            var mockData = _fixture.Build<CreateLectureViewModel>().Create();

            _unitOfWorkMock.Setup(x => x.LectureRepository.AddAsync(It.IsAny<Lecture>())).Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            //act
            var result = await _lectureService.CreateLecture(mockData);

            //assert
            _unitOfWorkMock.Verify(
                x => x.LectureRepository.AddAsync(It.IsAny<Lecture>()), Times.Once());

            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());

            result.Should().BeNull();
        }
        [Fact]
        public async Task UpdateLecture_ShouldReturnCorrectData_WhenSuccessSaved()
        {
            //arrange
            var lectureObj = _fixture.Build<Lecture>()
                                   .Without(x => x.Unit)
                                   .Create();
            _unitOfWorkMock.Setup(x => x.LectureRepository.GetByIdAsync(lectureObj.Id)).ReturnsAsync(lectureObj);
            var updateDataMock = _fixture.Build<UpdateLectureViewModel>().Create();

            //act
            await _lectureService.UpdateLecture(lectureObj.Id, updateDataMock);
            var result = _mapperConfig.Map<UpdateLectureViewModel>(lectureObj);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UpdateLectureViewModel>();
            result.LectureName.Should().Be(updateDataMock.LectureName);
            // add more property ...
            _unitOfWorkMock.Verify(x => x.LectureRepository.Update(lectureObj), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once);
        }
        [Fact]
        public async Task UpdateLecture_ShouldReturnNull_WhenFailedSave()
        {
            //arrange
            var lectureId = Guid.NewGuid();
            _unitOfWorkMock.Setup(x => x.LectureRepository.GetByIdAsync(lectureId)).ReturnsAsync(null as Lecture);
            var updateDataMock = _fixture.Build<UpdateLectureViewModel>().Create();

            //act
            var result = await _lectureService.UpdateLecture(lectureId, updateDataMock);

            //assert
            result.Should().BeNull();
            _unitOfWorkMock.Verify(x => x.LectureRepository.Update(It.IsAny<Lecture>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Never);
        }
    }
}

