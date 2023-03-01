using Application.ViewModels.UnitViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;

namespace Infrastructures.Tests.Mappers.UnitMapper
{
    public class UnitMapper : SetupTest
    {
        [Fact]
        public void TestUnitViewModel()
        {
            //arrange
            var quizzMock = _fixture.Build<Unit>()
                            .Without(x => x.Practices)
                            .Without(x => x.Lectures)
                            .Without(x => x.Assignments)
                            .Without(x => x.Quizzs)
                            .Without(x => x.ModuleUnits)
                            .Create();
            //act
            var result = _mapperConfig.Map<UnitViewModel>(quizzMock);

            //assert
            result.UnitId.Should().Be(quizzMock.Id.ToString());
        }
        [Fact]
        public void TestCreateUnitViewModel()
        {
            //arrange
            var quizzMock = _fixture.Build<Unit>()
                            .Without(x => x.Practices)
                            .Without(x => x.Lectures)
                            .Without(x => x.Assignments)
                            .Without(x => x.Quizzs)
                            .Without(x => x.ModuleUnits)
                            .Create();
            //act
            var result = _mapperConfig.Map<CreateUnitViewModel>(quizzMock);

            //assert
            result.UnitName.Should().Be(quizzMock.UnitName.ToString());
        }
    }
}
