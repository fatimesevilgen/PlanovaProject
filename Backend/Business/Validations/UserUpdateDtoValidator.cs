using Entities.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validations
{
	public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
	{
		public UserUpdateDtoValidator()
		{
			RuleFor(x => x.Email)
			.NotEmpty().WithMessage("Email boş bırakılamaz.")
			.EmailAddress().WithMessage("Geçerli bir email girin.")
			.MaximumLength(100);

			RuleFor(x => x.PhoneNumber)
				.NotEmpty().WithMessage("Telefon numarası boş olamaz.")
				.Matches(@"^\+?\d{10,15}$")
				.WithMessage("Geçerli bir telefon numarası girin.");

			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("İsim boş olamaz.");

			RuleFor(x => x.Surname)
				.NotEmpty().WithMessage("Soyad boş olamaz.");


		}
	}
}
