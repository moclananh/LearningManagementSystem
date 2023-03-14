using Applications.ViewModels.ClassViewModels;
using FluentValidation;

namespace APIs.Validations.ClassValidations
{
    public class CreateClassValidation : AbstractValidator<CreateClassViewModel>
    {
        public CreateClassValidation()
        {
            RuleFor(x => x.ClassName).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(x => x.ClassCode).NotNull().NotEmpty().MaximumLength(50);
            RuleFor(x => x.Location).NotNull().IsInEnum();
            RuleFor(x => x.ClassTime).NotNull().IsInEnum();
            RuleFor(x => x.FSU).NotNull().IsInEnum();
            RuleFor(x => x.Attendee).NotNull().IsInEnum();
        }
    }
}
