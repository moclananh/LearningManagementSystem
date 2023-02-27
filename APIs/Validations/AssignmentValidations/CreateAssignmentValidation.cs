using Applications.ViewModels.AssignmentViewModels;
using FluentValidation;
using FluentValidation.Validators;

namespace APIs.Validations.AssignmentValidations
{
    public class CreateAssignmentValidation : AbstractValidator<CreateAssignmentViewModel>
    {
        public CreateAssignmentValidation() {
            RuleFor(x => x.AssignmentName).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(x => x.Note).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(x => x.Deadline).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(x => x.isDone).NotNull();
            RuleFor(x => x.Status).NotNull();
            RuleFor(x => x.UnitId).NotNull().NotEmpty();
        }
    }
}
