using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Abstract
{
	public interface IUserPrizeRepository : IGenericRepository<UserPrize>
	{
		Task<List<UserPrize>> GetUserPrizesAsync(int userId);
	}
}
