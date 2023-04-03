using Application.ViewModels.UnitViewModels;
using FluentValidation;

namespace APIs.Validations.UnitValidations
{
    public class UnitValidation : AbstractValidator<CreateUnitViewModel>
    {
        public UnitValidation()
        {
            RuleFor(x => x.UnitName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Duration).NotEmpty();
            RuleFor(x => x.Status).IsInEnum();
        }
    }
}
