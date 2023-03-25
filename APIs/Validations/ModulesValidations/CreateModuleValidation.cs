using Applications.ViewModels.ModuleViewModels;
using FluentValidation;

namespace APIs.Validations.ModulesValidations
{
    public class CreateModuleValidation : AbstractValidator<CreateModuleViewModel>
    {
        public CreateModuleValidation()
        {
        }
    }
}
