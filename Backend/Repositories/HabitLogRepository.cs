using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstract;

namespace Repositories
{
    public class HabitLogRepository : GenericRepository<HabitLog>, IHabitLogRepository
    {
		private readonly AppDbContext _context;
		public HabitLogRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}
		public async Task<List<HabitLog>> GetCompletedByUserAndDateRangeAsync(int userId, DateTime start, DateTime end)
		{
			return await _context.HabitLogs
				.Include(x => x.Habit)
				.Where(x =>
					x.Habit.UserId == userId &&
					x.IsCompleted &&
					x.LogDate >= start &&
					x.LogDate < end)
				.ToListAsync();
		}
	}
}