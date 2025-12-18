using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
	public class UserPrizeResponseDto
	{
		public int Id { get; set; }
		public string PrizeName { get; set; }
		public string PrizeDescription { get; set; }
		public string ImgUrl { get; set; }     
		public int PointRequired { get; set; } 
		public DateTime ClaimedAt { get; set; }
	}
}
