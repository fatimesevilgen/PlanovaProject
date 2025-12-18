using Core.Entites;

namespace Entities
{
	public class Habit : BaseEntity
	{
		public string Name { get; set; }
		public string? Description { get; set; }
		public int CategoryId { get; set; }
		public Category Category { get; set; }
		public HabitFrequency Frequency { get; set; } //Gunluk, Haftalik, Aylik
		public int TargetCount { get; set; } = 1; //Hedef Sayısı
		public TimeSpan? ReminderTime { get; set; } //Gunluk hatirlatma saati
		public int CurrentStreak { get; set; } = 0; //Ard arda tamamlanan gun sayisi
		public DateTime StartDate { get; set; } = DateTime.UtcNow;
		public DateTime? EndDate { get; set; }
		public int UserId { get; set; } 
		public User User { get; set; }
		public ICollection<HabitProgress> HabitProgresses { get; set; } = new List<HabitProgress>();
		
	}
}
