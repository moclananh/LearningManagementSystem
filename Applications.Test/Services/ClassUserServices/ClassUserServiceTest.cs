using Application.Interfaces;
using Application.Services;
using Applications.Commons;
using AutoFixture;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Tests;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Applications.Tests.Services.ClassUserServices
{
    public class ClassUserServiceTest : SetupTest
    {
        private readonly IClassUserServices _classUserServices;

        public ClassUserServiceTest()
        {
            _classUserServices = new ClassUserService(_unitOfWorkMock.Object, _mapperConfig,_classServiceMock.Object);
        }

        [Fact]
        public async Task GetAllClassUsers_ShouldReturnCorrectData()
        {
            //arrange
            var userMockData = _fixture.Build<User>()
                                        .Without(x => x.AbsentRequests)
                                        .Without(x => x.Attendences)
                                        .Without(x => x.UserAuditPlans)
                                        .Without(x => x.ClassUsers)
                                        .CreateMany(30)
                                        .ToList();
            var classMockData = _fixture.Build<Class>()
                                        .Without(x => x.AbsentRequests)
                                        .Without(x => x.Attendences)
                                        .Without(x => x.AuditPlans)
                                        .Without(x => x.ClassUsers)
                                        .Without(x => x.ClassTrainingPrograms)
                                        .Create();
            await _dbContext.Users.AddRangeAsync(userMockData);
            await _dbContext.Classes.AddAsync(classMockData);
            await _dbContext.SaveChangesAsync();
            var MockData = new List<ClassUser>();
            foreach (var item in userMockData)
            {
                var data = new ClassUser
                {
                    User = item,
                    Class = classMockData
                };
                MockData.Add(data);
            }
            var itemCount = await _dbContext.ClassUser.CountAsync();
            var items = await _dbContext.ClassUser.OrderByDescending(x => x.CreationDate)
                                                  .Take(10)
                                                  .AsNoTracking()
            .ToListAsync();
            var resultset = new Pagination<ClassUser>()
            {
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = itemCount,
                Items = items,
            };
            _unitOfWorkMock.Setup(x => x.ClassUserRepository.ToPagination(0, 10)).ReturnsAsync(resultset);
            //act
            var result = await _classUserServices.GetAllClassUsersAsync();
            //assert
            _unitOfWorkMock.Verify(x => x.ClassUserRepository.ToPagination(0, 10), Times.Once());
        }
    }
}
