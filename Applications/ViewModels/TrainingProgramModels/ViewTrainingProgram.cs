using Domain.Enum.StatusEnum;

namespace Application.ViewModels.TrainingProgramModels
{
    public class ViewTrainingProgram
    {
        public string TrainingProgramName { get; set; }
        public string Duration { get; set; }
        public Status Status { get; set; }
    }
}
