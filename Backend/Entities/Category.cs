using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
	public class Category : BaseEntity
	{
		public string Name { get; set; }
		public string? Icon { get; set; }
		public ICollection<Habit> Habits { get; set; } = new List<Habit>();
	}
}
