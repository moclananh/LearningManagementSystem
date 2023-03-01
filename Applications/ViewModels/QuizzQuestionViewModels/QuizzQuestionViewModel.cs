
using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.QuizzQuestionViewModels
{
    public class QuizzQuestionViewModel
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Note { get; set; }
        public Guid QuizzId { get; set; }
    }
}
