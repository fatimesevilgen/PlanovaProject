using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
	public class HabitProgressSummaryDto
	{
		public int HabitId { get; set; }
		public string HabitName { get; set; }
		public int CompletedDays { get; set; }
		public int CompletedCount { get; set; }
		public int TargetCount { get; set; }
		public int TotalDays { get; set; } = 7;
		public double Percentage =>
		TotalDays == 0
			? 0
			: Math.Round((double)CompletedDays / TotalDays * 100, 2);
	}
}
