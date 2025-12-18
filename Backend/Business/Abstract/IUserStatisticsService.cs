using Core.Helpers.Methods;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
	public interface IUserStatisticsService
	{
		Task<ApiResponse<List<CalendarDayDto>>> GetCalendarAsync(int userId, DateTime start, DateTime end);
		Task<ApiResponse<ProgressSummaryDto>> GetWeeklySummaryAsync(int userId);
		Task<ApiResponse<List<HabitProgressSummaryDto>>> GetWeeklyHabitSummaryAsync(int userId);
	}
}
