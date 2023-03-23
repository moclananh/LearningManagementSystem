using Applications.ViewModels.ClassViewModels;
using FluentValidation;

namespace APIs.Validations.ClassValidations
{
    public class ClassFilterValidation : AbstractValidator<ClassFiltersViewModel>
    {
        public ClassFilterValidation()
        {
            RuleFor(x => x.Location).IsInEnum();
            RuleFor(x => x.StartDate).LessThan(x => x.EndDate);
            RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);
            RuleFor(x => x.FSU).IsInEnum();
            RuleFor(x => x.Attendee).IsInEnum();
        }
    }
}
