using Application.ViewModels.QuizzViewModels;

namespace Applications.Interfaces
{
    public interface IQuizzServices
    {
        public Task<List<QuizzViewModel>> ViewAllQuizzAsync();
        public Task<QuizzViewModel> GetQuizzByQuizzIdAsync(Guid QuizzId);
        public Task<List<QuizzViewModel>> GetQuizzByName(string Name);
        public Task<List<QuizzViewModel>> GetQuizzByUnitIdAsync(Guid UnitId);
        public Task<CreateQuizzViewModel> CreateQuizzAsync(CreateQuizzViewModel QuizzDTO);
        public Task<UpdateQuizzViewModel> UpdatQuizzAsync(Guid QuizzId, UpdateQuizzViewModel QuizzDTO);
        public Task<List<QuizzViewModel>> GetEnableQuizzes();
        public Task<List<QuizzViewModel>> GetDisableQuizzes();
    }
}
