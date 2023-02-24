using Domain.Entities;

namespace Applications.Repositories
{
    public interface ISyllabusRepository : IGenericRepository<Syllabus>
    {
        Task<List<Syllabus>> GetEnableSyllabus();
        Task<List<Syllabus>> GetDisableSyllabus();
        Task<List<Syllabus>> GetSyllabusByName(string SyllabusName);
        Task<List<Syllabus>> GetSyllabusByTrainingProgramId(Guid TrainingProgramId);
        Task<List<Syllabus>> GetSyllabusByOutputStandardId(Guid OutputStandardId);
    }
}
