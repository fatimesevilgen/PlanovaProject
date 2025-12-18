using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
	public class HabitResponseDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public string CategoryName { get; set; }
		public string Frequency { get; set; } // Daily / Weekly / Monthly
		public int TargetCount { get; set; }
		public int CurrentStreak { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public CategoryResponseDto Category { get; set; }
		public List<HabitProgressDto> Progress { get; set; } = new List<HabitProgressDto>();
	}
}
