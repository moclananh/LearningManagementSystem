using Domain.Entities;

namespace Applications.ViewModels.UserAuditPlanViewModels
{
    public class UserAuditPlanViewModel
    {
        public Guid UserId { get; set; }
        public Guid AuditPlanId { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string? ModificationBy { get; set; }
    }
}
