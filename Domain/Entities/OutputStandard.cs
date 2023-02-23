using Domain.Base;
using Domain.EntityRelationship;

namespace Domain.Entities
{
    public class OutputStandard : BaseEntity
    {
        public string OutputStandardCode { get; set; }
        public string Description { get; set; }
        public ICollection<SyllabusOutputStandard?> SyllabusOutputStandards { get; set; }
    }
}
