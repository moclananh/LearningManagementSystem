using Applications.Commons;
using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;
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
            var result = await _dbContext.Classes.Include(x => x.ClassUsers).ThenInclude(x => x.User)
                                           .Include(x => x.ClassTrainingPrograms).ThenInclude(x => x.TrainingProgram)
                                           .Include(x => x.AuditPlans)
                                           .FirstOrDefaultAsync(x => x.Id == ClassId);
            return result;
        }

        public async Task<Class?> GetClassByClassCode(string ClassCode)
        {
            return _dbContext.Classes.FirstOrDefault(x => x.ClassCode == ClassCode);
        }

        public async Task<Pagination<Class>> GetDisableClasses(int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Classes.Where(x => x.Status == Status.Disable).CountAsync();
            var items = await _dbContext.Classes.Where(x => x.Status == Status.Disable)
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
            var items = await _dbContext.Classes.Where(x => x.Status == Status.Enable)
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
