using Applications.ViewModels.AuditResultViewModels;
using FluentValidation;

namespace APIs.Validations.AuditResultValidations
{
    public class UpdateAuditResultValidation : AbstractValidator<UpdateAuditResultViewModel>
    {
        public UpdateAuditResultValidation()
        {
            RuleFor(x => x.Score).NotEmpty();
            RuleFor(x => x.Note).NotEmpty().MaximumLength(100);
        }
    }
}
