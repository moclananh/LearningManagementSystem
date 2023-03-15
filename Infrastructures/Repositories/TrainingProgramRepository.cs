using Applications.Commons;
using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class TrainingProgramRepository : GenericRepository<TrainingProgram>, ITrainingProgramRepository
    {
        private readonly AppDBContext _dbContext;
        public TrainingProgramRepository(AppDBContext dbContext, ICurrentTime currentTime, IClaimService claimService) : base(dbContext, currentTime, claimService)
        {
            _dbContext = dbContext;
        }
        public async Task<Pagination<TrainingProgram>> GetTrainingProgramByClassId(Guid ClassId, int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.TrainingPrograms.CountAsync();
            var items = await _dbContext.ClassTrainingProgram.Where(x => x.ClassId.Equals(ClassId))
                                    .Select(x => x.TrainingProgram)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<TrainingProgram>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<Pagination<TrainingProgram>> GetTrainingProgramByName(string name, int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.TrainingPrograms.CountAsync();
            var items = await _dbContext.TrainingPrograms.Where(x => x.TrainingProgramName.Contains(name))
                                                                .OrderByDescending(x => x.CreationDate)
                                                                .Skip(pageNumber * pageSize)
                                                                .Take(pageSize)
                                                                .AsNoTracking()
                                                                .ToListAsync();
            var result = new Pagination<TrainingProgram>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items
            };
            return result;
        }

        public async Task<Pagination<TrainingProgram>> GetTrainingProgramDisable(int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.TrainingPrograms.CountAsync();
            var items = await _dbContext.TrainingPrograms.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable)
                                    .OrderByDescending(x => x.CreationDate)
                                    .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<TrainingProgram>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }
        public async Task<Pagination<TrainingProgram>> GetTrainingProgramEnable(int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.TrainingPrograms.CountAsync();
            var items = await _dbContext.TrainingPrograms.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable)
                                    .OrderByDescending(x => x.CreationDate)
            .Skip(pageNumber * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<TrainingProgram>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<TrainingProgram> GetTrainingProgramDetails(Guid TrainingProgramId)
        {

            var result = _dbContext.TrainingPrograms.Include(x => x.TrainingProgramSyllabi).ThenInclude(x => x.Syllabus)
                                                        
                                                       .FirstOrDefault(x => x.Id == TrainingProgramId);
            return result;
        }
    }
}
