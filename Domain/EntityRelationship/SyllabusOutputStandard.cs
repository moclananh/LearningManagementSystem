using Domain.Base;
using Domain.Entities;

namespace Domain.EntityRelationship
{
    public class SyllabusOutputStandard : BaseEntity
    {
        public Guid SyllabusId { get; set; }
        public Guid OutputStandardId { get; set; }
        public Syllabus Syllabus { get; set; }
        public OutputStandard OutputStandard { get; set; }
    }
}
