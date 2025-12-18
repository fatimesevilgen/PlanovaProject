using Entities.Dtos;
using FluentValidation;

namespace Business.Validations
{
	public class RegisterDtoValidator : AbstractValidator<RegisterDto>
	{
		public RegisterDtoValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Ad zorunludur")
				.Length(1, 50).WithMessage("Ad 1-50 karakter arasında olmalıdır");

			RuleFor(x => x.Surname)
				.NotEmpty().WithMessage("Soyad zorunludur")
				.Length(1, 50).WithMessage("Soyad 1-50 karakter arasında olmalıdır");

			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email zorunludur")
				.EmailAddress().WithMessage("Geçerli bir email adresi giriniz");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Şifre zorunludur")
				.MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır");

			RuleFor(x => x.ConfirmPassword)
				.NotEmpty().WithMessage("Şifre tekrarı zorunludur")
				.Equal(x => x.Password).WithMessage("Şifreler eşleşmiyor");

			RuleFor(x => x.IsAgreedToPrivacyPolicy)
				.Equal(true).WithMessage("KVKK onayı zorunludur");
		}
	}

}
