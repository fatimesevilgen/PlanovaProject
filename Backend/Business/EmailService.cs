using Business.Abstract;
using Core.Helpers.Methods;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Business.Services
{
	public class EmailService : IEmailService
	{
		private readonly IConfiguration _configuration;
		private readonly string _smtpServer;
		private readonly int _smtpPort;
		private readonly string _smtpUsername;
		private readonly string _smtpPassword;
		private readonly string _senderEmail;
		private readonly string _senderName;

		public EmailService(IConfiguration configuration)
		{
			_configuration = configuration;

			_smtpServer = _configuration["EmailSettings:SmtpServer"];
			_smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
			_smtpUsername = _configuration["EmailSettings:SmtpUsername"];
			_smtpPassword = _configuration["EmailSettings:SmtpPassword"];
			_senderEmail = _configuration["EmailSettings:SenderEmail"];
			_senderName = _configuration["EmailSettings:SenderName"];
		}

		public async Task<ApiResponse<bool>> SendEmailAsync(string to, string subject, string body)
		{
			try
			{
				using var client = new SmtpClient(_smtpServer, _smtpPort)
				{
					Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
					EnableSsl = true
				};

				var mail = new MailMessage
				{
					From = new MailAddress(_senderEmail, _senderName),
					Subject = subject,
					Body = body,
					IsBodyHtml = true
				};

				mail.To.Add(to);

				await client.SendMailAsync(mail);

				return ApiResponse<bool>.SuccessResponse(true, "Mail başarıyla gönderildi.");
			}
			catch (Exception ex)
			{
				return ApiResponse<bool>.ErrorResponse($"Mail gönderilemedi: {ex.Message}");
			}
		}

		public async Task<ApiResponse<bool>> SendConfirmationEmailAsync(string to, string confirmationLink)
		{
			var subject = "Planova Hesap Doğrulama";
			var body = $@"
                <h2>Planova Hesap Doğrulama</h2>
                <p>Hesabınızı doğrulamak için aşağıdaki bağlantıya tıklayın:</p>
                <p><a href='{confirmationLink}' target='_blank'>Hesabımı Doğrula</a></p>
                <p>Bu bağlantı 24 saat geçerlidir.</p>
            ";

			return await SendEmailAsync(to, subject, body);
		}

		public async Task<ApiResponse<bool>> SendPasswordResetEmailAsync(string to, string resetLink)
		{
			var subject = "Planova Şifre Sıfırlama";
			var body = $@"
                <h2>Şifre Sıfırlama</h2>
                <p>Şifrenizi sıfırlamak için aşağıdaki bağlantıya tıklayın:</p>
                <p><a href='{resetLink}' target='_blank'>Şifremi Sıfırla</a></p>
                <p>Bu bağlantı 1 saat geçerlidir.</p>
            ";

			return await SendEmailAsync(to, subject, body);
		}
	}
}
