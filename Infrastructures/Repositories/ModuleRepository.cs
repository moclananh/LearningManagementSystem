using Applications.Commons;
using Applications.Interfaces;
using Applications.Repositories;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class ModuleRepository : GenericRepository<Module>, IModuleRepository
    {
        private readonly AppDBContext _dbContext;

        public ModuleRepository(AppDBContext appDBContext, ICurrentTime currentTime, IClaimService claimService) : base(appDBContext, currentTime, claimService)
        {
            _dbContext = appDBContext;
        }
        public async Task<Pagination<Module>> GetDisableModules(int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Modules.CountAsync();
            var items = await _dbContext.Modules.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<Module>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }
        public async Task<Pagination<Module>> GetEnableModules(int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Modules.CountAsync();
            var items = await _dbContext.Modules.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<Module>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }


        public async Task<Pagination<Module>> GetModuleByName(string name, int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Modules.CountAsync();
            var items = await _dbContext.Modules.Where(x => x.ModuleName.Contains(name))
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();
            var result = new Pagination<Module>
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items
            };
            return result;
        }
        public async Task<Pagination<Module>> GetModulesBySyllabusId(Guid syllabusId, int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Modules.CountAsync();
            var items = await _dbContext.SyllabusModule.Where(x => x.SyllabusId == syllabusId)
                                                       .Select(z => z.Module)
                                                       .OrderByDescending(x => x.CreationDate)
                                                       .Skip(pageNumber * pageSize)
                                                       .Take(pageSize)
                                                       .AsNoTracking()
                                                       .ToListAsync();
            var result = new Pagination<Module>
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items
            };
            return result;
        }

        
    }
}
