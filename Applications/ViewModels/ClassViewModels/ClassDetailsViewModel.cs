using Applications.ViewModels.UserViewModels;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Enum.ClassEnum;
using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.ClassViewModels
{
    public class ClassDetailsViewModel
    {
        public Guid Id { get; set; }
        public string ClassName { get; set; }
        public string ClassCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LocationEnum Location { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public FSUEnum FSU { get; set; }
        public AttendeeEnum Attendee { get; set; }
        public Status Status { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string? ModificationBy { get; set; }
        public DateTime? DeletionDate { get; set; }
        public string? DeleteBy { get; set; }
        public bool IsDeleted { get; set; }
        public double? TotalDuration { get; set; }
        public ICollection<User>? Trainner { get; set; }
        public ICollection<User>? SuperAdmin { get; set; }
        public ICollection<User>? Student { get; set; }
        public ICollection<User>? ClassAdmin { get; set; }
        public ICollection<ClassTrainingProgram>? ClassTrainingPrograms { get; set; }
        public ICollection<AuditPlan>? AuditPlans { get; set; }
        public ICollection<ClassUser>? ClassUsers { get; set; }
    }
}
