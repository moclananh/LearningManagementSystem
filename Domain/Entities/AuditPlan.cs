using Domain.Base;
using Domain.EntityRelationship;
using Domain.Enum.StatusEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AuditPlan : BaseEntity
    {
        public string AuditPlanName { get; set; }
        public string Description { get; set; }
        public DateTime AuditDate { get; set; }
        public string Note { get; set; }
        public Status Status { get; set; }
        public Guid ModuleId { get; set; }
        public Module Module { get; set; }
        public ICollection<AuditResult> AuditResults { get; set; }
        public ICollection<AuditQuestion> AuditQuestions { get; set; }
        public ICollection<UserAuditPlan?> UserAuditPlans { get; set; }
        public Guid ClassId { get; set; }
        public Class Class { get; set; }
    }
}
