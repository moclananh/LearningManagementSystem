using Application.ViewModels.QuizzViewModels;
using Applications.Commons;
using Applications.ViewModels.PracticeViewModels;

namespace Applications.Interfaces
{
    public interface IPracticeService
    {
        public Task<Pagination<PracticeViewModel>> GetPracticeByUnitId(Guid UnitId, int pageIndex = 0, int pageSize = 10);
        public Task<PracticeViewModel> GetPracticeById(Guid Id);
        public Task<CreatePracticeViewModel> CreatePracticeAsync(CreatePracticeViewModel QuizzDTO);
        public Task<Pagination<PracticeViewModel>> GetpracticeByName(string Name, int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<PracticeViewModel>> GetAllPractice(int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<PracticeViewModel>> GetEnablePractice(int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<PracticeViewModel>> GetDisablePractice(int pageIndex = 0, int pageSize = 10);
    }
}
