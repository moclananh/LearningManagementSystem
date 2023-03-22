using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.TrainingProgramModels
{
    public class UpdateTrainingProgramViewModel
    {
        public string TrainingProgramName { get; set; }
        public double Duration { get; set; }
        public string? trainingDeliveryPrinciple { get; set; }
        public double? quizCriteria { get; set; }
        public double? assignmentCriteria { get; set; }
        public double? finalTheoryCriteria { get; set; }
        public double? finalPracticalCriteria { get; set; }
        public double? passingGPA { get; set; }
        public Status Status { get; set; }
    }
}
