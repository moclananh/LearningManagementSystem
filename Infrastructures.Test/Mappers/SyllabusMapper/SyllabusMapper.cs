using Applications.ViewModels.SyllabusViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;

namespace Infrastructures.Tests.Mappers.SyllabusMapper
{
    public class SyllabusMapper : SetupTest
    {
        [Fact]
        public void TestSyllabusViewMapper()
        {
            //arrange
            var syllabusMock = _fixture.Build<Syllabus>()
                               .Without(s => s.TrainingProgramSyllabi)
                               .Without(s => s.SyllabusOutputStandards)
                               .Without(s => s.SyllabusModules)
                               .Create();

            //act
            var result = _mapperConfig.Map<SyllabusViewModel>(syllabusMock);

            //assert
            result.Id.Should().Be(syllabusMock.Id.ToString());
        }

        [Fact]
        public void TestCreateSyllabusViewMapper()
        {
            //arrange
            var syllabusMock = _fixture.Build<Syllabus>()
                               .Without(s => s.TrainingProgramSyllabi)
                               .Without(s => s.SyllabusOutputStandards)
                               .Without(s => s.SyllabusModules)
                               .Create();

            //act
            var result = _mapperConfig.Map<CreateSyllabusViewModel>(syllabusMock);

            //assert
            result.SyllabusName.Should().Be(syllabusMock.SyllabusName.ToString());
        }

        [Fact]
        public void TestUpdateSyllabusViewMapper()
        {
            //arrange
            var syllabusMock = _fixture.Build<Syllabus>()
                               .Without(s => s.TrainingProgramSyllabi)
                               .Without(s => s.SyllabusOutputStandards)
                               .Without(s => s.SyllabusModules)
                               .Create();

            //act
            var result = _mapperConfig.Map<UpdateSyllabusViewModel>(syllabusMock);

            //assert
            result.SyllabusName.Should().Be(syllabusMock.SyllabusName.ToString());
        }
    }
}
