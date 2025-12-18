using AutoMapper;
using Business.Abstract;
using Business.Validations;
using Core.Aspects.Validation;
using Core.Helpers.Methods;
using Entities;
using Entities.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repositories;
using Repositories.Abstract;
using System.Collections.Generic;

namespace Business
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IHabitRepository _habitRepository;
		private readonly IUserPrizeRepository _userPrizeRepository;
		private readonly IConfiguration _configuration;
		private readonly ILogger<UserService> _logger;
		private readonly IMapper _mapper;


		public UserService(IUserRepository userRepository, IHabitRepository habitRepository, IUserPrizeRepository userPrizeRepository, ILogger<UserService> logger, IMapper mapper, IConfiguration configuration)
		{
			_userRepository = userRepository;
			_habitRepository = habitRepository;
			_userPrizeRepository = userPrizeRepository;
			_configuration = configuration;
			_logger = logger;
			_mapper = mapper;
		}

		[ValidationAspect(typeof(ChangePasswordDtoValidator))]
		public async Task<ApiResponse<bool>> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
		{
			try
			{
				var user = await _userRepository.GetByIdAsync(userId);
				if (user == null)
					return ApiResponse<bool>.ErrorResponse("Kullanıcı bulunamadı.");


				// Mevcut şifre kontrolü
				if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
					return ApiResponse<bool>.ErrorResponse("Mevcut şifre hatalı.");

				// Yeni şifre hashleme
				user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
				await _userRepository.UpdateAsync(user);
				_logger.LogInformation("Kullanıcı şifresi değiştirildi. UserId: {UserId}, Zaman: {Time}", userId, DateTime.UtcNow);
				return ApiResponse<bool>.SuccessResponse(true, "Şifre başarıyla değiştirildi.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Şifre değişikliği sırasında bir hata oluştu. UserId: {UserId}", userId);
				return ApiResponse<bool>.ErrorResponse($"Şifre değiştirilemedi: {ex.Message}");
			}
		}

		public Task<ApiResponse<List<User>>> GetAllUsers()
		{
			throw new NotImplementedException();
		}

		public async Task<ApiResponse<UserResponseDto>> GetUserByIdAsync(int id)
		{
			var user = await _userRepository.GetByUserIdAsync(id);

			if (user == null)
			{
				return ApiResponse<UserResponseDto>.ErrorResponse("Kullanıcı bulunamadı.");
			}

			var userDto = new UserResponseDto
			{
				Id = user.Id,
				Name = user.Name,
				Surname = user.Surname,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				AvatarUrl = user.AvatarUrl,
				Role = user.Role,
				Level = user.Level,
				Points = user.Points,
				Habits = user.Habits.Select(h => new HabitResponseDto
				{
					Id = h.Id,
					Name = h.Name,
					Description = h.Description,
					CategoryName = h.Category.Name,
					Frequency = h.Frequency.ToString(),
					TargetCount = h.TargetCount,
					CurrentStreak = h.CurrentStreak,
					StartDate = h.StartDate,
					EndDate = h.EndDate,
					Progress = h.HabitProgresses.Select(p => new HabitProgressDto
					{
						Id = p.Id,
						ProgressDate = p.ProgressDate,
						CompletedCount = p.CompletedCount,
						TargetCount = p.TargetCount,
						IsCompleted = p.IsCompleted,
						CompletedRate = p.TargetCount == 0
		? 0
		: (double)p.CompletedCount / p.TargetCount * 100,
						Note = p.Note
					}).ToList()
				}).ToList(),
				Prizes = user.UserPrizes.Select(up => new UserPrizeResponseDto
				{
					Id = up.Id,
					PrizeName = up.Prize.Name,
					PrizeDescription = up.Prize.Description,
					ImgUrl = up.Prize.ImgUrl,
					PointRequired = up.Prize.PointRequired,
					ClaimedAt = up.ClaimedAt
				}).ToList()
			};
		

			return ApiResponse<UserResponseDto>.SuccessResponse(userDto, "Kullanıcı bilgileri getirildi.");
		}

		public async Task<ApiResponse<List<HabitResponseDto>>> GetUserHabitsAsync(int userId)
		{
			// 1) Repo'dan o kullanıcıya ait habit listesi alıyoruz
			var habitsEntity = await _habitRepository.GetHabitsByUserIdAsync(userId);

			// 2) Eğer hiç alışkanlık yoksa boş liste ile dönebiliriz (error de tercih edilebilir)
			if (habitsEntity == null || !habitsEntity.Any())
				return ApiResponse<List<HabitResponseDto>>.SuccessResponse(new List<HabitResponseDto>(), "Kullanıcının alışkanlığı bulunamadı.");

			// 3) Mapleme — entity'deki property isimlerine dikkat et
			var habits = habitsEntity.Select(h => new HabitResponseDto
			{
				Id = h.Id,
				Name = h.Name,
				Description = h.Description,
				Frequency = h.Frequency.ToString(), // eğer enum ise .ToString()
				TargetCount = h.TargetCount,
				CurrentStreak = h.CurrentStreak,
				StartDate = h.StartDate,
				EndDate = h.EndDate,

				Category = new CategoryResponseDto
				{
					Id = h.Category?.Id ?? 0,
					Name = h.Category?.Name,
					Icon = h.Category?.Icon
				},

				// Burada HabitProgress property'sinin doğru adını kullan:
				// Eğer entity'de "HabitProgress" ise h.HabitProgress, eğer "HabitProgresses" ise h.HabitProgresses
				Progress = (h.HabitProgresses ?? new List<HabitProgress>()).Select(p => new HabitProgressDto
				{
					Id = p.Id,
					ProgressDate = p.ProgressDate,
					CompletedCount = p.CompletedCount,
					TargetCount = p.TargetCount,
					IsCompleted = p.IsCompleted,
					CompletedRate = p.TargetCount == 0
			? 0
			: (double)p.CompletedCount / p.TargetCount * 100,
					Note = p.Note
				}).ToList()

			}).ToList();

			return ApiResponse<List<HabitResponseDto>>.SuccessResponse(habits, "Alışkanlıklar getirildi.");
		}

		public async Task<ApiResponse<List<PrizeResponseDto>>> GetUserPrizesAsync(int userId)
		{
			var userPrizes = await _userPrizeRepository.GetUserPrizesAsync(userId);

			var prizes = userPrizes.Select(up => new PrizeResponseDto
			{
				Id = up.Prize.Id,
				Name = up.Prize.Name,
				Description = up.Prize.Description,
				ImgUrl = up.Prize.ImgUrl,
				PointRequired = up.Prize.PointRequired,
				ClaimedAt = up.ClaimedAt
			}).ToList();

			return ApiResponse<List<PrizeResponseDto>>.SuccessResponse(prizes);
		}

		public async Task<ApiResponse<bool>> SoftDeleteAsync(int userId)
		{
			try
			{
				var user = await _userRepository.GetByUserIdAsync(userId);

				if (user == null || user.IsDeleted)
					return ApiResponse<bool>.ErrorResponse("Kullanıcı bulunamadı veya zaten silinmiş.");

				user.IsDeleted = true;

				await _userRepository.UpdateAsync(user);

				_logger.LogInformation("Kullanıcı soft delete ile silindi. UserId: {UserId}, Zaman: {Time}", userId, DateTime.UtcNow);

				return ApiResponse<bool>.SuccessResponse(true, "Hesabınız başarılı bir şekilde silindi.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Soft delete sırasında hata oluştu. UserId: {UserId}", userId);
				return ApiResponse<bool>.ErrorResponse($"Hesap silinemedi: {ex.Message}");
			}
		}

		[ValidationAspect(typeof(UserUpdateDtoValidator))]
		public async Task<ApiResponse<UserResponseDto>> UpdateProfileAsync(int userId, UserUpdateDto dto)
		{
			var user = await _userRepository.GetAsync(u => u.Id == userId);

			if (user == null)
				return ApiResponse<UserResponseDto>.ErrorResponse("Kullanıcı bulunamadı");

			// Güncellenebilir alanlar
			user.Name = dto.Name ?? user.Name;
			user.Surname = dto.Surname ?? user.Surname;

			// Email değişiyorsa → ileride doğrulama yapacağız
			if (dto.Email != null && dto.Email != user.Email)
			{
				user.Email = dto.Email;
				// TODO: email verification token (bir sonraki adımda yapılacak)
			}

			user.PhoneNumber = dto.PhoneNumber ?? user.PhoneNumber;
			user.AvatarUrl = dto.AvatarUrl ?? user.AvatarUrl;

			await _userRepository.UpdateAsync(user);

			// Güncellenmiş kullanıcıyı DTO olarak döndür
			var userDto = _mapper.Map<UserResponseDto>(user);

			return ApiResponse<UserResponseDto>.SuccessResponse(userDto);
		}
	}
}
