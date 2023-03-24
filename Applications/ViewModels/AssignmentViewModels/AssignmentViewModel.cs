using Domain.Entities;
using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.AssignmentViewModels
{
    public class AssignmentViewModel
    {
        public Guid Id { get; set; }
        public string AssignmentName { get; set; }
        public double Duration { get; set; }
        public string? Description { get; set; }
        public Status Status { get; set; }
        public bool IsOnline { get; set; }
        public Guid UnitId { get; set; }
        public bool IsDeleted { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string? ModificationBy { get; set; }
        public Unit Unit { get; set; }

    }
}
