namespace Applications.ViewModels.UnitModuleViewModel
{
    public class ModuleUnitViewModel
    {
        public Guid ModuleId { get; set; }
        public Guid UnitId { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public Guid? ModificationBy { get; set; }
        public DateTime? DeletionDate { get; set; }
        public Guid? DeleteBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
