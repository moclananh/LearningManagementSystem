using Application.Interfaces;
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
    }
}
