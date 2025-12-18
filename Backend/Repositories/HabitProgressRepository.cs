using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstract;
// Eğer hata verirse buraya using DataAccess... ekleriz

namespace Repositories
{
    public class HabitProgressRepository : GenericRepository<HabitProgress>, IHabitProgressRepository
    {
		private readonly AppDbContext _context;
		public HabitProgressRepository(AppDbContext context) : base(context)
		{ _context = context; }

		public async Task<List<HabitProgress>> GetCompletedByUserAndDateRangeAsync(int userId, DateTime start, DateTime end)
		{
			return await _context.HabitProgresses
		.Include(x => x.Habit)
		.Where(x =>
			x.UserId == userId &&
			x.ProgressDate >= start &&
			x.ProgressDate < end)
		.ToListAsync();
		}
	}
}