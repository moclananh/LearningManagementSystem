using Domain.Base;
using Domain.EntityRelationship;
using Domain.Enum.ClassEnum;
using Domain.Enum.StatusEnum;

namespace Domain.Entities
{
    public class Class : BaseEntity
    {
        public string ClassName { get; set; }
        public string ClassCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LocationEnum Location { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public FSUEnum FSU { get; set; }
        public AttendeeEnum Attendee { get; set; }
        public Status Status { get; set; }
        public double? TotalDuration { get; set; }
        public ICollection<ClassTrainingProgram>? ClassTrainingPrograms { get; set; }
        public ICollection<AuditPlan>? AuditPlans { get; set; }
        public ICollection<ClassUser>? ClassUsers { get; set; }
        public ICollection<Attendance>? Attendences { get; set; }
        public ICollection<AbsentRequest>? AbsentRequests { get; set; }
    }
}
