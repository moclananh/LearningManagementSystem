using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.AuditPlanViewModel
{
    public class UpdateAuditPlanViewModel
    {
        public string AuditPlanName { get; set; }
        public string Description { get; set; }
        public DateTime AuditDate { get; set; }
        public string Note { get; set; }
        public Status Status { get; set; }
    }
}
