

namespace Applications.ViewModels.AuditResultViewModels
{
    public class UpdateAuditResultViewModel
    {
        public string Score { get; set; }
        public string Note { get; set; }
        public Guid AuditPlanId { get; set; }
        public Guid UserId { get; set; }
    }
}
