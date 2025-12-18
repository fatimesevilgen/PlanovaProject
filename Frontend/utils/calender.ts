// utils/calendar.ts
export const buildMonthCalendar = (
  year: number,
  month: number,
  calendarStats: any[]
) => {
  const firstDay = new Date(year, month, 1).getDay();
  const daysInMonth = new Date(year, month + 1, 0).getDate();

  const startOffset = firstDay === 0 ? 6 : firstDay - 1;
  const totalCells = 42;

  const statsMap = calendarStats.reduce((acc, item) => {
    const day = new Date(item.date).getDate();
    acc[day] = item.isCompleted;
    return acc;
  }, {} as Record<number, boolean>);

  const cells = [];

  for (let i = 0; i < totalCells; i++) {
    const dayNumber = i - startOffset + 1;

    if (dayNumber < 1 || dayNumber > daysInMonth) {
      cells.push(null);
    } else {
      cells.push({
        day: dayNumber,
        completed: statsMap[dayNumber],
      });
    }
  }

  return cells;
};
