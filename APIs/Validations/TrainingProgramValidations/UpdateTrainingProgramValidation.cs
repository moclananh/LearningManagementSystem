using Applications.ViewModels.TrainingProgramModels;
using FluentValidation;

namespace APIs.Validations.TrainingProgramValidations
{
    public class UpdateTrainingProgramValidation : AbstractValidator<UpdateTrainingProgramViewModel>
    {
        public UpdateTrainingProgramValidation()
        {
            RuleFor(x => x.TrainingProgramName)
                .NotEmpty()
                .WithMessage("The 'TrainingProgramName' should not be empty")
                .MaximumLength(350);
        }
    }
}
