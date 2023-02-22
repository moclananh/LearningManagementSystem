using Domain.Base;
using Domain.Entities;

namespace Domain.EntityRelationship
{
    public class ClassTrainingProgram : BaseEntity
    {
        public Guid ClassId { get; set; }
        public Guid TrainingProgramId { get; set; }
        public Class Class { get; set; }
        public TrainingProgram TrainingProgram { get; set; }
    }
}
