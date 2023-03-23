
using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.QuizzQuestionViewModels
{
    public class QuizzQuestionViewModel
    {
        public Guid id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Note { get; set; }
        public Guid QuizzId { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string? ModificationBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
