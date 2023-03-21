using Applications.Commons;
using Applications.Services;
using Applications.ViewModels.AuditPlanViewModel;
using AutoFixture;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Tests;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Applications.Tests.Services.AuditPlanServices
{
    public class AuditPlanServicesTests : SetupTest
    {
        private readonly AuditPlanService _auditPlanService;

        public AuditPlanServicesTests()
        {
            _auditPlanService = new Applications.Services.AuditPlanService(_unitOfWorkMock.Object, _mapperConfig);
        }

        [Fact]
        public async Task GetAllAuditPlans_ShouldReturnCorrectData()
        {
            var auditplanMockData = new Pagination<AuditPlan>
            {
                Items = _fixture.Build<AuditPlan>()
                                .Without(x => x.Module)
                                .Without(x => x.Class)
                                .Without(x => x.UserAuditPlans)
                                .Without(x => x.AuditResults)
                                .Without(x => x.AuditQuestions)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };
            var user = _fixture.Build<User>()
                               .Without(x => x.UserAuditPlans)
                               .Without(x => x.AbsentRequests)
                               .Without(x => x.ClassUsers)
                               .Without(x => x.Attendences)
                               .CreateMany(3)
                               .ToList();
            var expected = _mapperConfig.Map<Pagination<AuditPlan>>(auditplanMockData);
            _unitOfWorkMock.Setup(x => x.AuditPlanRepository.ToPagination(0, 10)).ReturnsAsync(auditplanMockData);
            _unitOfWorkMock.Setup(x => x.UserRepository.GetEntitiesByIdsAsync(It.IsAny<List<Guid?>>())).ReturnsAsync(user);
            //act
            var result = await _auditPlanService.GetAllAuditPlanAsync();
            //assert
            _unitOfWorkMock.Verify(x => x.AuditPlanRepository.ToPagination(0, 10), Times.Once());
        }

        [Fact]
        public async Task GetDisableAuditPlans_ShouldReturnCorrectData()
        {
            //arrange
            var auditPlanMockData = new Pagination<AuditPlan>
            {
                Items = _fixture.Build<AuditPlan>()
                                .Without(x => x.Module)
                                .Without(x => x.Class)
                                .Without(x => x.UserAuditPlans)
                                .Without(x => x.AuditResults)
                                .Without(x => x.AuditQuestions)
                                .With(x => x.Status, Domain.Enum.StatusEnum.Status.Disable)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };
            var auditPlans = _mapperConfig.Map<Pagination<AuditPlan>>(auditPlanMockData);
            _unitOfWorkMock.Setup(x => x.AuditPlanRepository.GetDisableAuditPlans(0, 10)).ReturnsAsync(auditPlanMockData);
            var expected = _mapperConfig.Map<Pagination<AuditPlanViewModel>>(auditPlans);
            //act
            var result = await _auditPlanService.GetDisableAuditPlanAsync();
            //assert
            _unitOfWorkMock.Verify(x => x.AuditPlanRepository.GetDisableAuditPlans(0, 10), Times.Once());
        }

        [Fact]
        public async Task GetEnableAuditPlans_ShouldReturnCorrectData()
        {
            //arrange
            var auditPlanMockData = new Pagination<AuditPlan>
            {
                Items = _fixture.Build<AuditPlan>()
                                .Without(x => x.Module)
                                .Without(x => x.Class)
                                .Without(x => x.UserAuditPlans)
                                .Without(x => x.AuditResults)
                                .Without(x => x.AuditQuestions)
                                .With(x => x.Status, Domain.Enum.StatusEnum.Status.Enable)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30
            };
            var auditPlans = _mapperConfig.Map<Pagination<AuditPlan>>(auditPlanMockData);
            _unitOfWorkMock.Setup(x => x.AuditPlanRepository.GetEnableAuditPlans(0, 10)).ReturnsAsync(auditPlanMockData);
            var expected = _mapperConfig.Map<Pagination<AuditPlanViewModel>>(auditPlans);
            //act
            var result = await _auditPlanService.GetEnableAuditPlanAsync();
            //assert
            _unitOfWorkMock.Verify(x => x.AuditPlanRepository.GetEnableAuditPlans(0, 10), Times.Once());
        }

        [Fact]
        public async Task GetAuditPlanById_ShouldReturnCorrectData()
        {
            //arrange
            var auditPlanObj = _fixture.Build<AuditPlan>()
                                        .Without(x => x.Module)
                                        .Without(x => x.Class)
                                        .Without(x => x.UserAuditPlans)
                                        .Without(x => x.AuditResults)
                                        .Without(x => x.AuditQuestions)
                                        .Create();
            var expected = _mapperConfig.Map<AuditPlanViewModel>(auditPlanObj);
            var createBy = new User { Email = "mock@example.com" };
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(auditPlanObj.CreatedBy)).ReturnsAsync(createBy);
            expected.CreatedBy = createBy.Email;
            _unitOfWorkMock.Setup(x => x.AuditPlanRepository.GetByIdAsync(auditPlanObj.Id))
                           .ReturnsAsync(auditPlanObj);
            //act
            var result = await _auditPlanService.GetAuditPlanByIdAsync(auditPlanObj.Id);
            //assert
            _unitOfWorkMock.Verify(x => x.AuditPlanRepository.GetByIdAsync(auditPlanObj.Id), Times.Once());
        }

        [Fact]
        public async Task GetAuditPlanByModuleId_ShouldReturnCorrectData()
        {
            //arrange
            var auditPlanObj = _fixture.Build<AuditPlan>()
                                        .Without(x => x.Module)
                                        .Without(x => x.Class)
                                        .Without(x => x.UserAuditPlans)
                                        .Without(x => x.AuditResults)
                                        .Without(x => x.AuditQuestions)
                                        .Create();
            _unitOfWorkMock.Setup(x => x.AuditPlanRepository.GetAuditPlanByModuleId(auditPlanObj.ModuleId))
                           .ReturnsAsync(auditPlanObj);
            //act
            var result = await _auditPlanService.GetAuditPlanByModuleIdAsync(auditPlanObj.ModuleId);
            //assert
            _unitOfWorkMock.Verify(x => x.AuditPlanRepository.GetAuditPlanByModuleId(auditPlanObj.ModuleId), Times.Once());
        }

        [Fact]
        public async Task GetAuditPlanByClassId_ShouldReturnCorrectData()
        {
            //arrange
            var id = Guid.NewGuid();
            var auditPlanMockData = new Pagination<AuditPlan>
            {
                Items = _fixture.Build<AuditPlan>()
                                .Without(x => x.Module)
                                .Without(x => x.Class)
                                .Without(x => x.UserAuditPlans)
                                .Without(x => x.AuditResults)
                                .Without(x => x.AuditQuestions)
                                .With(x => x.ClassId, id)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };
            var auditPlans = _mapperConfig.Map<Pagination<AuditPlan>>(auditPlanMockData);
            _unitOfWorkMock.Setup(x => x.AuditPlanRepository.GetAuditPlanByClassId(id, 0, 10)).ReturnsAsync(auditPlanMockData);
            var expected = _mapperConfig.Map<Pagination<AuditPlanViewModel>>(auditPlans);
            //act
            var result = await _auditPlanService.GetAuditPlanbyClassIdAsync(id);
            //assert
            _unitOfWorkMock.Verify(x => x.AuditPlanRepository.GetAuditPlanByClassId(id, 0, 10), Times.Once());
        }

        [Fact]
        public async Task GetAuditPlanByName_ShouldReturnCorrectData()
        {
            //arrange
            var auditPlanMockData = new Pagination<AuditPlan>
            {
                Items = _fixture.Build<AuditPlan>()
                                .Without(x => x.Module)
                                .Without(x => x.Class)
                                .Without(x => x.UserAuditPlans)
                                .Without(x => x.AuditResults)
                                .Without(x => x.AuditQuestions)
                                .With(x => x.AuditPlanName, "Mock")
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };
            var auditPlans = _mapperConfig.Map<Pagination<AuditPlan>>(auditPlanMockData);
            _unitOfWorkMock.Setup(x => x.AuditPlanRepository.GetAuditPlanByName("Mock", 0, 10)).ReturnsAsync(auditPlanMockData);
            var expected = _mapperConfig.Map<Pagination<AuditPlanViewModel>>(auditPlans);
            //act
            var result = await _auditPlanService.GetAuditPlanByName("Mock");
            //assert
            _unitOfWorkMock.Verify(x => x.AuditPlanRepository.GetAuditPlanByName("Mock", 0, 10), Times.Once());
        }

        [Fact]
        public async Task CreateAuditPlan_ShouldReturnCorrectData_WhenSuccessSaved()
        {
            //arrange
            var auditPlanMockData = _fixture.Build<CreateAuditPlanViewModel>().Create();
            _unitOfWorkMock.Setup(x => x.AuditPlanRepository.AddAsync(It.IsAny<AuditPlan>()))
                            .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            //act
            var result = await _auditPlanService.CreateAuditPlanAsync(auditPlanMockData);
            //assert
            _unitOfWorkMock.Verify(x => x.AuditPlanRepository.AddAsync(It.IsAny<AuditPlan>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
        }

        [Fact]
        public async Task CreateAuditPlan_ShouldReturnNull_WhenFailedSave()
        {
            var auditPlanMockData = _fixture.Build<CreateAuditPlanViewModel>().Create();
            _unitOfWorkMock.Setup(x => x.AuditPlanRepository.AddAsync(It.IsAny<AuditPlan>()))
                            .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            //act
            var result = await _auditPlanService.CreateAuditPlanAsync(auditPlanMockData);
            //assert
            _unitOfWorkMock.Verify(x => x.AuditPlanRepository.AddAsync(It.IsAny<AuditPlan>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAuditPlan_ShouldReturnCorrectData_WhenSuccessSaved()
        {
            //arrange
            var auditPlanObj = _fixture.Build<AuditPlan>()
                                        .Without(x => x.Module)
                                        .Without(x => x.Class)
                                        .Without(x => x.UserAuditPlans)
                                        .Without(x => x.AuditResults)
                                        .Without(x => x.AuditQuestions)
                                        .Create();
            _unitOfWorkMock.Setup(x => x.AuditPlanRepository.GetByIdAsync(auditPlanObj.Id))
                           .ReturnsAsync(auditPlanObj);
            var updateAuditPlanDataMock = _fixture.Build<UpdateAuditPlanViewModel>()
                                         .Create();
            //act
            await _auditPlanService.UpdateAuditPlanAsync(auditPlanObj.Id, updateAuditPlanDataMock);
            var result = _mapperConfig.Map<UpdateAuditPlanViewModel>(auditPlanObj);
            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UpdateAuditPlanViewModel>();
            result.AuditPlanName.Should().Be(updateAuditPlanDataMock.AuditPlanName);
            // add more property ...
            _unitOfWorkMock.Verify(x => x.AuditPlanRepository.Update(auditPlanObj), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAuditPlan_ShouldReturnNull_WhenFailedSave()
        {
            //arrange
            var auditPlanId = Guid.NewGuid();
            _unitOfWorkMock.Setup(x => x.AuditPlanRepository.GetByIdAsync(auditPlanId))
                            .ReturnsAsync(null as AuditPlan);
            var updateAuditPlanDataMock = _fixture.Build<UpdateAuditPlanViewModel>()
                                                    .Create();
            //act
            var result = await _auditPlanService.UpdateAuditPlanAsync(auditPlanId, updateAuditPlanDataMock);
            //assert
            result.Should().BeNull();
            _unitOfWorkMock.Verify(x => x.AuditPlanRepository.Update(It.IsAny<AuditPlan>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Never);
        }

        [Fact]
        public async Task AddUserToAuditPlan_ShouldReturnCorrcetData()
        {
            //arrange
            var userMockData = _fixture.Build<User>()
                                        .Without(x => x.AbsentRequests)
                                        .Without(x => x.Attendences)
                                        .Without(x => x.UserAuditPlans)
                                        .Without(x => x.ClassUsers)
                                        .Create();
            var auditPLanMockData = _fixture.Build<AuditPlan>()
                                        .Without(x => x.Module)
                                        .Without(x => x.AuditResults)
                                        .Without(x => x.AuditQuestions)
                                        .Without(x => x.UserAuditPlans)
                                        .Without(x => x.Class)
                                        .Create();
            _unitOfWorkMock.Setup(x => x.AuditPlanRepository.GetByIdAsync(auditPLanMockData.Id))
                           .ReturnsAsync(auditPLanMockData);
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(userMockData.Id))
                           .ReturnsAsync(userMockData);
            var mockData = new UserAuditPlan()
            {
                User = userMockData,
                AuditPlan = auditPLanMockData
            };
            await _dbContext.AddAsync(mockData);
            await _dbContext.SaveChangesAsync();
            _unitOfWorkMock.Setup(x => x.UserAuditPlanRepository.AddAsync(It.IsAny<UserAuditPlan>()))
                            .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            //act
            var result = await _auditPlanService.AddUserToAuditPlan(auditPLanMockData.Id, userMockData.Id);
            //assert
            _unitOfWorkMock.Verify(x => x.UserAuditPlanRepository.AddAsync(It.IsAny<UserAuditPlan>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
        }

        [Fact]
        public async Task AddUserToAuditPlan_ShouldReturnNull_WhenNotFoundAuditPlan()
        {
            //arrange
            var userMockData = _fixture.Build<User>()
                                        .Without(x => x.AbsentRequests)
                                        .Without(x => x.Attendences)
                                        .Without(x => x.UserAuditPlans)
                                        .Without(x => x.ClassUsers)
                                        .Create();
            var auditPLanMockData = _fixture.Build<AuditPlan>()
                                        .Without(x => x.Module)
                                        .Without(x => x.AuditResults)
                                        .Without(x => x.AuditQuestions)
                                        .Without(x => x.UserAuditPlans)
                                        .Without(x => x.Class)
                                        .Create();
            _unitOfWorkMock.Setup(x => x.AuditPlanRepository.GetByIdAsync(auditPLanMockData.Id))
                           .ReturnsAsync(auditPLanMockData);
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(userMockData.Id))
                           .ReturnsAsync(userMockData);
            var mockData = new UserAuditPlan()
            {
                User = userMockData,
                AuditPlan = auditPLanMockData
            };
            await _dbContext.AddAsync(mockData);
            await _dbContext.SaveChangesAsync();
            _unitOfWorkMock.Setup(x => x.UserAuditPlanRepository.AddAsync(It.IsAny<UserAuditPlan>()))
                            .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            //act
            var result = await _auditPlanService.AddUserToAuditPlan(auditPLanMockData.Id, userMockData.Id);
            //assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task RemoveUserToAuditPlan_ShouldReturnCorrectData()
        {
            //arrange
            var userMockData = _fixture.Build<User>()
                                        .Without(x => x.AbsentRequests)
                                        .Without(x => x.Attendences)
                                        .Without(x => x.UserAuditPlans)
                                        .Without(x => x.ClassUsers)
                                        .Create();
            var auditPLanMockData = _fixture.Build<AuditPlan>()
                                        .Without(x => x.Module)
                                        .Without(x => x.AuditResults)
                                        .Without(x => x.AuditQuestions)
                                        .Without(x => x.UserAuditPlans)
                                        .Without(x => x.Class)
                                        .Create();
            var mockData = new UserAuditPlan()
            {
                User = userMockData,
                AuditPlan = auditPLanMockData
            };
            await _dbContext.AddAsync(mockData);
            await _dbContext.SaveChangesAsync();
            _unitOfWorkMock.Setup(x => x.UserAuditPlanRepository.GetUserAuditPlan(auditPLanMockData.Id, userMockData.Id))
                           .ReturnsAsync(mockData);
            _unitOfWorkMock.Setup(x => x.UserAuditPlanRepository.SoftRemove(It.IsAny<UserAuditPlan>()));
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            //act
            var result = await _auditPlanService.RemoveUserFromAuditPlan(auditPLanMockData.Id, userMockData.Id);
            //assert
            _unitOfWorkMock.Verify(x => x.UserAuditPlanRepository.SoftRemove(It.IsAny<UserAuditPlan>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
        }

        [Fact]
        public async Task RemoveUserToAuditPlan_ShouldReturnNull_WhenNotFoundAuditPlan()
        {
            //arrange
            var userMockData = _fixture.Build<User>()
                                        .Without(x => x.AbsentRequests)
                                        .Without(x => x.Attendences)
                                        .Without(x => x.UserAuditPlans)
                                        .Without(x => x.ClassUsers)
                                        .Create();
            var auditPLanMockData = _fixture.Build<AuditPlan>()
                                        .Without(x => x.Module)
                                        .Without(x => x.AuditResults)
                                        .Without(x => x.AuditQuestions)
                                        .Without(x => x.UserAuditPlans)
                                        .Without(x => x.Class)
                                        .Create();
            var mockData = new UserAuditPlan()
            {
                User = userMockData,
                AuditPlan = auditPLanMockData
            };
            await _dbContext.AddAsync(mockData);
            await _dbContext.SaveChangesAsync();
            _unitOfWorkMock.Setup(x => x.UserAuditPlanRepository.GetUserAuditPlan(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(() => null);
            _unitOfWorkMock.Setup(x => x.UserAuditPlanRepository.SoftRemove(It.IsAny<UserAuditPlan>()));
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            //act
            var result = await _auditPlanService.RemoveUserFromAuditPlan(auditPLanMockData.Id, userMockData.Id);
            //assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task RemoveUserToAuditPlan_ShouldReturnNull_WhenFailedSave()
        {
            //arrange
            var userMockData = _fixture.Build<User>()
                                        .Without(x => x.AbsentRequests)
                                        .Without(x => x.Attendences)
                                        .Without(x => x.UserAuditPlans)
                                        .Without(x => x.ClassUsers)
                                        .Create();
            var auditPLanMockData = _fixture.Build<AuditPlan>()
                                        .Without(x => x.Module)
                                        .Without(x => x.AuditResults)
                                        .Without(x => x.AuditQuestions)
                                        .Without(x => x.UserAuditPlans)
                                        .Without(x => x.Class)
                                        .Create();
            var mockData = new UserAuditPlan()
            {
                User = userMockData,
                AuditPlan = auditPLanMockData
            };
            await _dbContext.AddAsync(mockData);
            await _dbContext.SaveChangesAsync();
            _unitOfWorkMock.Setup(x => x.UserAuditPlanRepository.GetUserAuditPlan(auditPLanMockData.Id, userMockData.Id))
                           .ReturnsAsync(mockData);
            _unitOfWorkMock.Setup(x => x.UserAuditPlanRepository.SoftRemove(It.IsAny<UserAuditPlan>()));
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            //act
            var result = await _auditPlanService.RemoveUserFromAuditPlan(auditPLanMockData.Id, userMockData.Id);
            //assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllUserAuditPlan_ShouldReturnCorrectData()
        {
            //arrange
            var userMockData = _fixture.Build<User>()
                                        .Without(x => x.AbsentRequests)
                                        .Without(x => x.Attendences)
                                        .Without(x => x.UserAuditPlans)
                                        .Without(x => x.ClassUsers)
                                        .CreateMany(30)
                                        .ToList();
            var auditPlanMockData = _fixture.Build<AuditPlan>()
                                        .Without(x => x.Module)
                                        .Without(x => x.Class)
                                        .Without(x => x.UserAuditPlans)
                                        .Without(x => x.AuditResults)
                                        .Without(x => x.AuditQuestions)
                                        .Create();
            var user = _fixture.Build<User>()
                               .Without(x => x.UserAuditPlans)
                               .Without(x => x.AbsentRequests)
                               .Without(x => x.ClassUsers)
                               .Without(x => x.Attendences)
                               .CreateMany(3)
                               .ToList();
            await _dbContext.Users.AddRangeAsync(userMockData);
            await _dbContext.AuditPlans.AddAsync(auditPlanMockData);
            await _dbContext.SaveChangesAsync();
            var MockData = new List<UserAuditPlan>();
            foreach (var item in userMockData)
            {
                var data = new UserAuditPlan
                {
                    User = item,
                    AuditPlan = auditPlanMockData
                };
                MockData.Add(data);
            }
            var itemCount = await _dbContext.UserAuditPlan.CountAsync();
            var items = await _dbContext.UserAuditPlan.OrderByDescending(x => x.CreationDate)
                                                      .Take(10)
                                                      .AsNoTracking()
                                                      .ToListAsync();
            var userAuditPlans = new Pagination<UserAuditPlan>()
            {
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = itemCount,
                Items = items,
            };
            var expected = _mapperConfig.Map<Pagination<UserAuditPlan>>(userAuditPlans);
            _unitOfWorkMock.Setup(x => x.UserAuditPlanRepository.ToPagination(0, 10)).ReturnsAsync(userAuditPlans);
            _unitOfWorkMock.Setup(x => x.UserRepository.GetEntitiesByIdsAsync(It.IsAny<List<Guid?>>())).ReturnsAsync(user);
            //act
            var result = await _auditPlanService.GetAllUserAuditPlanAsync();
            //assert
            _unitOfWorkMock.Verify(x => x.UserAuditPlanRepository.ToPagination(0, 10), Times.Once());
        }
    }
}
