using Applications.ViewModels.AssignmentViewModels;
using FluentValidation;
using FluentValidation.Validators;

namespace APIs.Validations.AssignmentValidations
{
    public class UpdateAssignmentValidation : AbstractValidator<UpdateAssignmentViewModel>
    {
        public UpdateAssignmentValidation() {
            RuleFor(x => x.AssignmentName).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(x => x.Note).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(x => x.Deadline).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(x => x.isDone).NotNull();
            RuleFor(x => x.Status).NotNull();
        }
    }
}
