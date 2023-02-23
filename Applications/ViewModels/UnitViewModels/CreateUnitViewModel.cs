using Domain.Enum.StatusEnum;

namespace Application.ViewModels.UnitViewModels
{
    public class CreateUnitViewModel
    {
        public string UnitName { get; set; }
        public string Duration { get; set; }
        public Status Status { get; set; }

    }
}
