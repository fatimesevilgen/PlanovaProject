import { useAddHabit } from "@/hooks/use-tanstack-query";
import { Ionicons } from "@expo/vector-icons";
import DateTimePicker from "@react-native-community/datetimepicker";
import { useQueryClient } from "@tanstack/react-query";
import { LinearGradient } from "expo-linear-gradient";
import { Stack, useRouter } from "expo-router";
import { useState } from "react";
import {
  ScrollView,
  StyleSheet,
  Text,
  TextInput,
  TouchableOpacity,
  View,
} from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";

const CATEGORY_DATA = [
  { icon: "💪", label: "Sağlık", value:0 },
  { icon: "📘", label: "Kişisel Gelişim" ,value:1},
  { icon: "🧘‍♀️", label: "Zihinsel Sağlık" ,value:2},
  { icon: "💼", label: "Kariyer" ,value:3},
  { icon: "👥", label: "Sosyal" ,value:4},
  { icon: "🎨", label: "Hobi" ,value:5},
];
  
const ICONS = [
  "🏃‍♂️",
  "📚",
  "🧘‍♀️",
  "💧",
  "🎯",
  "💪",
  "🎨",
  "🎵",
  "💻",
  "🏋️‍♂️",
];

export default function AddHabitScreen() {
  return (
    <>
      <Stack.Screen options={{ headerShown: false }} />
      <AddHabitContent />
    </>
  );
}

function AddHabitContent() {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [selectedCategory, setSelectedCategory] = useState<any>();
  const [selectedIcon, setSelectedIcon] = useState("");
  const [frequency, setFrequency] = useState(1);
  const [targetCount, setTargetCount] = useState(0);

  const [endDate, setEndDate] = useState(new Date());
  const [showEndDatePicker, setShowEndDatePicker] = useState(false);

  const [time, setTime] = useState("--:--");
  const [showTimePicker, setShowTimePicker] = useState(false);

  const queryClient = useQueryClient();
  const router = useRouter()
  const { mutate, isPending } = useAddHabit({
    onSuccess: (newHabit) => {
      queryClient.setQueryData(["me"], (oldData: any) => {
        if (!oldData) return oldData;
        return {
          ...oldData,
          data: {
            ...oldData.data,
            habits: [...(oldData.data.habits || []), newHabit.data]
          }
        };
      });
      router.push("/home");
    },
  });

  const handleSubmit = () => {
  const data = {
    name: title,
    description: description,
    categoryId: selectedCategory ?? 0,  
    frequency: frequency,
    targetCount: targetCount,
    endDate: endDate,     
    newCategoryName: "",
    newCategoryIcon: "fa-hashtag", 
  };

  mutate(data);
};


  return (
    <LinearGradient colors={["#8E44FF", "#5B86E5"]} style={{ flex: 1 }}>
      <SafeAreaView style={{ flex: 1 }}>
        <View style={styles.header}>
          <TouchableOpacity onPress={() => router.back()}>
            <Ionicons name="arrow-back" size={26} color="#fff" />
          </TouchableOpacity>
          <Text style={styles.headerTitle}>Yeni Alışkanlık</Text>
          <View style={{ width: 26 }} />
        </View>

        <ScrollView
          style={styles.sheet}
          showsVerticalScrollIndicator={false}
          contentContainerStyle={{ paddingBottom: 50 }}
        >
          <Text style={styles.label}>Alışkanlık Adı *</Text>
          <View style={styles.inputBox}>
            <TextInput
              placeholder="Örn: Sabah Koşusu"
              placeholderTextColor="#aaa"
              style={styles.input}
              value={title}
              onChangeText={setTitle}
            />
          </View>

          <Text style={styles.label}>Açıklama *</Text>
          <View style={styles.textarea}>
            <TextInput
              placeholder="Alışkanlığınız hakkında kısa bir açıklama..."
              placeholderTextColor="#aaa"
              style={[styles.input, { height: 90 }]}
              multiline
              value={description}
              onChangeText={setDescription}
            />
          </View>

          <Text style={styles.label}>Kategori *</Text>
          <View style={styles.categoryGrid}>
            {CATEGORY_DATA.map((c, i) => (
              <TouchableOpacity
                key={i}
                style={[
                  styles.categoryItem,
                  selectedCategory === c.value && styles.categorySelected,
                ]}
                onPress={() => setSelectedCategory(c.value)}
              >
                <Text style={styles.categoryIcon}>{c.icon}</Text>
                <Text style={styles.categoryLabel}>{c.label}</Text>
              </TouchableOpacity>
            ))}
          </View>

          <Text style={styles.label}>İkon Seç *</Text>
          <View style={styles.iconGrid}>
            {ICONS.map((icon, i) => (
              <TouchableOpacity
                key={i}
                style={[
                  styles.iconItem,
                  selectedIcon === icon && styles.iconSelected,
                ]}
                onPress={() => setSelectedIcon(icon)}
              >
                <Text style={styles.iconText}>{icon}</Text>
              </TouchableOpacity>
            ))}
          </View>

          <Text style={styles.label}>Sıklık *</Text>
          <View>
            {[{name:"Günlük", value: 0}, { name:  "Haftalık", value: 1}, { name: "Aylık", value: 2}].map((f) => (
              <TouchableOpacity
                key={f.value}
                style={[
                  styles.freqItem,
                  frequency === f.value && styles.freqSelected,
                ]}
                onPress={() => setFrequency(f.value)}
              >
                <Text style={styles.freqTitle}>{f.name}</Text>
                <Text style={styles.freqSub}>
                  {f.value === 1 && "Her gün tekrarlanır"}
                  {f.value === 7 && "Haftada belirli günler"}
                  {f.value === 30 && "Ayda belirli günler"}
                </Text>
              </TouchableOpacity>
            ))}
          </View>

          <Text style={styles.label}>Hedef Sayısı *</Text>
          <View style={styles.counterBox}>
            <TouchableOpacity
              style={styles.counterBtn}
              onPress={() => setTargetCount(Math.max(0, targetCount - 1))}
            >
              <Text style={styles.counterBtnText}>-</Text>
            </TouchableOpacity>

            <Text style={styles.counterValue}>{targetCount}</Text>

            <TouchableOpacity
              style={styles.counterBtn}
              onPress={() => setTargetCount(targetCount + 1)}
            >
              <Text style={styles.counterBtnText}>+</Text>
            </TouchableOpacity>
          </View>

          <Text style={styles.label}>Bitiş Tarihi *</Text>
          <TouchableOpacity
            style={styles.dateBox}
            onPress={() => setShowEndDatePicker(true)}
          >
            <Text style={styles.dateText}>
              {endDate.toLocaleDateString("tr-TR")}{" "}
              {endDate.toLocaleTimeString("tr-TR", {
                hour: "2-digit",
                minute: "2-digit",
              })}
            </Text>
            <Ionicons name="calendar-outline" size={20} color="#666" />
          </TouchableOpacity>

          {showEndDatePicker && (
            <DateTimePicker
              value={endDate}
              mode="date"
              onChange={(e, d) => {
                setShowEndDatePicker(false);
                if (d) setEndDate(d);
              }}
            />
          )}

          <Text style={styles.label}>Hatırlatıcı Saati</Text>
          <TouchableOpacity
            style={styles.dateBox}
            onPress={() => setShowTimePicker(true)}
          >
            <Text style={styles.dateText}>{time}</Text>
            <Ionicons name="time-outline" size={20} color="#666" />
          </TouchableOpacity>

          {showTimePicker && (
            <DateTimePicker
              value={new Date()}
              mode="time"
              onChange={(e, d) => {
                setShowTimePicker(false);
                if (d) {
                  const hh = d.getHours().toString().padStart(2, "0");
                  const mm = d.getMinutes().toString().padStart(2, "0");
                  setTime(`${hh}:${mm}`);
                }
              }}
            />
          )}

          <TouchableOpacity style={styles.saveBtn} onPress={handleSubmit}>
            <LinearGradient
              colors={["#8E44FF", "#5B86E5"]}
              style={styles.saveInner}
            >
              <Text style={styles.saveText}>Alışkanlığı Oluştur</Text>
            </LinearGradient>
          </TouchableOpacity>
        </ScrollView>
      </SafeAreaView>
    </LinearGradient>
  );
}

const styles = StyleSheet.create({
  header: {
    paddingHorizontal: 22,
    paddingVertical: 12,
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
  },
  headerTitle: {
    color: "#fff",
    fontSize: 22,
    fontWeight: "700",
  },

  sheet: {
    marginTop: 20,
    backgroundColor: "#fff",
    borderTopLeftRadius: 26,
    borderTopRightRadius: 26,
    padding: 22,
  },

  label: {
    fontSize: 15,
    fontWeight: "700",
    marginTop: 20,
    color: "#333",
  },

  inputBox: {
    marginTop: 8,
    backgroundColor: "#F4F4FF",
    padding: 14,
    borderRadius: 14,
  },
  textarea: {
    marginTop: 8,
    backgroundColor: "#F4F4FF",
    padding: 14,
    borderRadius: 14,
  },
  input: {
    fontSize: 15,
    color: "#333",
  },

  categoryGrid: {
    flexDirection: "row",
    flexWrap: "wrap",
    marginTop: 12,
    justifyContent: "space-between",
  },
  categoryItem: {
    width: "48%",
    backgroundColor: "#F4F4FF",
    padding: 18,
    marginBottom: 12,
    borderRadius: 16,
    alignItems: "center",
  },
  categorySelected: {
    borderWidth: 2,
    borderColor: "#8E44FF",
  },
  categoryIcon: { fontSize: 30 },
  categoryLabel: { marginTop: 6, fontWeight: "600", fontSize: 14 },

  iconGrid: {
    flexDirection: "row",
    flexWrap: "wrap",
    marginTop: 10,
  },
  iconItem: {
    width: 55,
    height: 55,
    backgroundColor: "#F4F4FF",
    borderRadius: 14,
    marginRight: 10,
    marginBottom: 10,
    alignItems: "center",
    justifyContent: "center",
  },
  iconSelected: {
    borderWidth: 2,
    borderColor: "#8E44FF",
  },
  iconText: {
    fontSize: 26,
  },

  freqItem: {
    backgroundColor: "#F4F4FF",
    padding: 16,
    borderRadius: 16,
    marginTop: 10,
  },
  freqSelected: {
    borderWidth: 2,
    borderColor: "#8E44FF",
  },
  freqTitle: { fontSize: 16, fontWeight: "700" },
  freqSub: { color: "#666", fontSize: 12, marginTop: 3 },

  counterBox: {
    marginTop: 12,
    flexDirection: "row",
    backgroundColor: "#F4F4FF",
    padding: 16,
    justifyContent: "space-between",
    borderRadius: 16,
    alignItems: "center",
  },
  counterBtn: {
    backgroundColor: "#E9E5FF",
    width: 45,
    height: 45,
    borderRadius: 12,
    alignItems: "center",
    justifyContent: "center",
  },
  counterBtnText: { fontSize: 24, fontWeight: "700", color: "#8E44FF" },
  counterValue: { fontSize: 22, fontWeight: "700" },

  dateBox: {
    marginTop: 10,
    backgroundColor: "#F4F4FF",
    padding: 16,
    borderRadius: 16,
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
  },
  dateText: { fontSize: 15, color: "#333" },

  saveBtn: { marginTop: 40 },
  saveInner: {
    padding: 18,
    borderRadius: 20,
    alignItems: "center",
  },
  saveText: { color: "#fff", fontSize: 17, fontWeight: "700" },
});
