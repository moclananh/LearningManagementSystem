﻿using Domain.Base;

namespace Domain.Entities
{
    public class AuditResult : BaseEntity
    {
        public string Score { get; set; }
        public string Note { get; set; }
        public Guid AuditPlanId { get; set; }
        public AuditPlan AuditPlan { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
