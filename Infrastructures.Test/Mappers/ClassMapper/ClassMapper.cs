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
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            //arrange
            var classMock = _fixture.Create<Class>();

            //act
            var result = _mapperConfig.Map<ClassViewModel>(classMock);

            //assert
            result.Id.Should().Be(classMock.Id.ToString());
        }

    }
}
