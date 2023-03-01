using Applications.ViewModels.ModuleViewModels;
using FluentValidation;

namespace APIs.Validations.ModulesValidations
{
    public class CreateModuleValidation : AbstractValidator<CreateModuleViewModel>
    {
        public CreateModuleValidation()
        {
            RuleFor(x => x.ModuleName).NotEmpty().MaximumLength(100);
        }
    }
}
