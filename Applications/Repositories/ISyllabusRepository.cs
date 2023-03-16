using Applications.Commons;
using Domain.Entities;

namespace Applications.Repositories
{
    public interface ISyllabusRepository : IGenericRepository<Syllabus>
    {
        Task<Syllabus> GetSyllabusDetail(Guid SyllabusId);
        Task<Pagination<Syllabus>> GetAllSyllabusDetail(int pageNumber = 0, int pageSize = 10);
        Task<Pagination<Syllabus>> GetSyllabusByCreationDate(DateTime startDate, DateTime endDate, int pageNumber = 0, int pageSize = 10);
        Task<Pagination<Syllabus>> GetEnableSyllabus(int pageNumber = 0, int pageSize = 10);
        Task<Pagination<Syllabus>> GetDisableSyllabus(int pageNumber = 0, int pageSize = 10);
        Task<Pagination<Syllabus>> GetSyllabusByName(string SyllabusName, int pageNumber = 0, int pageSize = 10);
        Task<Pagination<Syllabus>> GetSyllabusByTrainingProgramId(Guid TrainingProgramId, int pageNumber = 0, int pageSize = 10);
        Task<Pagination<Syllabus>> GetSyllabusByOutputStandardId(Guid OutputStandardId, int pageNumber = 0, int pageSize = 10);
    }
}
