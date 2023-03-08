using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.TrainingProgramModels
{
    public class UpdateTrainingProgramViewModel
    {
        public string TrainingProgramName { get; set; }
        public double Duration { get; set; }
        public Status Status { get; set; }
    }
}
