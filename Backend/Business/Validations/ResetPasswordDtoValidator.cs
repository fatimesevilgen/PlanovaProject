using Entities.Dtos;
using FluentValidation;

namespace Business.Validations
{
	public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
	{
		public ResetPasswordDtoValidator()
		{
			RuleFor(x => x.Token)
				.NotEmpty().WithMessage("Token zorunludur");

			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email zorunludur")
				.EmailAddress().WithMessage("Geçerli bir email adresi giriniz");

			RuleFor(x => x.NewPassword)
				.NotEmpty().WithMessage("Şifre zorunludur")
				.MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır");

			RuleFor(x => x.ConfirmNewPassword)
				.Equal(x => x.NewPassword).WithMessage("Şifreler eşleşmiyor");
		}
	}
}
