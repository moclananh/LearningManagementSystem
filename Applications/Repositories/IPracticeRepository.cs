using Applications.Commons;
using Applications.Repositories;
using Domain.Entities;

namespace Applications.Interfaces
{
    public interface IPracticeRepository : IGenericRepository<Practice>
    {
        Task<Pagination<Practice>> GetPracticeByName(string Name, int pageNumber = 0, int pageSize = 10);
        Task<Pagination<Practice>> GetPracticeByUnitId(Guid UnitId, int pageNumber = 0, int pageSize = 10);
        Task<Practice> GetByPracticeId(Guid PracticeId);
        Task<Pagination<Practice>> GetEnablePractices(int pageNumber = 0, int pageSize = 10);
        Task<Pagination<Practice>> GetDisablePractices(int pageNumber = 0, int pageSize = 10);
    }
}
