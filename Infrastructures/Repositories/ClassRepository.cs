using Applications.Commons;
using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;
using Domain.Enum.ClassEnum;
using Domain.Enum.StatusEnum;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class ClassRepository : GenericRepository<Class>, IClassRepository
    {
        private readonly AppDBContext _dbContext;
        public ClassRepository(AppDBContext dbContext, ICurrentTime currentTime, IClaimService claimService) : base(dbContext, currentTime, claimService)
        {
            _dbContext = dbContext;
        }

        public async Task<Pagination<Class>> GetClassByFilter(LocationEnum locations,
                                                        ClassTimeEnum classTime,
                                                        Status status,
                                                        AttendeeEnum attendee,
                                                        FSUEnum fsu,
                                                        DateTime? startDate,
                                                        DateTime? endDate,
                                                        int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Classes.CountAsync();
            var items = await _dbContext.Classes.Where(x => x.Location == locations)
                                          .Where(x => x.ClassTime == classTime)
                                          .Where(x => x.StartDate >= startDate && x.EndDate <= endDate)
                                          .Where(x => x.Status == status)
                                          .Where(x => x.Attendee == attendee)
                                          .Where(x => x.FSU == fsu)
                                          .OrderByDescending(x => x.CreationDate)
                                          .Skip(pageNumber * pageSize)
                                          .Take(pageSize)
                                          .AsNoTracking()
                                          .ToListAsync();
            var result = new Pagination<Class>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<Pagination<Class>> GetClassByName(string Name, int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Classes.CountAsync();
            var items = await _dbContext.Classes.Where(x => x.ClassName.Contains(Name))
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<Class>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }
        public async Task<Pagination<Class>> GetDisableClasses(int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Classes.CountAsync();
            var items = await _dbContext.Classes.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<Class>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }
        public async Task<Pagination<Class>> GetEnableClasses(int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Classes.CountAsync();
            var items = await _dbContext.Classes.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<Class>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }
    }
}
