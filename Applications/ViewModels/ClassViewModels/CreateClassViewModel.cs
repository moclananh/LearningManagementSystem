using Domain.Entities;
using Domain.Enum.ClassEnum;
using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.ClassViewModels
{
    public class CreateClassViewModel
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
        public double? TotalDuration { get; set; }
        public Guid? TraingProgramId { get; set; }
        public List<Guid?>? TrainerId { get; set; }
        public List<Guid?>? AdminId { get; set; }
    }
}
