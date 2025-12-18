using Entities; // HabitProgress burada
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Abstract
{

    public interface IHabitProgressRepository : IGenericRepository<HabitProgress>
    {
		Task<List<HabitProgress>> GetCompletedByUserAndDateRangeAsync(int userId, DateTime start, DateTime end);
	
	}
}
