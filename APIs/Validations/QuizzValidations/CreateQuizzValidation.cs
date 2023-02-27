﻿using Application.ViewModels.QuizzViewModels;
using FluentValidation;

namespace APIs.Validations.QuizzValidations
{
    public class CreateQuizzValidation : AbstractValidator<CreateQuizzViewModel>
    {
        public CreateQuizzValidation()
        {
            RuleFor(x => x.QuizzName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Deadline).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}