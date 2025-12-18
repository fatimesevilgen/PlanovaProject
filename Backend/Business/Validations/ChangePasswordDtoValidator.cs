using Entities.Dtos;
using FluentValidation;

namespace Business.Validations
{
	public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
	{
		public ChangePasswordDtoValidator()
		{
			RuleFor(x => x.CurrentPassword)
				.NotEmpty().WithMessage("Mevcut şifre zorunludur");

			RuleFor(x => x.NewPassword)
				.NotEmpty().WithMessage("Yeni şifre zorunludur")
				.MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır");

			RuleFor(x => x.ConfirmNewPassword)
				.Equal(x => x.NewPassword).WithMessage("Şifreler eşleşmiyor");
		}
	}
}
