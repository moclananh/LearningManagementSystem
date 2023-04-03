

namespace Applications.ViewModels.AuditResultViewModels
{
    public class AuditResultViewModel
    {
        public Guid Id { get; set; }
        public string Score { get; set; }
        public string Note { get; set; }
        public string? CreatedBy { get; set; }
        public Guid AuditPlanId { get; set; }
        public Guid UserId { get; set; }
    }
}
