import { StyleSheet, Text, View } from "react-native";

interface Props {
  habitName: string;
  completedCount: number;
  targetCount: number;
}

export default function HabitProgressBar({
  habitName,
  completedCount,
  targetCount,
}: Props) {
  return (
    <View style={styles.container}>
      <Text style={styles.title}>{habitName}</Text>

      <View style={styles.barRow}>
        {Array.from({ length: targetCount }).map((_, index) => {
          const filled = index < completedCount;

          return (
            <View
              key={index}
              style={[
                styles.box,
                filled ? styles.boxFilled : styles.boxEmpty,
              ]}
            />
          );
        })}
      </View>

      <Text style={styles.counter}>
        {completedCount} / {targetCount}
      </Text>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    marginBottom: 16,
  },

  title: {
    fontSize: 14,
    fontWeight: "700",
    marginBottom: 6,
    color: "#333",
  },

  barRow: {
    flexDirection: "row",
    gap: 6,
  },

  box: {
    flex: 1,
    height: 12,
    borderRadius: 6,
  },

  boxFilled: {
    backgroundColor: "#36D65A", 
  },

  boxEmpty: {
    backgroundColor: "#E0E0E0", 
  },

  counter: {
    marginTop: 4,
    fontSize: 11,
    color: "#777",
  },
});
