using Domain.Enum.ClassEnum;
using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.ClassViewModels
{
    public class ClassViewModel
    {
        public Guid Id { get; set; }
        public string ClassName { get; set; }
        public string ClassCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Durations
        {
            get
            {
                TimeSpan durations = EndDate - StartDate;
                return durations.Days;
            }
        }
        public LocationEnum Location { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public FSUEnum FSU { get; set; }
        public AttendeeEnum Attendee { get; set; }
        public Status Status { get; set; }
        public DateTime? ModificationDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public Guid? ReviewBy { get; set; }
        public Guid? ApprovedBy { get; set; }
        public Guid? ModificationBy { get; set; }
        public DateTime? DeletionDate { get; set; }
        public Guid? DeleteBy { get; set; }
        public bool IsDeleted { get; set; }
        public double? TotalDuration { get; set; }
    }
}
