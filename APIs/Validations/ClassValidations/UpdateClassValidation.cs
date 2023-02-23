using Applications.ViewModels.ClassViewModels;
using FluentValidation;

namespace APIs.Validations.ClassValidations
{
    public class UpdateClassValidation : AbstractValidator<UpdateClassViewModel>
    {
        public UpdateClassValidation()
        {
            RuleFor(x => x.ClassName).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(x => x.ClassCode).NotNull().NotEmpty().MaximumLength(50);
            RuleFor(x => x.Location).NotNull().NotEmpty().MaximumLength(100);
        }
    }
}
