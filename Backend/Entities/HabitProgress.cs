using Core.Entites; 

namespace Entities
{
    public class HabitProgress : BaseEntity
    {
        public int HabitId { get; set; }
    public Habit Habit { get; set; }

    public int? UserId { get; set; } // Query kolaylığı
    public User? User { get; set; }

    public DateTime ProgressDate { get; set; } // 🔥 EN KRİTİK ALAN

    public int CompletedCount { get; set; } // Bugün kaç kere yaptı
    public int TargetCount { get; set; } // O günkü hedef (habit'ten kopya)

    public bool IsCompleted { get; set; } // CompletedCount >= TargetCount

    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
	}
}