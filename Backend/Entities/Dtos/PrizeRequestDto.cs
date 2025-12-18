using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
	public class PrizeRequestDto
	{
		public string Name { get; set; }        // Örn: "Usta"
        public string Description { get; set; } // Örn: "200 puana ulaşınca verilir"
        public string ImgUrl { get; set; }      // Örn: "badge_2.png"
        public int PointRequired { get; set; }  // Örn: 200

	}
}
