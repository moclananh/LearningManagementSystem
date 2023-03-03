using Applications.Commons;
using Applications.ViewModels.PracticeViewModels;
using Applications.ViewModels.Response;

namespace Applications.Interfaces
{
    public interface IPracticeService
    {
        public Task<Pagination<PracticeViewModel>> GetPracticeByUnitId(Guid UnitId, int pageIndex = 0, int pageSize = 10);
        public Task<PracticeViewModel> GetPracticeById(Guid Id);
        public Task<CreatePracticeViewModel> CreatePracticeAsync(CreatePracticeViewModel PracticeDTO);
        public Task<Response> GetPracticeByName(string Name, int pageIndex = 0, int pageSize = 10);
        public Task<Response> GetAllPractice(int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<PracticeViewModel>> GetEnablePractice(int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<PracticeViewModel>> GetDisablePractice(int pageIndex = 0, int pageSize = 10);
        public Task<UpdatePracticeViewModel> UpdatePractice(Guid UnitId, UpdatePracticeViewModel practiceDTO);
    }
}
