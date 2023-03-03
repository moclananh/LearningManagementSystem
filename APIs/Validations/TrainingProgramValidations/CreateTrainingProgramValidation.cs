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
                .WithMessage("The 'TrainngProgramName' should not be empty")
                .Length(10, 150);
            RuleFor(x => x.Duration)
                .NotEmpty()
                .WithMessage("The 'Duration' should not be empty")
                .MaximumLength(150);
            RuleFor(x => x.Status).NotNull().Must(x => x == Domain.Enum.StatusEnum.Status.Enable || x == Domain.Enum.StatusEnum.Status.Disable)
                              .WithMessage("Status must be either Enable or Disable");
        }
    }
}

