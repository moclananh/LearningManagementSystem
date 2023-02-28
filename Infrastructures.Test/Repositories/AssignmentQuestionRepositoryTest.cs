using Applications.Repositories;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Repositories;

namespace Infrastructures.Tests.Repositories
{
    public class AssignmentQuestionRepositoryTest : SetupTest
    {
        private readonly AssignmentQuestionRepository _assignmentQuestionRepository;
        public AssignmentQuestionRepositoryTest()
        {
            _assignmentQuestionRepository = new AssignmentQuestionRepository(_dbContext,
                _currentTimeMock.Object,
                _claimServiceMock.Object);
        }

        [Fact]
        public async Task AssignmentQuestionRepository_GetAssignmentQuestionByAssignmentId_ShouldReturnCorrectData()
        {
            //arrange
            var assignmentQuestionMock = _fixture.Build<AssignmentQuestion>()
                                .Without(x => x.Assignment)
                                .CreateMany(30)
                                .ToList();
            await _dbContext.AddRangeAsync(assignmentQuestionMock);
            await _dbContext.SaveChangesAsync();
            var i = Guid.NewGuid();
            foreach (var item in assignmentQuestionMock)
            {
                item.AssignmentId = i;
            }
            _dbContext.UpdateRange(assignmentQuestionMock);
            await _dbContext.SaveChangesAsync();
            var expected = assignmentQuestionMock.Where(x => x.AssignmentId.Equals(i))
                                        .OrderByDescending(x => x.CreationDate)
                                        .Take(10)
                                        .ToList();
            //act
            var resultPaging = await _assignmentQuestionRepository.GetAllAssignmentQuestionByAssignmentId(i);
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
    }
}
