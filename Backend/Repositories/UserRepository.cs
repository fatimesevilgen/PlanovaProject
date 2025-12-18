using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstract;

namespace Repositories
{
	public class UserRepository : GenericRepository<User>, IUserRepository
	{
		private readonly AppDbContext _context;

		public UserRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}


		public async Task<User> GetByUserIdAsync(int id)
		{
			return await _context.Users
				.Include(u => u.Habits)
					.ThenInclude(h => h.Category)
				.Include(u => u.Habits)
					.ThenInclude(h => h.HabitProgresses)

				.Include(u => u.UserPrizes)	
					.ThenInclude(up => up.Prize)


				.FirstOrDefaultAsync(u => u.Id == id);
		}
	}
}
