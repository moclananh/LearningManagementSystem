using Domain.Entities;
using Domain.Enum.StatusEnum;

namespace Application.ViewModels.QuizzViewModels
{
    public class QuizzViewModel
    {
        public Guid Id { get; set; }
        public string QuizzName { get; set; }
        public string? Description { get; set; }
        public double Duration { get; set; }
        public bool IsOnline { get; set; }
        public Status Status { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string? ModificationBy { get; set; }
        public bool IsDeleted { get; set; }
        public Guid UnitId { get; set; }
    }
}
