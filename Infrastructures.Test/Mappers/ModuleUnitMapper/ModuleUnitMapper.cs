using Applications.ViewModels.UnitModuleViewModel;
using AutoFixture;
using Domain.EntityRelationship;
using Domain.Tests;
using FluentAssertions;

namespace Infrastructures.Tests.Mappers.ModuleUnitMapper
{
    public class ModuleUnitMapper : SetupTest
    {
        [Fact]
        public void TestModuleUnitViewMapper()
        {
            //arrange
            var ModuleUnitMock = _fixture.Build<ModuleUnit>()
                               .Without(s => s.Module)
                               .Without(s => s.Unit)
                               .Create();
            //act
            var result1 = _mapperConfig.Map<ModuleUnitViewModel>(ModuleUnitMock);
            var result2 = _mapperConfig.Map<ModuleUnitViewModel>(ModuleUnitMock);
            //assert
            result1.ModuleId.Should().Be(ModuleUnitMock.ModuleId.ToString());
            result2.UnitId.Should().Be(ModuleUnitMock.UnitId.ToString());
        }
    }
}
