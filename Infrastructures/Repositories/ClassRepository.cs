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

        public async Task<Pagination<Class>> GetClassByFilter(LocationEnum? locations,
                                                        ClassTimeEnum? classTime,
                                                        Status? status,
                                                        AttendeeEnum? attendee,
                                                        FSUEnum? fsu,
                                                        DateTime? startDate,
                                                        DateTime? endDate,
                                                        int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Classes.CountAsync();
            var items = await _dbContext.Classes.Where(x => x.StartDate >= startDate && x.EndDate <= endDate)
                                                .OrderByDescending(x => x.CreationDate)
                                                .Skip(pageNumber * pageSize)
                                                .Take(pageSize)
                                                .AsNoTracking()
                                                .ToListAsync();
            if (locations.HasValue)
            {
                items = await _dbContext.Classes.Where(x => x.Location == locations)
                                                .ToListAsync();
            }
            if (classTime.HasValue)
            {
                items = await _dbContext.Classes.Where(x => x.ClassTime == classTime)
                                                .ToListAsync();
            }
            if (status.HasValue)
            {
                items = await _dbContext.Classes.Where(x => x.Status == status)
                                                .ToListAsync();
            }
            if (attendee.HasValue)
            {
                items = await _dbContext.Classes.Where(x => x.Attendee == attendee)
                                                .ToListAsync();
            }
            if (fsu.HasValue)
            {
                items = await _dbContext.Classes.Where(x => x.FSU == fsu)
                                                .ToListAsync();
            }

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

        public async Task<Class> GetClassDetails(Guid ClassId)
        {
            var result = _dbContext.Classes.Include(x => x.ClassUsers)
                                           .Include(x => x.ClassTrainingPrograms)
                                           .Include(x => x.AuditPlans)
                                           .FirstOrDefault(x => x.Id == ClassId);
            return result;
        }

        public async Task<Class?> GetClassByClassCode(string ClassCode)
        {
            return _dbContext.Classes.FirstOrDefault(x => x.ClassCode == ClassCode);
        }

        public async Task<Pagination<Class>> GetDisableClasses(int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Classes.Where(x => x.Status == Status.Disable).CountAsync();
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
            var itemCount = await _dbContext.Classes.Where(x => x.Status == Status.Enable).CountAsync();
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
