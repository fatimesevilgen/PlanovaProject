using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
	public class UserResponseDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Email { get; set; }
		public string? PhoneNumber { get; set; }
		public string? AvatarUrl { get; set; } 
		public UserRole Role { get; set; }      
		public int Level { get; set; }  
		public int Points { get; set; }
		public List<HabitResponseDto> Habits { get; set; }
		public List<UserPrizeResponseDto> Prizes { get; set; }
	}
}
