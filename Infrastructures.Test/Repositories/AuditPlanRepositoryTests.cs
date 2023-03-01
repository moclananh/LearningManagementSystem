using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Repositories;

namespace Infrastructures.Tests.Repositories
{
    public class AuditPlanRepositoryTests : SetupTest
    {
        private readonly AuditPlanRepository _AuditPlanRepository;

        public AuditPlanRepositoryTests()
        {
            _AuditPlanRepository = new AuditPlanRepository(
                _dbContext,
                _currentTimeMock.Object,
                _claimServiceMock.Object
                );
        }

        [Fact]
        public async Task AuditPlanRepository_GetAuditPlanByClassId_ShouldReturnCorrectData()
        {
            //arrange
            var i = Guid.NewGuid();
            var auditplanMock = _fixture.Build<AuditPlan>()
                                .Without(x => x.AuditResults)
                                .Without(x => x.AuditQuestions)
                                .Without(x => x.UserAuditPlans)
                                .Without(x => x.Module)
                                .Without(x => x.Class)
                                .With(x => x.ClassId, i)
                                .CreateMany(30)
                                .ToList();
            await _dbContext.AddRangeAsync(auditplanMock);
            await _dbContext.SaveChangesAsync();
            var expected = auditplanMock.Where(x => x.ClassId.Equals(i))
                                        .OrderByDescending(x => x.CreationDate)
                                        .Take(10)
                                        .ToList();
            //act
            var resultPaging = await _AuditPlanRepository.GetAuditPlanByClassId(i);
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

        [Fact]
        public async Task AuditPlanRepository_GetAuditPlanByModuleId_ShouldReturnCorrectData()
        {
            //arrange
            var i = Guid.NewGuid();
            var auditplanMock = _fixture.Build<AuditPlan>()
                                .Without(x => x.AuditResults)
                                .Without(x => x.AuditQuestions)
                                .Without(x => x.UserAuditPlans)
                                .Without(x => x.Module)
                                .Without(x => x.Class)
                                .With(x => x.ModuleId, i)
                                .CreateMany(30)
                                .ToList();
            await _dbContext.AddRangeAsync(auditplanMock);
            await _dbContext.SaveChangesAsync();
            var expected = auditplanMock.Where(x => x.ModuleId.Equals(i)).FirstOrDefault();
            //act
            var result = await _AuditPlanRepository.GetAuditPlanByModuleId(i);
            //assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task AuditPlanRepository_GetAuditPlanByname_ShouldReturnCorrectData()
        {
            //arrange
            var auditplanMock = _fixture.Build<AuditPlan>()
                                .Without(x => x.AuditResults)
                                .Without(x => x.AuditQuestions)
                                .Without(x => x.UserAuditPlans)
                                .Without(x => x.Module)
                                .Without(x => x.Class)
                                .With(x => x.AuditPlanName, "Mock") 
                                .CreateMany(30)
                                .ToList();
            await _dbContext.AddRangeAsync(auditplanMock);
            await _dbContext.SaveChangesAsync();
            var expected = auditplanMock.Where(x => x.AuditPlanName.Contains("Mock"))
                                        .OrderByDescending(x => x.CreationDate)
                                        .Take(10)
                                        .ToList();
            //act
            var resultPaging = await _AuditPlanRepository.GetAuditPlanByName("Mock");
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

        [Fact]
        public async Task AuditPlanRepository_GetDisableAuditPlans_ShouldReturnCorrectData()
        {
            var auditplanMock = _fixture.Build<AuditPlan>()
                                .Without(x => x.AuditResults)
                                .Without(x => x.AuditQuestions)
                                .Without(x => x.UserAuditPlans)
                                .Without(x => x.Module)
                                .Without(x => x.Class)
                                .With(x => x.Status, Domain.Enum.StatusEnum.Status.Disable)
                                .CreateMany(30)
                                .ToList();
            await _dbContext.AddRangeAsync(auditplanMock);
            await _dbContext.SaveChangesAsync();
            var expected = auditplanMock.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable)
                                        .OrderByDescending(x => x.CreationDate)
                                        .Take(10)
                                        .ToList();
            //act
            var resultPaging = await _AuditPlanRepository.GetDisableAuditPlans();
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
        public async Task AuditPlanRepository_GetEnableAuditPlans_ShouldReturnCorrectData()
        {
            var auditplanMock = _fixture.Build<AuditPlan>()
                                .Without(x => x.AuditResults)
                                .Without(x => x.AuditQuestions)
                                .Without(x => x.UserAuditPlans)
                                .Without(x => x.Module)
                                .Without(x => x.Class)
                                .With(x => x.Status, Domain.Enum.StatusEnum.Status.Enable)
                                .CreateMany(30)
                                .ToList();
            await _dbContext.AddRangeAsync(auditplanMock);
            await _dbContext.SaveChangesAsync();
            var expected = auditplanMock.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable)
                                        .OrderByDescending(x => x.CreationDate)
                                        .Take(10)
                                        .ToList();
            //act
            var resultPaging = await _AuditPlanRepository.GetEnableAuditPlans();
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
