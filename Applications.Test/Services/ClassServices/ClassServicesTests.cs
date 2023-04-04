using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.ClassTrainingProgramViewModels;
using Applications.ViewModels.ClassUserViewModels;
using Applications.ViewModels.ClassViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Enum.RoleEnum;
using Domain.Enum.StatusEnum;
using Domain.Tests;
using FluentAssertions;
using Moq;

namespace Applications.Tests.Services.ClassServices
{
    public class ClassServicesTests : SetupTest
    {
        private readonly IClassService _classService;
        public ClassServicesTests()
        {
            _classService = new Applications.Services.ClassServices(_unitOfWorkMock.Object, _mapperConfig);
        }
        [Fact]
        public async Task GetAllClasses_ShouldReturnCorrectData()
        {
            //arrange
            var classMockData = new Pagination<Class>
            {
                Items = _fixture.Build<Class>()
                                .Without(x => x.AbsentRequests)
                                .Without(x => x.Attendences)
                                .Without(x => x.AuditPlans)
                                .Without(x => x.ClassUsers)
                                .Without(x => x.ClassTrainingPrograms)
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
            var expected = _mapperConfig.Map<Pagination<Class>>(classMockData);
            _unitOfWorkMock.Setup(x => x.ClassRepository.ToPagination(0, 10)).ReturnsAsync(classMockData);
            _unitOfWorkMock.Setup(x => x.UserRepository.GetEntitiesByIdsAsync(It.IsAny<List<Guid?>>())).ReturnsAsync(user);
            //act
            var result = await _classService.GetAllClasses();
            //assert
            _unitOfWorkMock.Verify(x => x.ClassRepository.ToPagination(0, 10), Times.Once());
        }/*
        [Fact]
        public async Task CreateClass_ShouldReturnCorrectData_WhenSuccessSaved()
        {
            //arrange
            var mocks = _fixture.Build<CreateClassViewModel>()
                                .Create();
            _unitOfWorkMock.Setup(x => x.ClassRepository.AddAsync(It.IsAny<Class>()))
                           .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            //act
            var result = await _classService.CreateClass(mocks);
            //assert
            _unitOfWorkMock.Verify(x => x.ClassRepository.AddAsync(It.IsAny<Class>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
        }
        /*[Fact]
        public async Task CreateClass_ShouldReturnNull_WhenFailedSave()
        {
            //arrange
            var mocks = _fixture.Build<CreateClassViewModel>()
                                .Create();
            _unitOfWorkMock.Setup(x => x.ClassRepository.AddAsync(It.IsAny<Class>()))
                           .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            //act
            var result = await _classService.CreateClass(mocks);
            //assert
            _unitOfWorkMock.Verify(x => x.ClassRepository.AddAsync(It.IsAny<Class>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
            result.Should().BeNull();
        }*/
        [Fact]
        public async Task CreateClass_ShouldThrowException_WhenFailed()
        {
            //arrange
            var mocks = _fixture.Build<CreateClassViewModel>()
                                .Create();
            _unitOfWorkMock.Setup(x => x.ClassRepository.AddAsync(It.IsAny<Class>()))
                   .Throws(new ArgumentException("Error at CreateClass"));
            //act
            //assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _classService.CreateClass(mocks));
            Assert.Equal("Error at CreateClass", ex.Message);
        }
        [Fact]
        public async Task UpdateClass_ShouldReturnCorrectData_WhenSuccessSaved()
        {
            //arrange
            var classObj = _fixture.Build<Class>()
                                   .Without(x => x.AbsentRequests)
                                   .Without(x => x.Attendences)
                                   .Without(x => x.AuditPlans)
                                   .Without(x => x.ClassUsers)
                                   .Without(x => x.ClassTrainingPrograms)
                                   .Create();
            var updateDataMock = _fixture.Build<UpdateClassViewModel>()
                                         .Create();
            _unitOfWorkMock.Setup(x => x.ClassRepository.GetByIdAsync(It.IsAny<Guid>()))
                           .ReturnsAsync(classObj);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            //act
            await _classService.UpdateClass(classObj.Id, updateDataMock);
            var result = _mapperConfig.Map<UpdateClassViewModel>(classObj);
            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UpdateClassViewModel>();
            result.ClassName.Should().Be(updateDataMock.ClassName);
            // add more property ...
            _unitOfWorkMock.Verify(x => x.ClassRepository.Update(classObj), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once);
        }
        [Fact]
        public async Task UpdateClass_ShouldReturnNull_WhenNotFoundClass()
        {
            //arrange
            var classId = _fixture.Create<Guid>();
            _unitOfWorkMock.Setup(x => x.ClassRepository.GetByIdAsync(It.IsAny<Guid>()))
                           .ReturnsAsync(null as Class);
            var updateDataMock = _fixture.Build<UpdateClassViewModel>()
                                         .Create();
            //act
            var result = await _classService.UpdateClass(classId, updateDataMock);
            //assert
            result.Should().BeNull();
        }
        [Fact]
        public async Task UpdateClass_ShouldReturnNull_WhenFailedSaveChange()
        {
            //arrange
            var classObj = _fixture.Build<Class>()
                                   .Without(x => x.AbsentRequests)
                                   .Without(x => x.Attendences)
                                   .Without(x => x.AuditPlans)
                                   .Without(x => x.ClassUsers)
                                   .Without(x => x.ClassTrainingPrograms)
                                   .Create();
            _unitOfWorkMock.Setup(x => x.ClassRepository.GetByIdAsync(It.IsAny<Guid>()))
                           .ReturnsAsync(classObj);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            var updateDataMock = _fixture.Build<UpdateClassViewModel>()
                                         .Create();
            //act
            var result = await _classService.UpdateClass(classObj.Id, updateDataMock);
            //assert
            result.Should().BeNull();
        }
        [Fact]
        public async Task GetEnableClasses_ShouldReturnCorrectData()
        {
            //arrange
            var classMockData = new Pagination<Class>
            {
                Items = _fixture.Build<Class>()
                                .Without(x => x.AbsentRequests)
                                .Without(x => x.Attendences)
                                .Without(x => x.AuditPlans)
                                .Without(x => x.ClassUsers)
                                .Without(x => x.ClassTrainingPrograms)
                                .With(x => x.Status, Status.Enable)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30
            };
            var classes = _mapperConfig.Map<Pagination<Class>>(classMockData);
            _unitOfWorkMock.Setup(x => x.ClassRepository.GetEnableClasses(0, 10)).ReturnsAsync(classMockData);
            var expected = _mapperConfig.Map<Pagination<ClassViewModel>>(classes);
            //act
            var result = await _classService.GetEnableClasses(0, 10);
            //assert
            result.Should().BeEquivalentTo(expected);
        }
        [Fact]
        public async Task GetDisableClasses_ShouldReturnCorrectData()
        {
            //arrange
            var classMockData = new Pagination<Class>
            {
                Items = _fixture.Build<Class>()
                                .Without(x => x.AbsentRequests)
                                .Without(x => x.Attendences)
                                .Without(x => x.AuditPlans)
                                .Without(x => x.ClassUsers)
                                .Without(x => x.ClassTrainingPrograms)
                                .With(x => x.Status, Status.Disable)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30
            };
            var classes = _mapperConfig.Map<Pagination<Class>>(classMockData);
            _unitOfWorkMock.Setup(x => x.ClassRepository.GetDisableClasses(0, 10)).ReturnsAsync(classMockData);
            var expected = _mapperConfig.Map<Pagination<ClassViewModel>>(classes);
            //act
            var result = await _classService.GetDisableClasses(0, 10);
            //assert
            result.Should().BeEquivalentTo(expected);
        }
        [Fact]
        public async Task GetClassDetails_ShouldReturnCorrectData()
        {
            //arrange
            var supperAdmin = _fixture.Build<User>()
                               .Without(x => x.UserAuditPlans)
                               .Without(x => x.AbsentRequests)
                               .Without(x => x.Attendences)
                               .Without(x => x.ClassUsers)
                               .With(x => x.Role, Role.SuperAdmin)
                               .Create();
            var classAdmin = _fixture.Build<User>()
                               .Without(x => x.UserAuditPlans)
                               .Without(x => x.AbsentRequests)
                               .Without(x => x.Attendences)
                               .Without(x => x.ClassUsers)
                               .With(x => x.Role, Role.ClassAdmin)
                               .Create();
            var trainer = _fixture.Build<User>()
                               .Without(x => x.UserAuditPlans)
                               .Without(x => x.AbsentRequests)
                               .Without(x => x.Attendences)
                               .Without(x => x.ClassUsers)
                               .With(x => x.Role, Role.Trainer)
                               .Create();
            var student = _fixture.Build<User>()
                               .Without(x => x.UserAuditPlans)
                               .Without(x => x.AbsentRequests)
                               .Without(x => x.Attendences)
                               .Without(x => x.ClassUsers)
                               .With(x => x.Role, Role.Student)
                               .CreateMany(10)
                               .ToList();
            var mocks = _fixture.Build<Class>()
                                .Without(x => x.AbsentRequests)
                                .Without(x => x.Attendences)
                                .Without(x => x.AuditPlans)
                                .Without(x => x.ClassUsers)
                                .Without(x => x.ClassTrainingPrograms)
                                .Create();

            var listUser = new List<ClassUser>();
            foreach (var item in student)
            {
                var classStudent = new ClassUser()
                {
                    ClassId = mocks.Id,
                    UserId = item.Id,
                    Class = mocks,
                    User = item,
                };
                listUser.Add(classStudent);
            }

            listUser.AddRange(new List<ClassUser>
            {
                new ClassUser
                {
                    ClassId = mocks.Id,
                    UserId = supperAdmin.Id,
                    Class = mocks,
                    User = supperAdmin
                },
                new ClassUser
                {
                    ClassId = mocks.Id,
                    UserId = classAdmin.Id,
                    Class = mocks,
                    User = classAdmin
                },
                new ClassUser
                {
                    ClassId = mocks.Id,
                    UserId = trainer.Id,
                    Class = mocks,
                    User = trainer
                }
            });

            mocks.ClassUsers = listUser;

            _unitOfWorkMock.Setup(x => x.ClassRepository.GetClassDetails(It.IsAny<Guid>())).ReturnsAsync(mocks);
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(supperAdmin);
            var expected = _mapperConfig.Map<ClassDetailsViewModel>(mocks);

            //act
            var result = await _classService.GetClassDetails(mocks.Id);

            //assert
            result.ClassUsers.Should().BeEquivalentTo(expected.ClassUsers);
        }
        [Fact]
        public async Task AddTrainingProgramToClass_ShouldReturnCorrectData()
        {
            //arrange
            var classMocks = _fixture.Build<Class>()
                                .Without(x => x.AbsentRequests)
                                .Without(x => x.Attendences)
                                .Without(x => x.AuditPlans)
                                .Without(x => x.ClassUsers)
                                .Without(x => x.ClassTrainingPrograms)
                                .Create();
            var trainingProgramMocks = _fixture.Build<TrainingProgram>()
                                               .Without(x => x.ClassTrainingPrograms)
                                               .Without(x => x.TrainingProgramSyllabi)
                                               .Create();
            var classTrainingProgram = new ClassTrainingProgram()
            {
                Class = classMocks,
                TrainingProgram = trainingProgramMocks
            };
            _unitOfWorkMock.Setup(x => x.ClassRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(classMocks);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(trainingProgramMocks);
            _unitOfWorkMock.Setup(x => x.ClassTrainingProgramRepository.AddAsync(It.IsAny<ClassTrainingProgram>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            var expected = _mapperConfig.Map<CreateClassTrainingProgramViewModel>(classTrainingProgram);
            //act
            var result = await _classService.AddTrainingProgramToClass(classMocks.Id, trainingProgramMocks.Id);
            //assert
            result.Should().BeEquivalentTo(expected);
        }
        [Fact]
        public async Task AddTrainingProgramToClass_ShouldReturnNull_WhenNotFoundClass()
        {
            //arrange
            var classMocks = _fixture.Build<Class>()
                                .Without(x => x.AbsentRequests)
                                .Without(x => x.Attendences)
                                .Without(x => x.AuditPlans)
                                .Without(x => x.ClassUsers)
                                .Without(x => x.ClassTrainingPrograms)
                                .Create();
            var trainingProgramMocks = _fixture.Build<TrainingProgram>()
                                               .Without(x => x.ClassTrainingPrograms)
                                               .Without(x => x.TrainingProgramSyllabi)
                                               .Create();
            var classTrainingProgram = new ClassTrainingProgram()
            {
                Class = classMocks,
                TrainingProgram = trainingProgramMocks
            };
            _unitOfWorkMock.Setup(x => x.ClassRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(classMocks);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(trainingProgramMocks);
            _unitOfWorkMock.Setup(x => x.ClassTrainingProgramRepository.AddAsync(It.IsAny<ClassTrainingProgram>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            var expected = _mapperConfig.Map<CreateClassTrainingProgramViewModel>(classTrainingProgram);
            //act
            var result = await _classService.AddTrainingProgramToClass(classMocks.Id, trainingProgramMocks.Id);
            //assert
            result.Should().BeNull();
        }
        [Fact]
        public async Task AddTrainingProgramToClass_ShouldThrowException_WhenFail()
        {
            //arrange
            var classId = _fixture.Create<Guid>();
            var trainingProgramId = _fixture.Create<Guid>();
            _unitOfWorkMock.Setup(x => x.ClassTrainingProgramRepository.AddAsync(It.IsAny<ClassTrainingProgram>())).Throws(new ArgumentException("Error at AddTrainingProgramToClass"));
            //act 
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _classService.AddTrainingProgramToClass(classId, trainingProgramId));
            //assert
            Assert.Equal("Error at AddTrainingProgramToClass", ex.Message);
        }
        [Fact]
        public async Task RemoveTrainingProgramToClass_ShouldReturnCorrectData()
        {
            //arrange
            var classMocks = _fixture.Build<Class>()
                                .Without(x => x.AbsentRequests)
                                .Without(x => x.Attendences)
                                .Without(x => x.AuditPlans)
                                .Without(x => x.ClassUsers)
                                .Without(x => x.ClassTrainingPrograms)
                                .Create();
            var trainingProgramMocks = _fixture.Build<TrainingProgram>()
                                               .Without(x => x.ClassTrainingPrograms)
                                               .Without(x => x.TrainingProgramSyllabi)
                                               .Create();
            var classTrainingProgram = new ClassTrainingProgram()
            {
                ClassId = classMocks.Id,
                TrainingProgramId = trainingProgramMocks.Id,
                Class = classMocks,
                TrainingProgram = trainingProgramMocks
            };
            _unitOfWorkMock.Setup(x => x.ClassTrainingProgramRepository.GetClassTrainingProgram(classMocks.Id, trainingProgramMocks.Id)).ReturnsAsync(classTrainingProgram);
            _unitOfWorkMock.Setup(x => x.ClassTrainingProgramRepository.SoftRemove(It.IsAny<ClassTrainingProgram>()));
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            var expected = _mapperConfig.Map<CreateClassTrainingProgramViewModel>(classTrainingProgram);
            //act
            var result = await _classService.RemoveTrainingProgramFromClass(classMocks.Id, trainingProgramMocks.Id);
            //assert
            result.Should().BeEquivalentTo(expected);
        }
        [Fact]
        public async Task RemoveTrainingProgramToClass_ShouldReturnNull_WhenNotFoundClass()
        {
            //arrange
            var classMocks = _fixture.Build<Class>()
                                .Without(x => x.AbsentRequests)
                                .Without(x => x.Attendences)
                                .Without(x => x.AuditPlans)
                                .Without(x => x.ClassUsers)
                                .Without(x => x.ClassTrainingPrograms)
                                .Create();
            var trainingProgramMocks = _fixture.Build<TrainingProgram>()
                                               .Without(x => x.ClassTrainingPrograms)
                                               .Without(x => x.TrainingProgramSyllabi)
                                               .Create();
            var classTrainingProgram = new ClassTrainingProgram()
            {
                ClassId = classMocks.Id,
                TrainingProgramId = trainingProgramMocks.Id,
                Class = classMocks,
                TrainingProgram = trainingProgramMocks
            };
            _unitOfWorkMock.Setup(x => x.ClassTrainingProgramRepository.GetClassTrainingProgram(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(() => null);
            _unitOfWorkMock.Setup(x => x.ClassTrainingProgramRepository.SoftRemove(It.IsAny<ClassTrainingProgram>()));
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            var expected = _mapperConfig.Map<CreateClassTrainingProgramViewModel>(classTrainingProgram);
            //act
            var result = await _classService.RemoveTrainingProgramFromClass(classMocks.Id, trainingProgramMocks.Id);
            //assert
            result.Should().BeNull();
        }
        [Fact]
        public async Task RemoveTrainingProgramToClass_ShouldReturnNull_WhenSavedFail()
        {
            //arrange
            var classMocks = _fixture.Build<Class>()
                                .Without(x => x.AbsentRequests)
                                .Without(x => x.Attendences)
                                .Without(x => x.AuditPlans)
                                .Without(x => x.ClassUsers)
                                .Without(x => x.ClassTrainingPrograms)
                                .Create();
            var trainingProgramMocks = _fixture.Build<TrainingProgram>()
                                               .Without(x => x.ClassTrainingPrograms)
                                               .Without(x => x.TrainingProgramSyllabi)
                                               .Create();
            var classTrainingProgram = new ClassTrainingProgram()
            {
                ClassId = classMocks.Id,
                TrainingProgramId = trainingProgramMocks.Id,
                Class = classMocks,
                TrainingProgram = trainingProgramMocks
            };
            _unitOfWorkMock.Setup(x => x.ClassTrainingProgramRepository.GetClassTrainingProgram(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(classTrainingProgram);
            _unitOfWorkMock.Setup(x => x.ClassTrainingProgramRepository.SoftRemove(It.IsAny<ClassTrainingProgram>()));
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            var expected = _mapperConfig.Map<CreateClassTrainingProgramViewModel>(classTrainingProgram);
            //act
            var result = await _classService.RemoveTrainingProgramFromClass(classMocks.Id, trainingProgramMocks.Id);
            //assert
            result.Should().BeNull();
        }
        [Fact]
        public async Task RemoveTrainingProgramToClass_ShouldReturnNull_WhenSaveChangedFailed()
        {
            //arrange
            var classMocks = _fixture.Build<Class>()
                                .Without(x => x.AbsentRequests)
                                .Without(x => x.Attendences)
                                .Without(x => x.AuditPlans)
                                .Without(x => x.ClassUsers)
                                .Without(x => x.ClassTrainingPrograms)
                                .Create();
            var trainingProgramMocks = _fixture.Build<TrainingProgram>()
                                               .Without(x => x.ClassTrainingPrograms)
                                               .Without(x => x.TrainingProgramSyllabi)
                                               .Create();
            var classTrainingProgram = new ClassTrainingProgram()
            {
                ClassId = classMocks.Id,
                TrainingProgramId = trainingProgramMocks.Id,
                Class = classMocks,
                TrainingProgram = trainingProgramMocks
            };
            _unitOfWorkMock.Setup(x => x.ClassTrainingProgramRepository.GetClassTrainingProgram(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(() => null);
            _unitOfWorkMock.Setup(x => x.ClassTrainingProgramRepository.SoftRemove(It.IsAny<ClassTrainingProgram>()));
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            var expected = _mapperConfig.Map<CreateClassTrainingProgramViewModel>(classTrainingProgram);
            //act
            var result = await _classService.RemoveTrainingProgramFromClass(classMocks.Id, trainingProgramMocks.Id);
            //assert
            result.Should().BeNull();
        }
        [Fact]
        public async Task GetClassById_ShouldReturnCorrectData()
        {
            //arrange
            var classMocks = _fixture.Build<Class>()
                                .Without(x => x.AbsentRequests)
                                .Without(x => x.Attendences)
                                .Without(x => x.AuditPlans)
                                .Without(x => x.ClassUsers)
                                .Without(x => x.ClassTrainingPrograms)
                                .Create();
            _unitOfWorkMock.Setup(x => x.ClassRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(classMocks);
            var expected = _mapperConfig.Map<ClassViewModel>(classMocks);
            //act
            var result = _classService.GetClassById(classMocks.Id);
            //assert
            result.Result.Should().BeEquivalentTo(expected);
        }
        [Fact]
        public async Task GetClassesByName_ShouldReturnCorrectData()
        {
            //arrange
            var classMockData = new Pagination<Class>
            {
                Items = _fixture.Build<Class>()
                                .Without(x => x.AbsentRequests)
                                .Without(x => x.Attendences)
                                .Without(x => x.AuditPlans)
                                .Without(x => x.ClassUsers)
                                .Without(x => x.ClassTrainingPrograms)
                                .With(x => x.ClassName, "NetCore")
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30
            };
            var classes = _mapperConfig.Map<Pagination<Class>>(classMockData);
            _unitOfWorkMock.Setup(x => x.ClassRepository.GetClassByName(It.IsAny<string>(), 0, 10)).ReturnsAsync(classMockData);
            var expected = _mapperConfig.Map<Pagination<ClassViewModel>>(classes);
            //act
            var result = await _classService.GetClassByName("Net", 0, 10);
            //assert
            result.Should().BeEquivalentTo(expected);
        }
        [Fact]
        public async Task RemoveUserToClass_ShouldReturnCorrectData()
        {
            //arrange
            var classMocks = _fixture.Build<Class>()
                                .Without(x => x.AbsentRequests)
                                .Without(x => x.Attendences)
                                .Without(x => x.AuditPlans)
                                .Without(x => x.ClassUsers)
                                .Without(x => x.ClassTrainingPrograms)
                                .Create();
            var userMockData = _fixture.Build<User>()
                                        .Without(x => x.AbsentRequests)
                                        .Without(x => x.Attendences)
                                        .Without(x => x.UserAuditPlans)
                                        .Without(x => x.ClassUsers)
                                        .Create();
            var classUser = new ClassUser()
            {
                ClassId = classMocks.Id,
                UserId = userMockData.Id,
                Class = classMocks,
                User = userMockData
            };
            _unitOfWorkMock.Setup(x => x.ClassUserRepository.GetClassUser(classMocks.Id, userMockData.Id)).ReturnsAsync(classUser);
            _unitOfWorkMock.Setup(x => x.ClassUserRepository.SoftRemove(It.IsAny<ClassUser>()));
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            var expected = _mapperConfig.Map<CreateClassUserViewModel>(classUser);
            //act
            var result = await _classService.RemoveUserFromClass(classMocks.Id, userMockData.Id);
            //assert
            result.Should().BeEquivalentTo(expected);
        }
        [Fact]
        public async Task RemoveUserToClass_ShouldReturnNull_WhenNotFoundClass()
        {
            //arrange
            var classMocks = _fixture.Build<Class>()
                                .Without(x => x.AbsentRequests)
                                .Without(x => x.Attendences)
                                .Without(x => x.AuditPlans)
                                .Without(x => x.ClassUsers)
                                .Without(x => x.ClassTrainingPrograms)
                                .Create();
            var userMockData = _fixture.Build<User>()
                                        .Without(x => x.AbsentRequests)
                                        .Without(x => x.Attendences)
                                        .Without(x => x.UserAuditPlans)
                                        .Without(x => x.ClassUsers)
                                        .Create();
            var classUser = new ClassUser()
            {
                ClassId = classMocks.Id,
                UserId = userMockData.Id,
                Class = classMocks,
                User = userMockData
            };
            _unitOfWorkMock.Setup(x => x.ClassUserRepository.GetClassUser(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(() => null);
            _unitOfWorkMock.Setup(x => x.ClassUserRepository.SoftRemove(It.IsAny<ClassUser>()));
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            var expected = _mapperConfig.Map<CreateClassUserViewModel>(classUser);
            //act
            var result = await _classService.RemoveUserFromClass(classMocks.Id, userMockData.Id);
            //assert
            result.Should().BeNull();
        }
        [Fact]
        public async Task RemoveUserToClass_ShouldReturnNull_WhenSavedFail()
        {
            //arrange
            var classMocks = _fixture.Build<Class>()
                                .Without(x => x.AbsentRequests)
                                .Without(x => x.Attendences)
                                .Without(x => x.AuditPlans)
                                .Without(x => x.ClassUsers)
                                .Without(x => x.ClassTrainingPrograms)
                                .Create();
            var userMockData = _fixture.Build<User>()
                                        .Without(x => x.AbsentRequests)
                                        .Without(x => x.Attendences)
                                        .Without(x => x.UserAuditPlans)
                                        .Without(x => x.ClassUsers)
                                        .Create();
            var classUser = new ClassUser()
            {
                ClassId = classMocks.Id,
                UserId = userMockData.Id,
                Class = classMocks,
                User = userMockData
            };
            _unitOfWorkMock.Setup(x => x.ClassUserRepository.GetClassUser(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(classUser);
            _unitOfWorkMock.Setup(x => x.ClassUserRepository.SoftRemove(It.IsAny<ClassUser>()));
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            var expected = _mapperConfig.Map<CreateClassUserViewModel>(classUser);
            //act
            var result = await _classService.RemoveUserFromClass(classMocks.Id, userMockData.Id);
            //assert
            result.Should().BeNull();
        }
        [Fact]
        public async Task RemoveUserToClass_ShouldReturnNull_WhenSaveChangedFailed()
        {
            //arrange
            var classMocks = _fixture.Build<Class>()
                                .Without(x => x.AbsentRequests)
                                .Without(x => x.Attendences)
                                .Without(x => x.AuditPlans)
                                .Without(x => x.ClassUsers)
                                .Without(x => x.ClassTrainingPrograms)
                                .Create();
            var userMockData = _fixture.Build<User>()
                                        .Without(x => x.AbsentRequests)
                                        .Without(x => x.Attendences)
                                        .Without(x => x.UserAuditPlans)
                                        .Without(x => x.ClassUsers)
                                        .Create();
            var classUser = new ClassUser()
            {
                ClassId = classMocks.Id,
                UserId = userMockData.Id,
                Class = classMocks,
                User = userMockData
            };
            _unitOfWorkMock.Setup(x => x.ClassUserRepository.GetClassUser(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(() => null);
            _unitOfWorkMock.Setup(x => x.ClassUserRepository.SoftRemove(It.IsAny<ClassUser>()));
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            var expected = _mapperConfig.Map<CreateClassUserViewModel>(classUser);
            //act
            var result = await _classService.RemoveUserFromClass(classMocks.Id, userMockData.Id);
            //assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateStatusOnlyOfClass_ShouldReturnCorrectData()
        {
            // Arrange
            var ClassId = Guid.NewGuid();
            var updateStatusOnlyOfClass = new UpdateStatusOnlyOfClass { Status = Domain.Enum.StatusEnum.Status.Enable };

            _unitOfWorkMock.Setup(x => x.ClassRepository.GetByIdAsync(ClassId))
                           .ReturnsAsync(null as Class);

            // Act
            var result = await _classService.UpdateStatusOnlyOfClass(ClassId, updateStatusOnlyOfClass);

            // Assert
            _unitOfWorkMock.Verify(x => x.ClassRepository.Update(It.IsAny<Class>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Never);
        }
    }
}
