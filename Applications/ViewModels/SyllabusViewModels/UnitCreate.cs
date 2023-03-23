
namespace Applications.ViewModels.SyllabusViewModels
{
    public class UnitCreate
    {
        public string UnitName { get; set; }
        public string UnitCode { get; set; }
        public List<LectureCreate>? Lectures { get; set; }
        public List<AssignmentCreate>? Assignments { get; set; }
        public List<QuizzCreate>? Quizzes { get; set; }
        public List<PracticeCreate>? Practices { get; set; }
    }
}
