using Applications.ViewModels.TrainingProgramSyllabi;
using AutoFixture;
using Domain.EntityRelationship;
using Domain.Tests;
using FluentAssertions;

namespace Infrastructures.Tests.Mappers.TrainingProgramSyllabusMapper
{
    public class TrainingProgramSyllabusMapperTest : SetupTest
    {
        [Fact]
        public void TestTrainingProgramViewModelMapper()
        {
            //arrange
            var TrainingProgramSyllabusMock = _fixture.Build<TrainingProgramSyllabus>()
                .Without(t => t.TrainingProgram)
                .Without(t => t.Syllabus)
                .Create();
            //act
            var result = _mapperConfig.Map<TrainingProgramSyllabiView>(TrainingProgramSyllabusMock);
            //assert
            result.TrainingProgramId.Should().Be(TrainingProgramSyllabusMock.TrainingProgramId.ToString());
            result.SyllabusId.Should().Be(TrainingProgramSyllabusMock.SyllabusId.ToString());
        }
    }
}
