using Domain.Enum.StatusEnum;

namespace Application.ViewModels.QuizzViewModels
{
    public class UpdateQuizzViewModel
    {
        public string QuizzName { get; set; }
        public string? Description { get; set; }
        public double Duration { get; set; }
        public bool IsOnline { get; set; }
        public Status Status { get; set; }
    }
}
