using Applications.ViewModels.ClassViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;

namespace Infrastructures.Tests.Mappers.ClassMapper
{
    public class ClassMapper : SetupTest
    {
        [Fact]
        public void TestMapper()
        {
            //arrange
            var classMock = _fixture.Build<Class>()
                            .Without(x => x.AbsentRequests)
                            .Without(x => x.Attendences)
                            .Without(x => x.AuditPlans)
                            .Without(x => x.ClassUsers)
                            .Without(x => x.ClassTrainingPrograms)
                            .Create();
            //act
            var result = _mapperConfig.Map<ClassViewModel>(classMock);

            //assert
            result.Id.Should().Be(classMock.Id.ToString());
        }

    }
}
