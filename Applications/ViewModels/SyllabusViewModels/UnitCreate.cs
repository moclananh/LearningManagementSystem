
namespace Applications.ViewModels.SyllabusViewModels
{
    public class UnitCreate
    {
        public string UnitName { get; set; }
        public double Duration { get; set; }
        public List<LectureCreate>? Lectures { get; set; }
        public List<AssignmentCreate>? Assignments { get; set; }
        public List<QuizzCreate>? Quizzs { get; set; }
        public List<PracticeCreate>? Practices { get; set; }
    }
}
