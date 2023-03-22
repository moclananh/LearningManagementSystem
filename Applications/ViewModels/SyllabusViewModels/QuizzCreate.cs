using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.SyllabusViewModels
{
    public class QuizzCreate
    {
        public string QuizzName { get; set; }
        public string Description { get; set; }
        public double Duration { get; set; }
        public Status Status { get; set; }
    }
}
