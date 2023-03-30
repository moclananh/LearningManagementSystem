using Applications.ViewModels.OutputStandardViewModels;
using Applications.ViewModels.Response;
using Applications.ViewModels.SyllabusOutputStandardViewModels;
namespace Applications.Interfaces
{
    public interface IOutputStandardService
    {
        public Task<Response> GetAllOutputStandardAsync(int pageIndex = 0, int pageSize = 10);
        public Task<Response> GetOutputStandardByOutputStandardIdAsync(Guid OutputStandardId);
        public Task<CreateOutputStandardViewModel> CreateOutputStandardAsync(CreateOutputStandardViewModel OutputStandardDTO);
        public Task<UpdateOutputStandardViewModel> UpdatOutputStandardAsync(Guid OutputStandardId, UpdateOutputStandardViewModel OutputStandardDTO);
        public Task<Response> GetOutputStandardBySyllabusIdAsync(Guid SyllabusId, int pageIndex = 0, int pageSize = 10);
        public Task<CreateSyllabusOutputStandardViewModel> AddOutputStandardToSyllabus(Guid SyllabusId, Guid OutputStandardId);
        public Task<CreateSyllabusOutputStandardViewModel> RemoveOutputStandardToSyllabus(Guid SyllabusId, Guid OutputStandardId);
    }
}
