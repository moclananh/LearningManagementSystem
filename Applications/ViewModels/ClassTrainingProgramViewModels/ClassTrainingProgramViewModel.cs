﻿namespace Applications.ViewModels.ClassTrainingProgramViewModels
{
    public class ClassTrainingProgramViewModel
    {
        public Guid ClassId { get; set; }
        public Guid TrainingProgramId { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public Guid? ModificationBy { get; set; }
        public DateTime? DeletionDate { get; set; }
        public Guid? DeleteBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
