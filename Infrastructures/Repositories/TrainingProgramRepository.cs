using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class TrainingProgramRepository : GenericRepository<TrainingProgram>, ITrainingProgramRepository
    {
        private readonly AppDBContext _dbContext;

        public TrainingProgramRepository(AppDBContext dbContext,ICurrentTime currentTime,IClaimService claimService) : base(dbContext,currentTime,claimService)
        {
            _dbContext = dbContext;
        }
        public async Task<List<TrainingProgram>> GetTrainingProgramByClassId(Guid ClassId) => await _dbContext.ClassTrainingProgram.Where(x => x.ClassId == ClassId).Select(x => x.TrainingProgram).ToListAsync();
        public async Task<List<TrainingProgram>> GetTrainingProgramDisable() => await _dbContext.TrainingPrograms.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable).ToListAsync();
        public async Task<List<TrainingProgram>> GetTrainingProgramEnable() => await _dbContext.TrainingPrograms.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable).ToListAsync();
    }
}
