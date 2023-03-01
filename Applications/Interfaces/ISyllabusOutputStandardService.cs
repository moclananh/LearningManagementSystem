using Applications.Commons;
using Applications.ViewModels.SyllabusOutputStandardViewModels;

namespace Applications.Interfaces
{
    public interface ISyllabusOutputStandardService
    {
        public Task<Pagination<SyllabusOutputStandardViewModel>> GetAllSyllabusOutputStandards(int pageIndex = 0, int pageSize = 10);
    }
}
