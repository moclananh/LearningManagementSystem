using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.ModuleViewModels
{
    public class UpdateModuleViewModel
    {
        public string? ModuleName { get; set; }
        public Status Status { get; set; }      
    }
}
