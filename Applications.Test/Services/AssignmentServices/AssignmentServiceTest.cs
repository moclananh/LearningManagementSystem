
using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.AssignmentViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Moq;

namespace Applications.Tests.Services.AssignmentServices
{
    public class AssignmentServiceTest : SetupTest
    {
        private readonly IAssignmentService _assignmentService;
        public AssignmentServiceTest()
        {
            _assignmentService = new Applications.Services.AssignmentService(_unitOfWorkMock.Object, _mapperConfig);
        }

        [Fact]
        public async Task GetAllAssignments_ShouldReturnCorrectData()
        {
            var mockdata = new Pagination<Assignment>
            {
                Items = _fixture.Build<Assignment>()
                                .Without(x => x.AssignmentQuestions)
                                .Without(x => x.Unit)
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
            var expected = _mapperConfig.Map<Pagination<AssignmentViewModel>>(mockdata);
            _unitOfWorkMock.Setup(x => x.AssignmentRepository.ToPagination(0, 10)).ReturnsAsync(mockdata);
            _unitOfWorkMock.Setup(x => x.UserRepository.GetEntitiesByIdsAsync(It.IsAny<List<Guid?>>())).ReturnsAsync(user);
            //act
            var result = await _assignmentService.ViewAllAssignmentAsync();
            //assert
            _unitOfWorkMock.Verify(x => x.AssignmentRepository.ToPagination(0, 10), Times.Once());
        }

        [Fact]
        public async Task CreateAssignment_ShouldReturnCorrectData_WhenSuccessSaved()
        {
            //arrange
            var mocks = _fixture.Build<CreateAssignmentViewModel>()
                                .Create();
            _unitOfWorkMock.Setup(x => x.AssignmentRepository.AddAsync(It.IsAny<Assignment>()))
                           .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            //act
            var result = await _assignmentService.CreateAssignmentAsync(mocks);
            //assert
            _unitOfWorkMock.Verify(x => x.AssignmentRepository.AddAsync(It.IsAny<Assignment>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
        }

        [Fact]
        public async Task CreateAssignment_ShouldReturnNull_WhenFailedSave()
        {
            //arrange
            var mocks = _fixture.Build<CreateAssignmentViewModel>()
                                .Create();
            _unitOfWorkMock.Setup(x => x.AssignmentRepository.AddAsync(It.IsAny<Assignment>()))
                           .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            //act
            var result = await _assignmentService.CreateAssignmentAsync(mocks);
            //assert
            _unitOfWorkMock.Verify(x => x.AssignmentRepository.AddAsync(It.IsAny<Assignment>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAssignment_ShouldReturnCorrectData_WhenSuccessSaved()
        {
            //arrange
            var assignmentObj = _fixture.Build<Assignment>()
                                  .Without(x => x.AssignmentQuestions)
                                  .Without(x => x.Unit)
                                   .Create();
            _unitOfWorkMock.Setup(x => x.AssignmentRepository.GetByIdAsync(assignmentObj.Id))
                           .ReturnsAsync(assignmentObj);
            var updateAssignmentDataMock = _fixture.Build<UpdateAssignmentViewModel>()
                                         .Create();
            //act
            await _assignmentService.UpdateAssignment(assignmentObj.Id, updateAssignmentDataMock);
            var result = _mapperConfig.Map<UpdateAssignmentViewModel>(assignmentObj);
            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UpdateAssignmentViewModel>();
            result.AssignmentName.Should().Be(updateAssignmentDataMock.AssignmentName);
            // add more property ...
            _unitOfWorkMock.Verify(x => x.AssignmentRepository.Update(assignmentObj), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once);

        }

        [Fact]
        public async Task GetEnableAssignments_ShouldReturnCorrectData()
        {
            //arrange
            var assignmentMockData = new Pagination<Assignment>
            {
                Items = _fixture.Build<Assignment>()
                                .Without(x => x.AssignmentQuestions)
                                .Without(x => x.Unit)
                                .With(x => x.Status, Domain.Enum.StatusEnum.Status.Enable)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30
            };
            var expected = _mapperConfig.Map<Pagination<Assignment>>(assignmentMockData);
            _unitOfWorkMock.Setup(x => x.AssignmentRepository.GetEnableAssignmentAsync(0, 10)).ReturnsAsync(assignmentMockData);
            //act
            var result = await _assignmentService.GetEnableAssignments();
            //assert
            _unitOfWorkMock.Verify(x => x.AssignmentRepository.GetEnableAssignmentAsync(0, 10), Times.Once());
        }

        [Fact]
        public async Task GetDisableAssignments_ShouldReturnCorrectData()
        {
            //arrange
            var assignmentMockData = new Pagination<Assignment>
            {
                Items = _fixture.Build<Assignment>()
                                .Without(x => x.AssignmentQuestions)
                                .Without(x => x.Unit)
                                .With(x => x.Status, Domain.Enum.StatusEnum.Status.Disable)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30
            };
            var expected = _mapperConfig.Map<Pagination<Assignment>>(assignmentMockData);
            _unitOfWorkMock.Setup(x => x.AssignmentRepository.GetDisableAssignmentAsync(0, 10)).ReturnsAsync(assignmentMockData);
            //act
            var result = await _assignmentService.GetDisableAssignments();
            //assert
            _unitOfWorkMock.Verify(x => x.AssignmentRepository.GetDisableAssignmentAsync(0, 10), Times.Once());
        }

        [Fact]
        public async Task UpdateAssignment_ShouldReturnNull_WhenFailedSave()
        {
            //arrange
            var assignmentId = Guid.NewGuid();
            _unitOfWorkMock.Setup(x => x.AssignmentRepository.GetByIdAsync(assignmentId))
                           .ReturnsAsync(null as Assignment);
            var updateDataMock = _fixture.Build<UpdateAssignmentViewModel>()
                                         .Create();
            //act
            var result = await _assignmentService.UpdateAssignment(assignmentId, updateDataMock);
            //assert
            result.Should().BeNull();
            _unitOfWorkMock.Verify(x => x.AssignmentRepository.Update(It.IsAny<Assignment>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Never);
        }

        [Fact]
        public async Task GetAssignmentById_ShouldReturnCorrectData()
        {
            //arrange
            var assignmentObj = _fixture.Build<Assignment>()
                                .Without(x => x.AssignmentQuestions)
                                .Without(x => x.Unit)
                                        .Create();
            var expected = _mapperConfig.Map<AssignmentViewModel>(assignmentObj);
            _unitOfWorkMock.Setup(x => x.AssignmentRepository.GetByIdAsync(assignmentObj.Id)).ReturnsAsync(assignmentObj);
            var createBy = new User { Email = "mock@example.com" };
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(assignmentObj.CreatedBy)).ReturnsAsync(createBy);
            expected.CreatedBy = createBy.Email;

            //act
            var result = await _assignmentService.GetAssignmentById(assignmentObj.Id);
            //assert
            _unitOfWorkMock.Verify(x => x.AssignmentRepository.GetByIdAsync(assignmentObj.Id), Times.Once());
        }

        [Fact]
        public async Task GetAssignmentByUnitId_ShouldReturnCorrectData()
        {
            //arrange
            var id = Guid.NewGuid();
            var assignmentMockData = new Pagination<Assignment>
            {
                Items = _fixture.Build<Assignment>()
                                .Without(x => x.AssignmentQuestions)
                                .Without(x => x.Unit)
                                .With(x => x.UnitId, id)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };
            var asm = _mapperConfig.Map<Pagination<Assignment>>(assignmentMockData);
            _unitOfWorkMock.Setup(x => x.AssignmentRepository.GetAssignmentByUnitId(id, 0, 10)).ReturnsAsync(assignmentMockData);
            var expected = _mapperConfig.Map<Pagination<AssignmentViewModel>>(asm);
            //act
            var result = await _assignmentService.GetAssignmentByUnitId(id);
            //assert
            _unitOfWorkMock.Verify(x => x.AssignmentRepository.GetAssignmentByUnitId(id, 0, 10), Times.Once());
        }

        [Fact]
        public async Task GetAssignmentByName_ShouldReturnCorrectData()
        {
            //arrange
            var auditPlanMockData = new Pagination<Assignment>
            {
                Items = _fixture.Build<Assignment>()
                                .Without(x => x.AssignmentQuestions)
                                .Without(x => x.Unit)
                                .With(x => x.AssignmentName, "Mock")
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };
            var auditPlans = _mapperConfig.Map<Pagination<Assignment>>(auditPlanMockData);
            _unitOfWorkMock.Setup(x => x.AssignmentRepository.GetAssignmentByName("Mock", 0, 10)).ReturnsAsync(auditPlanMockData);
            var expected = _mapperConfig.Map<Pagination<AssignmentViewModel>>(auditPlans);
            //act
            var result = await _assignmentService.GetAssignmentByName("Mock");
            //assert
            _unitOfWorkMock.Verify(x => x.AssignmentRepository.GetAssignmentByName("Mock", 0, 10), Times.Once());
        }
    }
}

