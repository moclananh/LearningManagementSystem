using Applications.ViewModels.AuditPlanViewModel;
using FluentValidation;

namespace APIs.Validations.AuditPlanValidations
{
    public class UpdateAuditPlanValidation : AbstractValidator<UpdateAuditPlanViewModel>
    {
        public UpdateAuditPlanValidation()
        {
            RuleFor(x => x.AuditPlanName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.AuditDate).NotEmpty();
        }
    }
}
