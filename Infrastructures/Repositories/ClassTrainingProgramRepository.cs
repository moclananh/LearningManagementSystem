using Applications.Interfaces;
using Applications.Repositories;
using Domain.EntityRelationship;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class ClassTrainingProgramRepository : GenericRepository<ClassTrainingProgram>, IClassTrainingProgramRepository
    {
        private readonly AppDBContext _dbContext;
        public ClassTrainingProgramRepository(AppDBContext appDBContext, ICurrentTime currentTime, IClaimService claimService) : base(appDBContext, currentTime, claimService)
        {
            _dbContext = appDBContext;
        }

        public async Task<ClassTrainingProgram> GetClassTrainingProgram(Guid ClassId, Guid TrainingProgramId) => await _dbContext.ClassTrainingProgram.FirstOrDefaultAsync(x => x.ClassId == ClassId && x.TrainingProgramId == TrainingProgramId);
    }
}
