namespace Entities
{
	public class Prize : BaseEntity
	{
		public string Name { get; set; }   
		public string Description { get; set; }
		public string ImgUrl { get; set; }
		public int PointRequired { get; set; }
		public ICollection<UserPrize> UserPrizes { get; set; } = new List<UserPrize>();
	}
}
