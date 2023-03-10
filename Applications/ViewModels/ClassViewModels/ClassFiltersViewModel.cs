using Domain.Enum.ClassEnum;
using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.ClassViewModels
{
    public class ClassFiltersViewModel
    {
        LocationEnum? locations { get; set; }
        ClassTimeEnum? classTime { get; set; }
        Status? status { get; set; }
        AttendeeEnum? attendee { get; set; }
        FSUEnum? fsu { get; set; }
        DateTime? startDate { get; set; }
        DateTime? endDate { get; set; }
    }
}
