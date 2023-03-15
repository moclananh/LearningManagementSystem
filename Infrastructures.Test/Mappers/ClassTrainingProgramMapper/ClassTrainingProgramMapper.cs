
using Applications.ViewModels.ClassTrainingProgramViewModels;
using Applications.ViewModels.ClassUserViewModels;
using AutoFixture;
using Domain.EntityRelationship;
using Domain.Tests;
using FluentAssertions;

namespace Infrastructures.Tests.Mappers.ClassTrainingProgramMapper
{
    public class ClassTrainingProgramMapper : SetupTest
    {
        [Fact]
        public void TestClassTrainingProgramViewMapper()
        {
            //arrange
            var trainingProgramMock = _fixture.Build<ClassTrainingProgram>()
                               .Without(s => s.Class)
                               .Without(s => s.TrainingProgram)
                               .Create();
            //act
            var result = _mapperConfig.Map<ClassTrainingProgramViewModel>(trainingProgramMock);
            //assert
            result.ClassId.Should().Be(trainingProgramMock.ClassId.ToString());
            result.TrainingProgramId.Should().Be(trainingProgramMock.TrainingProgramId.ToString());
        }
    }
}
