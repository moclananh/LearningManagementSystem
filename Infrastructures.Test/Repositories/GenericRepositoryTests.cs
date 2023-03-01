using Applications.Repositories;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Repositories;

namespace Infrastructures.Tests.Repositories
{
    public class GenericRepositoryTests : SetupTest
    {
        private readonly IGenericRepository<Class> _genericRepository;
        public GenericRepositoryTests()
        {
            _genericRepository = new GenericRepository<Class>(
                _dbContext,
                _currentTimeMock.Object,
                _claimServiceMock.Object);
        }

        [Fact]
        public async Task GenericRepository_GetAllAsync_ShouldReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Class>()
                            .Without(x => x.AbsentRequests)
                            .Without(x => x.Attendences)
                            .Without(x => x.AuditPlans)
                            .Without(x => x.ClassUsers)
                            .Without(x => x.ClassTrainingPrograms)
                            .CreateMany(10)
                            .ToList();
            //act
            await _dbContext.Classes.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var result = await _genericRepository.GetAllAsync();
            //assert
            result.Should().BeEquivalentTo(mockData);
        }

        [Fact]
        public async Task GenericRepository_GetAllAsync_ShouldReturnEmptyWhenHaveNoData()
        {
            //act
            var result = await _genericRepository.GetAllAsync();
            //assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GenericRepository_GetByIdAsync_ShouldReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Class>()
                            .Without(x => x.AbsentRequests)
                            .Without(x => x.Attendences)
                            .Without(x => x.AuditPlans)
                            .Without(x => x.ClassUsers)
                            .Without(x => x.ClassTrainingPrograms)
                            .Create();
            //act
            await _dbContext.Classes.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var result = await _genericRepository.GetByIdAsync(mockData.Id);
            //assert
            result.Should().BeEquivalentTo(mockData);
        }


        [Fact]
        public async Task GenericRepository_GetByIdAsync_ShouldReturnEmptyWhenHaveNoData()
        {
            //act
            var result = await _genericRepository.GetByIdAsync(Guid.Empty);
            //assert
            result.Should().BeNull();
        }
        [Fact]
        public async Task GenericRepository_AddAsync_ShouldReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Class>()
                            .Without(x => x.AbsentRequests)
                            .Without(x => x.Attendences)
                            .Without(x => x.AuditPlans)
                            .Without(x => x.ClassUsers)
                            .Without(x => x.ClassTrainingPrograms)
                            .Create();

            //act
            await _genericRepository.AddAsync(mockData);
            var result = await _dbContext.SaveChangesAsync();
            //assert
            result.Should().Be(1);
        }

        [Fact]
        public async Task GenericRepository_AddRangeAsync_ShouldReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Class>()
                            .Without(x => x.AbsentRequests)
                            .Without(x => x.Attendences)
                            .Without(x => x.AuditPlans)
                            .Without(x => x.ClassUsers)
                            .Without(x => x.ClassTrainingPrograms)
                            .CreateMany(10)
                            .ToList();
            //act
            await _genericRepository.AddRangeAsync(mockData);
            var result = await _dbContext.SaveChangesAsync();
            //assert
            result.Should().Be(10);
            _dbContext.Classes.RemoveRange(mockData);
            await _dbContext.SaveChangesAsync();
        }


        [Fact]
        public async Task GenericRepository_SoftRemove_ShouldReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Class>()
                            .Without(x => x.AbsentRequests)
                            .Without(x => x.Attendences)
                            .Without(x => x.AuditPlans)
                            .Without(x => x.ClassUsers)
                            .Without(x => x.ClassTrainingPrograms)
                            .Create();
            _dbContext.Classes.Add(mockData);
            await _dbContext.SaveChangesAsync();
            //act
            _genericRepository.SoftRemove(mockData);
            var result = await _dbContext.SaveChangesAsync();
            //assert
            result.Should().Be(1);
        }

        [Fact]
        public async Task GenericRepository_Update_ShouldReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Class>()
                            .Without(x => x.AbsentRequests)
                            .Without(x => x.Attendences)
                            .Without(x => x.AuditPlans)
                            .Without(x => x.ClassUsers)
                            .Without(x => x.ClassTrainingPrograms)
                            .Create();
            _dbContext.Classes.Add(mockData);
            await _dbContext.SaveChangesAsync();
            mockData.IsDeleted = false;
            //act
            _genericRepository.Update(mockData);
            var result = await _dbContext.SaveChangesAsync();
            //assert
            result.Should().Be(1);
        }

        [Fact]
        public async Task GenericRepository_SoftRemoveRange_ShouldReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Class>()
                            .Without(x => x.AbsentRequests)
                            .Without(x => x.Attendences)
                            .Without(x => x.AuditPlans)
                            .Without(x => x.ClassUsers)
                            .Without(x => x.ClassTrainingPrograms)
                            .CreateMany(10)
                            .ToList();
            await _dbContext.Classes.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            //act
            _genericRepository.SoftRemoveRange(mockData);
            var result = await _dbContext.SaveChangesAsync();
            //assert
            result.Should().Be(10);
        }

        [Fact]
        public async Task GenericRepository_UpdateRange_ShouldReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Class>()
                            .Without(x => x.AbsentRequests)
                            .Without(x => x.Attendences)
                            .Without(x => x.AuditPlans)
                            .Without(x => x.ClassUsers)
                            .Without(x => x.ClassTrainingPrograms)
                            .With(x => x.IsDeleted, false)
                            .CreateMany(10)
                            .ToList();
            await _dbContext.Classes.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            //act
            _genericRepository.UpdateRange(mockData);
            var result = await _dbContext.SaveChangesAsync();
            //assert
            result.Should().Be(10);
        }

        [Fact]
        public async Task GenericRepository_ToPagination_ShouldReturnCorrectDataFirstsPage()
        {
            //arrange
            var mockData = _fixture.Build<Class>()
                            .Without(x => x.AbsentRequests)
                            .Without(x => x.Attendences)
                            .Without(x => x.AuditPlans)
                            .Without(x => x.ClassUsers)
                            .Without(x => x.ClassTrainingPrograms)
                            .CreateMany(30)
                            .ToList();
            await _dbContext.Classes.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            //act
            var paginasion = await _genericRepository.ToPagination();
            //assert
            paginasion.Previous.Should().BeFalse();
            paginasion.Next.Should().BeTrue();
            paginasion.Items.Count.Should().Be(10);
            paginasion.TotalItemsCount.Should().Be(30);
            paginasion.TotalPagesCount.Should().Be(3);
            paginasion.PageIndex.Should().Be(0);
            paginasion.PageSize.Should().Be(10);
        }

        [Fact]
        public async Task GenericRepository_ToPagination_ShouldReturnCorrectDataSecoundPage()
        {
            //arrange
            var mockData = _fixture.Build<Class>()
                            .Without(x => x.AbsentRequests)
                            .Without(x => x.Attendences)
                            .Without(x => x.AuditPlans)
                            .Without(x => x.ClassUsers)
                            .Without(x => x.ClassTrainingPrograms)
                            .CreateMany(55)
                            .ToList();
            await _dbContext.Classes.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            //act
            var paginasion = await _genericRepository.ToPagination(1, 20);
            //assert
            paginasion.Previous.Should().BeTrue();
            paginasion.Next.Should().BeTrue();
            paginasion.Items.Count.Should().Be(20);
            paginasion.TotalItemsCount.Should().Be(55);
            paginasion.TotalPagesCount.Should().Be(3);
            paginasion.PageIndex.Should().Be(1);
            paginasion.PageSize.Should().Be(20);
        }

        [Fact]
        public async Task GenericRepository_ToPagination_ShouldReturnCorrectDataLastPage()
        {
            //arrange
            var mockData = _fixture.Build<Class>()
                            .Without(x => x.AbsentRequests)
                            .Without(x => x.Attendences)
                            .Without(x => x.AuditPlans)
                            .Without(x => x.ClassUsers)
                            .Without(x => x.ClassTrainingPrograms)
                            .CreateMany(55)
                            .ToList();
            await _dbContext.Classes.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            //act
            var paginasion = await _genericRepository.ToPagination(2, 20);
            //assert
            paginasion.Previous.Should().BeTrue();
            paginasion.Next.Should().BeFalse();
            paginasion.Items.Count.Should().Be(15);
            paginasion.TotalItemsCount.Should().Be(55);
            paginasion.TotalPagesCount.Should().Be(3);
            paginasion.PageIndex.Should().Be(2);
            paginasion.PageSize.Should().Be(20);
        }

        [Fact]
        public async Task GenericRepository_ToPagination_ShouldReturnWithoutData()
        {
            //act
            var paginasion = await _genericRepository.ToPagination();
            //assert
            paginasion.Previous.Should().BeFalse();
            paginasion.Next.Should().BeFalse();
            paginasion.Items.Count.Should().Be(0);
            paginasion.TotalItemsCount.Should().Be(0);
            paginasion.TotalPagesCount.Should().Be(0);
            paginasion.PageIndex.Should().Be(0);
            paginasion.PageSize.Should().Be(10);
        }
    }

}
