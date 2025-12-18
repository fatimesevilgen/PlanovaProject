using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
	public class PrizeResponseDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string ImgUrl { get; set; }
		public int PointRequired { get; set; }
		public DateTime ClaimedAt { get; set; }
	}
}
