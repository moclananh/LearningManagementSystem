using Domain.Entities;

namespace Applications.ViewModels.UserAuditPlanViewModels
{
    public class UserAuditPlanViewModel
    {
        public Guid UserId { get; set; }
        public Guid AuditPlanId { get; set; }
    }
}
