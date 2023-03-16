using Applications.Commons;
using Applications.ViewModels.Response;
using Domain.Entities;

namespace Applications.Repositories
{
    public interface IOutputStandardRepository : IGenericRepository<OutputStandard>
    {
        Task<Pagination<OutputStandard>> GetOutputStandardBySyllabusIdAsync(Guid SyllabusId, int pageNumber = 0, int pageSize = 10);
    }
}
