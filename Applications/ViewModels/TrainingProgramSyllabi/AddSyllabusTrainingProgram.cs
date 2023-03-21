namespace Applications.ViewModels.TrainingProgramSyllabi
{
    public class AddSyllabusTrainingProgram
    {
        public Guid Id { get; set; }
        public Guid TrainingProgramId { get; set; }
        public List<Guid> SyllabusIds { get; set; }
        public string? CreatedBy { get; set; }

    }
}
