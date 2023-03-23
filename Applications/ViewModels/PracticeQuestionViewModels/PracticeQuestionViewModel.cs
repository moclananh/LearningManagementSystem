
namespace Applications.ViewModels.PracticeQuestionViewModels
{
    public class PracticeQuestionViewModel
    {
        public Guid PracticeId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Note { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string? ModificationBy { get; set; }
        public bool IsDeleted { get; set; }

    }
}
