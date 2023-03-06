using Applications.Commons;
using Domain.Entities;

namespace Applications.Repositories
{
    public interface ISyllabusRepository : IGenericRepository<Syllabus>
    {
        Task<Pagination<Syllabus>> GetEnableSyllabus(int pageNumber = 0, int pageSize = 10);
        Task<Pagination<Syllabus>> GetDisableSyllabus(int pageNumber = 0, int pageSize = 10);
        Task<Pagination<Syllabus>> GetSyllabusByName(string SyllabusName, int pageNumber = 0, int pageSize = 10);
        Task<Pagination<Syllabus>> GetSyllabusByTrainingProgramId(Guid TrainingProgramId, int pageNumber = 0, int pageSize = 10);
        Task<Pagination<Syllabus>> GetSyllabusByOutputStandardId(Guid OutputStandardId, int pageNumber = 0, int pageSize = 10);
        Task<Syllabus> GetSyllabusDetails(Guid syllabusId);
        Task<Pagination<Syllabus>> GetAllSyllabusDetail(int pageNumber = 0, int pageSize = 10);
    }
}
