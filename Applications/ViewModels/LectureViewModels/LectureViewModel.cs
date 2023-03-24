using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.LectureViewModels
{
    public class LectureViewModel
    {
        public Guid Id { get; set; }
        public string LectureName { get; set; }
        public double Duration { get; set; }
        public string? Description { get; set; }
        public Status Status { get; set; }
        public bool IsOnline { get; set; }
        public Guid UnitId { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
