using Domain.Enum.StatusEnum;

namespace Application.ViewModels.UnitViewModels
{
    public class UnitViewModel
    {
        public Guid UnitId { get; set; }
        public string UnitName { get; set; }
        public double Duration { get; set; }
        public Status Status { get; set; }
    }
}
