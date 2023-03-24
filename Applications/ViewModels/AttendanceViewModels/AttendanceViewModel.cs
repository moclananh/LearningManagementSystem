using Domain.Enum.AttendenceEnum;

namespace Applications.ViewModels.AttendanceViewModels
{
    public class AttendanceViewModel
    {
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public AttendenceStatus Status { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string? ModificationBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
