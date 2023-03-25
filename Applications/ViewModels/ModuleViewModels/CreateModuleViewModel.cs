using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.ModuleViewModels
{
    public class CreateModuleViewModel
    {
        public string? ModuleName { get; set; }
        public Status Status { get; set; }
    }
}
