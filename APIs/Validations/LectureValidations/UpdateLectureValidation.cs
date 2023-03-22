using Applications.ViewModels.LectureViewModels;
using FluentValidation;

namespace APIs.Validations.LectureValidations
{
    public class UpdateLectureValidation : AbstractValidator<UpdateLectureViewModel>
    {
        public UpdateLectureValidation()
        {
            RuleFor(x => x.LectureName).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(x => x.Status).NotNull();
        }
    }
}
