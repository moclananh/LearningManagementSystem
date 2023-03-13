using Application.ViewModels.QuizzViewModels;
using Applications.Commons;
using Applications.Interfaces;
using Applications.Services;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Moq;

namespace Applications.Tests.Services.QuizzServices
{
    public class QuizzServiceTest : SetupTest
    {
        private readonly IQuizzService _quizzService;
        public QuizzServiceTest()
        {
            _quizzService = new QuizzService(_unitOfWorkMock.Object, _mapperConfig);
        }

       /* [Fact]
        public async Task GetAllQuizzes_ShouldReturnCorrectData()
        {
            //arrange
            var MockData = new Pagination<Quizz>
            {
                Items = _fixture.Build<Quizz>()
                                .Without(x => x.QuizzQuestions)
                                .Without(x => x.Unit)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30
            };
            var expected = _mapperConfig.Map<Pagination<Quizz>>(MockData);
            _unitOfWorkMock.Setup(x => x.QuizzRepository.ToPagination(0, 10)).ReturnsAsync(MockData);
            //act
            var result = await _quizzService.GetAllQuizzes();
            //assert
            _unitOfWorkMock.Verify(x => x.QuizzRepository.ToPagination(0, 10), Times.Once());
        }*/

        [Fact]
        public async Task CreateQuizz_ShouldReturnCorrectData_WhenSuccessSaved()
        {
            //arrange
            var mocks = _fixture.Build<CreateQuizzViewModel>()
                                .Create();
            _unitOfWorkMock.Setup(x => x.QuizzRepository.AddAsync(It.IsAny<Quizz>()))
                           .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);
            //act
            var result = await _quizzService.CreateQuizzAsync(mocks);
            //assert
            _unitOfWorkMock.Verify(x => x.QuizzRepository.AddAsync(It.IsAny<Quizz>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
        }

        [Fact]
        public async Task CreateQuizz_ShouldReturnNull_WhenFailedSave()
        {
            //arrange
            var mocks = _fixture.Build<CreateQuizzViewModel>()
                                .Create();
            _unitOfWorkMock.Setup(x => x.QuizzRepository.AddAsync(It.IsAny<Quizz>()))
                           .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            //act
            var result = await _quizzService.CreateQuizzAsync(mocks);
            //assert
            _unitOfWorkMock.Verify(x => x.QuizzRepository.AddAsync(It.IsAny<Quizz>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateQuizz_ShouldReturnCorrectData_WhenSuccessSaved()
        {
            //arrange
            var quizzObj = _fixture.Build<Quizz>()
                                   .Without(x => x.QuizzQuestions)
                                   .Without(x => x.Unit)
                                   .Create();
            _unitOfWorkMock.Setup(x => x.QuizzRepository.GetByIdAsync(quizzObj.Id))
                           .ReturnsAsync(quizzObj);
            var updateQuizzDataMock = _fixture.Build<UpdateQuizzViewModel>()
                                         .Create();
            //act
            await _quizzService.UpdatQuizzAsync(quizzObj.Id, updateQuizzDataMock);
            var result = _mapperConfig.Map<UpdateQuizzViewModel>(quizzObj);
            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UpdateQuizzViewModel>();
            result.QuizzName.Should().Be(updateQuizzDataMock.QuizzName);
            // add more property ...
            _unitOfWorkMock.Verify(x => x.QuizzRepository.Update(quizzObj), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateQuizz_ShouldReturnNull_WhenFailedSave()
        {
            //arrange
            var quizzId = Guid.NewGuid();
            _unitOfWorkMock.Setup(x => x.QuizzRepository.GetByIdAsync(quizzId))
                            .ReturnsAsync(null as Quizz);
            var updateQuizzDataMock = _fixture.Build<UpdateQuizzViewModel>()
                                                    .Create();
            //act
            var result = await _quizzService.UpdatQuizzAsync(quizzId, updateQuizzDataMock);
            //assert
            result.Should().BeNull();
            _unitOfWorkMock.Verify(x => x.QuizzRepository.Update(It.IsAny<Quizz>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Never);
        }

        [Fact]
        public async Task GetQuizzByName_ShouldReturnCorrectData()
        {
            //arrange
            var MockData = new Pagination<Quizz>
            {
                Items = _fixture.Build<Quizz>()
                                .Without(x => x.QuizzQuestions)
                                .Without(x => x.Unit)
                                .With(x => x.QuizzName, "Mock")
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };
            var quizzes = _mapperConfig.Map<Pagination<Quizz>>(MockData);
            _unitOfWorkMock.Setup(x => x.QuizzRepository.GetQuizzByName("Mock", 0, 10)).ReturnsAsync(MockData);
            var expected = _mapperConfig.Map<Pagination<QuizzViewModel>>(quizzes);
            //act
            var result = await _quizzService.GetQuizzByName("Mock");
            //assert
            _unitOfWorkMock.Verify(x => x.QuizzRepository.GetQuizzByName("Mock", 0, 10), Times.Once());
        }

        [Fact]
        public async Task GetEnableQuizz_ShouldReturnCorrectData()
        {
            //arrange
            var MockData = new Pagination<Quizz>
            {
                Items = _fixture.Build<Quizz>()
                                .Without(x => x.QuizzQuestions)
                                .Without(x => x.Unit)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };
            var units = _mapperConfig.Map<Pagination<Quizz>>(MockData);
            _unitOfWorkMock.Setup(x => x.QuizzRepository.GetEnableQuizzes(0, 10)).ReturnsAsync(MockData);
            var expected = _mapperConfig.Map<Pagination<QuizzViewModel>>(units);
            //act
            var result = await _quizzService.GetEnableQuizzes();
            //assert
            _unitOfWorkMock.Verify(x => x.QuizzRepository.GetEnableQuizzes(0, 10), Times.Once());
        }

        [Fact]
        public async Task GetDisableQuizz_ShouldReturnCorrectData()
        {
            //arrange
            var MockData = new Pagination<Quizz>
            {
                Items = _fixture.Build<Quizz>()
                                .Without(x => x.QuizzQuestions)
                                .Without(x => x.Unit)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };
            var units = _mapperConfig.Map<Pagination<Quizz>>(MockData);
            _unitOfWorkMock.Setup(x => x.QuizzRepository.GetDisableQuizzes(0, 10)).ReturnsAsync(MockData);
            var expected = _mapperConfig.Map<Pagination<QuizzViewModel>>(units);
            //act
            var result = await _quizzService.GetDisableQuizzes();
            //assert
            _unitOfWorkMock.Verify(x => x.QuizzRepository.GetDisableQuizzes(0, 10), Times.Once());
        }

    }
}

