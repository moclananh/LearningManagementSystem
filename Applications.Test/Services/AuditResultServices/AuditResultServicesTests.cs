using Application.Interfaces;
using Application.ViewModels.QuizzViewModels;
using Applications.ViewModels.AuditResultViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Moq;

namespace Applications.Tests.Services.AuditResultServices
{
    public class AuditResultServicesTests : SetupTest
    {
        private readonly IAuditResultServices _auditResultServices;
        public AuditResultServicesTests()
        {
            _auditResultServices = new Application.Services.AuditResultServices(_unitOfWorkMock.Object, _mapperConfig);
        }

        [Fact]
        public async Task UpdateAuditResult_ShouldReturnCorrectData_WhenSuccessSaved()
        {
            //arrange
            var auditResultObj = _fixture.Build<AuditResult>()
                                   .Without(x => x.AuditPlan)
                                   .Create();
            _unitOfWorkMock.Setup(x => x.AuditResultRepository.GetByIdAsync(auditResultObj.Id))
                           .ReturnsAsync(auditResultObj);
            var updateDataMock = _fixture.Build<UpdateAuditResultViewModel>()
                                         .Create();
            //act
            await _auditResultServices.UpdateAuditResult(auditResultObj.Id, updateDataMock);
            var result = _mapperConfig.Map<UpdateAuditResultViewModel>(auditResultObj);
            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UpdateAuditResultViewModel>();
            result.Score.Should().Be(updateDataMock.Score);
            // add more property ...
            _unitOfWorkMock.Verify(x => x.AuditResultRepository.Update(auditResultObj), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAuditResult_ShouldReturnNull_WhenFailedSave()
        {
            //arrange
            var auditResultId = Guid.NewGuid();
            _unitOfWorkMock.Setup(x => x.AuditResultRepository.GetByIdAsync(auditResultId))
                           .ReturnsAsync(null as AuditResult);
            var updateDataMock = _fixture.Build<UpdateAuditResultViewModel>()
                                         .Create();
            //act
            var result = await _auditResultServices.UpdateAuditResult(auditResultId, updateDataMock);
            //assert
            result.Should().BeNull();
            _unitOfWorkMock.Verify(x => x.AuditResultRepository.Update(It.IsAny<AuditResult>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Never);
        }

        [Fact]
        public async Task GetAuditResultByAuditPlanId_ShouldReturnCorrectData()
        {
            var auditPlanId = Guid.NewGuid();
            var auditResultObj = _fixture.Build<AuditResult>()
                                   .Without(x => x.AuditPlan)
                                   .Create();
            //act

            var expected = _mapperConfig.Map<AuditResultViewModel>(auditResultObj);
            var createBy = new User { Email = "mock@example.com" };
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(auditResultObj.CreatedBy)).ReturnsAsync(createBy);
            expected.CreatedBy = createBy.Email;
            _unitOfWorkMock.Setup(x => x.AuditResultRepository.GetByAuditPlanId(auditPlanId)).ReturnsAsync(auditResultObj);
            var result = _auditResultServices.GetByAudiPlanId(auditPlanId);

            // Assert
            _unitOfWorkMock.Verify(x => x.AuditResultRepository.GetByAuditPlanId(auditPlanId), Times.Once());
        }

        [Fact]
        public async Task GetAuditResultById_ShouldReturnCorrectData()
        {
            // Arrange
            var auditResultObj = _fixture.Build<AuditResult>()
                .Without(x => x.AuditPlan)
                .Create();
            var expected = _mapperConfig.Map<AuditResultViewModel>(auditResultObj);
            var createBy = new User { Email = "mock@example.com" };
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(auditResultObj.CreatedBy)).ReturnsAsync(createBy);
            expected.CreatedBy = createBy.Email;
            _unitOfWorkMock.Setup(x => x.AuditResultRepository.GetAuditResultById(auditResultObj.Id))
                .ReturnsAsync(auditResultObj);

            // Act
            var result = await _auditResultServices.GetAuditResultById(auditResultObj.Id);

            // Assert
            _unitOfWorkMock.Verify(x => x.AuditResultRepository.GetAuditResultById(auditResultObj.Id), Times.Once());

        }
    }
}
