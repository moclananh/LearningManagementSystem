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
            var result1 = _mapperConfig.Map<SyllabusOutputStandardViewModel>(syllabusOutputStandardMock);
            var result2 = _mapperConfig.Map<SyllabusOutputStandardViewModel>(syllabusOutputStandardMock);
            //assert
            result1.SyllabusId.Should().Be(syllabusOutputStandardMock.SyllabusId.ToString());
            result2.OutputStandardId.Should().Be(syllabusOutputStandardMock.OutputStandardId.ToString());
        }
    }
}
