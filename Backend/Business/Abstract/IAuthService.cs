using Core.Helpers.Methods;
using Entities.Dtos;

namespace Business.Abstract
{
	public interface IAuthService
	{
		Task<ApiResponse<UserRequestDto>> RegisterAsync(RegisterDto registerDto);
		Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto loginDto);
		Task<ApiResponse<bool>> LogoutAsync(int userId);
		Task<ApiResponse<bool>> ForgotPasswordAsync(string email);
		Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
		Task<ApiResponse<bool>> ValidateTokenAsync(string token);
		Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(string token);
		Task<ApiResponse<AuthResponseDto>> LoginWithGoogleAsync(GoogleLoginDto googleLoginDto);


	}
}
