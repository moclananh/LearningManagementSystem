using Applications.ViewModels.LectureViewModels;
using FluentValidation;
using FluentValidation.Validators;

namespace APIs.Validations.LectureValidations
{
    public class CreateLectureValidation : AbstractValidator<CreateLectureViewModel>
    {
        public CreateLectureValidation()
        {
            RuleFor(x => x.LectureName).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(x => x.Status).NotNull();

        }
    }
}
