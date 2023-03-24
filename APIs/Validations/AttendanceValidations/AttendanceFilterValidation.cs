using Applications.ViewModels.AttendanceViewModels;
using FluentValidation;

namespace APIs.Validations.AttendanceValidations
{
    public class AttendanceFilterValidation : AbstractValidator<AttendanceFilterViewModel>
    {
        public AttendanceFilterValidation()
        {
            RuleFor(s => s.Status).IsInEnum();
        }
    }
}
