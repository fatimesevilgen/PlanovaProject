using Business.Abstract;
using Core.Helpers.Methods;
using Entities;
using Entities.Dtos;
using Repositories.Abstract;

namespace Business.Concrete
{
	public class HabitManager : IHabitService
	{
		private readonly IHabitRepository _habitRepository;
		private readonly IHabitProgressRepository _progressRepository;
		private readonly ICategoryRepository _categoryRepository;
		private readonly IUserRepository _userRepository;
		private readonly IPrizeRepository _prizeRepository;
		private readonly IUserPrizeRepository _userPrizeRepository;

		public HabitManager(
			IHabitRepository habitRepository,
			IHabitProgressRepository progressRepository,
			ICategoryRepository categoryRepository,
			IUserRepository userRepository,
			IPrizeRepository prizeRepository,
			IUserPrizeRepository userPrizeRepository)
		{
			_habitRepository = habitRepository;
			_progressRepository = progressRepository;
			_categoryRepository = categoryRepository;
			_userRepository = userRepository;
			_prizeRepository = prizeRepository;
			_userPrizeRepository = userPrizeRepository;
		}

		// --------------------------------------------------
		// HABIT EKLEME
		// --------------------------------------------------
		public async Task<ApiResponse<Habit>> AddAsync(HabitAddDto habitDto, int userId)
		{
			var category = await _categoryRepository.GetByIdAsync(habitDto.CategoryId);
			if (category == null)
				return ApiResponse<Habit>.ErrorResponse("Kategori bulunamadı.");

			var habit = new Habit
			{
				Name = habitDto.Name,
				Description = habitDto.Description,
				CategoryId = habitDto.CategoryId,
				Frequency = habitDto.Frequency,
				TargetCount = habitDto.TargetCount,
				UserId = userId,
				StartDate = DateTime.UtcNow,
				EndDate = habitDto.EndDate,
				CurrentStreak = 0
			};

			await _habitRepository.AddAsync(habit);

			return ApiResponse<Habit>.SuccessResponse(habit, "Alışkanlık oluşturuldu.");
		}

		// --------------------------------------------------
		// KULLANICIYA AİT HABITLER
		// --------------------------------------------------
		public async Task<ApiResponse<List<Habit>>> GetListByUserIdAsync(int userId)
		{
			var habits = await _habitRepository.GetAllAsync(
				h => h.UserId == userId,
				h => h.Category,
				h => h.HabitProgresses
			);

			return ApiResponse<List<Habit>>.SuccessResponse(habits.ToList());
		}

		// --------------------------------------------------
		// HABIT GÜNCELLEME
		// --------------------------------------------------
		public async Task<ApiResponse<Habit>> UpdateAsync(HabitUpdateDto habitDto, int userId)
		{
			var habit = await _habitRepository.GetByIdAsync(habitDto.Id);

			if (habit == null || habit.UserId != userId)
				return ApiResponse<Habit>.ErrorResponse("Yetkisiz işlem.");

			habit.Name = habitDto.Name;
			habit.Description = habitDto.Description;
			habit.TargetCount = habitDto.TargetCount;
			habit.Frequency = habitDto.Frequency;
			habit.UpdatedDate = DateTime.UtcNow;

			await _habitRepository.UpdateAsync(habit);

			return ApiResponse<Habit>.SuccessResponse(habit, "Güncellendi.");
		}

		// --------------------------------------------------
		// HABIT SİLME
		// --------------------------------------------------
		public async Task<ApiResponse<bool>> DeleteAsync(int habitId, int userId)
		{
			var habit = await _habitRepository.GetByIdAsync(habitId);

			if (habit == null || habit.UserId != userId)
				return ApiResponse<bool>.ErrorResponse("Yetkisiz işlem.");

			await _habitRepository.SoftDeleteAsync(habitId);

			return ApiResponse<bool>.SuccessResponse(true, "Silindi.");
		}

		// --------------------------------------------------
		// TIK ATMA (TEK MERKEZ: HABITPROGRESS)
		// --------------------------------------------------
		public async Task<ApiResponse<HabitProgress>> TickHabitAsync(int habitId, int userId)
		{
			var habit = await _habitRepository.GetByIdAsync(habitId);
			if (habit == null || habit.UserId != userId)
				return ApiResponse<HabitProgress>.ErrorResponse("Alışkanlık bulunamadı.");

			int target = habit.TargetCount <= 0 ? 1 : habit.TargetCount;
			DateTime today = DateTime.UtcNow.Date;

			var todayProgress = await _progressRepository.GetFirstAsync(
				p => p.HabitId == habitId && p.ProgressDate == today
			);

			bool isJustCompleted = false;

			if (todayProgress == null)
			{
				todayProgress = new HabitProgress
				{
					HabitId = habitId,
					UserId = userId,
					ProgressDate = today,
					CompletedCount = 1,
					TargetCount = target,
					IsCompleted = (1 >= target),
					CreatedAt = DateTime.UtcNow
				};

				if (todayProgress.IsCompleted)
					isJustCompleted = true;

				await _progressRepository.AddAsync(todayProgress);
			}
			else
			{
				bool wasCompletedBefore = todayProgress.IsCompleted;

				todayProgress.CompletedCount++;

				if (todayProgress.CompletedCount >= target)
				{
					todayProgress.IsCompleted = true;
					if (!wasCompletedBefore)
						isJustCompleted = true;
				}

				todayProgress.UpdatedAt = DateTime.UtcNow;
				await _progressRepository.UpdateAsync(todayProgress);
			}

			// --------------------------------------------------
			// PUAN, SEVİYE VE ÖDÜL (Her tikte)
			// --------------------------------------------------
			var user = await _userRepository.GetByIdAsync(userId);
			if (user != null)
			{
				// Her tikte puan ekle
				user.Points += 10;

				// Seviye güncelle
				int newLevel = (user.Points / 50) + 1;
				if (newLevel > user.Level)
					user.Level = newLevel;

				// Ödül kontrolü
				var eligiblePrizes = await _prizeRepository.GetAllAsync(p => p.PointRequired <= user.Points);
				foreach (var prize in eligiblePrizes)
				{
					var alreadyHas = await _userPrizeRepository.GetFirstAsync(up => up.UserId == userId && up.PrizeId == prize.Id);
					if (alreadyHas == null)
					{
						await _userPrizeRepository.AddAsync(new UserPrize
						{
							UserId = userId,
							PrizeId = prize.Id,
							ClaimedAt = DateTime.UtcNow
						});

						user.Title = prize.Name; // Son alınan ödül başlık olarak
					}
				}

				await _userRepository.UpdateAsync(user);
			}

			// --------------------------------------------------
			// STREAK (Hedef tamamlandığında)
			// --------------------------------------------------
			if (isJustCompleted)
			{
				habit.CurrentStreak++;
				await _habitRepository.UpdateAsync(habit);
			}

			return ApiResponse<HabitProgress>.SuccessResponse(todayProgress, "İlerleme kaydedildi.");
		}

	}
}
