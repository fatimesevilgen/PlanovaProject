using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstract;

namespace Repositories
{
	public class UserPrizeRepository : GenericRepository<UserPrize>, IUserPrizeRepository
	{
		private readonly AppDbContext _context;
		public UserPrizeRepository(AppDbContext context) : base(context) 
		{ 
			_context = context;
		}

		public async Task<List<UserPrize>> GetUserPrizesAsync(int userId)
		{
			return await _context.UserPrizes
				.Include(up => up.Prize)
				.Where(up => up.UserId == userId)
				.ToListAsync();
		}

	}
}
