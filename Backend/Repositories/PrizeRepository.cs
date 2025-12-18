using Entities;
using Repositories.Abstract;

namespace Repositories
{
	public class PrizeRepository : GenericRepository<Prize>, IPrizeRepository
	{
		public PrizeRepository(AppDbContext context) : base(context)
		{
		}
	}
}
