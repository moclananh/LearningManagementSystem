
using Domain.EntityRelationship;
using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.SyllabusViewModels
{
    public class SyllabusViewModel
    {
        public Guid Id { get; set; }
        public string SyllabusName { get; set; }
        public string SyllabusCode { get; set; }
        public string Duration { get; set; }
        public string Level { get; set; }
        public string CourseObjective { get; set; }
        public string Version { get; set; }
        public Status Status { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public Guid? ModificationBy { get; set; }
        public DateTime? DeletionDate { get; set; }
        public Guid? DeleteBy { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<TrainingProgramSyllabus?> TrainingProgramSyllabi { get; set; }
        public ICollection<SyllabusOutputStandard?> SyllabusOutputStandards { get; set; }
        public ICollection<SyllabusModule?> SyllabusModules { get; set; }
    }
}
