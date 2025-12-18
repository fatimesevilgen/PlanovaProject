import { useAuth } from "@/contexts/auth-context";
import {
  useCalendarStats,
  useHabitWeeklyStats,
  useWeeklyStats,
} from "@/hooks/use-tanstack-query";
import {
  Dimensions,
  ScrollView,
  StyleSheet,
  Text,
  View,
} from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";

const screenWidth = Dimensions.get("window").width;
const DAYS = ["Pzt", "Sal", "Çar", "Per", "Cum", "Cts", "Paz"];

function buildMonthCalendar(
  year: number,
  month: number,
  calendarData: any[]
) {
  const firstDay = new Date(year, month, 1).getDay(); 
  const daysInMonth = new Date(year, month + 1, 0).getDate();

  const completedMap: Record<number, boolean> = {};
  calendarData.forEach((d) => {
    const day = new Date(d.date).getDate();
    completedMap[day] = d.isCompleted;
  });

  const cells: any[] = [];
  const offset = firstDay === 0 ? 6 : firstDay - 1;

  for (let i = 0; i < offset; i++) cells.push(null);

  for (let day = 1; day <= daysInMonth; day++) {
    cells.push({
      day,
      completed: completedMap[day] ?? false,
    });
  }

  return cells;
}

function HabitProgressBar({ habit }: { habit: any }) {
  const blocks = Array.from({ length: habit.targetCount });

  return (
    <View style={styles.habitRow}>
      <Text style={styles.habitName} numberOfLines={1}>
        {habit.habitName}
      </Text>

      <View style={styles.progressRow}>
        {blocks.map((_, i) => (
          <View
            key={i}
            style={[
              styles.progressBlock,
              i < habit.completedCount
                ? styles.progressFilled
                : styles.progressEmpty,
            ]}
          />
        ))}
      </View>

      <Text style={styles.progressText}>
        {habit.completedCount}/{habit.targetCount}
      </Text>
    </View>
  );
}


export default function StatisticsScreen() {
  const { user } = useAuth();

  const start = new Date(Date.now() - 6 * 86400000).toISOString();
  const end = new Date().toISOString();

  const { data: calendarData, isLoading } = useCalendarStats(start, end);
  const { data: weeklyData } = useWeeklyStats();
  const { data: habitWeeklyData } = useHabitWeeklyStats();

  if (isLoading) {
    return (
      <SafeAreaView style={styles.container}>
        <Text>Yükleniyor...</Text>
      </SafeAreaView>
    );
  }

  const dayLabels =
    calendarData?.map((x: any) =>
      new Date(x.date).getDate().toString()
    ) ?? [];

  const dayValues =
    calendarData?.map((x: any) => (x.isCompleted ? 1 : 0)) ?? [];

  return (
    <SafeAreaView style={styles.container}>
      <ScrollView showsVerticalScrollIndicator={false}>
        <Text style={styles.title}>İstatistikler</Text>

        {weeklyData && (
          <View style={styles.summaryRow}>
            <View style={styles.summaryBox}>
              <Text style={styles.summaryValue}>
                {weeklyData.completedDays}/{weeklyData.totalDays}
              </Text>
              <Text style={styles.summaryLabel}>Tamamlanan Gün</Text>
            </View>

            <View style={styles.summaryBox}>
              <Text style={styles.summaryValue}>
                %{weeklyData.percentage}
              </Text>
              <Text style={styles.summaryLabel}>Başarı</Text>
            </View>
          </View>
        )}


        <View style={styles.card}>
          <Text style={styles.cardTitle}>📅 Bu Ay</Text>

          <View style={styles.calendarHeader}>
            {DAYS.map((d) => (
              <Text key={d} style={styles.calendarDayLabel}>
                {d}
              </Text>
            ))}
          </View>

          <View style={styles.calendarGrid}>
            {buildMonthCalendar(
              new Date().getFullYear(),
              new Date().getMonth(),
              calendarData ?? []
            ).map((cell, i) => (
              <View key={i} style={styles.calendarCell}>
                {cell && (
                  <>
                    <Text style={styles.calendarDay}>{cell.day}</Text>
                    <View
                      style={[
                        styles.calendarDot,
                        cell.completed
                          ? styles.calendarDone
                          : styles.calendarMiss,
                      ]}
                    />
                  </>
                )}
              </View>
            ))}
          </View>
        </View>

        {habitWeeklyData?.length > 0 && (
          <View style={styles.card}>
            <Text style={styles.cardTitle}>
              Alışkanlık Bazlı Haftalık İlerleme
            </Text>

            {habitWeeklyData.map((habit: any) => (
              <HabitProgressBar
                key={habit.habitId}
                habit={habit}
              />
            ))}
          </View>
        )}
      </ScrollView>
    </SafeAreaView>
  );
}

const chartConfig = {
  backgroundGradientFrom: "#F4F4FF",
  backgroundGradientTo: "#F4F4FF",
  decimalPlaces: 0,
  color: (opacity = 1) => `rgba(142, 68, 255, ${opacity})`,
  labelColor: () => "#555",
  propsForDots: {
    r: "5",
    strokeWidth: "2",
    stroke: "#8E44FF",
  },
};


const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#FAFAFA",
    padding: 16,
  },
  title: {
    fontSize: 26,
    fontWeight: "800",
    marginBottom: 16,
  },
  card: {
    backgroundColor: "#fff",
    borderRadius: 16,
    padding: 16,
    marginBottom: 20,
    elevation: 3,
  },
  cardTitle: {
    fontSize: 16,
    fontWeight: "700",
    marginBottom: 12,
  },
  chart: {
    borderRadius: 16,
  },

  summaryRow: {
    flexDirection: "row",
    gap: 12,
    marginBottom: 20,
  },
  summaryBox: {
    flex: 1,
    backgroundColor: "#9B6FFF",
    borderRadius: 16,
    padding: 16,
    alignItems: "center",
  },
  summaryValue: {
    color: "#fff",
    fontSize: 22,
    fontWeight: "800",
  },
  summaryLabel: {
    color: "#fff",
    fontSize: 12,
    opacity: 0.9,
    marginTop: 4,
  },

  calendarHeader: {
    flexDirection: "row",
    justifyContent: "space-between",
    marginBottom: 6,
  },
  calendarDayLabel: {
    width: "14%",
    textAlign: "center",
    fontSize: 12,
    fontWeight: "700",
    color: "#555",
  },
  calendarGrid: {
    flexDirection: "row",
    flexWrap: "wrap",
  },
  calendarCell: {
    width: "14%",
    height: 44,
    alignItems: "center",
    justifyContent: "center",
    marginVertical: 4,
  },
  calendarDay: {
    fontSize: 12,
    fontWeight: "700",
  },
  calendarDot: {
    width: 10,
    height: 10,
    borderRadius: 5,
    marginTop: 2,
  },
  calendarDone: {
    backgroundColor: "#36D65A",
  },
  calendarMiss: {
    backgroundColor: "#FF4D4F",
  },

  habitRow: {
    marginBottom: 14,
  },
  habitName: {
    fontSize: 14,
    fontWeight: "700",
    marginBottom: 6,
  },
  progressRow: {
    flexDirection: "row",
  },
  progressBlock: {
    flex: 1,
    height: 10,
    borderRadius: 4,
    marginRight: 4,
  },
  progressFilled: {
    backgroundColor: "#36D65A",
  },
  progressEmpty: {
    backgroundColor: "#E0E0E0",
  },
  progressText: {
    fontSize: 11,
    color: "#666",
    marginTop: 4,
    textAlign: "right",
  },
});
