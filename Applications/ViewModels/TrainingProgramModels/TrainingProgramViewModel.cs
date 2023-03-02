﻿using Domain.EntityRelationship;
using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.TrainingProgramModels
{
    public class TrainingProgramViewModel
    {
        public Guid Id { get; set; }
        public string TrainingProgramName { get; set; }
        public string Duration { get; set; }
        public Status Status { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public Guid? ModificationBy { get; set; }
        public DateTime? DeletionDate { get; set; }
        public Guid? DeleteBy { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<ClassTrainingProgram?> ClassTrainingPrograms { get; set; }
        public ICollection<TrainingProgramSyllabus?> TrainingProgramSyllabi { get; set; }
    }
}