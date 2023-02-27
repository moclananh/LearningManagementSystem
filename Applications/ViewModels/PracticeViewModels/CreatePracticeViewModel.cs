using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.PracticeViewModels
{
    public class CreatePracticeViewModel
    {
        public string PracticeName { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string Deadline { get; set; }
        public bool isDone { get; set; }
        public Status Status { get; set; }
        public Guid UnitId { get; set; }
    }
}
