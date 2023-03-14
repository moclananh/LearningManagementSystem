using Applications.ViewModels.AuditPlanViewModel;
using FluentValidation;

namespace APIs.Validations.AuditPlanValidations
{
    public class CreateAuditPlanValidation : AbstractValidator<CreateAuditPlanViewModel>
    {
        public CreateAuditPlanValidation()
        {
            RuleFor(x => x.AuditPlanName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.AuditDate).NotEmpty();
        }
    }
}
