using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.ModuleViewModels
{
    public class ModuleViewModels
    {
        public Guid Id { get; set; }
        public string ModuleName { get; set; }
        public Status Status { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public Guid? ModificationBy { get; set; }
        public DateTime? DeletionDate { get; set; }
        public Guid? DeleteBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
