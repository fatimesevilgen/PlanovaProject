using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstract;

namespace Repositories
{
	public class HabitRepository : GenericRepository<Habit>, IHabitRepository
	{
	private readonly AppDbContext _context;

	public HabitRepository(AppDbContext context) : base(context)
	{
		_context = context;
	}

	public async Task<List<Habit>> GetHabitsByUserIdAsync(int userId)
		{
			return await _context.Habits
				.Where(h => h.UserId == userId)
				.ToListAsync();
		}
	}
}
