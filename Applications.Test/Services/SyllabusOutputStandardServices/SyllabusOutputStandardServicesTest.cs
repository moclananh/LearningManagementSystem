using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.Response;
using Applications.ViewModels.SyllabusOutputStandardViewModels;
using AutoFixture;
using AutoMapper;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Tests;
using FluentAssertions;
using Moq;
using System.Net;

namespace Applications.Tests.Services.SyllabusOutputStandardServices
{
    public class SyllabusOutputStandardServicesTest : SetupTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapperConfig;
        private readonly SyllabusOutputStandardService _moduleService;

        public SyllabusOutputStandardServicesTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SyllabusOutputStandard, CreateSyllabusOutputStandardViewModel>()
                    .ForMember(dest => dest.OutputStandardId, opt => opt.MapFrom(src => src.OutputStandard.Id));
            }).CreateMapper();
            _moduleService = new SyllabusOutputStandardService(_unitOfWorkMock.Object, _mapperConfig);
        }

        [Fact]
        public async Task AddMultipleOutputStandardsToSyllabus_WhenSyllabusNotFound_ShouldReturnNotFoundResponse()
        {
            // Arrange
            var syllabusId = Guid.NewGuid();
            var outputStandardIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            _unitOfWorkMock.Setup(x => x.SyllabusRepository.GetByIdAsync(syllabusId)).ReturnsAsync(null as Syllabus);

            // Act
            var result = await _moduleService.AddMultipleOutputStandardsToSyllabus(syllabusId, outputStandardIds);

            // Assert
            result.Should().NotBeNull();
            result.Message.Should().Be("Syllabus Not Found");
        }

        [Fact]
        public async Task AddMultipleOutputStandardsToSyllabus_WhenOutputStandardNotFound_ShouldReturnNotFoundResponse()
        {
            // Arrange
            var syllabusId = Guid.NewGuid();
            var outputStandardIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            var syllabusObj = new Syllabus();
            _unitOfWorkMock.Setup(x => x.SyllabusRepository.GetByIdAsync(syllabusId)).ReturnsAsync(syllabusObj);
            _unitOfWorkMock.Setup(x => x.OutputStandardRepository.GetByIdAsync(outputStandardIds[0])).ReturnsAsync(null as OutputStandard);

            // Act
            var result = await _moduleService.AddMultipleOutputStandardsToSyllabus(syllabusId, outputStandardIds);

            // Assert
            result.Should().NotBeNull();
            result.Message.Should().Be("Output Standard Not Found");
        }
        [Fact]
        public async Task AddMultipleOutputStandardsToSyllabus_ShouldReturnCorrectData()
        {
            // Arrange
            var syllabusMockData = new Syllabus();
            var outputStandardMockData = new List<OutputStandard>
            {
                new OutputStandard(),
                new OutputStandard()
            };
            var outputStandardIds = new List<Guid>();
            foreach (var item in outputStandardMockData)
            {
                outputStandardIds.Add(item.Id);
            }

            _unitOfWorkMock.Setup(x => x.SyllabusRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(syllabusMockData);
            foreach (var item in outputStandardMockData)
            {
                _unitOfWorkMock.Setup(x => x.OutputStandardRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(item);
            }
            _unitOfWorkMock.Setup(x => x.SyllabusOutputStandardRepository.AddAsync(It.IsAny<SyllabusOutputStandard>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            var expected = new Response(HttpStatusCode.OK, "Output Standards Added Successfully");

            // Configure AutoFixture to omit circular references
            var fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Act
            var result = await _moduleService.AddMultipleOutputStandardsToSyllabus(syllabusMockData.Id, outputStandardIds);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
