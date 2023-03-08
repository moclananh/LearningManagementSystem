using Domain.Base;
using Domain.EntityRelationship;
using Domain.Enum.StatusEnum;

namespace Domain.Entities
{
    public class Syllabus : BaseEntity
    {
        public string SyllabusName { get; set; }
        public string SyllabusCode { get; set; }
        public float Duration { get; set; }
        public string Level { get; set; }
        public string CourseObjective { get; set; }
        public string Version { get; set; }
        public string techicalrequirement { get; set; }
        
        public Status Status { get; set; }
        public ICollection<TrainingProgramSyllabus> TrainingProgramSyllabi { get; set; }
        public ICollection<SyllabusOutputStandard> SyllabusOutputStandards { get; set; }
        public ICollection<SyllabusModule> SyllabusModules { get; set; }
    }
}
