using Applications.ViewModels.ModuleViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;

namespace Infrastructures.Tests.Mappers.ModuleMapper
{
    public class ModuleMapper : SetupTest
    {
        [Fact]
        public void TestMapper()
        {
            //arrange
            var moduleMock = _fixture.Build<Module>()
                            .Without(x => x.AuditPlan)
                            .Without(x => x.ModuleUnits)
                            .Without(x => x.SyllabusModules)
                            .Create();
            //act
            var result = _mapperConfig.Map<ModuleViewModels>(moduleMock);

            //assert
            result.Id.Should().Be(moduleMock.Id.ToString());
        }
        [Fact]  
        public void TestCreateModuleViewModel()
        {
            //arrange
            var moduleMock = _fixture.Build<Module>()
                            .Without(x => x.AuditPlan)
                            .Without(x => x.ModuleUnits)
                            .Without(x => x.SyllabusModules)
                            .Create();
            //act
            var result = _mapperConfig.Map<CreateModuleViewModel>(moduleMock);

            //assert
            result.ModuleName.Should().Be(moduleMock.ModuleName.ToString());
        }
        [Fact]
        public void TestUpdateModuleViewModel()
        {
            //arrange
            var moduleMock = _fixture.Build<Module>()
                            .Without(x => x.AuditPlan)
                            .Without(x => x.ModuleUnits)
                            .Without(x => x.SyllabusModules)
                            .Create();
            //act
            var result = _mapperConfig.Map<UpdateModuleViewModel>(moduleMock);

            //assert
            result.ModuleName.Should().Be(moduleMock.ModuleName.ToString());
        }
    }
}
