using Applications.ViewModels.ModuleViewModels;
using FluentValidation;

namespace APIs.Validations
{
    public class ModuleValidation : AbstractValidator<ModuleViewModels>
    {
        public ModuleValidation()
        {
            RuleFor(x => x.ModuleName).NotEmpty().MaximumLength(100);
        }
    }
}
