using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Infrastructure.Repositories;

namespace Infrastructures.Tests.Repositories
{
    public class AuditResultRepositoryTests : SetupTest
    {
        private readonly AuditResultRepository _auditResultRepository;

        public AuditResultRepositoryTests()
        {
            _auditResultRepository = new AuditResultRepository(_dbContext,
                                                                _currentTimeMock.Object,
                                                                _claimServiceMock.Object);
        }

        [Fact]
        public async Task AuditResultRepository_GetAuditResultByAuditPlanId_ShouldReturnCorrectData()
        {
            var auditResultMock = _fixture.Build<AuditResult>()
                                .Without(x => x.AuditPlan)
                                .CreateMany(30)
                                .ToList();
            await _dbContext.AddRangeAsync(auditResultMock);
            await _dbContext.SaveChangesAsync();
            var i = Guid.NewGuid();
            foreach (var item in auditResultMock)
            {
                item.AuditPlanId = i;
            }
            _dbContext.UpdateRange(auditResultMock);
            await _dbContext.SaveChangesAsync();
            var expected = auditResultMock.Where(x => x.AuditPlanId.Equals(i)).FirstOrDefault();
            //act
            var result = await _auditResultRepository.GetByAuditPlanId(i);
            //assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
