using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
	public class HabitProgressDto
	{
		public int Id { get; set; }
		public DateTime ProgressDate { get; set; }

		public int CompletedCount { get; set; }
		public int TargetCount { get; set; }

		public bool IsCompleted { get; set; }
		public double CompletedRate { get; set; } // Hesaplanmış

		public string? Note { get; set; }
	}
}
