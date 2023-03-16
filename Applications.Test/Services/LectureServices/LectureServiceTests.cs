using Applications.Commons;
using Applications.Interfaces;
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
            _lectureService = new Applications.Services.LectureServices(_unitOfWorkMock.Object, _mapperConfig);
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
            var user = _fixture.Build<User>()
                               .Without(x => x.UserAuditPlans)
                               .Without(x => x.AbsentRequests)
                               .Without(x => x.ClassUsers)
                               .Without(x => x.Attendences)
                               .CreateMany(3)
                               .ToList();
            var expected = _mapperConfig.Map<Pagination<LectureViewModel>>(mockData);
            _unitOfWorkMock.Setup(x => x.LectureRepository.ToPagination(0, 10)).ReturnsAsync(mockData);
            _unitOfWorkMock.Setup(x => x.UserRepository.GetEntitiesByIdsAsync(It.IsAny<List<Guid?>>())).ReturnsAsync(user);
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
            var updateDataMock = _fixture.Build<UpdateLectureViewModel>()
                                         .Create();
            _unitOfWorkMock.Setup(x => x.LectureRepository.GetByIdAsync(It.IsAny<Guid>()))
                          .ReturnsAsync(lectureObj);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
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
        public async Task UpdateLecture_ShouldReturnNull_WhenNotFoundLecture()
        {
            //arrange
            var lectureObj = _fixture.Build<Lecture>()
                                   .Without(x => x.Unit)
                                   .Create();
            _unitOfWorkMock.Setup(x => x.LectureRepository.GetByIdAsync(It.IsAny<Guid>()))
                           .ReturnsAsync(lectureObj);
            var updateDataMock = _fixture.Build<UpdateLectureViewModel>()
                                         .Create();
            //act
            var result = await _lectureService.UpdateLecture(lectureObj.Id, updateDataMock);
            //assert
            result.Should().BeNull();
        }
        [Fact]
        public async Task UpdateLecture_ShouldReturnNull_WhenFailedSaveChange()
        {
            //arrange
            var lectureObj = _fixture.Build<Lecture>()
                                   .Without(x => x.Unit)
                                   .Create();
            _unitOfWorkMock.Setup(x => x.LectureRepository.GetByIdAsync(It.IsAny<Guid>()))
                           .ReturnsAsync(lectureObj);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            var updateDataMock = _fixture.Build<UpdateLectureViewModel>()
                                         .Create();
            //act
            var result = await _lectureService.UpdateLecture(lectureObj.Id, updateDataMock);
            //assert
            result.Should().BeNull();
        }
        [Fact]
        public async Task GetEnableLectures_ShoulReturnCorrectData()
        {
            //arrange
            var lectureMockData = new Pagination<Lecture>
            {
                Items = _fixture.Build<Lecture>()
                                .Without(x => x.Unit)
                                .With(x => x.Status, Domain.Enum.StatusEnum.Status.Enable)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30
            };
            var lectures = _mapperConfig.Map<Pagination<Lecture>>(lectureMockData);
            _unitOfWorkMock.Setup(x => x.LectureRepository.GetEnableLectures(0, 10)).ReturnsAsync(lectureMockData);
            var expected = _mapperConfig.Map<Pagination<LectureViewModel>>(lectures);
            //act
            var result = await _lectureService.GetEnableLectures();
            //assert
            _unitOfWorkMock.Verify(x => x.LectureRepository.GetEnableLectures(0, 10), Times.Once());
        }
        [Fact]
        public async Task GetDisableLectures_ShoulReturnCorrectData()
        {
            //arrange
            var lectureMockData = new Pagination<Lecture>
            {
                Items = _fixture.Build<Lecture>()
                                .Without(x => x.Unit)
                                .With(x => x.Status, Domain.Enum.StatusEnum.Status.Disable)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30
            };
            var lectures = _mapperConfig.Map<Pagination<Lecture>>(lectureMockData);
            _unitOfWorkMock.Setup(x => x.LectureRepository.GetDisableLectures(0, 10)).ReturnsAsync(lectureMockData);
            var expected = _mapperConfig.Map<Pagination<LectureViewModel>>(lectures);
            //act
            var result = await _lectureService.GetDisableLectures();
            //assert
            _unitOfWorkMock.Verify(x => x.LectureRepository.GetDisableLectures(0, 10), Times.Once());
        }
        [Fact]
        public async Task GetLectureByName_ShouldReturnCorrectData()
        {
            //arrange
            var MockData = new Pagination<Lecture>
            {
                Items = _fixture.Build<Lecture>()
                                .Without(x => x.Unit)
                                .With(x => x.LectureName, "Mock")
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };
            var lectures = _mapperConfig.Map<Pagination<Lecture>>(MockData);
            _unitOfWorkMock.Setup(x => x.LectureRepository.GetLectureByName("Mock", 0, 10)).ReturnsAsync(MockData);
            //act
            var result = await _lectureService.GetLectureByName("Mock");
            //assert
            _unitOfWorkMock.Verify(x => x.LectureRepository.GetLectureByName("Mock", 0, 10), Times.Once());
        }
        [Fact]
        public async Task GetLectureById_ShouldReturnCorrectData()
        {
            //arrange
            var lectures = _fixture.Build<Lecture>()
                                        .Without(x => x.Unit)
                                        .Create();
            var expected = _mapperConfig.Map<LectureViewModel>(lectures);
            var createBy = new User { Email = "mock@example.com" };
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(lectures.CreatedBy))
                           .ReturnsAsync(createBy);
            expected.CreatedBy = createBy.Email;
            _unitOfWorkMock.Setup(x => x.LectureRepository.GetByIdAsync(lectures.Id))
                           .ReturnsAsync(lectures);
            //act
            var result = await _lectureService.GetLectureById(lectures.Id);
            //assert
            _unitOfWorkMock.Verify(x => x.LectureRepository.GetByIdAsync(lectures.Id), Times.Once());
        }
        [Fact]
        public async Task GetLecutreByUnitId_ShouldReturnCorrectData()
        {
            //arrange
            var id = Guid.NewGuid();
            var lectureMockData = new Pagination<Lecture>
            {
                Items = _fixture.Build<Lecture>()
                                .Without(x => x.Unit)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };
            var lecture = _mapperConfig.Map<Pagination<Lecture>>(lectureMockData);
            _unitOfWorkMock.Setup(x => x.LectureRepository.GetLectureByUnitId(id, 0, 10)).ReturnsAsync(lectureMockData);
            var expected = _mapperConfig.Map<Pagination<LectureViewModel>>(lecture);
            //act
            var result = await _lectureService.GetLectureByUnitId(id);
            //assert
            _unitOfWorkMock.Verify(x => x.LectureRepository.GetLectureByUnitId(id, 0, 10), Times.Once());
        }
    }
}

