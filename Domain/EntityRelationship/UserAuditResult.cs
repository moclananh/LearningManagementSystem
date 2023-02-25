using Domain.Base;
using Domain.Entities;

namespace Domain.EntityRelationship
{
    public class UserAuditResult : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid AuditResultId { get; set; }
        public User User { get; set; }
        public AuditResult AuditResult { get; set; }
    }
}
