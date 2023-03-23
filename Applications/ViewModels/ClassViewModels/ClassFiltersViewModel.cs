using Domain.Enum.ClassEnum;
using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.ClassViewModels
{
    public class ClassFiltersViewModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public LocationEnum? Location { get; set; }
        public FSUEnum? FSU { get; set; }
        public AttendeeEnum? Attendee { get; set; }
        public Status? Status { get; set; }
    }
}
