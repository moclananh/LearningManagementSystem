using Domain.Base;
using Domain.EntityRelationship;
using Domain.Enum.StatusEnum;

namespace Domain.Entities
{
    public class Module : BaseEntity
    {
        public string? ModuleName { get; set; }
        public Status Status { get; set; }
        public ICollection<SyllabusModule?> SyllabusModules { get; set; }
        public ICollection<ModuleUnit?> ModuleUnits { get; set; }
        public AuditPlan AuditPlan { get; set; }
    }
}
