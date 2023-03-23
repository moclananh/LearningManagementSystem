namespace Applications.ViewModels.AssignmentQuestionViewModels
{
    public class AssignmentQuestionViewModel
    {
        public Guid id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Note { get; set; }
        public Guid AssignmentId { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string? ModificationBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
