using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class SyllabusRepository : GenericRepository<Syllabus>, ISyllabusRepository
    {
        private readonly AppDBContext _appDBContext;

        public SyllabusRepository(AppDBContext appDBContext, ICurrentTime currentTime, IClaimService claimService) : base(appDBContext, currentTime, claimService)
        {
            _appDBContext = appDBContext;
        }

        public async Task<List<Syllabus>> GetDisableSyllabus() => await _appDBContext.Syllabi.Where(s => s.Status == Domain.Enum.StatusEnum.Status.Disable).ToListAsync();

        public async Task<List<Syllabus>> GetEnableSyllabus() => await _appDBContext.Syllabi.Where(s => s.Status == Domain.Enum.StatusEnum.Status.Enable).ToListAsync();

        public async Task<List<Syllabus>> GetSyllabusByName(string SyllabusName) => await (from s in _appDBContext.Syllabi select s).Where(m => m.SyllabusName!.Contains(SyllabusName)).ToListAsync();

        public async Task<List<Syllabus>> GetSyllabusByOutputStandardId(Guid OutputStandardId) => await _appDBContext.SyllabusOutputStandard.Where(s => s.OutputStandardId == OutputStandardId).Select(s => s.Syllabus).ToListAsync();

        public async Task<List<Syllabus>> GetSyllabusByTrainingProgramId(Guid TrainingProgramId) => await _appDBContext.TrainingProgramSyllabi.Where(s => s.TrainingProgramId == TrainingProgramId).Select(S => S.Syllabus).ToListAsync();
    }
}
