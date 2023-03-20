namespace Applications.ViewModels.UnitModuleViewModel
{
    public class ModuleUnitViewModel
    {
        public Guid ModuleId { get; set; }
        public Guid UnitId { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string? ModificationBy { get; set; }
        public DateTime? DeletionDate { get; set; }
        public string? DeleteBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
