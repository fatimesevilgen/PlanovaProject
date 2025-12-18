using Core.Entites; // BaseEntity buradaysa

namespace Entities
{
    public class HabitLog : BaseEntity
    {
        public int HabitId { get; set; }
        public Habit Habit { get; set; } // İlişki (Hangi alışkanlığa ait?)

        public DateTime LogDate { get; set; } // Hangi gün?
        public int CompletedCount { get; set; } // Kaç kere yapıldı? (Örn: 3. bardak su)
        public bool IsCompleted { get; set; } // Günlük hedef tamamlandı mı?
        public string? Note { get; set; } // Kullanıcı notu (Opsiyonel)
    }
}