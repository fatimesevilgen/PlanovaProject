using Entities; // HabitLog nesnesi için
using Repositories.Abstract;

namespace Repositories.Abstract
{
    // IGenericRepository'den miras alıyoruz ki hazır metotlar (Add, Update vs.) gelsin.
    public interface IHabitLogRepository : IGenericRepository<HabitLog> 
    {
		Task<List<HabitLog>> GetCompletedByUserAndDateRangeAsync(int userId, DateTime start, DateTime end);
	}
}