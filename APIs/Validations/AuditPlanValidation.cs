using Applications.ViewModels.AuditPlanViewModel;
using FluentValidation;

namespace APIs.Validations
{
    public class AuditPlanValidation : AbstractValidator<AuditPlanViewModel>
    {
        public AuditPlanValidation()
        {
            RuleFor(x => x.AuditPlanName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.AuditDate).NotEmpty();
        }
    }
}
