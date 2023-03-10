using Applications.ViewModels.LectureViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;


namespace Infrastructures.Tests.Mappers.LectureMapper
{
    public class LectureMapper : SetupTest
    {
        [Fact]
        public void TestLectureViewModel()
        {
            //arrange
            var lectureMock = _fixture.Build<Lecture>()
                            .Without(x => x.Unit)
                            .Create();
            //act
            var result = _mapperConfig.Map<LectureViewModel>(lectureMock);

            //assert
            result.Id.Should().Be(lectureMock.Id.ToString());
        }
        [Fact]
        public void TestCreateLectureViewModel()
        {
            //arrange
            var lectureMock = _fixture.Build<Lecture>()
                            .Without(x => x.Unit)
                            .Create();
            //act
            var result = _mapperConfig.Map<CreateLectureViewModel>(lectureMock);

            //assert
            result.LectureName.Should().Be(lectureMock.LectureName.ToString());
        }
        [Fact]
        public void TestUpdateLectureViewModel()
        {
            //arrange
            var lectureMock = _fixture.Build<Lecture>()
                            .Without(x => x.Unit)
                            .Create();
            //act
            var result = _mapperConfig.Map<UpdateLectureViewModel>(lectureMock);

            //assert
            result.LectureName.Should().Be(lectureMock.LectureName.ToString());
        }
    }
}
