using Applications.ViewModels.ClassUserViewModels;
using AutoFixture;
using Domain.EntityRelationship;
using Domain.Tests;
using FluentAssertions;


namespace Infrastructures.Tests.Mappers.ClassUserMapper
{
    public class ClassUserMapper : SetupTest
    {
        [Fact]
        public void TestClassUserViewMapper()
        {
            //arrange
            var ClasUserMock = _fixture.Build<ClassUser>()
                               .Without(s => s.Class)
                               .Without(s => s.User)
                               .Create();
            //act
            var result = _mapperConfig.Map<CreateClassUserViewModel>(ClasUserMock);
            //assert
            result.ClassId.Should().Be(ClasUserMock.ClassId.ToString());
            result.UserId.Should().Be(ClasUserMock.UserId.ToString());
        }
    }
}
