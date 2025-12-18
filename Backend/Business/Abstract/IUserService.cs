using Core.Helpers.Methods;
using Entities;
using Entities.Dtos;

namespace Business.Abstract
{
	public interface IUserService
	{
		Task<ApiResponse<List<User>>> GetAllUsers();
		Task<ApiResponse<UserResponseDto>> GetUserByIdAsync(int id);
		Task<ApiResponse<UserResponseDto>> UpdateProfileAsync(int userId, UserUpdateDto dto);
		Task<ApiResponse<bool>> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
		Task<ApiResponse<List<HabitResponseDto>>> GetUserHabitsAsync(int userId);
		Task<ApiResponse<List<PrizeResponseDto>>> GetUserPrizesAsync(int userId);
		Task<ApiResponse<bool>> SoftDeleteAsync(int userId);
	}
}
