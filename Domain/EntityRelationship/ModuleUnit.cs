using Domain.Base;
using Domain.Entities;

namespace Domain.EntityRelationship
{
    public class ModuleUnit : BaseEntity
    {
        public Guid ModuleId { get; set; }
        public Guid UnitId { get; set; }
        public Module Module { get; set; }
        public Unit Unit { get; set; }
    }
}
