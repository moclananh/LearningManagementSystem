using Applications.ViewModels.UserViewModels;
using FluentValidation;

namespace APIs.Validations;

public class LoginValidation : AbstractValidator<UserLoginViewModel>
{
	public LoginValidation()
	{
		RuleFor(x => x.Email).NotEmpty().MaximumLength(100);
		RuleFor(x => x.Password).NotEmpty();
	}
}
