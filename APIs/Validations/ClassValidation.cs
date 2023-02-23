using Applications.ViewModels.ClassViewModels;
using FluentValidation;

namespace APIs.Validations
{
    public class ClassValidation : AbstractValidator<ClassViewModel>
    {
        public ClassValidation()
        {
            RuleFor(x => x.ClassName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.ClassCode).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Location).NotEmpty().MaximumLength(100);
        }
    }
}
