using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.SyllabusViewModels
{
    public class PracticeCreate
    {
        public string PracticeName { get; set; }
        public double Duration { get; set; }
        public bool IsOnline { get; set; }
    }
}
