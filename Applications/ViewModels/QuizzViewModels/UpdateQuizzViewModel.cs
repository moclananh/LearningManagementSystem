using Domain.Enum.StatusEnum;

namespace Application.ViewModels.QuizzViewModels
{
    public class UpdateQuizzViewModel
    {
        public string QuizzName { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string Deadline { get; set; }
        public bool isDone { get; set; }
        public Status Status { get; set; }
    }
}
