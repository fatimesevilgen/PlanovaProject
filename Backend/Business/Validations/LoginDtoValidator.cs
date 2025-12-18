using Entities.Dtos;
using FluentValidation;

namespace Business.Validations
{
	public class LoginDtoValidator : AbstractValidator<LoginDto>
	{
		public LoginDtoValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email zorunludur")
				.EmailAddress().WithMessage("Geçerli bir email adresi giriniz");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Şifre zorunludur")
				.MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır");

		}
	}

}
