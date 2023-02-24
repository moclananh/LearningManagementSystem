using Applications.ViewModels.OutputStandardViewModels;

namespace Applications.Interfaces
{
    public interface IOutputStandardService
    {
        public Task<List<OutputStandardViewModel>> ViewAllOutputStandardAsync();
        public Task<OutputStandardViewModel> GetOutputStandardByOutputStandardIdAsync(Guid OutputStandardId);
        public Task<CreateOutputStandardViewModel> CreateOutputStandardAsync(CreateOutputStandardViewModel OutputStandardDTO);
        public Task<UpdateOutputStandardViewModel> UpdatOutputStandardAsync(Guid OutputStandardId, UpdateOutputStandardViewModel OutputStandardDTO);
    }
}
