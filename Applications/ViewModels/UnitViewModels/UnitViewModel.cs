using Domain.Entities;
using Domain.Enum.StatusEnum;

namespace Application.ViewModels.UnitViewModels
{
    public class UnitViewModel
    {
        public Guid UnitId { get; set; }
        public string UnitName { get; set; }
        public double Duration { get; set; }
        public Status Status { get; set; }
        public string? UnitCode { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string? ModificationBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
