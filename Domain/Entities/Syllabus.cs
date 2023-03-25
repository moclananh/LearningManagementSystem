using Domain.Base;
using Domain.EntityRelationship;
using Domain.Enum.StatusEnum;

namespace Domain.Entities
{
    public class Syllabus : BaseEntity
    {
        public string SyllabusName { get; set; }
        public string SyllabusCode { get; set; }
        public double Duration { get; set; }
        public string Level { get; set; }
        public string CourseObjective { get; set; }
        public string Version { get; set; }
        public string techicalrequirement { get; set; }
        public Status Status { get; set; }
        public string? trainingDeliveryPrinciple { get; set; }
        public double? quizCriteria { get; set; }
        public double? assignmentCriteria { get; set; }
        public double? finalCriteria { get; set; }
        public double? finalTheoryCriteria { get; set; }
        public double? finalPracticalCriteria { get; set; }
        public double? passingGPA { get; set; }
        public ICollection<TrainingProgramSyllabus> TrainingProgramSyllabi { get; set; }
        public ICollection<SyllabusOutputStandard> SyllabusOutputStandards { get; set; }
        public ICollection<SyllabusModule> SyllabusModules { get; set; }
    }
}
