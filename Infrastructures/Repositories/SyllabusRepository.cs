using Applications.Commons;
using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class SyllabusRepository : GenericRepository<Syllabus>, ISyllabusRepository
    {
        private readonly AppDBContext _dbContext;

        public SyllabusRepository(AppDBContext appDBContext, ICurrentTime currentTime, IClaimService claimService) : base(appDBContext, currentTime, claimService)
        {
            _dbContext = appDBContext;
        }

        public async Task<Pagination<Syllabus>> GetDisableSyllabus(int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Syllabi.CountAsync();
            var items = await _dbContext.Syllabi.Where(s => s.Status == Domain.Enum.StatusEnum.Status.Disable)
                                                .Include(x => x.SyllabusOutputStandards).ThenInclude(x => x.OutputStandard)
                                                .OrderByDescending(x => x.CreationDate)
                                                .Skip(pageNumber * pageSize)
                                                .Take(pageSize)
                                                .AsNoTracking()
                                                .ToListAsync();

            var result = new Pagination<Syllabus>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<Pagination<Syllabus>> GetEnableSyllabus(int pageNumber = 0, int pageSize = 10)
        {
            {
                var itemCount = await _dbContext.Syllabi.CountAsync();
                var items = await _dbContext.Syllabi.Where(s => s.Status == Domain.Enum.StatusEnum.Status.Enable)
                                                    .Include(x => x.SyllabusOutputStandards).ThenInclude(x => x.OutputStandard)
                                                    .OrderByDescending(x => x.CreationDate)
                                                    .Skip(pageNumber * pageSize)
                                                    .Take(pageSize)
                                                    .AsNoTracking()
                                                    .ToListAsync();

                var result = new Pagination<Syllabus>()
                {
                    PageIndex = pageNumber,
                    PageSize = pageSize,
                    TotalItemsCount = itemCount,
                    Items = items,
                };

                return result;
            }
        }

        public async Task<Pagination<Syllabus>> GetSyllabusByName(string SyllabusName, int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Syllabi.CountAsync();
            var items = await _dbContext.Syllabi.Where(s => s.SyllabusName!.Contains(SyllabusName))
                                                .Include(x => x.SyllabusOutputStandards).ThenInclude(x => x.OutputStandard)
                                                .OrderByDescending(x => x.CreationDate)
                                                .Skip(pageNumber * pageSize)
                                                .Take(pageSize)
                                                .AsNoTracking()
                                                .ToListAsync();

            var result = new Pagination<Syllabus>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<Pagination<Syllabus>> GetSyllabusByOutputStandardId(Guid OutputStandardId, int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Syllabi.CountAsync();
            var items = await _dbContext.SyllabusOutputStandard.Where(s => s.OutputStandardId == OutputStandardId).Select(s => s.Syllabus)
                                                               .OrderByDescending(x => x.CreationDate)
                                                               .Skip(pageNumber * pageSize)
                                                               .Take(pageSize)
                                                               .AsNoTracking()
                                                               .ToListAsync();

            var result = new Pagination<Syllabus>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<Pagination<Syllabus>> GetSyllabusByTrainingProgramId(Guid TrainingProgramId, int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Syllabi.CountAsync();
            var items = await _dbContext.TrainingProgramSyllabi.Where(s => s.TrainingProgramId == TrainingProgramId).Select(s => s.Syllabus)
                                                               .OrderByDescending(x => x.CreationDate)
                                                               .Skip(pageNumber * pageSize)
                                                               .Take(pageSize)
                                                               .AsNoTracking()
                                                               .ToListAsync();

            var result = new Pagination<Syllabus>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<Syllabus> GetSyllabusDetail(Guid SyllabusId)
        {

            var result = _dbContext.Syllabi.Include(x => x.SyllabusOutputStandards).ThenInclude(x => x.OutputStandard)
                                                    .Include(x => x.SyllabusModules).ThenInclude(x => x.Module).ThenInclude(x => x.ModuleUnits).ThenInclude(x => x.Unit).ThenInclude(x => x.Lectures)
                                                    .Include(x => x.SyllabusModules).ThenInclude(x => x.Module).ThenInclude(x => x.ModuleUnits).ThenInclude(x => x.Unit).ThenInclude(x => x.Assignments)
                                                    .Include(x => x.SyllabusModules).ThenInclude(x => x.Module).ThenInclude(x => x.ModuleUnits).ThenInclude(x => x.Unit).ThenInclude(x => x.Practices)
                                                    .Include(x => x.SyllabusModules).ThenInclude(x => x.Module).ThenInclude(x => x.ModuleUnits).ThenInclude(x => x.Unit).ThenInclude(x => x.Quizzs)
                                                    .Include(x => x.TrainingProgramSyllabi)
                                           .FirstOrDefault(x => x.Id == SyllabusId);
            return result;
        }

        public async Task<Pagination<Syllabus>> GetAllSyllabusDetail(int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Syllabi.CountAsync();
            var items = await _dbContext.Syllabi.Include(x => x.SyllabusOutputStandards).ThenInclude(x => x.OutputStandard)
                                                    .Include(x => x.SyllabusModules)
                                                    .Include(x => x.TrainingProgramSyllabi)
                                                    .OrderByDescending(x => x.CreationDate)
                                                    .Skip(pageNumber * pageSize)
                                                    .Take(pageSize)
                                                    .AsNoTracking()
                                                    .ToListAsync();

            var result = new Pagination<Syllabus>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<Pagination<Syllabus>> GetSyllabusByCreationDate(DateTime startDate, DateTime endDate, int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.Syllabi.CountAsync();
            var items = await _dbContext.Syllabi.Where(x => x.CreationDate >= startDate && x.CreationDate <= endDate).Include(x => x.SyllabusOutputStandards).ThenInclude(x => x.OutputStandard).ToListAsync();
            var result = new Pagination<Syllabus>()
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
