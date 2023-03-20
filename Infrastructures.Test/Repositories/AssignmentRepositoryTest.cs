using Applications.Repositories;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Repositories;
using Org.BouncyCastle.Asn1.Pkcs;

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
                .With(x => x.AssignmentName, "Mock")
                .CreateMany(30)
                .ToList();
            await _dbContext.Assignments.AddRangeAsync(mockData);
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
                .With(x => x.Status, Domain.Enum.StatusEnum.Status.Enable)
                .CreateMany(30)
                .ToList();
            await _dbContext.Assignments.AddRangeAsync(mockData);
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
                .With(x => x.Status, Domain.Enum.StatusEnum.Status.Disable)
                .CreateMany(30)
                .ToList();
            await _dbContext.Assignments.AddRangeAsync(mockData);
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

        [Fact]
        public async Task AssignmentRepository_GetAssignmentByUnitId_ShouldReturnCorrectData()
        {
            //arrange
            var i = Guid.NewGuid();
            var assignmentMock = _fixture.Build<Assignment>()
                                .Without(x => x.AssignmentQuestions)
                                .Without(x => x.Unit)
                                .With(x => x.UnitId, i)
                                .CreateMany(30)
                                .ToList();
            await _dbContext.AddRangeAsync(assignmentMock);
            await _dbContext.SaveChangesAsync();
            var expected = assignmentMock.Where(x => x.UnitId.Equals(i))
                                        .OrderByDescending(x => x.CreationDate)
                                        .Take(10)
                                        .ToList();
            //act
            var resultPaging = await _assignmentRepository.GetAssignmentByUnitId(i);
            var result = resultPaging.Items.ToList();
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

        /*[Fact]
        public async Task GetAssignmentDetail_ShouldReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Assignment>()
                                   .Without(s => s.AssignmentQuestions)
                                   .Without(s => s.Unit)
                                   .Create();
            _dbContext.Assignments.Add(mockData);
            await _dbContext.SaveChangesAsync();
            //act
            var result = await _assignmentRepository.GetAssignmentDetail(mockData.Id);
            //assert
            result.Should().NotBeNull();
            result.Should().Be(mockData.Id);
        }*/
    }
}
