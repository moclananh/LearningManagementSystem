using Application.ViewModels.QuizzViewModels;
using Applications.Commons;
using Applications.Interfaces;
using Applications.Services;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Repositories;
using Moq;
using System.Net;

namespace Applications.Tests.Services.QuizzServices
{
    public class QuizzServiceTest : SetupTest
    {
        private readonly IQuizzService _quizzService;
        public QuizzServiceTest()
        {
            _quizzService = new QuizzService(_unitOfWorkMock.Object, _mapperConfig);
        }

        [Fact]
        public async Task GetAllQuizzes_ShouldReturnCorrectData()
        {
            //arrange
            var mockdata = new Pagination<Quizz>
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
            var user = _fixture.Build<User>()
                               .Without(x => x.UserAuditPlans)
                               .Without(x => x.AbsentRequests)
                               .Without(x => x.ClassUsers)
                               .Without(x => x.Attendences)
                               .CreateMany(3)
                               .ToList();
            var expected = _mapperConfig.Map<Pagination<QuizzViewModel>>(mockdata);
            _unitOfWorkMock.Setup(x => x.QuizzRepository.ToPagination(0, 10)).ReturnsAsync(mockdata);
            _unitOfWorkMock.Setup(x => x.UserRepository.GetEntitiesByIdsAsync(It.IsAny<List<Guid?>>())).ReturnsAsync(user);
            //act
            var result = await _quizzService.GetAllQuizzes();
            //assert
            _unitOfWorkMock.Verify(x => x.QuizzRepository.ToPagination(0, 10), Times.Once());
        }

        [Fact]
        public async Task GetQuizzById_ShouldReturnCorrectData()
        {
            //arrange
            var mockdata = _fixture.Build<Quizz>()
                                   .Without(x => x.QuizzQuestions)
                                   .Without(x => x.Unit)
                                   .Create();
            var expected = _mapperConfig.Map<QuizzViewModel>(mockdata);
            var createBy = new User { Email = "mock@example.com" };
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(mockdata.CreatedBy)).ReturnsAsync(createBy);
            expected.CreatedBy = createBy.Email;
            _unitOfWorkMock.Setup(x => x.QuizzRepository.GetByIdAsync(mockdata.Id))
                           .ReturnsAsync(mockdata);
            //act
            var result = await _quizzService.GetQuizzByQuizzIdAsync(mockdata.Id);
            //assert
            _unitOfWorkMock.Verify(x => x.QuizzRepository.GetByIdAsync(mockdata.Id), Times.Once());
        }

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
            Assert.Equal(HttpStatusCode.BadRequest.ToString(), result.Status);
            Assert.Equal("Create Failed", result.Message);
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
            await _quizzService.UpdateQuizzAsync(quizzObj.Id, updateQuizzDataMock);
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
            var result = await _quizzService.UpdateQuizzAsync(quizzId, updateQuizzDataMock);
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

        [Fact]
        public async Task GetQuizzByUnitId_ShouldReturnCorrectData()
        {
            //arrange
            var id = Guid.NewGuid();
            var Mock = new Pagination<Quizz>()
            {
                Items = _fixture.Build<Quizz>()
                                .Without(x => x.Unit)
                                .Without(x => x.QuizzQuestions)
                                .With(x => x.UnitId, id)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };

            var quizz = _mapperConfig.Map<Pagination<Quizz>>(Mock);
            _unitOfWorkMock.Setup(x => x.QuizzRepository.GetQuizzByUnitIdAsync(id, 0, 10)).ReturnsAsync(Mock);
            var expected = _mapperConfig.Map<Pagination<QuizzViewModel>>(quizz);
            //act
            var result = await _quizzService.GetQuizzByUnitIdAsync(id);
            //assert
            _unitOfWorkMock.Verify(x => x.QuizzRepository.GetQuizzByUnitIdAsync(id, 0, 10), Times.Once());
        }
    }
}

