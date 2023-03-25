using Applications.ViewModels.ModuleViewModels;
using FluentValidation;

namespace APIs.Validations.ModulesValidations
{
    public class UpdateModuleValidation : AbstractValidator<UpdateModuleViewModel>
    {
        public UpdateModuleValidation()
        {
        }
    }
}
