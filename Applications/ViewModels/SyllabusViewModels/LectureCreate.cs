using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.SyllabusViewModels
{
    public class LectureCreate
    {
        public string LectureName { get; set; }
        public double Duration { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
    }
}
