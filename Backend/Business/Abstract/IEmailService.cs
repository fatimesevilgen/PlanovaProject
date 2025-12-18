using Core.Helpers.Methods;
using System.Threading.Tasks;

public interface IEmailService
{
	Task<ApiResponse<bool>> SendEmailAsync(string to, string subject, string body);

	Task<ApiResponse<bool>> SendConfirmationEmailAsync(string to, string confirmationLink);

	Task<ApiResponse<bool>> SendPasswordResetEmailAsync(string to, string resetLink);

}
