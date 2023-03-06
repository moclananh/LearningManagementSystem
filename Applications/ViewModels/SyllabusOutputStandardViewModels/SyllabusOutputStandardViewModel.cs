using Domain.Entities;

namespace Applications.ViewModels.SyllabusOutputStandardViewModels
{
    public class SyllabusOutputStandardViewModel
    {
        public Guid SyllabusId { get; set; }
        public Guid OutputStandardId { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public Guid? ModificationBy { get; set; }
        public DateTime? DeletionDate { get; set; }
        public Guid? DeleteBy { get; set; }
        public bool IsDeleted { get; set; }
        public OutputStandard OutputStandard { get; set; }
    }
}
