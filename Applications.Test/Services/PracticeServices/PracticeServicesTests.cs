using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.PracticeViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Moq;

namespace Applications.Tests.Services.PracticeServices
{
    public class PracticeServicesTests : SetupTest
    {
        private readonly IPracticeService _practiceService;
        public PracticeServicesTests()
        {
            _practiceService = new PracticeService(_unitOfWorkMock.Object, _mapperConfig);
        }

        [Fact]
        public async Task GetPracticeById_ShouldReturnCorrectData()
        {
            var practiceMocks = _fixture.Build<Practice>()
                                .Without(x => x.Unit)
                                .Without(x => x.PracticeQuestions)
                                .Create();
            _unitOfWorkMock.Setup(x => x.PracticeRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(practiceMocks);
            var expected = _mapperConfig.Map<PracticeViewModel>(practiceMocks);
            //act
            var result = _practiceService.GetPracticeById(practiceMocks.Id);
            //assert
            result.Result.Should().BeEquivalentTo(expected);
        }
    }
}
