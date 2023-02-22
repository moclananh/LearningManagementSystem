using Domain.Base;
using Domain.Entities;

namespace Domain.EntityRelationship
{
    public class UserAuditPlan : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid AuditPlanId { get; set; }
        public User User { get; set; }
        public AuditPlan AuditPlan { get; set; }
    }
}
