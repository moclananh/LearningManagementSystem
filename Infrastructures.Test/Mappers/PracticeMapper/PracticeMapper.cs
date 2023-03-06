using Applications.ViewModels.PracticeViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;

namespace Infrastructures.Tests.Mappers.PracticeMapper
{
    public class PracticeMapper: SetupTest
    {
        [Fact]
        public void TestPracticeViewModel()
        {
            //arrange
            var practiceMock = _fixture.Build<Practice>()
                            .Without(X => X.PracticeQuestions)
                            .Without(x => x.Unit)
                            .Create();
            //act
            var result = _mapperConfig.Map<PracticeViewModel>(practiceMock);

            //assert
            result.PracticeName.Should().Be(practiceMock.PracticeName.ToString());
        }
        [Fact]
        public void TestCreatePracticeViewModel()
        {
            //arrange
            var practiceMock = _fixture.Build<Practice>()
                            .Without(X => X.PracticeQuestions)
                            .Without(x => x.Unit)
                            .Create();
            //act
            var result = _mapperConfig.Map<CreatePracticeViewModel>(practiceMock);

            //assert
            result.PracticeName.Should().Be(practiceMock.PracticeName.ToString());
        }
        [Fact]
        public void TestUpdatePracticeViewModel()
        {
            //arrange
            var quizzMock = _fixture.Build<Practice>()
                            .Without(X => X.PracticeQuestions)
                            .Without(x => x.Unit)
                            .Create();
            //act
            var result = _mapperConfig.Map<UpdatePracticeViewModel>(quizzMock);

            //assert
            result.PracticeName.Should().Be(quizzMock.PracticeName.ToString());
        }
    }
}
