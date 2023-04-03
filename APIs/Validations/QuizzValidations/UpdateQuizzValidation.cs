using Application.ViewModels.QuizzViewModels;
using FluentValidation;

namespace APIs.Validations.QuizzValidations
{
    public class UpdateQuizzValidation : AbstractValidator<UpdateQuizzViewModel>
    {
        public UpdateQuizzValidation() 
        {
            RuleFor(x => x.QuizzName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Status).IsInEnum();
        }
    }
}
