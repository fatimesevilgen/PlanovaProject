using Entities;
using Repositories.Abstract;

namespace Repositories
{
	public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
	{
		public CategoryRepository(AppDbContext context) : base(context) { }
	}
}
