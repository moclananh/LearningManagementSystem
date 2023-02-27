using Applications.Commons;
using Applications.ViewModels.PracticeViewModels;

namespace Applications.Interfaces
{
    public interface IPracticeService
    {
        public Task<Pagination<PracticeViewModel>> GetPracticeByUnitId(Guid UnitId, int pageIndex = 0, int pageSize = 10);
        public Task<PracticeViewModel> GetPracticeById(Guid Id);
    }
}
