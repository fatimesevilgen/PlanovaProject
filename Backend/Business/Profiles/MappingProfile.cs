using AutoMapper;
using Entities;
using Entities.Dtos;

namespace Business.Mappings // Namespace'ine dikkat et (Genelde Business içindedir)
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			
			CreateMap<User, UserRequestDto>()
				.ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
				.ReverseMap();

			CreateMap<User, UserUpdateDto>().ReverseMap();

			CreateMap<User, UserResponseDto>()
				.ForMember(dest => dest.Prizes, opt => opt.MapFrom(src => src.UserPrizes))
				.ReverseMap();

			CreateMap<User, RegisterDto>().ReverseMap();
			CreateMap<Habit, HabitAddDto>().ReverseMap();
			CreateMap<HabitProgress, HabitProgressDto>();

			CreateMap<Habit, HabitResponseDto>()
				.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
				.ForMember(dest => dest.Progress, opt => opt.MapFrom(src =>src.HabitProgresses))
			   .ReverseMap();

			CreateMap<UserPrize, UserPrizeResponseDto>()
				.ForMember(dest => dest.PrizeName, opt => opt.MapFrom(src => src.Prize.Name))
				.ForMember(dest => dest.PointRequired, opt => opt.MapFrom(src => src.Prize.PointRequired))
				.ForMember(dest => dest.ImgUrl, opt => opt.MapFrom(src => src.Prize.ImgUrl))
				.ForMember(dest => dest.ClaimedAt, opt => opt.MapFrom(src => src.ClaimedAt));

			//CreateMap<Category, CategoryDto>().ReverseMap();

			//CreateMap<Habit, HabitAddDto>()
			//	.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
			//	.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Name + " " + src.User.Surname : ""))
			//	.ForMember(dest => dest.Frequency, opt => opt.MapFrom(src => src.Frequency.ToString()));


			// HabitLog
			//CreateMap<HabitLog, HabitLogDto>()
			//	.ForMember(dest => dest.HabitName, opt => opt.MapFrom(src => src.Habit.Name))
			//	.ReverseMap();
		}
	}
}