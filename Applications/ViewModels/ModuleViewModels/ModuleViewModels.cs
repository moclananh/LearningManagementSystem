using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.ModuleViewModels
{
    public class ModuleViewModels
    {
        public Guid Id { get; set; }
        public string? ModuleName { get; set; }
        public Status Status { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
