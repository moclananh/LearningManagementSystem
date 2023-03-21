

namespace Applications.ViewModels.AbsentRequest
{
    public class AbsentRequestViewModel
    {
        public string AbsentReason { get; set; }
        public DateTime AbsentDate { get; set; }
        public bool IsAccepted { get; set; }
        public Guid UserId { get; set; }
        public Guid ClassId { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public Guid? ReviewBy { get; set; }
        public Guid? ApprovedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public Guid? ModificationBy { get; set; }
        public DateTime? DeletionDate { get; set; }
        public Guid? DeleteBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
