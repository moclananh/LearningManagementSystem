using Domain.Base;
using Domain.EntityRelationship;
using Domain.Enum.StatusEnum;

namespace Domain.Entities
{
    public class Unit : BaseEntity
    {
        public string UnitName { get; set; }
        public double Duration { get; set; }
        public Status Status { get; set; }
        public string? UnitCode { get; set; }
        public ICollection<ModuleUnit?> ModuleUnits { get; set; }
        public ICollection<Lecture> Lectures { get; set; } 
        public ICollection<Assignment> Assignments { get; set; }
        public ICollection<Practice> Practices { get; set; }
        public ICollection<Quizz> Quizzs { get; set; }
    }
}
