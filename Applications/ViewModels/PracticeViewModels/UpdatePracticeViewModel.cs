using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.PracticeViewModels
{
    public class UpdatePracticeViewModel
    {
        public string PracticeName { get; set; }
        public string? Description { get; set; }
        public double Duration { get; set; }
        public bool IsOnline { get; set; }
        public Status Status { get; set; }
    }
}
