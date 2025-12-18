using Core.Helpers.Methods;
using Entities;
using Entities.Dtos;

namespace Business.Abstract
{
	public interface IHabitService
	{
		// Alışkanlık oluşturma
		Task<ApiResponse<Habit>> AddAsync(HabitAddDto habitDto, int userId);

		// Kullanıcıya ait tüm alışkanlıkları getir
		Task<ApiResponse<List<Habit>>> GetListByUserIdAsync(int userId);

		// Alışkanlık güncelleme
		Task<ApiResponse<Habit>> UpdateAsync(HabitUpdateDto habitDto, int userId);

		// Alışkanlık silme (soft delete)
		Task<ApiResponse<bool>> DeleteAsync(int habitId, int userId);

		// Günlük tik atma / ilerleme kaydetme
		Task<ApiResponse<HabitProgress>> TickHabitAsync(int habitId, int userId);
	}
}
