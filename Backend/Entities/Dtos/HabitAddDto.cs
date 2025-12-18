using System.ComponentModel.DataAnnotations;
using Core.Entites;
using Entities; 

namespace Entities.Dtos
{
    public class HabitAddDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        
        // Frontend'den 0 (Günlük), 1 (Haftalık) gibi sayı gelecek, Enum karşılayacak.
        public HabitFrequency Frequency { get; set; } 
        
        public int TargetCount { get; set; } // Hedef sayısı (Günde 50 sayfa vb.)
        public DateTime EndDate { get; set; }
    }
}
