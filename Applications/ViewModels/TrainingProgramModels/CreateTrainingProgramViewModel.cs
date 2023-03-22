using Domain.Enum.StatusEnum;

namespace Application.ViewModels.TrainingProgramModels
{
    public class CreateTrainingProgramViewModel
    {
        public string TrainingProgramName { get; set; }
        public string? trainingDeliveryPrinciple { get; set; }
        public double? quizCriteria { get; set; }
        public double? assignmentCriteria { get; set; }
        public double? finalTheoryCriteria { get; set; }
        public double? finalPracticalCriteria { get; set; }
        public double? passingGPA { get; set; }
        public Status Status { get; set; }
    }
}
