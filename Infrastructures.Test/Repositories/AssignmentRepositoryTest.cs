using Applications.Repositories;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Repositories;

namespace Infrastructures.Tests.Repositories
{
    public class AssignmentRepositoryTest : SetupTest
    {
        private readonly AssignmentRepository _assignmentRepository;

        public AssignmentRepositoryTest()
        {
            _assignmentRepository = new AssignmentRepository(
                _dbContext,
                _currentTimeMock.Object,
                _claimServiceMock.Object
                );
        }
        [Fact]
        public async Task AssignmentRepository_GetAssignmentByName_ShoulReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Assignment>()
                .Without(x => x.AssignmentQuestions)
                .Without(x => x.Unit)
                .CreateMany(30)
                .ToList();
            await _dbContext.Assignments.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            int i = 0;
            foreach ( var item in mockData )
            {
                item.AssignmentName = $"Mock{i}";
                i++;
            }
            _dbContext.UpdateRange(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(x => x.AssignmentName.Contains("Mock"))
                                    .OrderByDescending(x => x.CreationDate)
                                    .Take(10)
                                    .ToList();
            //act
            var resultPaging = await _assignmentRepository.GetAssignmentByName("Mock");
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
        public async Task AssignmentRepository_GetEnableAssignments_ShoulReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Assignment>()
                .Without(x => x.AssignmentQuestions)
                .Without(x => x.Unit)
                .CreateMany(30)
                .ToList();
            await _dbContext.Assignments.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            foreach (var item in mockData)
            {
                item.Status = Domain.Enum.StatusEnum.Status.Enable;
            }
            _dbContext.UpdateRange(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Take(10)
                                    .ToList();
            //act
            var resultPaging = await _assignmentRepository.GetEnableAssignmentAsync();
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
        public async Task AssignmentRepository_GetDiableAssignments_ShoulReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Assignment>()
                .Without(x => x.AssignmentQuestions)
                .Without(x => x.Unit)
                .CreateMany(30)
                .ToList();
            await _dbContext.Assignments.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            foreach (var item in mockData)
            {
                item.Status = Domain.Enum.StatusEnum.Status.Disable;
            }
            _dbContext.UpdateRange(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = mockData.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Take(10)
                                    .ToList();
            //act
            var resultPaging = await _assignmentRepository.GetDisableAssignmentAsync();
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
    }
}
