using Domain.Base;
using Domain.Entities;

namespace Domain.EntityRelationship
{
    public class SyllabusModule : BaseEntity
    {
        public Guid SyllabusId { get; set; }
        public Guid ModuleId { get; set; }
        public Syllabus Syllabus { get; set; }
        public Module Module { get; set; }
    }
}
