using Applications.Repositories;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Repositories;

namespace Infrastructures.Tests.Repositories
{
    public class ClassRepositoryTests : SetupTest
    {
        private readonly IClassRepository _classRepository;
        public ClassRepositoryTests()
        {
            _classRepository = new ClassRepository(
                _dbContext,
                _currentTimeMock.Object,
                _claimServiceMock.Object
                );
        }
        [Fact]
        public async Task ClassRepository_GetClassByName_ShoulReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Class>()
                            .Without(x => x.AbsentRequests)
                            .Without(x => x.Attendences)
                            .Without(x => x.AuditPlans)
                            .Without(x => x.ClassUsers)
                            .Without(x => x.ClassTrainingPrograms)
                            .With(x => x.ClassName, "Mock")
                            .CreateMany(30)
                            .ToList();
            await _dbContext.Classes.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(x => x.ClassName.Contains("Mock"))
                                    .OrderByDescending(x => x.CreationDate)
                                    .Take(10)
                                    .ToList();
            //act
            var resultPaging = await _classRepository.GetClassByName("Mock");
            var result = resultPaging.Items;
            //assert
            resultPaging.Previous.Should().BeFalse();
            resultPaging.Next.Should().BeTrue();
            resultPaging.Items.Count.Should().Be(10);
            resultPaging.TotalItemsCount.Should().Be(30);
            resultPaging.TotalPagesCount.Should().Be(3);
            resultPaging.PageIndex.Should().Be(0);
            resultPaging.PageSize.Should().Be(10);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ClassRepository_GetEnableClasses_ShoulReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Class>()
                            .Without(x => x.AbsentRequests)
                            .Without(x => x.Attendences)
                            .Without(x => x.AuditPlans)
                            .Without(x => x.ClassUsers)
                            .Without(x => x.ClassTrainingPrograms)
                            .With(x => x.Status, Domain.Enum.StatusEnum.Status.Enable)
                            .CreateMany(30)
                            .ToList();
            await _dbContext.Classes.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Take(10)
                                    .ToList();
            //act
            var resultPaging = await _classRepository.GetEnableClasses();
            var result = resultPaging.Items;
            //assert
            resultPaging.Previous.Should().BeFalse();
            resultPaging.Next.Should().BeTrue();
            resultPaging.Items.Count.Should().Be(10);
            resultPaging.TotalItemsCount.Should().Be(30);
            resultPaging.TotalPagesCount.Should().Be(3);
            resultPaging.PageIndex.Should().Be(0);
            resultPaging.PageSize.Should().Be(10);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ClassRepository_GetDiableClasses_ShoulReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Class>()
                            .Without(x => x.AbsentRequests)
                            .Without(x => x.Attendences)
                            .Without(x => x.AuditPlans)
                            .Without(x => x.ClassUsers)
                            .Without(x => x.ClassTrainingPrograms)
                            .With(x => x.Status, Domain.Enum.StatusEnum.Status.Disable)
                            .CreateMany(30)
                            .ToList();
            await _dbContext.Classes.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Take(10)
                                    .ToList();
            //act
            var resultPaging = await _classRepository.GetDisableClasses();
            var result = resultPaging.Items;
            //assert
            resultPaging.Previous.Should().BeFalse();
            resultPaging.Next.Should().BeTrue();
            resultPaging.Items.Count.Should().Be(10);
            resultPaging.TotalItemsCount.Should().Be(30);
            resultPaging.TotalPagesCount.Should().Be(3);
            resultPaging.PageIndex.Should().Be(0);
            resultPaging.PageSize.Should().Be(10);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ClassRepository_GetClassByFilter_ShoulReturnCorrectData()
        {
            //arrange
            var startDate = new DateTime(2023, 01, 01);
            var endDate = new DateTime(2023, 04, 01);
            var mockData1 = _fixture.Build<Class>()
                            .Without(x => x.AbsentRequests)
                            .Without(x => x.Attendences)
                            .Without(x => x.AuditPlans)
                            .Without(x => x.ClassUsers)
                            .Without(x => x.ClassTrainingPrograms)
                            .CreateMany(30)
                            .ToList();
            var mockData2 = _fixture.Build<Class>()
                            .Without(x => x.AbsentRequests)
                            .Without(x => x.Attendences)
                            .Without(x => x.AuditPlans)
                            .Without(x => x.ClassUsers)
                            .Without(x => x.ClassTrainingPrograms)
                            .CreateMany(10)
                            .ToList();
            await _dbContext.Classes.AddRangeAsync(mockData1);
            await _dbContext.Classes.AddRangeAsync(mockData2);
            await _dbContext.SaveChangesAsync();
            foreach (var item in mockData2)
            {
                item.StartDate = startDate;
                item.EndDate = endDate;
                item.Location = Domain.Enum.ClassEnum.LocationEnum.Hanoi;
                item.ClassTime = Domain.Enum.ClassEnum.ClassTimeEnum.Morning;
                item.FSU = Domain.Enum.ClassEnum.FSUEnum.FHM;
                item.Attendee = Domain.Enum.ClassEnum.AttendeeEnum.Intern;
                item.Status = Domain.Enum.StatusEnum.Status.Enable;
            }
            _dbContext.UpdateRange(mockData2);
            await _dbContext.SaveChangesAsync();
            var expected = mockData2.OrderByDescending(x => x.CreationDate)
                                    .Take(10)
                                    .ToList();
            //act
            var resultPaging = await _classRepository.GetClassByFilter(Domain.Enum.ClassEnum.LocationEnum.Hanoi, Domain.Enum.ClassEnum.ClassTimeEnum.Morning, Domain.Enum.StatusEnum.Status.Enable, Domain.Enum.ClassEnum.AttendeeEnum.Intern, Domain.Enum.ClassEnum.FSUEnum.FHM, DateTime.Parse("2023-01-01"), DateTime.Parse("2023-04-01"));
            var result = resultPaging.Items;
            //assert
            resultPaging.Previous.Should().BeFalse();
            resultPaging.Next.Should().BeTrue();
            resultPaging.Items.Count.Should().Be(10);
            resultPaging.TotalItemsCount.Should().Be(40);
            resultPaging.TotalPagesCount.Should().Be(4);
            resultPaging.PageIndex.Should().Be(0);
            resultPaging.PageSize.Should().Be(10);
            result.Should().BeEquivalentTo(expected);
        }
    }
}
