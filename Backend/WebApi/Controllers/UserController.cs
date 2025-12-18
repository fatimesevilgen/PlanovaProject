using Business.Abstract;
using Core.Helpers.Extensions;
using Core.Helpers.Methods;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		/// <summary>
		/// Giriş yapan kullanıcının bilgilerini getirir.
		/// </summary>
		/// <param name="me"></param>
		/// <returns></returns>
		[HttpGet("me")]
		public async Task<ActionResult<ApiResponse<UserResponseDto>>> GetUserByIdAsync()
		{
			var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(userIdString))
				return Unauthorized();

			int userId = int.Parse(userIdString);

			var userResult = await _userService.GetUserByIdAsync(userId); // ApiResponse<UserResponseDto> döner

			if (!userResult.Success)
				return NotFound(ApiResponse<UserResponseDto>.ErrorResponse(userResult.Message ?? "Kullanıcı bulunamadı"));

			return Ok(userResult);
		}

		/// <summary>
		/// Giriş yapan kullanıcının alışkanlıklarını getirir.
		/// </summary>
		/// <param name="me/habits"></param>
		/// <returns></returns>
		[HttpGet("me/habits")]
		public async Task<IActionResult> GetMyHabits()
		{
			var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (string.IsNullOrEmpty(userIdString))
				return Unauthorized(ApiResponse<string>.ErrorResponse("Token geçersiz"));

			int userId = int.Parse(userIdString);

			var result = await _userService.GetUserHabitsAsync(userId);

			if (!result.Success)
				return NotFound(result);

			return Ok(result);
		}

		/// <summary>
		/// Giriş yapan kullanıcının ödüllerini getirir.
		/// </summary>
		/// <param name="me/prizes"></param>
		/// <returns></returns>
		[HttpGet("me/prizes")]
		public async Task<IActionResult> GetMyPrizes()
		{
			var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (string.IsNullOrEmpty(userIdString))
				return Unauthorized(ApiResponse<string>.ErrorResponse("Token geçersiz"));

			int userId = int.Parse(userIdString);

			var result = await _userService.GetUserPrizesAsync(userId);

			if (!result.Success)
				return NotFound(result);

			return Ok(result);
		}

		/// <summary>
		/// Kullanıcı profili bilgilerini günceller.
		/// </summary>
		/// <param name="updateProfileDto"></param>
		/// <returns></returns>
		[HttpPut("profile")]
		public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateDto dto)
		{
			var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (string.IsNullOrEmpty(userIdString))
				return Unauthorized(ApiResponse<string>.ErrorResponse("Token geçersiz"));

			int userId = int.Parse(userIdString);

			var result = await _userService.UpdateProfileAsync(userId, dto);

			return result.Success ? Ok(result) : BadRequest(result);
		}

		/// <summary>
		/// Kullanıcının şifresini değiştirir.
		/// </summary>
		/// <param name="dto"></param>
		/// <returns></returns>
		[HttpPost("change-password")]
		public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
		{
			int userId = User.GetUserId();
			var result = await _userService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword);
			return result.Success
				? Ok(result)
				: BadRequest(ApiResponse<bool>.ErrorResponse(result.Message ?? "Şifre değiştirilemedi"));
		}

		/// <summary>
		/// Kullanıcı kendi hesabını siler (soft delete)
		/// </summary>
		/// <param name="me/delete"></param>
		/// <returns></returns>
		[HttpDelete("me")]
		public async Task<IActionResult> DeleteMyAccount()
		{
			var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (string.IsNullOrEmpty(userIdString))
				return Unauthorized(ApiResponse<string>.ErrorResponse("Token geçersiz"));

			int userId = int.Parse(userIdString);

			var result = await _userService.SoftDeleteAsync(userId);

			if (!result.Success)
				return BadRequest(result);

			return Ok(result);
		}
	}
}
