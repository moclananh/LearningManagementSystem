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
            RuleFor(x => x.Status).NotNull().Must(x => x == Domain.Enum.StatusEnum.Status.Enable || x == Domain.Enum.StatusEnum.Status.Disable)
                              .WithMessage("Status must be either Enable or Disable");
        }
    }
}
