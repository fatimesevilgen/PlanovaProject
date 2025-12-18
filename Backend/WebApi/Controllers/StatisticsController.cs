using Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class StatisticsController : ControllerBase
	{
		private readonly IUserStatisticsService _statisticsService;

		public StatisticsController(IUserStatisticsService statisticsService)
		{
			_statisticsService = statisticsService;
		}

		/// <summary>
		/// Takvim görünümü için – hangi günler tamamlandı
		/// </summary>
		/// <param name="start">Başlangıç tarihi</param>
		/// <param name="end">Bitiş tarihi</param>
		[HttpGet("calendar")]
		public async Task<IActionResult> GetCalendar(
			[FromQuery] DateTime start,
			[FromQuery] DateTime end)
		{
			int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

			start = DateTime.SpecifyKind(start, DateTimeKind.Utc);
			end = DateTime.SpecifyKind(end, DateTimeKind.Utc);

			var result = await _statisticsService.GetCalendarAsync(userId, start, end);

			return result.Success ? Ok(result) : BadRequest(result);
		}

		/// <summary>
		/// Haftalık özet (7 günün kaçı tamamlandı)
		/// </summary>
		[HttpGet("weekly")]
		public async Task<IActionResult> GetWeeklySummary()
		{
			int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

			var result = await _statisticsService.GetWeeklySummaryAsync(userId);

			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpGet("habits/weekly")]
		public async Task<IActionResult> GetWeeklyHabitProgress()
		{
			int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var result = await _statisticsService.GetWeeklyHabitSummaryAsync(userId);
			return result.Success ? Ok(result) : BadRequest(result);
		}
	}
}
