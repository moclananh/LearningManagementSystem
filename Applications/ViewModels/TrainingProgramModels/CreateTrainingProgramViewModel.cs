using Domain.Enum.StatusEnum;

namespace Application.ViewModels.TrainingProgramModels
{
    public class CreateTrainingProgramViewModel
    {
        public string TrainingProgramName { get; set; }
        public double Duration { get; set; }
        public Status Status { get; set; }
    }
}
