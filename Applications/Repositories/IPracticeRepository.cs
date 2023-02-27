using Applications.Commons;
using Applications.Repositories;
using Domain.Entities;

namespace Applications.Interfaces
{
    public interface IPracticeRepository : IGenericRepository<Practice>
    {
        Task<Pagination<Practice>> GetPracticeByUnitId(Guid UnitId, int pageNumber = 0, int pageSize = 10);
        Task<Practice> GetByPracticeId(Guid PracticeId);
    }
}
