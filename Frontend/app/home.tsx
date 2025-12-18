import { useAuth } from "@/contexts/auth-context";
import {
  useDeleteHabit,
  useTickHabit,
} from "@/hooks/use-tanstack-query";
import { Ionicons, MaterialCommunityIcons } from "@expo/vector-icons";
import { LinearGradient } from "expo-linear-gradient";
import { router, Stack } from "expo-router";
import {
  useEffect,
  useMemo,
  useRef,
  useState,
} from "react";
import {
  Image,
  Modal,
  ScrollView,
  StyleSheet,
  Text,
  TouchableOpacity,
  View,
} from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";

export default function HomeScreen() {
  return (
    <>
      <Stack.Screen options={{ headerShown: false }} />
      <HomeContent />
    </>
  );
}

function HomeContent() {
  const { user, refreshUser } = useAuth();

  const [localHabits, setLocalHabits] = useState<any[]>([]);

  useEffect(() => {
    if (user?.habits) {
      setLocalHabits(user.habits);
    }
  }, [user?.habits]);

  const activeBadge = useMemo(() => {
    if (!user?.prizes?.length) return null;
    return [...user.prizes].sort((a, b) => b.id - a.id)[0];
  }, [user]);

  const prevPrizeCount = useRef<number>(0);
  const [showPrizeModal, setShowPrizeModal] = useState(false);
  const [newPrize, setNewPrize] = useState<any>(null);

  useEffect(() => {
    if (!user?.prizes?.length) return;

    if (prevPrizeCount.current === 0) {
      prevPrizeCount.current = user.prizes.length;
      return;
    }

    if (user.prizes.length > prevPrizeCount.current) {
      const latestPrize = [...user.prizes].sort(
        (a, b) => b.id - a.id
      )[0];

      setNewPrize(latestPrize);
      setShowPrizeModal(true);
    }

    prevPrizeCount.current = user.prizes.length;
  }, [user?.prizes]);

  /* ---------- TICK ---------- */
  const { mutate: tickHabit, isPending: ticking } =
    useTickHabit({
      onSuccess: () => {
        refreshUser();
      },
    });

  const handleTick = (habitId: number) => {
    tickHabit(habitId);
  };

  const { mutate: deleteHabit } = useDeleteHabit({
    onSuccess: (_, habitId) => {
      setLocalHabits((prev) =>
        prev.filter((h) => h.id !== habitId)
      );

      refreshUser();
    },
  });

  const successRate = useMemo(() => {
    if (!user?.habits?.length) return 0;
    const completed = user.habits.filter(
      (h: any) => h.progress?.[0]?.isCompleted
    ).length;
    return Math.round(
      (completed / user.habits.length) * 100
    );
  }, [user]);

  return (
    <SafeAreaView style={styles.container}>
      <ScrollView
        showsVerticalScrollIndicator={false}
        contentContainerStyle={{ paddingBottom: 90 }}
      >
        <LinearGradient
          colors={["#8E44FF", "#5B86E5"]}
          style={styles.header}
        >
          <View style={styles.headerTop}>
            <Text style={styles.headerTitle}>
              Merhaba {user?.name} 👋
            </Text>

            <View style={styles.rightInfo}>
              <View style={styles.pointRow}>
                <Ionicons
                  name="trophy"
                  size={16}
                  color="#FFD700"
                />
                <Text style={styles.pointsText}>
                  {user?.points ?? 0}
                </Text>
              </View>

              {activeBadge && (
                <View style={styles.badgeRow}>
                  <Image
                    source={{ uri: activeBadge.imgUrl }}
                    style={styles.badgeImg}
                  />
                  <Text style={styles.badgeName}>
                    {activeBadge.prizeName}
                  </Text>
                </View>
              )}
            </View>
          </View>

          <View style={styles.statsRow}>
            <View style={styles.statBox}>
              <Text style={styles.statNumber}>
                {user?.level ?? 1}
              </Text>
              <Text style={styles.statLabel}>
                Seviye
              </Text>
            </View>

            <View style={styles.statBox}>
              <Text style={styles.statNumber}>
                {successRate}%
              </Text>
              <Text style={styles.statLabel}>
                Başarı
              </Text>
            </View>
          </View>
        </LinearGradient>

        <View style={styles.section}>
          <View style={styles.sectionHeader}>
            <Text style={styles.sectionTitle}>
              Bugünün Alışkanlıkları
            </Text>
            <TouchableOpacity
              style={styles.addBtn}
              onPress={() =>
                router.push("/add-habit")
              }
            >
              <Ionicons
                name="add"
                size={24}
                color="#fff"
              />
            </TouchableOpacity>
          </View>

          {localHabits.map((habit: any) => {
            const isDone =
              habit.progress?.[0]?.isCompleted;

            return (
              <View
                key={habit.id}
                style={styles.habitCard}
              >
                <View style={styles.habitInfo}>
                  <MaterialCommunityIcons
                    name="checkbox-marked-circle-outline"
                    size={26}
                    color="#8E44FF"
                  />
                  <View style={{ marginLeft: 10 }}>
                    <Text style={styles.habitTitle}>
                      {habit.name}
                    </Text>
                    <Text
                      style={styles.habitCategory}
                    >
                      {habit.categoryName} · 🔥{" "}
                      {habit.currentStreak}
                    </Text>
                  </View>
                </View>

                <View
                  style={{
                    flexDirection: "row",
                    alignItems: "center",
                  }}
                >
                  <TouchableOpacity
                    disabled={isDone || ticking}
                    onPress={() =>
                      handleTick(habit.id)
                    }
                  >
                    <Ionicons
                      name={
                        isDone
                          ? "checkmark-circle"
                          : "checkmark-circle-outline"
                      }
                      size={32}
                      color={
                        isDone ? "#36D65A" : "#ccc"
                      }
                    />
                  </TouchableOpacity>

                  <TouchableOpacity
                    onPress={() =>
                      deleteHabit(habit.id)
                    }
                    style={{ marginLeft: 12 }}
                  >
                    <Ionicons
                      name="trash-outline"
                      size={22}
                      color="#FF4D4D"
                    />
                  </TouchableOpacity>
                </View>
              </View>
            );
          })}
        </View>
      </ScrollView>

      <View style={styles.tabBar}>
        <Tab icon="home" label="Ana Sayfa" active />
        <Tab
          icon="bar-chart-outline"
          label="İstatistik"
          onPress={() =>
            router.push("/statistics")
          }
        />
        <Tab
          icon="sparkles-outline"
          label="AI"
          onPress={() => router.push("/ai")}
        />
        <Tab
          icon="person-circle-outline"
          label="Profil"
          onPress={() =>
            router.push("/profile")
          }
        />
      </View>

      <Modal
        transparent
        visible={showPrizeModal}
        animationType="fade"
      >
        <View style={styles.modalOverlay}>
          <View style={styles.modalCard}>
            <Text style={styles.modalTitle}>
              🎉 Tebrikler!
            </Text>
            {newPrize && (
              <>
                <Image
                  source={{ uri: newPrize.imgUrl }}
                  style={styles.modalImg}
                />
                <Text style={styles.modalName}>
                  {newPrize.prizeName}
                </Text>
                <Text style={styles.modalDesc}>
                  {newPrize.prizeDescription}
                </Text>
              </>
            )}
            <TouchableOpacity
              style={styles.modalBtn}
              onPress={() =>
                setShowPrizeModal(false)
              }
            >
              <Text style={styles.modalBtnText}>
                Harika 🚀
              </Text>
            </TouchableOpacity>
          </View>
        </View>
      </Modal>
    </SafeAreaView>
  );
}


function Tab({ icon, label, onPress, active }: any) {
  return (
    <TouchableOpacity
      style={styles.tabItem}
      onPress={onPress}
    >
      <Ionicons
        name={icon}
        size={24}
        color={active ? "#8E44FF" : "#777"}
      />
      <Text
        style={
          active
            ? styles.tabLabelActive
            : styles.tabLabel
        }
      >
        {label}
      </Text>
    </TouchableOpacity>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: "#F4F4FF" },
  header: {
    padding: 20,
    paddingTop: 40,
    borderBottomLeftRadius: 28,
    borderBottomRightRadius: 28,
  },
  headerTop: {
    flexDirection: "row",
    justifyContent: "space-between",
  },
  headerTitle: { color: "#fff", fontSize: 24, fontWeight: "800" },
  rightInfo: { alignItems: "flex-end" },
  pointRow: { flexDirection: "row", alignItems: "center" },
  pointsText: { color: "#FFD700", marginLeft: 4, fontWeight: "700" },
  badgeRow: { flexDirection: "row", alignItems: "center", marginTop: 4 },
  badgeImg: { width: 20, height: 20, marginRight: 4 },
  badgeName: { color: "#fff", fontSize: 11, fontWeight: "600" },
  statsRow: { flexDirection: "row", marginTop: 20 },
  statBox: {
    flex: 1,
    backgroundColor: "rgba(255,255,255,0.25)",
    marginHorizontal: 4,
    borderRadius: 16,
    paddingVertical: 14,
    alignItems: "center",
  },
  statNumber: { color: "#fff", fontSize: 20, fontWeight: "700" },
  statLabel: { color: "#fff", fontSize: 12 },
  section: { padding: 20 },
  sectionHeader: {
    flexDirection: "row",
    justifyContent: "space-between",
    marginBottom: 16,
  },
  sectionTitle: { fontSize: 18, fontWeight: "700" },
  addBtn: { backgroundColor: "#8E44FF", padding: 6, borderRadius: 10 },
  habitCard: {
    backgroundColor: "#fff",
    padding: 14,
    borderRadius: 16,
    marginBottom: 12,
    flexDirection: "row",
    justifyContent: "space-between",
  },
  habitInfo: { flexDirection: "row", alignItems: "center" },
  habitTitle: { fontSize: 15, fontWeight: "700" },
  habitCategory: { fontSize: 12, color: "#777" },
  tabBar: {
    position: "absolute",
    bottom: 0,
    height: 70,
    width: "100%",
    backgroundColor: "#fff",
    flexDirection: "row",
    justifyContent: "space-around",
    alignItems: "center",
    borderTopWidth: 1,
    borderColor: "#ddd",
  },
  tabItem: { alignItems: "center" },
  tabLabel: { fontSize: 11, color: "#777", marginTop: 2 },
  tabLabelActive: {
    fontSize: 11,
    color: "#8E44FF",
    fontWeight: "700",
  },
  modalOverlay: {
    flex: 1,
    backgroundColor: "rgba(0,0,0,0.55)",
    justifyContent: "center",
    alignItems: "center",
  },
  modalCard: {
    width: "80%",
    backgroundColor: "#fff",
    borderRadius: 20,
    padding: 24,
    alignItems: "center",
  },
  modalTitle: { fontSize: 22, fontWeight: "800" },
  modalImg: { width: 80, height: 80, marginVertical: 12 },
  modalName: { fontSize: 18, fontWeight: "700" },
  modalDesc: { fontSize: 14, textAlign: "center", color: "#666" },
  modalBtn: {
    marginTop: 16,
    backgroundColor: "#8E44FF",
    paddingHorizontal: 26,
    paddingVertical: 10,
    borderRadius: 20,
  },
  modalBtnText: { color: "#fff", fontWeight: "700" },
});