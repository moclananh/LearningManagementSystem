using Domain.Base;

namespace Domain.Entities
{
    public class AuditQuestion : BaseEntity
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Note { get; set; }
        public Guid AuditPlanId { get; set; }
        public AuditPlan AuditPlan { get; set; }
    }
}
