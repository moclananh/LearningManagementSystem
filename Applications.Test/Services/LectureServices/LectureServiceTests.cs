using Applications.Commons;
using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.LectureViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Moq;
using Org.BouncyCastle.Asn1.Pkcs;


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
        public async Task CreateChemicalAsync_ShouldReturnNull_WhenFailedSave()
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



    }
}

