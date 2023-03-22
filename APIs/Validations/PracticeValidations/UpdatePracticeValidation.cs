using Applications.ViewModels.LectureViewModels;
using Applications.ViewModels.PracticeViewModels;
using FluentValidation;

namespace APIs.Validations.PracticeValidations
{
    public class UpdatePracticeValidation : AbstractValidator<UpdatePracticeViewModel>
    {
        public UpdatePracticeValidation() 
        {
            RuleFor(x => x.PracticeName).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(x => x.Status).NotNull();
        }
    }
}
