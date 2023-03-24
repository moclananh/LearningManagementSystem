using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.PracticeViewModels
{
    public class PracticeViewModel
    {
        public Guid Id { get; set; }
        public string PracticeName { get; set; }
        public string? Description { get; set; }
        public double Duration { get; set; }
        public bool IsOnline { get; set; }
        public Status Status { get; set; }
        public Guid UnitId { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string? ModificationBy { get; set; }
    }
}
