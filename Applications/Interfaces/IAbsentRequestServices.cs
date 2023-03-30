using Applications.Commons;
using Applications.ViewModels.AbsentRequest;
using Applications.ViewModels.Response;

namespace Applications.Interfaces
{
    public interface IAbsentRequestServices
    {
        public Task<Response> GetAllAbsentRequestByEmail(string Email, int pageIndex = 0, int pageSize = 10);
        public Task<Response> GetAbsentById(Guid AbsentId);

    }
}
