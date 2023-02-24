﻿using Applications.ViewModels.SyllabusViewModels;
using FluentValidation;

namespace APIs.Validations.SyllabusValidations
{
    public class CreateSyllabusValidation : AbstractValidator<CreateSyllabusViewModel>
    {
        public CreateSyllabusValidation()
        {
            RuleFor(x => x.SyllabusName)
                .NotEmpty()
                .WithMessage("The 'SyllabusName' should not empty")
                .Length(10, 150);
            RuleFor(x => x.SyllabusCode)
                .NotEmpty()
                .WithMessage("The 'SyllabusCode' should not empty")
                .Length(3, 4);
            RuleFor(x => x.Duration)
                .NotEmpty()
                .WithMessage("The 'Duration' should not empty");
            RuleFor(x => x.Level)
                .NotEmpty()
                .WithMessage("The 'Level' should not empty");
            RuleFor(x => x.CourseObjective)
                .NotEmpty()
                .WithMessage("The 'CourseObjective' should not empty");
            RuleFor(x => x.Version)
                .NotEmpty()
                .WithMessage("The 'Version' should not empty");
        }
    }
}