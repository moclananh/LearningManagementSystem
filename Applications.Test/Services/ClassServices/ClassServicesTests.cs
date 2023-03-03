﻿using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.ClassViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.EntityRelationship;
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
            var expected = _mapperConfig.Map<Pagination<Class>>(classMockData);
            _unitOfWorkMock.Setup(x => x.ClassRepository.ToPagination(0, 10)).ReturnsAsync(classMockData);
            //act
            var result = await _classService.GetAllClasses();
            //assert
            _unitOfWorkMock.Verify(x => x.ClassRepository.ToPagination(0, 10), Times.Once());
        }
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
        [Fact]
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
            _unitOfWorkMock.Setup(x => x.ClassRepository.GetByIdAsync(classObj.Id))
                           .ReturnsAsync(classObj);
            var updateDataMock = _fixture.Build<UpdateClassViewModel>()
                                         .Create();
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
        public async Task UpdateClass_ShouldReturnNull_WhenFailedSave()
        {
            //arrange
            var classId = Guid.NewGuid();
            _unitOfWorkMock.Setup(x => x.ClassRepository.GetByIdAsync(classId))
                           .ReturnsAsync(null as Class);
            var updateDataMock = _fixture.Build<UpdateClassViewModel>()
                                         .Create();
            //act
            var result = await _classService.UpdateClass(classId, updateDataMock);
            //assert
            result.Should().BeNull();
            _unitOfWorkMock.Verify(x => x.ClassRepository.Update(It.IsAny<Class>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Never);
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
                                .With(x => x.Status, Domain.Enum.StatusEnum.Status.Enable)
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
                                .With(x => x.Status, Domain.Enum.StatusEnum.Status.Disable)
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
            var mocks = _fixture.Build<Class>()
                                .Without(x => x.AbsentRequests)
                                .Without(x => x.Attendences)
                                .Without(x => x.AuditPlans)
                                .Without(x => x.ClassUsers)
                                .Without(x => x.ClassTrainingPrograms)
                                .Create();
            var user = _fixture.Build<User>()
                               .Without(x => x.UserAuditPlans)
                               .Without(x => x.AbsentRequests)
                               .Without(x => x.Attendences)
                               .Without(x => x.ClassUsers)
                               .CreateMany(10)
                               .ToList();
            var listUser = new List<ClassUser>();
            foreach (var item in user)
            {
                var classUser = new ClassUser()
                {
                    Class = mocks,
                    User = item,
                };
                listUser.Add(classUser);
            }
            mocks.ClassUsers = new List<ClassUser>();
            mocks.ClassUsers.ToList().AddRange(listUser);
            _unitOfWorkMock.Setup(x => x.ClassRepository.GetClassDetails(It.IsAny<Guid>())).ReturnsAsync(mocks);
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user[0]);
            var expected = _mapperConfig.Map<ClassDetailsViewModel>(mocks);
            //act
            var result = await _classService.GetClassDetails(mocks.Id);
            //assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
