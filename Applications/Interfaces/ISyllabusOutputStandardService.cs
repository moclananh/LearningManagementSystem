using Applications.Commons;
using Applications.ViewModels.Response;
using Applications.ViewModels.SyllabusOutputStandardViewModels;

namespace Applications.Interfaces
{
    public interface ISyllabusOutputStandardService
    {
        public Task<Response> GetAllSyllabusOutputStandards(int pageIndex = 0, int pageSize = 10);
        public Task<Response> AddMultipleOutputStandardsToSyllabus(Guid syllabusId, List<Guid> outputStandardIds);
    }
}
