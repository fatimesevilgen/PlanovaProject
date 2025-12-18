using Entities.Dtos;
using FluentValidation;

namespace Business.Validations
{
	public class ForgotPasswordDtoValidator : AbstractValidator<ForgotPasswordDto>
	{
		public ForgotPasswordDtoValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email zorunludur")
				.EmailAddress().WithMessage("Geçerli bir email adresi giriniz");
		}
	}
}
