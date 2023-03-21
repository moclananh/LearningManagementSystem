using Applications.Commons;
using Domain.Entities;

namespace Applications.Repositories
{
    public interface IAbsentRequestRepository : IGenericRepository<AbsentRequest>
    {
        Task<Pagination<AbsentRequest>> GetAllAbsentRequestByEmail(string Email, int pageNumber = 0, int pageSize = 10);
    }
}
