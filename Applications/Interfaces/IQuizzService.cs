using Application.ViewModels.QuizzViewModels;
using Applications.Commons;

namespace Applications.Interfaces
{
    public interface IQuizzServices
    {
        public Task<QuizzViewModel> GetQuizzByQuizzIdAsync(Guid QuizzId);
        public Task<CreateQuizzViewModel> CreateQuizzAsync(CreateQuizzViewModel QuizzDTO);
        public Task<UpdateQuizzViewModel> UpdatQuizzAsync(Guid QuizzId, UpdateQuizzViewModel QuizzDTO);
        public Task<Pagination<QuizzViewModel>> GetQuizzByName(string QuizzName, int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<QuizzViewModel>> GetAllQuizzes(int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<QuizzViewModel>> GetEnableQuizzes(int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<QuizzViewModel>> GetDisableQuizzes(int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<QuizzViewModel>> GetQuizzByUnitIdAsync(Guid UnitId, int pageIndex = 0, int pageSize = 10);
    }
}
