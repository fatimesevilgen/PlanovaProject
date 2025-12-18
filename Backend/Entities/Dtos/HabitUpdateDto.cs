using Core.Entites;

namespace Entities.Dtos
{
    public class HabitUpdateDto
    {
        public int Id { get; set; } // hangi alışkanlığı güncelleyeceğiz
        public string Name {get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TargetCount { get; set; }  // hedefi değiştirme (arttırma/azaltma)
		public HabitFrequency Frequency { get; set; }

	}
}