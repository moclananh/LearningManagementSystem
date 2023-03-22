using Application.ViewModels.QuizzViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;

namespace Infrastructures.Tests.Mappers.QuizzMapper
{
    public class QuizzMapper : SetupTest
    {
        [Fact]
        public void TestQuizzViewModel()
        {
            //arrange
            var quizzMock = _fixture.Build<Quizz>()
                            .Without(X => X.QuizzQuestions)
                            .Without(x => x.Unit)
                            .Create();
            //act
            var result = _mapperConfig.Map<QuizzViewModel>(quizzMock);

            //assert
            result.Id.Should().Be(quizzMock.Id.ToString());
        }
        [Fact]
        public void TestCreateQuizzViewModel()
        {
            //arrange
            var quizzMock = _fixture.Build<Quizz>()
                            .Without(X => X.QuizzQuestions)
                            .Without(x => x.Unit)
                            .Create();
            //act
            var result = _mapperConfig.Map<CreateQuizzViewModel>(quizzMock); 

            //assert
            result.QuizzName.Should().Be(quizzMock.QuizzName.ToString());
        }
        [Fact]
        public void TestUpdateQuizzViewModel()
        {
            //arrange
            var quizzMock = _fixture.Build<Quizz>()
                            .Without(X => X.QuizzQuestions)
                            .Without(x => x.Unit)
                            .Create();
            //act
            var result = _mapperConfig.Map<UpdateQuizzViewModel>(quizzMock);

            //assert
            result.QuizzName.Should().Be(quizzMock.QuizzName.ToString());
        }
    }
}
