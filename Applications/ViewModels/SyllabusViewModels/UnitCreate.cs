
namespace Applications.ViewModels.SyllabusViewModels
{
    public class UnitCreate
    {
        public string UnitName { get; set; }
        public string UnitCode { get; set; }
        public List<LectureCreate>? Lecture { get; set; }
        public List<AssignmentCreate>? Assignment { get; set; }
        public List<QuizzCreate>? Quizz { get; set; }
        public List<PracticeCreate>? Practice { get; set; }
    }
}
