using Applications.ViewModels.OutputStandardViewModels;
using FluentValidation;

namespace APIs.Validations.OutputStandardValidations
{
    public class CreateOutputStandardValidation : AbstractValidator<CreateOutputStandardViewModel>
    {
        public CreateOutputStandardValidation()
        {
            RuleFor(x => x.OutputStandardCode).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
