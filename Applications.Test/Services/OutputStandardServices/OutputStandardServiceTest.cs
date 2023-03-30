using Applications.Commons;
using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.ModuleUnitViewModels;
using Applications.ViewModels.OutputStandardViewModels;
using Applications.ViewModels.SyllabusOutputStandardViewModels;
using AutoFixture;
using AutoMapper;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Tests;
using FluentAssertions;
using Moq;
using System.Net;

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

        [Fact]
        public async Task GetOuputStandardById_ShouldReturnCorrectData()
        {
            //arrange
            var mockdata = _fixture.Build<OutputStandard>()
                .Without(x => x.Description)
                .Without(x => x.SyllabusOutputStandards)
                                   .Create();
            var expected = _mapperConfig.Map<OutputStandardViewModel>(mockdata);
            var createBy = new User { Email = "mock@example.com" };
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(mockdata.CreatedBy)).ReturnsAsync(createBy);
            expected.CreatedBy = createBy.Email;
            _unitOfWorkMock.Setup(x => x.OutputStandardRepository.GetByIdAsync(mockdata.Id))
                           .ReturnsAsync(mockdata);
            //act
            var result = await _outputStandardService.GetOutputStandardByOutputStandardIdAsync(mockdata.Id);
            //assert
            _unitOfWorkMock.Verify(x => x.OutputStandardRepository.GetByIdAsync(mockdata.Id), Times.Once());
        }

        [Fact]
        public async Task GetOutputStandardBySyllabusId_ShouldReturnCorrectData()
        {
            //arrange
            var id = Guid.NewGuid();
            var Mock = new Pagination<OutputStandard>()
            {
                Items = _fixture.Build<OutputStandard>()
                                .Without(x => x.Description)
                                .Without(x => x.SyllabusOutputStandards)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };

            var outputStandardObj = _mapperConfig.Map<Pagination<OutputStandardViewModel>>(Mock);
            _unitOfWorkMock.Setup(x => x.OutputStandardRepository.GetOutputStandardBySyllabusIdAsync(id, 0, 10)).ReturnsAsync(Mock);
            var expected = _mapperConfig.Map<Pagination<OutputStandardViewModel>>(outputStandardObj);
            //act
            var result = await _outputStandardService.GetOutputStandardBySyllabusIdAsync(id);
            //assert
            _unitOfWorkMock.Verify(x => x.OutputStandardRepository.GetOutputStandardBySyllabusIdAsync(id, 0, 10), Times.Once());
        }

        [Fact]
        public async Task AddOutputStandardToSyllabus_ShouldReturnCorrectData()
        {
            //arrange
            var OutputStandardMockData = _fixture.Build<OutputStandard>()
                                       .Without(x => x.SyllabusOutputStandards)
                                       .Create();
            var SyllabusMockData = _fixture.Build<Syllabus>()
                                         .Without(x => x.TrainingProgramSyllabi)
                                         .Without(x => x.SyllabusOutputStandards)
                                         .Without(x => x.SyllabusModules)
                                         .Create();
            var mockData = new SyllabusOutputStandard()
            {
                OutputStandard = OutputStandardMockData,
                Syllabus = SyllabusMockData
            };
            _unitOfWorkMock.Setup(x => x.OutputStandardRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(OutputStandardMockData);
            _unitOfWorkMock.Setup(x => x.SyllabusRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(SyllabusMockData);
            _unitOfWorkMock.Setup(x => x.SyllabusOutputStandardRepository.AddAsync(It.IsAny<SyllabusOutputStandard>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            var expected = _mapperConfig.Map<CreateSyllabusOutputStandardViewModel>(mockData);
            //act
            var result = await _outputStandardService.AddOutputStandardToSyllabus(OutputStandardMockData.Id, SyllabusMockData.Id);
            //assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task AddOutputStandardToSyllabus_ShouldReturnNull_WhenNotFoundModule()
        {
            //arrange
            var OutputStandardMockData = _fixture.Build<OutputStandard>()
                                       .Without(x => x.SyllabusOutputStandards)
                                       .Create();
            var SyllabusMockData = _fixture.Build<Syllabus>()
                                         .Without(x => x.TrainingProgramSyllabi)
                                         .Without(x => x.SyllabusOutputStandards)
                                         .Without(x => x.SyllabusModules)
                                         .Create();
            var mockData = new SyllabusOutputStandard()
            {
                OutputStandard = OutputStandardMockData,
                Syllabus = SyllabusMockData
            };
            _unitOfWorkMock.Setup(x => x.OutputStandardRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(OutputStandardMockData);
            _unitOfWorkMock.Setup(x => x.SyllabusRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(SyllabusMockData);
            _unitOfWorkMock.Setup(x => x.SyllabusOutputStandardRepository.AddAsync(It.IsAny<SyllabusOutputStandard>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            var expected = _mapperConfig.Map<CreateSyllabusOutputStandardViewModel>(mockData);
            //act
            var result = await _outputStandardService.AddOutputStandardToSyllabus(OutputStandardMockData.Id, SyllabusMockData.Id);
            //assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllOutputStandard_ShouldReturnCorrectData()
        {
            //arrange
            var MockData = new Pagination<OutputStandard>
            {
                Items = _fixture.Build<OutputStandard>()
                                .Without(x => x.SyllabusOutputStandards)
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
            var expected = _mapperConfig.Map<Pagination<OutputStandardViewModel>>(MockData);
            _unitOfWorkMock.Setup(x => x.OutputStandardRepository.ToPagination(0, 10)).ReturnsAsync(MockData);
            _unitOfWorkMock.Setup(x => x.UserRepository.GetEntitiesByIdsAsync(It.IsAny<List<Guid?>>())).ReturnsAsync(user);
            //act
            var result = await _outputStandardService.GetAllOutputStandardAsync();
            //assert
            _unitOfWorkMock.Verify(x => x.OutputStandardRepository.ToPagination(0, 10), Times.Once());
        }
        [Fact]
        public async Task RemoveOutputStandardToSyllabus_ShouldRemoveOutputStandardFromSyllabus()
        {
            // Arrange
            var outputStandardId = Guid.NewGuid();
            var syllabusId = Guid.NewGuid();
            var outputStandard = new SyllabusOutputStandard
            {
                Id = outputStandardId,
                SyllabusId = syllabusId,
            };
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(uow => uow.SyllabusOutputStandardRepository.GetSyllabusOutputStandard(syllabusId, outputStandardId))
                          .ReturnsAsync(outputStandard);
            var mockMapper = new Mock<IMapper>();
            var service = new OutputStandardService(mockUnitOfWork.Object, mockMapper.Object);

            // Act
            var result = await service.RemoveOutputStandardToSyllabus(syllabusId, outputStandardId);

            // Assert
            mockUnitOfWork.Verify(uow => uow.SyllabusOutputStandardRepository.SoftRemove(outputStandard), Times.Once);
            mockUnitOfWork.Verify(uow => uow.SaveChangeAsync(), Times.Once);
            Assert.Null(result);
        }
    }
}
