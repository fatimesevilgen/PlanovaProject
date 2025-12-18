using Business.Abstract;
using Core.Helpers.Methods;
using Entities;
using Entities.Dtos;
using Repositories.Abstract;

namespace Business
{
	public class UserStatisticsService : IUserStatisticsService
	{
		private readonly IHabitProgressRepository _habitProgressRepository;

		public UserStatisticsService(IHabitProgressRepository habitProgressRepository)
		{
			_habitProgressRepository = habitProgressRepository;
		}

		// 📅 TAKVİM
		public async Task<ApiResponse<List<CalendarDayDto>>> GetCalendarAsync(
			int userId, DateTime start, DateTime end)
		{

			start = DateTime.SpecifyKind(start.Date, DateTimeKind.Utc);
			end = DateTime.SpecifyKind(end.Date.AddDays(1), DateTimeKind.Utc);

			var progresses = await _habitProgressRepository
				.GetCompletedByUserAndDateRangeAsync(userId, start, end);

			Console.WriteLine("=== CALENDAR DEBUG ===");
			Console.WriteLine($"UserId: {userId}");
			Console.WriteLine($"Start: {start}");
			Console.WriteLine($"End: {end}");
			Console.WriteLine($"Progress Count: {progresses.Count}");

			foreach (var p in progresses)
			{
				Console.WriteLine(
					$"ProgressDate: {p.ProgressDate}, " +
					$"CompletedCount: {p.CompletedCount}, " +
					$"UserId: {p.UserId}");
			}

			var result = new List<CalendarDayDto>();

			for (var date = start.Date; date < end.Date; date = date.AddDays(1))
			{
				var dayStart = date.Date;
				var dayEnd = date.Date.AddDays(1);

				result.Add(new CalendarDayDto
				{
					Date = date,
					IsCompleted = progresses.Any(p =>
						p.ProgressDate >= dayStart &&
						p.ProgressDate < dayEnd &&
						p.CompletedCount > 0)
				});
			}

			return ApiResponse<List<CalendarDayDto>>.SuccessResponse(result);
		}

		// 📊 HAFTALIK ÖZET
		public async Task<ApiResponse<ProgressSummaryDto>> GetWeeklySummaryAsync(int userId)
		{
			var today = DateTime.UtcNow.Date;
			var start = today.AddDays(-(int)today.DayOfWeek + 1);
			var end = start.AddDays(7);

			var progresses = await _habitProgressRepository
				.GetCompletedByUserAndDateRangeAsync(userId, start, end);

			var completedDays = progresses
				.Where(p => p.CompletedCount > 0)
				.Select(p => p.ProgressDate.Date)
				.Distinct()
				.Count();

			return ApiResponse<ProgressSummaryDto>.SuccessResponse(
				new ProgressSummaryDto
				{
					TotalDays = 7,
					CompletedDays = completedDays,
					Percentage = Math.Round((double)completedDays / 7 * 100, 2)
				});
		}

		// 📊 HAFTALIK HABIT BAZLI ÖZET
		public async Task<ApiResponse<List<HabitProgressSummaryDto>>> GetWeeklyHabitSummaryAsync(int userId)
		{
			var today = DateTime.UtcNow.Date;
			var start = today.AddDays(-(int)today.DayOfWeek + 1);
			var end = start.AddDays(7);

			var progresses = await _habitProgressRepository
				.GetCompletedByUserAndDateRangeAsync(userId, start, end);

			var result = progresses
		.GroupBy(p => p.Habit)
		.Select(g => new HabitProgressSummaryDto
		{
			HabitId = g.Key.Id,
			HabitName = g.Key.Name,
			TargetCount = g.Key.TargetCount,                  
			CompletedCount = g.Sum(p => p.CompletedCount),   
			CompletedDays = g
				.Select(p => p.ProgressDate.Date)
				.Distinct()
				.Count(),
			TotalDays = 7
		})
		.ToList();

			return ApiResponse<List<HabitProgressSummaryDto>>.SuccessResponse(result);
		}
	}
}
