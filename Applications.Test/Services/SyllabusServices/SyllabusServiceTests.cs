using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.SyllabusModuleViewModel;
using Applications.ViewModels.SyllabusViewModels;
using AutoFixture;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Tests;
using FluentAssertions;
using Moq;
using System.Net;

namespace Applications.Tests.Services.SyllabusServices
{
    public class SyllabusServicesTests : SetupTest
    {
        private readonly ISyllabusServices _syllabusService;
        public SyllabusServicesTests()
        {
            _syllabusService = new Applications.Services.SyllabusServices(_unitOfWorkMock.Object, _mapperConfig, _claimServiceMock.Object);
        }

        [Fact]
        public async Task GetAllSyllabus_ShouldReturnCorrectData()
        {
            //arrange
            var syllabusMockData = new Pagination<Syllabus>
            {
                Items = _fixture.Build<Syllabus>()
                                .Without(s => s.SyllabusModules)
                                .Without(s => s.SyllabusOutputStandards)
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
            var expected = _mapperConfig.Map<Pagination<SyllabusViewModel>>(syllabusMockData);
            _unitOfWorkMock.Setup(s => s.SyllabusRepository.ToPagination(0, 10)).ReturnsAsync(syllabusMockData);
            _unitOfWorkMock.Setup(s => s.UserRepository.GetEntitiesByIdsAsync(It.IsAny<List<Guid?>>())).ReturnsAsync(user);

            //act
            var result = await _syllabusService.GetAllSyllabus(0, 10);

            //assert
            _unitOfWorkMock.Verify(s => s.SyllabusRepository.ToPagination(0, 10), Times.Once());
        }

        [Fact]
        public async Task CreateSyllabus_ShouldReturnCorrectData_WhenSuccessSaved()
        {
            //arrange
            var syllabusMockData = _fixture.Build<CreateSyllabusViewModel>()
                                           .Create();
            _unitOfWorkMock.Setup(s => s.SyllabusRepository.AddAsync(It.IsAny<Syllabus>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(s => s.SaveChangeAsync()).ReturnsAsync(1);

            //act
            var result = await _syllabusService.CreateSyllabus(syllabusMockData);

            //assert
            _unitOfWorkMock.Verify(s => s.SyllabusRepository.AddAsync(It.IsAny<Syllabus>()), Times.Once());
            _unitOfWorkMock.Verify(s => s.SaveChangeAsync(), Times.Once());
        }

        [Fact]
        public async Task CreateSyllabus_ShouldReturnNull_WhenFaildSaved()
        {
            //arrange
            var syllabusMockData = _fixture.Build<CreateSyllabusViewModel>()
                                           .Create();
            _unitOfWorkMock.Setup(s => s.SyllabusRepository.AddAsync(It.IsAny<Syllabus>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(s => s.SaveChangeAsync()).ReturnsAsync(0);

            //act
            var result = await _syllabusService.CreateSyllabus(syllabusMockData);

            //assert
            _unitOfWorkMock.Verify(s => s.SyllabusRepository.AddAsync(It.IsAny<Syllabus>()), Times.Once());
            _unitOfWorkMock.Verify(s => s.SaveChangeAsync(), Times.Once());
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateSyllabus_ShouldReturnCorrectData_WhenSuccessSaved()
        {
            var syllabus = _fixture.Build<Syllabus>()
                                   .Without(s => s.SyllabusModules)
                                   .Without(s => s.SyllabusOutputStandards)
                                   .Without(s => s.TrainingProgramSyllabi)
                                   .Create();
            _unitOfWorkMock.Setup(s => s.SyllabusRepository.GetByIdAsync(syllabus.Id)).ReturnsAsync(syllabus);
            //var expected = _mapperConfig.Map<UpdateSyllabusViewModel>(syllabus);
            var syllabusMockData = _fixture.Build<UpdateSyllabusViewModel>()
                                           .Create();
            _unitOfWorkMock.Setup(s => s.SyllabusRepository.Update(It.IsAny<Syllabus>()));
            _unitOfWorkMock.Setup(s => s.SaveChangeAsync());

            //act
            var result = await _syllabusService.UpdateSyllabus(syllabus.Id, syllabusMockData);

            //assert
            _unitOfWorkMock.Verify(s => s.SyllabusRepository.Update(It.IsAny<Syllabus>()), Times.Once());
            _unitOfWorkMock.Verify(s => s.SaveChangeAsync(), Times.Once());
        }

        [Fact]
        public async Task UpdateSyllabus_ShouldReturnCorrectData_WhenFaildSaved()
        {
            var syllabus = _fixture.Build<Syllabus>()
                                   .Without(s => s.SyllabusModules)
                                   .Without(s => s.SyllabusOutputStandards)
                                   .Without(s => s.TrainingProgramSyllabi)
                                   .Create();
            _unitOfWorkMock.Setup(s => s.SyllabusRepository.GetByIdAsync(syllabus.Id)).ReturnsAsync(syllabus);
            //var expected = _mapperConfig.Map<UpdateSyllabusViewModel>(syllabus);
            var syllabusMockData = _fixture.Build<UpdateSyllabusViewModel>()
                                           .Create();
            _unitOfWorkMock.Setup(s => s.SyllabusRepository.Update(It.IsAny<Syllabus>()));
            _unitOfWorkMock.Setup(s => s.SaveChangeAsync());

            //act
            var result = await _syllabusService.UpdateSyllabus(syllabus.Id, syllabusMockData);

            //assert
            _unitOfWorkMock.Verify(s => s.SyllabusRepository.Update(It.IsAny<Syllabus>()), Times.Once());
            _unitOfWorkMock.Verify(s => s.SaveChangeAsync(), Times.Once());
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetEnableSyllabus_ShouldReturnCorrectData()
        {
            //arrange
            var syllabusMockData = new Pagination<Syllabus>
            {
                Items = _fixture.Build<Syllabus>()
                                .Without(s => s.SyllabusModules)
                                .Without(s => s.SyllabusOutputStandards)
                                .Without(s => s.TrainingProgramSyllabi)
                                .With(s => s.Status, Domain.Enum.StatusEnum.Status.Enable)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };
            //var syllabus = _mapperConfig.Map<Pagination<Syllabus>>(syllabusMockData);
            _unitOfWorkMock.Setup(s => s.SyllabusRepository.GetEnableSyllabus(0, 10)).ReturnsAsync(syllabusMockData);
            var expected = _mapperConfig.Map<Pagination<SyllabusViewModel>>(syllabusMockData);

            //act
            var result = await _syllabusService.GetEnableSyllabus(0, 10);

            //assert
            _unitOfWorkMock.Verify(s => s.SyllabusRepository.GetEnableSyllabus(0, 10), Times.Once());
        }

        [Fact]
        public async Task GetDisableSyllabus_ShouldReturnCorrectData()
        {
            //arrange
            var syllabusMockData = new Pagination<Syllabus>
            {
                Items = _fixture.Build<Syllabus>()
                                .Without(s => s.SyllabusModules)
                                .Without(s => s.SyllabusOutputStandards)
                                .Without(s => s.TrainingProgramSyllabi)
                                .With(s => s.Status, Domain.Enum.StatusEnum.Status.Disable)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };
            //var syllabus = _mapperConfig.Map<Pagination<Syllabus>>(syllabusMockData);
            _unitOfWorkMock.Setup(s => s.SyllabusRepository.GetDisableSyllabus(0, 10)).ReturnsAsync(syllabusMockData);
            var expected = _mapperConfig.Map<Pagination<SyllabusViewModel>>(syllabusMockData);

            //act
            var result = await _syllabusService.GetDisableSyllabus(0, 10);

            //assert
            _unitOfWorkMock.Verify(s => s.SyllabusRepository.GetDisableSyllabus(0, 10), Times.Once());
        }

        [Fact]
        public async Task GetSyllabusById_ShouldReturnCorrectData()
        {
            //arrange
            var syllabusMockData = _fixture.Build<Syllabus>()
                                           .Without(s => s.SyllabusModules)
                                           .Without(s => s.SyllabusOutputStandards)
                                           .Without(S => S.TrainingProgramSyllabi)
                                           .Create();
            //var syllabus = _mapperConfig.Map<Syllabus>(syllabusMockData);
            _unitOfWorkMock.Setup(s => s.SyllabusRepository.GetByIdAsync(syllabusMockData.Id)).ReturnsAsync(syllabusMockData);
            var expected = _mapperConfig.Map<SyllabusViewModel>(syllabusMockData);
            var createBy = new User { Email = "mock@example.com" };
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(syllabusMockData.CreatedBy)).ReturnsAsync(createBy);
            expected.CreatedBy = createBy.Email;

            //act
            var result = await _syllabusService.GetSyllabusById(syllabusMockData.Id);

            //assert
            _unitOfWorkMock.Verify(s => s.SyllabusRepository.GetByIdAsync(syllabusMockData.Id), Times.Once());
        }

        [Fact]
        public async Task GetSyllabusByName_ShouldReturnCorrectData()
        {
            //arrange
            var syllabusMockData = new Pagination<Syllabus>
            {
                Items = _fixture.Build<Syllabus>()
                                .Without(s => s.SyllabusModules)
                                .Without(s => s.SyllabusOutputStandards)
                                .Without(s => s.TrainingProgramSyllabi)
                                .With(s => s.SyllabusName, "Mock")
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

            //var syllabus = _mapperConfig.Map<Pagination<Syllabus>>(syllabusMockData);
            _unitOfWorkMock.Setup(s => s.SyllabusRepository.GetSyllabusByName("Mock", 0, 10)).ReturnsAsync(syllabusMockData);
            var expected = _mapperConfig.Map<Pagination<SyllabusViewModel>>(syllabusMockData);
            _unitOfWorkMock.Setup(s => s.UserRepository.GetEntitiesByIdsAsync(It.IsAny<List<Guid?>>())).ReturnsAsync(user);

            //act
            var result = await _syllabusService.GetSyllabusByName("Mock", 0, 10);

            //assert
            _unitOfWorkMock.Verify(s => s.SyllabusRepository.GetSyllabusByName("Mock", 0, 10), Times.Once());
        }

        [Fact]
        public async Task GetSyllabusByOutputStandardId_ShouldReturnCorrectData()
        {
            //arrange
            var outputStandardId = Guid.NewGuid();
            var syllabusMockData = new Pagination<Syllabus>
            {
                Items = _fixture.Build<Syllabus>()
                                .Without(s => s.SyllabusModules)
                                .Without(s => s.SyllabusOutputStandards)
                                .Without(s => s.TrainingProgramSyllabi)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };
            //var sylllabusOutputStandard = _mapperConfig.Map<Syllabus>(syllabusMockData);
            _unitOfWorkMock.Setup(s => s.SyllabusRepository.GetSyllabusByOutputStandardId(outputStandardId, 0, 10)).ReturnsAsync(syllabusMockData);
            var expected = _mapperConfig.Map<Pagination<SyllabusViewModel>>(syllabusMockData);

            //act
            var result = await _syllabusService.GetSyllabusByOutputStandardId(outputStandardId, 0, 10);

            //assert
            _unitOfWorkMock.Verify(s => s.SyllabusRepository.GetSyllabusByOutputStandardId(outputStandardId, 0, 10), Times.Once());
        }

        [Fact]
        public async Task GetSyllabusByTrainingProgramId_ShouldReturnCorrectData()
        {
            //arrange
            var trainingProgramId = Guid.NewGuid();
            var syllabusMockData = new Pagination<Syllabus>
            {
                Items = _fixture.Build<Syllabus>()
                                .Without(s => s.SyllabusModules)
                                .Without(s => s.SyllabusOutputStandards)
                                .Without(s => s.TrainingProgramSyllabi)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };
            //var sylllabusTrainingProgram = _mapperConfig.Map<Syllabus>(syllabusMockData);
            _unitOfWorkMock.Setup(s => s.SyllabusRepository.GetSyllabusByTrainingProgramId(trainingProgramId, 0, 10)).ReturnsAsync(syllabusMockData);
            var expected = _mapperConfig.Map<Pagination<SyllabusViewModel>>(syllabusMockData);

            //act
            var result = await _syllabusService.GetSyllabusByTrainingProgramId(trainingProgramId, 0, 10);

            //assert
            _unitOfWorkMock.Verify(s => s.SyllabusRepository.GetSyllabusByTrainingProgramId(trainingProgramId, 0, 10), Times.Once());
        }

        [Fact]
        public async Task AddModuleToSyllabus_ShouldReturnCorrectData_WhenSuccessSaved()
        {
            //arrange
            var syllabusMockData = _fixture.Build<Syllabus>()
                                           .Without(s => s.SyllabusModules)
                                           .Without(s => s.SyllabusOutputStandards)
                                           .Without(S => S.TrainingProgramSyllabi)
                                           .Create();
            var moduleMockData = _fixture.Build<Module>()
                                         .Without(s => s.ModuleUnits)
                                         .Without(s => s.SyllabusModules)
                                         .Without(s => s.AuditPlan)
                                         .Create();
            var syllabusModuleMockData = new SyllabusModule()
            {
                Syllabus = syllabusMockData,
                Module = moduleMockData,
            };
            _unitOfWorkMock.Setup(s => s.SyllabusRepository.GetByIdAsync(syllabusMockData.Id)).ReturnsAsync(syllabusMockData);
            _unitOfWorkMock.Setup(s => s.ModuleRepository.GetByIdAsync(moduleMockData.Id)).ReturnsAsync(moduleMockData);
            _unitOfWorkMock.Setup(s => s.SyllabusModuleRepository.AddAsync(It.IsAny<SyllabusModule>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(s => s.SaveChangeAsync()).ReturnsAsync(1);
            var expected = _mapperConfig.Map<SyllabusModuleViewModel>(syllabusModuleMockData);

            //act
            var result = await _syllabusService.AddSyllabusModule(syllabusMockData.Id, moduleMockData.Id);

            //assert
            _unitOfWorkMock.Verify(s => s.SyllabusModuleRepository.AddAsync(It.IsAny<SyllabusModule>()), Times.Once());
            _unitOfWorkMock.Verify(s => s.SaveChangeAsync(), Times.Once());
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task AddModuleToSyllabus_ShouldReturnCorrectData_WhenSuccessFailed()
        {
            //arrange
            var syllabusMockData = _fixture.Build<Syllabus>()
                                           .Without(s => s.SyllabusModules)
                                           .Without(s => s.SyllabusOutputStandards)
                                           .Without(S => S.TrainingProgramSyllabi)
                                           .Create();
            var moduleMockData = _fixture.Build<Module>()
                                         .Without(s => s.ModuleUnits)
                                         .Without(s => s.SyllabusModules)
                                         .Without(s => s.AuditPlan)
                                         .Create();
            var syllabusModuleMockData = new SyllabusModule()
            {
                Syllabus = syllabusMockData,
                Module = moduleMockData,
            };
            _unitOfWorkMock.Setup(s => s.SyllabusRepository.GetByIdAsync(syllabusMockData.Id)).ReturnsAsync(syllabusMockData);
            _unitOfWorkMock.Setup(s => s.ModuleRepository.GetByIdAsync(moduleMockData.Id)).ReturnsAsync(moduleMockData);
            _unitOfWorkMock.Setup(s => s.SyllabusModuleRepository.AddAsync(It.IsAny<SyllabusModule>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(s => s.SaveChangeAsync()).ReturnsAsync(0);
            var expected = _mapperConfig.Map<SyllabusModuleViewModel>(syllabusModuleMockData);

            //act
            var result = await _syllabusService.AddSyllabusModule(syllabusMockData.Id, moduleMockData.Id);

            //assert
            _unitOfWorkMock.Verify(s => s.SyllabusModuleRepository.AddAsync(It.IsAny<SyllabusModule>()), Times.Once());
            _unitOfWorkMock.Verify(s => s.SaveChangeAsync(), Times.Once());
            result.Should().BeNull();
        }

        [Fact]
        public async Task RemoveModuleToSyllabus_ShouldReturnCorrectData_WhenSuccessSaved()
        {
            //arrange
            var syllabusMockData = _fixture.Build<Syllabus>()
                                           .Without(s => s.SyllabusModules)
                                           .Without(s => s.SyllabusOutputStandards)
                                           .Without(S => S.TrainingProgramSyllabi)
                                           .Create();
            var moduleMockData = _fixture.Build<Module>()
                                         .Without(s => s.ModuleUnits)
                                         .Without(s => s.SyllabusModules)
                                         .Without(s => s.AuditPlan)
                                         .Create();
            var syllabusModuleMockData = new SyllabusModule()
            {
                Syllabus = syllabusMockData,
                Module = moduleMockData,
            };
            _unitOfWorkMock.Setup(s => s.SyllabusModuleRepository.GetSyllabusModule(syllabusMockData.Id, moduleMockData.Id)).ReturnsAsync(syllabusModuleMockData);
            _unitOfWorkMock.Setup(s => s.SyllabusModuleRepository.SoftRemove(It.IsAny<SyllabusModule>()));
            _unitOfWorkMock.Setup(s => s.SaveChangeAsync()).ReturnsAsync(1);
            var expected = _mapperConfig.Map<SyllabusModuleViewModel>(syllabusModuleMockData);

            //act
            var result = await _syllabusService.RemoveSyllabusModule(syllabusMockData.Id, moduleMockData.Id);

            //assert
            _unitOfWorkMock.Verify(s => s.SyllabusModuleRepository.SoftRemove(It.IsAny<SyllabusModule>()), Times.Once());
            _unitOfWorkMock.Verify(s => s.SaveChangeAsync(), Times.Once());
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task RemoveModuleToSyllabus_ShouldReturnCorrectData_WhenSuccessFailed()
        {
            //arrange
            var syllabusMockData = _fixture.Build<Syllabus>()
                                           .Without(s => s.SyllabusModules)
                                           .Without(s => s.SyllabusOutputStandards)
                                           .Without(S => S.TrainingProgramSyllabi)
                                           .Create();
            var moduleMockData = _fixture.Build<Module>()
                                         .Without(s => s.ModuleUnits)
                                         .Without(s => s.SyllabusModules)
                                         .Without(s => s.AuditPlan)
                                         .Create();
            var syllabusModuleMockData = new SyllabusModule()
            {
                Syllabus = syllabusMockData,
                Module = moduleMockData,
            };
            _unitOfWorkMock.Setup(s => s.SyllabusModuleRepository.GetSyllabusModule(syllabusMockData.Id, moduleMockData.Id)).ReturnsAsync(syllabusModuleMockData);
            _unitOfWorkMock.Setup(s => s.SyllabusModuleRepository.SoftRemove(It.IsAny<SyllabusModule>()));
            _unitOfWorkMock.Setup(s => s.SaveChangeAsync()).ReturnsAsync(0);
            var expected = _mapperConfig.Map<SyllabusModuleViewModel>(syllabusModuleMockData);

            //act
            var result = await _syllabusService.RemoveSyllabusModule(syllabusMockData.Id, moduleMockData.Id);

            //assert
            _unitOfWorkMock.Verify(s => s.SyllabusModuleRepository.SoftRemove(It.IsAny<SyllabusModule>()), Times.Once());
            _unitOfWorkMock.Verify(s => s.SaveChangeAsync(), Times.Once());
            result.Should().BeNull();
        }
        [Fact]
        public async Task GetAllSyllabusDetail_ShouldReturnCorrectData()
        {
            //arrange
            var syllabusMockData = new Pagination<Syllabus>
            {
                Items = _fixture.Build<Syllabus>()
                                .Without(s => s.SyllabusModules)
                                .Without(s => s.SyllabusOutputStandards)
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
            var expected = _mapperConfig.Map<Pagination<SyllabusViewModel>>(syllabusMockData);
            _unitOfWorkMock.Setup(s => s.SyllabusRepository.GetAllSyllabusDetail(0, 10)).ReturnsAsync(syllabusMockData);
            _unitOfWorkMock.Setup(s => s.UserRepository.GetEntitiesByIdsAsync(It.IsAny<List<Guid?>>())).ReturnsAsync(user);
            //act
            var result = await _syllabusService.GetAllSyllabusDetail(0, 10);
            //assert
            _unitOfWorkMock.Verify(s => s.SyllabusRepository.GetAllSyllabusDetail(0, 10), Times.Once());
        }

        [Fact]
        public async Task GetSyllabusDetails_ShouldReturnCorrectData()
        {
            //arrange
            var mocks = _fixture.Build<Syllabus>()
                                 .Without(s => s.SyllabusModules)
                                 .Without(S => S.SyllabusOutputStandards)
                                 .Without(s => s.TrainingProgramSyllabi)
                                 .Create();

            _unitOfWorkMock.Setup(x => x.SyllabusRepository.GetSyllabusDetail(It.IsAny<Guid>())).ReturnsAsync(mocks);
            var expected = _mapperConfig.Map<SyllabusViewModel>(mocks);
            var createBy = new User { Email = "mock@example.com" };
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(mocks.CreatedBy)).ReturnsAsync(createBy);
            expected.CreatedBy = createBy.Email;
            //act
            var result = await _syllabusService.GetSyllabusDetails(mocks.Id);
            //assert
            _unitOfWorkMock.Verify(x => x.SyllabusRepository.GetSyllabusDetail(mocks.Id), Times.Once());
        }

        [Fact]
        public async Task GetSyllabusByCreationDate_ShouldReturnCorrectData()
        {
            //arrange
            var startDate = new DateTime(2022, 1, 1);
            var endDate = new DateTime(2022, 12, 31);
            var syllabusMockData = new Pagination<Syllabus>
            {
                Items = _fixture.Build<Syllabus>()
                                .Without(s => s.SyllabusModules)
                                .Without(s => s.SyllabusOutputStandards)
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
            var expected = _mapperConfig.Map<Pagination<SyllabusViewModel>>(syllabusMockData);
            _unitOfWorkMock.Setup(s => s.SyllabusRepository.GetSyllabusByCreationDate(startDate, endDate, 0, 10)).ReturnsAsync(syllabusMockData);
            _unitOfWorkMock.Setup(s => s.UserRepository.GetEntitiesByIdsAsync(It.IsAny<List<Guid?>>())).ReturnsAsync(user);
            //act
            var result = await _syllabusService.GetSyllabusByCreationDate(startDate, endDate, 0, 10);
            //assert
            _unitOfWorkMock.Verify(s => s.SyllabusRepository.GetSyllabusByCreationDate(startDate, endDate, 0, 10), Times.Once());
        }

        [Fact]
        public async Task UpdateStatusOnlyOfSyllabus_ShouldReturnCorrectData()
        {
            // Arrange
            var syllabusId = Guid.NewGuid();
            var updateStatusOnlyOfSyllabus = new UpdateStatusOnlyOfSyllabus { Status = Domain.Enum.StatusEnum.Status.Enable };

            _unitOfWorkMock.Setup(x => x.SyllabusRepository.GetByIdAsync(syllabusId))
                           .ReturnsAsync(null as Syllabus);

            // Act
            var result = await _syllabusService.UpdateStatusOnlyOfSyllabus(syllabusId, updateStatusOnlyOfSyllabus);

            // Assert
            _unitOfWorkMock.Verify(x => x.SyllabusRepository.Update(It.IsAny<Syllabus>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Never);
        }
    }
}
