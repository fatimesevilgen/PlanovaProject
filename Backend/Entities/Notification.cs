using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
	public class Notification : BaseEntity
	{
		public int UserId { get; set; }
		public User User { get; set; }
		public string Title { get; set; }
		public string Message { get; set; }
		public bool IsRead { get; set; } = false;
		public NotificationType Type { get; set; }
		public string? TargetUrl { get; set; } //Bildirim tıklanınca gidilecek URL
	}
}
