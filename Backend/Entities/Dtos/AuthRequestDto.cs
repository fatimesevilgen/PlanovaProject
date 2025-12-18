using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
	public class RegisterDto
	{
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string ConfirmPassword { get; set; }
		public bool IsAgreedToPrivacyPolicy { get; set; }
	}

	public class LoginDto
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public bool RememberMe { get; set; }
	}

	public class ForgotPasswordDto
	{
		public string Email { get; set; }
	}

	public class ResetPasswordDto
	{
		public string Token { get; set; }
		public string Email { get; set; }
		public string NewPassword { get; set; }
		public string ConfirmNewPassword { get; set; }
	}

	public class ChangePasswordDto
	{
		public string CurrentPassword { get; set; }
		public string NewPassword { get; set; }
		public string ConfirmNewPassword { get; set; }
	}
}

