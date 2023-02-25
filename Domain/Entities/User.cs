﻿using Domain.Base;
using Domain.EntityRelationship;
using Domain.Enum.RoleEnum;
using Domain.Enum.StatusEnum;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DOB { get; set; }
        public bool Gender { get; set; }
        public string Image { get; set; }
        public string Level { get; set; }
        public Role Role { get; set; }
        public Status Status { get; set; }
        public ICollection<UserAuditPlan?> UserAuditPlans { get; set; }
        public ICollection<UserAuditResult> UserAuditResults { get; set; }
        public ICollection<AbsentRequest> AbsentRequests { get; set; }
        public ICollection<ClassUser?> ClassUsers { get; set; }
        public ICollection<Attendance> Attendences { get; set; }
    }
}
