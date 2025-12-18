using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
	public class ProgressSummaryDto
	{
		public int TotalDays { get; set; }     // 7 veya 30
		public int CompletedDays { get; set; } // kaç gün boyalı
		public double Percentage { get; set; } // % kaç
	}
}
