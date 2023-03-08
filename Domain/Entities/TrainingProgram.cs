using Domain.Base;
using Domain.EntityRelationship;
using Domain.Enum.StatusEnum;

namespace Domain.Entities
{
    public class TrainingProgram : BaseEntity
    {
        public string TrainingProgramName { get; set; }
        public double Duration { get; set; }
        public Status Status { get; set; }
        public ICollection<ClassTrainingProgram?> ClassTrainingPrograms { get; set; }
        public ICollection<TrainingProgramSyllabus?> TrainingProgramSyllabi { get; set; }
    }
}
