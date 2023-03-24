using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.PracticeViewModels
{
    public class CreatePracticeViewModel
    {
        public string PracticeName { get; set; }
        public double Duration { get; set; }
        public string? Description { get; set; }
        public bool IsOnline { get; set; }
        public Status Status { get; set; }
        public Guid UnitId { get; set; }
    }
}
