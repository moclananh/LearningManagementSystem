using Applications.Repositories;
using AutoFixture;
using AutoFixture.Kernel;
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

        public class IgnoreCircularReferenceSpecimenBuilder : ISpecimenBuilder
        {
            private readonly Stack<object> _history = new Stack<object>();

            public object Create(object request, ISpecimenContext context)
            {
                // Check if the request is a recursive request
                if (_history.Contains(request))
                {
                    return new OmitSpecimen();
                }

                // Add the request to the history
                _history.Push(request);

                // Create the specimen
                var specimen = context.Resolve(request);

                // Remove the request from the history
                _history.Pop();

                return specimen;
            }
        }

        [Fact]
        public async Task GenericRepository_GetAllAsync_ShouldReturnCorrectData()
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(new IgnoreCircularReferenceSpecimenBuilder());

            var mockData = _fixture.Build<Class>().CreateMany(10).ToList();
            await _dbContext.Classes.AddRangeAsync(mockData);

            await _dbContext.SaveChangesAsync();

            var result = await _genericRepository.GetAllAsync();

            result.Should().BeEquivalentTo(mockData);
        }

    }
}
