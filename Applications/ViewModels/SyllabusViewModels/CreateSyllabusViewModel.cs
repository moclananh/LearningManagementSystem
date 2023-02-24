
using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.SyllabusViewModels
{
    public class CreateSyllabusViewModel
    {
        public string SyllabusName { get; set; }
        public string SyllabusCode { get; set; }
        public string Duration { get; set; }
        public string Level { get; set; }
        public string CourseObjective { get; set; }
        public string Version { get; set; }
        public Status Status { get; set; }
    }
}
