import { useAuth } from "@/contexts/auth-context";
import { Ionicons } from "@expo/vector-icons";
import { LinearGradient } from "expo-linear-gradient";
import { useRouter } from "expo-router";
import React, { useMemo } from "react";
import {
  Image,
  ScrollView,
  StyleSheet,
  Text,
  TouchableOpacity,
  View,
} from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";

export default function ProfileScreen() {
  const { user, logout } = useAuth();
  const router = useRouter();

  const activeBadge = useMemo(() => {
    if (!user?.prizes?.length) return null;
    return [...user.prizes].sort((a: any, b: any) => b.id - a.id)[0];
  }, [user]);

  const successRate = useMemo(() => {
    if ((user as any)?.successRate !== undefined)
      return (user as any).successRate;

    if (!user?.habits?.length) return 0;
    const completed = user.habits.filter(
      (h: any) => h.progress?.[0]?.isCompleted
    ).length;
    return Math.round((completed / user.habits.length) * 100);
  }, [user]);

  return (
    <SafeAreaView style={styles.container}>
      <ScrollView showsVerticalScrollIndicator={false}>
        {/* HEADER */}
        <View style={styles.headerTop}>
          <TouchableOpacity onPress={() => router.back()}>
            <Ionicons name="chevron-back" size={28} color="#000" />
          </TouchableOpacity>

          <Text style={styles.headerTitle}>Profil</Text>

          <TouchableOpacity onPress={() => router.push("/edit")}>
            <Ionicons name="pencil" size={22} color="#8E44FF" />
          </TouchableOpacity>
        </View>

        <LinearGradient
          colors={["#9B6FFF", "#D4A5FF"]}
          start={[0, 0]}
          end={[1, 1]}
          style={styles.cover}
        />

        <View style={styles.avatarWrapper}>
          <Image
            source={{ uri: user?.avatarUrl }}
            style={styles.avatar}
          />
        </View>

        <View style={styles.userInfo}>
          <Text style={styles.userName}>{user?.name}</Text>
          <Text style={styles.userEmail}>{user?.email}</Text>

          {activeBadge ? (
            <View style={styles.activeBadge}>
              <Image
                source={{ uri: activeBadge.imgUrl }}
                style={styles.badgeImg}
              />
              <Text style={styles.badgeName}>
                {activeBadge.prizeName}
              </Text>
            </View>
          ) : (
            <Text style={styles.noBadgeText}>
              Henüz rozet kazanılmadı
            </Text>
          )}

          <View style={styles.quickStats}>
            <Stat label="Seviye" value={user?.level ?? 1} />
            <Divider />
            <Stat label="Puan" value={user?.points ?? 0} />
            <Divider />
            <Stat label="Başarı" value={`${successRate}%`} />
          </View>
        </View>

        <View style={styles.actions}>
          <ActionButton
            icon="share-social"
            text="Profili Paylaş"
            primary
          />
        </View>

        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Rozetler</Text>

          <View style={styles.badgeGrid}>
            {user?.prizes?.map((p: any, i: number) => (
              <View key={i} style={styles.badgeCard}>
                <Image
                  source={{ uri: p.imgUrl }}
                  style={styles.badgeGridImg}
                />
                <Text style={styles.badgeGridText}>
                  {p.prizeName}
                </Text>
              </View>
            ))}

            {!user?.prizes?.length && (
              <Text style={styles.noBadgeText}>
                Henüz rozet yok
              </Text>
            )}
          </View>
        </View>

        <View style={styles.section}>
          <TouchableOpacity
            style={styles.logoutBtn}
            onPress={logout}
          >
            <Ionicons
              name="log-out-outline"
              size={20}
              color="#9B6FFF"
            />
            <Text style={styles.logoutText}>Çıkış Yap</Text>
          </TouchableOpacity>
        </View>
      </ScrollView>
    </SafeAreaView>
  );
}


const Stat = ({ label, value }: { label: string; value: any }) => (
  <View style={{ alignItems: "center", flex: 1 }}>
    <Text style={styles.statValue}>{value}</Text>
    <Text style={styles.statLabel}>{label}</Text>
  </View>
);

const Divider = () => <View style={styles.divider} />;

const ActionButton = ({
  icon,
  text,
  primary,
}: {
  icon: React.ComponentProps<typeof Ionicons>["name"];
  text: string;
  primary?: boolean;
}) => (
  <TouchableOpacity
    style={[
      styles.actionBtn,
      primary ? styles.primaryBtn : styles.secondaryBtn,
    ]}
  >
    <Ionicons
      name={icon}
      size={20}
      color={primary ? "#fff" : "#9B6FFF"}
    />
    <Text
      style={[
        styles.actionText,
        !primary && { color: "#9B6FFF" },
      ]}
    >
      {text}
    </Text>
  </TouchableOpacity>
);

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: "#FAFAFA" },

  headerTop: {
    flexDirection: "row",
    justifyContent: "space-between",
    padding: 16,
    alignItems: "center",
  },

  headerTitle: {
    fontSize: 20,
    fontWeight: "800",
  },

  cover: {
    height: 120,
  },

  avatarWrapper: {
    alignItems: "center",
    marginTop: -50,
  },

  avatar: {
    width: 100,
    height: 100,
    borderRadius: 50,
    borderWidth: 4,
    borderColor: "#FFF",
  },

  userInfo: {
    alignItems: "center",
    padding: 20,
    backgroundColor: "#FFF",
  },

  userName: {
    fontSize: 26,
    fontWeight: "800",
  },

  userEmail: {
    fontSize: 14,
    color: "#999",
    marginTop: 4,
  },

  activeBadge: {
    flexDirection: "row",
    alignItems: "center",
    gap: 8,
    marginTop: 10,
  },

  badgeImg: {
    width: 26,
    height: 26,
    borderRadius: 6,
  },

  badgeName: {
    fontSize: 14,
    fontWeight: "700",
    color: "#9B6FFF",
  },

  noBadgeText: {
    marginTop: 10,
    fontSize: 13,
    color: "#999",
  },

  quickStats: {
    flexDirection: "row",
    marginTop: 20,
    backgroundColor: "#F5F5F5",
    borderRadius: 16,
    paddingVertical: 16,
    width: "100%",
  },

  statValue: {
    fontSize: 18,
    fontWeight: "800",
    color: "#9B6FFF",
  },

  statLabel: {
    fontSize: 11,
    color: "#999",
    marginTop: 4,
  },

  divider: {
    width: 1,
    backgroundColor: "#DDD",
  },

  actions: {
    padding: 20,
  },

  actionBtn: {
    height: 50,
    borderRadius: 14,
    flexDirection: "row",
    justifyContent: "center",
    alignItems: "center",
    gap: 8,
  },

  primaryBtn: {
    backgroundColor: "#9B6FFF",
  },

  secondaryBtn: {
    backgroundColor: "#F5F5F5",
    borderWidth: 2,
    borderColor: "#9B6FFF",
  },

  actionText: {
    color: "#fff",
    fontSize: 16,
    fontWeight: "700",
  },

  section: {
    padding: 20,
  },

  sectionTitle: {
    fontSize: 18,
    fontWeight: "800",
    marginBottom: 12,
  },

  badgeGrid: {
    flexDirection: "row",
    flexWrap: "wrap",
    gap: 12,
  },

  badgeCard: {
    width: "48%",
    backgroundColor: "#F5F5F5",
    borderRadius: 14,
    padding: 16,
    alignItems: "center",
  },

  badgeGridImg: {
    width: 40,
    height: 40,
    marginBottom: 6,
  },

  badgeGridText: {
    fontSize: 13,
    fontWeight: "700",
  },

  logoutBtn: {
    flexDirection: "row",
    justifyContent: "center",
    alignItems: "center",
    gap: 8,
    paddingVertical: 14,
    backgroundColor: "#F5F5F5",
    borderRadius: 14,
  },

  logoutText: {
    fontSize: 16,
    fontWeight: "700",
    color: "#9B6FFF",
  },
});
