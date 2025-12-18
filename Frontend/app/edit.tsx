import { useAuth } from "@/contexts/auth-context";
import { Ionicons } from "@expo/vector-icons";
import { LinearGradient } from "expo-linear-gradient";
import { useRouter } from "expo-router";
import { useState } from "react";
import {
  KeyboardAvoidingView,
  Platform,
  StyleSheet,
  Text,
  TextInput,
  TouchableOpacity,
  View,
} from "react-native";

export default function EditProfileScreen() {
  const router = useRouter();
  const { user } = useAuth();

  const [name, setName] = useState(user?.firstName ?? "");
  const [surname, setSurname] = useState(user?.lastName ?? "");
  const [email, setEmail] = useState(user?.email ?? "");
  const [phoneNumber, setPhoneNumber] = useState(user?.phoneNumber ?? "");
  const [avatarUrl, setAvatarUrl] = useState(user?.avatarUrl ?? "");

  return (
    <KeyboardAvoidingView
      style={{ flex: 1 }}
      behavior={Platform.OS === "ios" ? "padding" : undefined}
    >
      <View style={styles.container}>
        {/* HEADER */}
        <View style={styles.header}>
          <TouchableOpacity onPress={() => router.back()}>
            <Ionicons name="arrow-back" size={26} color="#000" />
          </TouchableOpacity>

          <Text style={styles.headerTitle}>Profili Düzenle</Text>

          <View style={{ width: 26 }} />
        </View>

        {/* GRADIENT */}
        <LinearGradient
          colors={["#9B6BFF", "#C9A7FF"]}
          style={styles.gradient}
        />

        {/* AVATAR */}
        <View style={styles.avatarWrapper}>
          <View style={styles.avatar}>
            <Ionicons name="person" size={42} color="#8E44FF" />
          </View>
        </View>

        {/* FORM */}
        <View style={styles.form}>
          <Input label="Ad" value={name} onChangeText={setName} />
          <Input label="Soyad" value={surname} onChangeText={setSurname} />
          <Input label="E-posta" value={email} onChangeText={setEmail} />
          <Input
            label="Telefon"
            value={phoneNumber}
            onChangeText={setPhoneNumber}
          />
          <Input
            label="Avatar URL"
            value={avatarUrl}
            onChangeText={setAvatarUrl}
          />

          {/* SAVE BUTTON */}
          <TouchableOpacity style={styles.saveBtn}>
            <Text style={styles.saveText}>Kaydet</Text>
          </TouchableOpacity>
        </View>
      </View>
    </KeyboardAvoidingView>
  );
}

const Input = ({
  label,
  value,
  onChangeText,
}: {
  label: string;
  value: string;
  onChangeText: (text: string) => void;
}) => (
  <View style={styles.inputWrapper}>
    <Text style={styles.label}>{label}</Text>
    <TextInput
      value={value}
      onChangeText={onChangeText}
      style={styles.input}
      placeholder={label}
      placeholderTextColor="#999"
    />
  </View>
);

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#F6F6FF",
  },

  header: {
    height: 70,
    paddingHorizontal: 16,
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "space-between",
  },

  headerTitle: {
    fontSize: 18,
    fontWeight: "700",
  },

  gradient: {
    height: 120,
  },

  avatarWrapper: {
    alignItems: "center",
    marginTop: -50,
  },

  avatar: {
    width: 96,
    height: 96,
    borderRadius: 48,
    backgroundColor: "#fff",
    alignItems: "center",
    justifyContent: "center",
    elevation: 6,
  },

  form: {
    padding: 20,
    marginTop: 10,
  },

  inputWrapper: {
    marginBottom: 14,
  },

  label: {
    marginBottom: 6,
    fontWeight: "600",
    color: "#555",
  },

  input: {
    backgroundColor: "#fff",
    borderRadius: 14,
    paddingHorizontal: 14,
    paddingVertical: 12,
    fontSize: 15,
    elevation: 2,
  },

  saveBtn: {
    backgroundColor: "#8E44FF",
    paddingVertical: 16,
    borderRadius: 18,
    marginTop: 24,
    alignItems: "center",
  },

  saveText: {
    color: "#fff",
    fontSize: 16,
    fontWeight: "700",
  },
});
