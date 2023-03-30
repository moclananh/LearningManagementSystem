using Application.ViewModels.TrainingProgramModels;
using FluentValidation;

namespace APIs.Validations.TrainingProgramValidations
{
    public class CreateTrainingProgramValidation : AbstractValidator<CreateTrainingProgramViewModel>
    {
        public CreateTrainingProgramValidation()
        {
            RuleFor(x => x.TrainingProgramName)
                .NotEmpty()
                .WithMessage("The 'TrainingProgramName' should not be empty")
                .MaximumLength(350);
        }
    }
}

