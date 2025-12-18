using Core.Entites;

namespace Entities
{
	public class User : BaseEntity
	{
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Email { get; set; }
		public string? PhoneNumber { get; set; }
		public string? AvatarUrl { get; set; }
		public string PasswordHash { get; set; }
		public UserRole Role { get; set; } = UserRole.Individual;
		public string? RefreshToken { get; set; }
		public DateTime? RefreshTokenExpiry { get; set; }
		public string? PasswordResetToken { get; set; }
		public DateTime? PasswordResetTokenExpiry { get; set; }
		public DateTime? LastLoginDate { get; set; }
		public DateTime? LastLogoutDate { get; set; }
		public int Level { get; set; } = 1;
		public int Points { get; set; } = 0;
		public string Title { get; set; } = "Başlangıç";
		public ICollection<UserPrize> UserPrizes { get; set; } = new List<UserPrize>();
		public ICollection<Habit> Habits { get; set; } = new List<Habit>();
		public ICollection<Notification> Notifications { get; set; } = new List<Notification>();


	}
}
