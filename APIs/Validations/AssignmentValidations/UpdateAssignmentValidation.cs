using Applications.ViewModels.AssignmentViewModels;
using FluentValidation;

namespace APIs.Validations.AssignmentValidations
{
    public class UpdateAssignmentValidation : AbstractValidator<UpdateAssignmentViewModel>
    {
        public UpdateAssignmentValidation() {
            RuleFor(x => x.AssignmentName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Status).IsInEnum();
        }
    }
}
