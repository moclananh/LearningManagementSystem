
namespace Applications.ViewModels.OutputStandardViewModels
{
    public class OutputStandardViewModel
    {
        public Guid Id { get; set; }
        public string OutputStandardCode { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string? ModificationBy { get; set; }
    }
}
