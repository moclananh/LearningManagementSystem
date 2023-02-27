using Applications.Interfaces;
using Applications.Repositories;
using Domain.EntityRelationship;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class TrainingProgramSyllabiRepository : GenericRepository<TrainingProgramSyllabus>, ITrainingProgramSyllabiRepository
    {
        private readonly AppDBContext _dbContext;
        public TrainingProgramSyllabiRepository(AppDBContext dbContext, ICurrentTime currentTime, IClaimService claimService) : base(dbContext, currentTime, claimService)
        {
            _dbContext = dbContext;
        }
        public async Task<TrainingProgramSyllabus> GetTrainingProgramSyllabus(Guid SyllabusId,Guid TrainingProgramId) => await _dbContext.TrainingProgramSyllabi.FirstOrDefaultAsync(x => x.TrainingProgramId == TrainingProgramId && x.SyllabusId == SyllabusId);
    }
}
