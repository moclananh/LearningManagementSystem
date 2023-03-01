using Application.ViewModels.TrainingProgramModels;
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
                .WithMessage("The 'TrainngProgramName' should not be empty")
                .Length(10, 150);
            RuleFor(x => x.Duration)
                .NotEmpty()
                .WithMessage("The 'Duration' should not be empty")
                .MaximumLength(150);
            RuleFor(x => x.Status).NotNull();
        }
    }
}
