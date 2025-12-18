using Business;
using Business.Abstract;
using Core.Helpers.Extensions;
using Core.Helpers.Methods;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{

		private readonly IAuthService _authService;
		private readonly IUserService _userService;

		public AuthController(IAuthService authService, IUserService userService)
		{
			_authService = authService;
			_userService = userService;
		}

		/// <summary>
		/// Yeni bir kullanıcı kaydeder.
		/// </summary>
		/// <param name="registerDto"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost("register")]
		public async Task<ActionResult<UserRequestDto>> RegisterAsync(RegisterDto registerDto)
		{
			var result = await _authService.RegisterAsync(registerDto);
			return result.Success
				? Ok(result)
				: BadRequest(ApiResponse<UserRequestDto>.ErrorResponse(result.Message ?? "Kayıt başarısız"));
		}

		/// <summary>
		/// Kullanıcı sisteme giriş yapar.
		/// </summary>
		/// <param name="loginDto"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost("login")]
		public async Task<ActionResult<string>> LoginAsync(LoginDto loginDto)
		{
			var token = await _authService.LoginAsync(loginDto);
			return token.Success
				? Ok(token)
				: Unauthorized(ApiResponse<AuthResponseDto>.ErrorResponse(token.Message ?? "Giriş başarısız"));
		}

		/// <summary>
		/// Kullanıcı sistemden çıkış yapar.
		/// </summary>
		/// <returns></returns>
		[HttpPost("logout")]
		public async Task<ActionResult> LogoutAsync()
		{
			int userId = User.GetUserId();
			var result = await _authService.LogoutAsync(userId);
			return result.Success
				? Ok(result)
				: BadRequest(ApiResponse<bool>.ErrorResponse(result.Message));
		}

		/// <summary>
		/// Kullanıcı şifresini unuttuğunda şifre sıfırlama talebi oluşturur.
		/// </summary>
		/// <param name="forgotPasswordDto"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost("forgot-password")]
		public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
		{
			var result = await _authService.ForgotPasswordAsync(forgotPasswordDto.Email);
			return result.Success
				? Ok(result)
				: BadRequest(ApiResponse<bool>.ErrorResponse(result.Message ?? "Şifre sıfırlama başarısız"));
		}

		/// <summary>
		/// Kullanıcının şifresini sıfırlar.
		/// </summary>
		/// <param name="resetPasswordDto"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
		{
			var result = await _authService.ResetPasswordAsync(resetPasswordDto);
			return result.Success
				? Ok(result)
				: BadRequest(ApiResponse<bool>.ErrorResponse(result.Message ?? "Şifre sıfırlama başarısız"));
		}

	}
}
