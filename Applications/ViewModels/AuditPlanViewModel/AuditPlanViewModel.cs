using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.AuditPlanViewModel
{
    public class AuditPlanViewModel
    {
        public Guid Id { get; set; }
        public string AuditPlanName { get; set; }
        public string Description { get; set; }
        public DateTime AuditDate { get; set; }
        public string Note { get; set; }
        public Status Status { get; set; }
        public Guid ModuleId { get; set; }
        public Guid ClassId { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string? ModificationBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
