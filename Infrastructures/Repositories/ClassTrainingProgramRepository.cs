using Applications.Commons;
using Applications.Interfaces;
using Applications.Repositories;
using DocumentFormat.OpenXml.InkML;
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

        public async Task<Pagination<ClassTrainingProgram>> GetAllClassTrainingProgram(int pageNumber = 0, int pageSize = 10)
        {
            var itemCount = await _dbContext.ClassTrainingProgram.CountAsync();
            var items = await _dbContext.ClassTrainingProgram.ToListAsync();
            var result = new Pagination<ClassTrainingProgram>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items
            };
            return result;
        }

        public async Task<ClassTrainingProgram> GetClassTrainingProgram(Guid ClassId, Guid TrainingProgramId) => await _dbContext.ClassTrainingProgram.FirstOrDefaultAsync(x => x.ClassId == ClassId && x.TrainingProgramId == TrainingProgramId);


    }
}
