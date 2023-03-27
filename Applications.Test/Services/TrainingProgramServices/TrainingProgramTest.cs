using Application.ViewModels.TrainingProgramModels;
using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.TrainingProgramModels;
using Applications.ViewModels.TrainingProgramSyllabi;
using AutoFixture;
using Domain.Entities;
using Domain.EntityRelationship;
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
            var expected = _mapperConfig.Map<Pagination<TrainingProgramViewModel>>(mockData);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.ToPagination(0, 10)).ReturnsAsync(mockData);
            _unitOfWorkMock.Setup(x => x.UserRepository.GetEntitiesByIdsAsync(It.IsAny<List<Guid?>>())).ReturnsAsync(user);
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
            var expected = _mapperConfig.Map<TrainingProgramViewModel>(trainingProgramObj);
            var createBy = new User { Email = "mock@example.com" };
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(trainingProgramObj.CreatedBy)).ReturnsAsync(createBy);
            expected.CreatedBy = createBy.Email;
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
        [Fact]
        public async Task AddSyllabusToTrainingProgram_ShouldReturnCorrectData()
        {
            //arrange
            var syllabusMock = _fixture.Build<Syllabus>()
                               .Without(s => s.TrainingProgramSyllabi)
                               .Without(s => s.SyllabusOutputStandards)
                               .Without(s => s.SyllabusModules)
                               .Create();
            var trainingProgramMocks = _fixture.Build<TrainingProgram>()
                                               .Without(x => x.ClassTrainingPrograms)
                                               .Without(x => x.TrainingProgramSyllabi)
                                               .Create();
            var trainingProgramSyllabi = new TrainingProgramSyllabus()
            {
                TrainingProgram = trainingProgramMocks,
                Syllabus = syllabusMock
            };
            _unitOfWorkMock.Setup(x => x.SyllabusRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(syllabusMock);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(trainingProgramMocks);
            _unitOfWorkMock.Setup(x => x.TrainingProgramSyllabiRepository.AddAsync(It.IsAny<TrainingProgramSyllabus>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            var expected = _mapperConfig.Map<CreateTrainingProgramSyllabi>(trainingProgramSyllabi);
            //act
            var result = await _trainingProgramService.AddSyllabusToTrainingProgram(trainingProgramMocks.Id, syllabusMock.Id);
            //assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task AddSyllabusToTrainingProgram_ShouldReturnNull_WhenNotFoundClass()
        {
            //arrange
            var syllabusMock = _fixture.Build<Syllabus>()
                               .Without(s => s.TrainingProgramSyllabi)
                               .Without(s => s.SyllabusOutputStandards)
                               .Without(s => s.SyllabusModules)
                               .Create();
            var trainingProgramMocks = _fixture.Build<TrainingProgram>()
                                               .Without(x => x.ClassTrainingPrograms)
                                               .Without(x => x.TrainingProgramSyllabi)
                                               .Create();
            var trainingProgramSyllabi = new TrainingProgramSyllabus()
            {
                TrainingProgram = trainingProgramMocks,
                Syllabus = syllabusMock
            };
            _unitOfWorkMock.Setup(x => x.SyllabusRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(syllabusMock);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(trainingProgramMocks);
            _unitOfWorkMock.Setup(x => x.TrainingProgramSyllabiRepository.AddAsync(It.IsAny<TrainingProgramSyllabus>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            var expected = _mapperConfig.Map<CreateTrainingProgramSyllabi>(trainingProgramSyllabi);
            //act
            var result = await _trainingProgramService.AddSyllabusToTrainingProgram(trainingProgramMocks.Id, syllabusMock.Id);
            //assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task RemoveTrainingProgramToClass_ShouldReturnCorrectData()
        {
            //arrange
            var syllabusMock = _fixture.Build<Syllabus>()
                               .Without(s => s.TrainingProgramSyllabi)
                               .Without(s => s.SyllabusOutputStandards)
                               .Without(s => s.SyllabusModules)
                               .Create();
            var trainingProgramMocks = _fixture.Build<TrainingProgram>()
                                               .Without(x => x.ClassTrainingPrograms)
                                               .Without(x => x.TrainingProgramSyllabi)
                                               .Create();
            var trainingProgramSyllabi = new TrainingProgramSyllabus()
            {
                TrainingProgramId = trainingProgramMocks.Id,
                SyllabusId = trainingProgramMocks.Id,
                TrainingProgram = trainingProgramMocks,
                Syllabus = syllabusMock
            };
            _unitOfWorkMock.Setup(x => x.TrainingProgramSyllabiRepository.GetTrainingProgramSyllabus(syllabusMock.Id, trainingProgramMocks.Id)).ReturnsAsync(trainingProgramSyllabi);
            _unitOfWorkMock.Setup(x => x.TrainingProgramSyllabiRepository.SoftRemove(It.IsAny<TrainingProgramSyllabus>()));
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            var expected = _mapperConfig.Map<CreateTrainingProgramSyllabi>(trainingProgramSyllabi);
            //act
            var result = await _trainingProgramService.RemoveSyllabusToTrainingProgram(syllabusMock.Id, trainingProgramMocks.Id);
            //assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task RemoveTrainingProgramToClass_ShouldReturnNull_WhenNotFoundClass()
        {
            //arrange
            var syllabusMock = _fixture.Build<Syllabus>()
                               .Without(s => s.TrainingProgramSyllabi)
                               .Without(s => s.SyllabusOutputStandards)
                               .Without(s => s.SyllabusModules)
                               .Create();
            var trainingProgramMocks = _fixture.Build<TrainingProgram>()
                                               .Without(x => x.ClassTrainingPrograms)
                                               .Without(x => x.TrainingProgramSyllabi)
                                               .Create();
            var trainingProgramSyllabi = new TrainingProgramSyllabus()
            {
                TrainingProgramId = trainingProgramMocks.Id,
                SyllabusId = trainingProgramMocks.Id,
                TrainingProgram = trainingProgramMocks,
                Syllabus = syllabusMock
            };
            _unitOfWorkMock.Setup(x => x.TrainingProgramSyllabiRepository.GetTrainingProgramSyllabus(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(() => null);
            _unitOfWorkMock.Setup(x => x.TrainingProgramSyllabiRepository.SoftRemove(It.IsAny<TrainingProgramSyllabus>()));
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            var expected = _mapperConfig.Map<CreateTrainingProgramSyllabi>(trainingProgramSyllabi);
            //act
            var result = await _trainingProgramService.RemoveSyllabusToTrainingProgram(syllabusMock.Id, trainingProgramMocks.Id);
            //assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task RemoveTrainingProgramToClass_ShouldReturnNull_WhenSavedFail()
        {
            //arrange
            var syllabusMock = _fixture.Build<Syllabus>()
                               .Without(s => s.TrainingProgramSyllabi)
                               .Without(s => s.SyllabusOutputStandards)
                               .Without(s => s.SyllabusModules)
                               .Create();
            var trainingProgramMocks = _fixture.Build<TrainingProgram>()
                                               .Without(x => x.ClassTrainingPrograms)
                                               .Without(x => x.TrainingProgramSyllabi)
                                               .Create();
            var trainingProgramSyllabi = new TrainingProgramSyllabus()
            {
                TrainingProgramId = trainingProgramMocks.Id,
                SyllabusId = trainingProgramMocks.Id,
                TrainingProgram = trainingProgramMocks,
                Syllabus = syllabusMock
            };
            _unitOfWorkMock.Setup(x => x.TrainingProgramSyllabiRepository.GetTrainingProgramSyllabus(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(trainingProgramSyllabi);
            _unitOfWorkMock.Setup(x => x.TrainingProgramSyllabiRepository.SoftRemove(It.IsAny<TrainingProgramSyllabus>()));
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            var expected = _mapperConfig.Map<CreateTrainingProgramSyllabi>(trainingProgramSyllabi);
            //act
            var result = await _trainingProgramService.RemoveSyllabusToTrainingProgram(syllabusMock.Id, trainingProgramMocks.Id);
            //assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task RemoveTrainingProgramToClass_ShouldReturnNull_WhenSaveChangedFailed()
        {
            //arrange
            var syllabusMock = _fixture.Build<Syllabus>()
                               .Without(s => s.TrainingProgramSyllabi)
                               .Without(s => s.SyllabusOutputStandards)
                               .Without(s => s.SyllabusModules)
                               .Create();
            var trainingProgramMocks = _fixture.Build<TrainingProgram>()
                                               .Without(x => x.ClassTrainingPrograms)
                                               .Without(x => x.TrainingProgramSyllabi)
                                               .Create();
            var trainingProgramSyllabi = new TrainingProgramSyllabus()
            {
                TrainingProgramId = trainingProgramMocks.Id,
                SyllabusId = trainingProgramMocks.Id,
                TrainingProgram = trainingProgramMocks,
                Syllabus = syllabusMock
            };
            _unitOfWorkMock.Setup(x => x.TrainingProgramSyllabiRepository.GetTrainingProgramSyllabus(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(() => null);
            _unitOfWorkMock.Setup(x => x.TrainingProgramSyllabiRepository.SoftRemove(It.IsAny<TrainingProgramSyllabus>()));
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            var expected = _mapperConfig.Map<CreateTrainingProgramSyllabi>(trainingProgramSyllabi);
            //act
            var result = await _trainingProgramService.RemoveSyllabusToTrainingProgram(syllabusMock.Id, trainingProgramMocks.Id);
            //assert
            result.Should().BeNull();
        }
        [Fact]
        public async Task GetTrainingProgramByName_ShouldReturnCorrectData()
        {
            //arrange
            var trainingProgramMockData = new Pagination<TrainingProgram>
            {
                Items = _fixture.Build<TrainingProgram>()
                                .Without(s => s.ClassTrainingPrograms)
                                .Without(s => s.TrainingProgramSyllabi)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };
            var user = _fixture.Build<User>()
                               .Without(s => s.UserAuditPlans)
                               .Without(s => s.AbsentRequests)
                               .Without(s => s.ClassUsers)
                               .Without(s => s.Attendences)
                               .CreateMany(3)
                               .ToList();
            var expected = _mapperConfig.Map<Pagination<TrainingProgramViewModel>>(trainingProgramMockData);
            _unitOfWorkMock.Setup(s => s.TrainingProgramRepository.GetTrainingProgramByName("abc", 0, 10)).ReturnsAsync(trainingProgramMockData);
            _unitOfWorkMock.Setup(s => s.UserRepository.GetEntitiesByIdsAsync(It.IsAny<List<Guid?>>())).ReturnsAsync(user);
            //act
            var result = await _trainingProgramService.GetByName("abc", 0, 10);
            //assert
            _unitOfWorkMock.Verify(s => s.TrainingProgramRepository.GetTrainingProgramByName("abc", 0, 10), Times.Once());
        }

        [Fact]
        public async Task GetTrainingProgramDetails_ShouldReturnCorrectData()
        {
            //arrange
            var mocks = _fixture.Build<TrainingProgram>()
                                 .Without(s => s.ClassTrainingPrograms)
                                 .Without(s => s.TrainingProgramSyllabi)
                                 .Create();

            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetTrainingProgramDetails(It.IsAny<Guid>())).ReturnsAsync(mocks);
            var expected = _mapperConfig.Map<TrainingProgramViewModel>(mocks);
            var createBy = new User { Email = "mock@example.com" };
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(mocks.CreatedBy)).ReturnsAsync(createBy);
            expected.CreatedBy = createBy.Email;
            //act
            var result = await _trainingProgramService.GetTrainingProgramDetails(mocks.Id);
            //assert
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepository.GetTrainingProgramDetails(mocks.Id), Times.Once());
        }

        [Fact]
        public async Task UpdateStatusOnlyOfTrainingProgram_ShouldReturnCorrectData()
        {
            // Arrange
            var trainingProgramId = Guid.NewGuid();
            var updateStatusOnlyOfTrainingProgram = new UpdateStatusOnlyOfTrainingProgram { Status = Domain.Enum.StatusEnum.Status.Enable };

            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetByIdAsync(trainingProgramId))
                           .ReturnsAsync(null as TrainingProgram);

            // Act
            var result = await _trainingProgramService.UpdateStatusOnlyOfTrainingProgram(trainingProgramId, updateStatusOnlyOfTrainingProgram);

            // Assert
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepository.Update(It.IsAny<TrainingProgram>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Never);
        }
    }
}
