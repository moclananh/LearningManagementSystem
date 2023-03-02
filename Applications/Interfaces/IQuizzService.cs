using Application.ViewModels.QuizzViewModels;
using Applications.ViewModels.Response;

namespace Applications.Interfaces
{
    public interface IQuizzServices
    {
        public Task<Response> GetQuizzByQuizzIdAsync(Guid QuizzId);
        public Task<CreateQuizzViewModel> CreateQuizzAsync(CreateQuizzViewModel QuizzDTO);
        public Task<UpdateQuizzViewModel> UpdatQuizzAsync(Guid QuizzId, UpdateQuizzViewModel QuizzDTO);
        public Task<Response> GetQuizzByName(string QuizzName, int pageIndex = 0, int pageSize = 10);
        public Task<Response> GetAllQuizzes(int pageIndex = 0, int pageSize = 10);
        public Task<Response> GetEnableQuizzes(int pageIndex = 0, int pageSize = 10);
        public Task<Response> GetDisableQuizzes(int pageIndex = 0, int pageSize = 10);
        public Task<Response> GetQuizzByUnitIdAsync(Guid UnitId, int pageIndex = 0, int pageSize = 10);
    }
}
