using Applications.ViewModels.AssignmentViewModels;
using Applications.ViewModels.OutputStandardViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;

namespace Infrastructures.Tests.Mappers.OutputStandardMapper
{
    public class OutputStandardMapper : SetupTest
    {
        [Fact]
        public void TestViewMapper()
        {
            //arrange
            var outputStandardMock = _fixture.Build<OutputStandard>()
                                .Without(x => x.SyllabusOutputStandards)
                                .Create();
            //act
            var result = _mapperConfig.Map<OutputStandardViewModel>(outputStandardMock);
            //assert
            result.OutputStandardCode.Should().Be(outputStandardMock.OutputStandardCode.ToString());
        }

        [Fact]
        public void TestCreateViewMapper()
        {
            //arrange
            var outputStandardMock = _fixture.Build<OutputStandard>()
                                .Without(x => x.SyllabusOutputStandards)
                                .Create();
            //act
            var result = _mapperConfig.Map<CreateOutputStandardViewModel>(outputStandardMock);
            //assert
            result.OutputStandardCode.Should().Be(outputStandardMock.OutputStandardCode.ToString());
        }

        [Fact]
        public void TestUpdateViewMapper()
        {
            //arrange
            var outputStandardMock = _fixture.Build<OutputStandard>()
                                .Without(x => x.SyllabusOutputStandards)
                                .Create();
            //act
            var result = _mapperConfig.Map<UpdateOutputStandardViewModel>(outputStandardMock);
            //assert
            result.OutputStandardCode.Should().Be(outputStandardMock.OutputStandardCode.ToString());
        }
    }
}
