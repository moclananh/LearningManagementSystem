using Applications.ViewModels.OutputStandardViewModels;
using FluentValidation;

namespace APIs.Validations.OutputStandardValidations
{
    public class UpdateOutputStandardValidation : AbstractValidator<UpdateOutputStandardViewModel>
    {
        public UpdateOutputStandardValidation()
        {
            RuleFor(x => x.OutputStandardCode).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
