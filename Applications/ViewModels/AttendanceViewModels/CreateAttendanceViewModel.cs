using Domain.Enum.AttendenceEnum;

namespace Applications.ViewModels.AttendanceViewModels
{
    public class CreateAttendanceViewModel
    {
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public AttendenceStatus Status { get; set; }
        public Guid UserId { get; set; }
        public Guid ClassId { get; set; }
        public string ClassCode { get; set; }
        public string fullname { get; set; }

    }
}
