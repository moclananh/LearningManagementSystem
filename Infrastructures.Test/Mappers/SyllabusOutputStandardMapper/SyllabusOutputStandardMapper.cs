using Applications.ViewModels.SyllabusOutputStandardViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Mappers;

namespace Infrastructures.Tests.Mappers.SyllabusOutputStandardMapper
{
    public class SyllabusOutputStandardMapper : SetupTest
    {
        [Fact]
        public void TestSyllabusOutputStandardViewMapper()
        {
            //arrange
            var syllabusOutputStandardMock = _fixture.Build<SyllabusOutputStandard>()
                               .Without(s => s.Syllabus)
                               .Without(s => s.OutputStandard)
                               .Create();

            //act
            var result = _mapperConfig.Map<SyllabusOutputStandardViewModel>(syllabusOutputStandardMock);
            //assert
            result.SyllabusId.Should().Be(syllabusOutputStandardMock.SyllabusId.ToString());
            result.OutputStandardId.Should().Be(syllabusOutputStandardMock.OutputStandardId.ToString());
        }
    }
}
