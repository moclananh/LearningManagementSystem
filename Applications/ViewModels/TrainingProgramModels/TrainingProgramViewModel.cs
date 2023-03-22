using Domain.EntityRelationship;
using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.TrainingProgramModels
{
    public class TrainingProgramViewModel
    {
        public Guid Id { get; set; }
        public string TrainingProgramName { get; set; }
        public double Duration { get; set; }
        public Status Status { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string? ModificationBy { get; set; }
        public DateTime? DeletionDate { get; set; }
        public Guid? DeleteBy { get; set; }
        public bool IsDeleted { get; set; }
        public string? trainingDeliveryPrinciple { get; set; }
        public double? quizCriteria { get; set; }
        public double? assignmentCriteria { get; set; }
        public double? finalTheoryCriteria { get; set; }
        public double? finalPracticalCriteria { get; set; }
        public double? passingGPA { get; set; }
        public ICollection<ClassTrainingProgram?> ClassTrainingPrograms { get; set; }
        public ICollection<TrainingProgramSyllabus?> TrainingProgramSyllabi { get; set; }
    }
}
