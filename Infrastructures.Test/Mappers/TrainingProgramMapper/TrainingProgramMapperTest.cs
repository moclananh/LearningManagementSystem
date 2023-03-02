using Application.ViewModels.TrainingProgramModels;
using Applications.ViewModels.TrainingProgramModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;

namespace Infrastructures.Tests.Mappers.TrainingProgramMapper
{
    public class TrainingProgramMapperTest : SetupTest
    {
        [Fact]
        public void TestTrainingProgramViewModelMapper()
        {
            //arrange
            var TrainingProgramMock = _fixture.Build<TrainingProgram>()
                .Without(t => t.ClassTrainingPrograms)
                .Without(t => t.TrainingProgramSyllabi)
                .Create();
            //act
            var result = _mapperConfig.Map<TrainingProgramViewModel>(TrainingProgramMock);
            //assert
            result.Id.Should().Be(TrainingProgramMock.Id.ToString());
        }

        [Fact]
        public void TestCreateTrainingProgramViewModelMapper()
        {
            //arrange
            var TrainingProgramMock = _fixture.Build<TrainingProgram>()
                .Without(t => t.ClassTrainingPrograms)
                .Without(t => t.TrainingProgramSyllabi)
                .Create();
            //act
            var result = _mapperConfig.Map<CreateTrainingProgramViewModel>(TrainingProgramMock);
            //assert
            result.TrainingProgramName.Should().Be(TrainingProgramMock.TrainingProgramName.ToString());
        }

        [Fact]
        public void TestUpdateTrainingProgramViewModelMapper()
        {
            //arrange
            var TrainingProgramMock = _fixture.Build<TrainingProgram>()
                .Without(t => t.ClassTrainingPrograms)
                .Without(t => t.TrainingProgramSyllabi)
                .Create();
            //act
            var result = _mapperConfig.Map<CreateTrainingProgramViewModel>(TrainingProgramMock);
            //assert
            result.TrainingProgramName.Should().Be(TrainingProgramMock.TrainingProgramName.ToString());
        }
    }
}
