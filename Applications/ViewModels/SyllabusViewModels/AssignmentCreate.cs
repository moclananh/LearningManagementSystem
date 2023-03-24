using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.SyllabusViewModels
{
    public class AssignmentCreate
    {
        public string AssignmentName { get; set; }
        public double Duration { get; set; }
        public bool IsOnline { get; set; }
    }
}
