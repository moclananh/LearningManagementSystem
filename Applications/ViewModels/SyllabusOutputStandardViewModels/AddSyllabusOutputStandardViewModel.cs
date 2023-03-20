
namespace Applications.ViewModels.SyllabusOutputStandardViewModels
{
    public class AddSyllabusOutputStandardViewModel
    {
        public Guid Id { get; set; }
        public Guid SyllabusId { get; set; }
        public List<Guid> OutputStandardIds { get; set; }
        public string? CreatedBy { get; set; }
    }

}
