using Applications.ViewModels.PracticeViewModels;
using FluentValidation;

namespace APIs.Validations.PracticeValidations
{
    public class CreatePracticeValidation : AbstractValidator<CreatePracticeViewModel>
    {
        public CreatePracticeValidation()
        {
            RuleFor(x => x.PracticeName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
