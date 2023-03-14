using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.ClassViewModels;
using Applications.ViewModels.OutputStandardViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Moq;

namespace Applications.Tests.Services.OutputStandardServices
{
    public class OutputStandardServiceTest : SetupTest
    {
        private readonly IOutputStandardService _outputStandardService;
        public OutputStandardServiceTest()
        {
            _outputStandardService = new Applications.Services.OutputStandardService(_unitOfWorkMock.Object, _mapperConfig);
        }

        [Fact]
        public async Task UpdateOutputStandard_ShouldReturnCorrectData_WhenSuccessSaved()
        {
            //arrange
            var outputStandardObj = _fixture.Build<OutputStandard>()
                                    .Without(x => x.Description)
                                    .Without(x => x.OutputStandardCode)
                                    .Without(x => x.SyllabusOutputStandards)
                                   .Create();
            _unitOfWorkMock.Setup(x => x.OutputStandardRepository.GetByIdAsync(outputStandardObj.Id))
                           .ReturnsAsync(outputStandardObj);
            var updateDataMock = _fixture.Build<UpdateOutputStandardViewModel>()
                                         .Create();
            //act
            await _outputStandardService.UpdatOutputStandardAsync(outputStandardObj.Id, updateDataMock);
            var result = _mapperConfig.Map<UpdateOutputStandardViewModel>(outputStandardObj);
            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UpdateOutputStandardViewModel>();
            result.OutputStandardCode.Should().Be(updateDataMock.OutputStandardCode);
            // add more property ...
            _unitOfWorkMock.Verify(x => x.OutputStandardRepository.Update(outputStandardObj), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateOutputStandard_ShouldReturnNull_WhenFailedSave()
        {
            //arrange
            var OutputStandardId = Guid.NewGuid();
            _unitOfWorkMock.Setup(x => x.OutputStandardRepository.GetByIdAsync(OutputStandardId))
                           .ReturnsAsync(null as OutputStandard);
            var updateDataMock = _fixture.Build<UpdateOutputStandardViewModel>()
                                         .Create();
            //act
            var result = await _outputStandardService.UpdatOutputStandardAsync(OutputStandardId, updateDataMock);
            //assert
            result.Should().BeNull();
            _unitOfWorkMock.Verify(x => x.OutputStandardRepository.Update(It.IsAny<OutputStandard>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Never);
        }
        [Fact]
        public async Task CreateOutputStandard_ShouldReturnCorrectData_WhenSuccessSaved()
        {
            //arrange
            var mocks = _fixture.Build<CreateOutputStandardViewModel>()
                                .Create();
            _unitOfWorkMock.Setup(x => x.OutputStandardRepository.AddAsync(It.IsAny<OutputStandard>()))
                           .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            //act
            var result = await _outputStandardService.CreateOutputStandardAsync(mocks);
            //assert
            _unitOfWorkMock.Verify(x => x.OutputStandardRepository.AddAsync(It.IsAny<OutputStandard>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
        }
        [Fact]
        public async Task CreateOutputStandard_ShouldReturnNull_WhenFailedSave()
        {
            //arrange
            var mocks = _fixture.Build<CreateOutputStandardViewModel>()
                                .Create();
            _unitOfWorkMock.Setup(x => x.OutputStandardRepository.AddAsync(It.IsAny<OutputStandard>()))
                           .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            //act
            var result = await _outputStandardService.CreateOutputStandardAsync(mocks);
            //assert
            _unitOfWorkMock.Verify(x => x.OutputStandardRepository.AddAsync(It.IsAny<OutputStandard>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
            result.Should().BeNull();
        }

    }
}
