using Domain.Enum.AttendenceEnum;

namespace Applications.ViewModels.AttendanceViewModels
{
    public class AttendanceFilterViewModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Email { get; set; }
        public AttendenceStatus? Status { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
