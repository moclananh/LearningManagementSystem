using Applications.Commons;
using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.PracticeQuestionViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using Moq;

namespace Applications.Tests.Services.PracticeQuestionServices
{
    public class PracticeQuestionServiceTest : SetupTest
    {
        private readonly IPracticeQuestionService _practiceQuestionService;
        public PracticeQuestionServiceTest()
        {
            _practiceQuestionService = new PracticeQuestionService(_unitOfWorkMock.Object, _mapperConfig);
        }

        [Fact]
        public async Task GetPracticeQuestionById_ShouldReturnCorrectData()
        {
            //arrange
            var id = Guid.NewGuid();
            var MockData = new Pagination<PracticeQuestion>
            {
                Items = _fixture.Build<PracticeQuestion>()
                                .Without(x => x.Practice)
                                .With(x => x.PracticeId, id)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };
            var practiceQuestion = _mapperConfig.Map<Pagination<PracticeQuestion>>(MockData);
            _unitOfWorkMock.Setup(x => x.PracticeQuestionRepository.GetAllPracticeQuestionById(id, 0, 10)).ReturnsAsync(MockData);
            var expected = _mapperConfig.Map<Pagination<PracticeQuestionViewModel>>(practiceQuestion);
            //act
            var result = await _practiceQuestionService.GetPracticeQuestionByPracticeId(id);
            //assert
            _unitOfWorkMock.Verify(x => x.PracticeQuestionRepository.GetAllPracticeQuestionById(id, 0, 10), Times.Once());
        }
    }
}
