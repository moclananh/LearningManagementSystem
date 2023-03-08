using Domain.Entities;


namespace Applications.ViewModels.AuditResultViewModels
{
    public class AuditResultViewModel
    {
        public Guid Id { get; set; }
        public string Score { get; set; }
        public string Note { get; set; }
        public Guid AuditPlanId { get; set; }
        public AuditPlan AuditPlan { get; set; }
        public Guid UserId { get; set; }
    }
}
