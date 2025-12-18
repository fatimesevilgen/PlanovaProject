
namespace Entities
{
	public class UserPrize : BaseEntity
	{
		public int UserId { get; set; }
		public User User { get; set; }

		public int PrizeId { get; set; }
		public Prize Prize { get; set; }

		public DateTime ClaimedAt { get; set; } = DateTime.UtcNow;
	}
}
