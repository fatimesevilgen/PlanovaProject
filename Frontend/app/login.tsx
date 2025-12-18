import { Ionicons } from "@expo/vector-icons";
import { LinearGradient } from "expo-linear-gradient";
import { router } from "expo-router";
import { useState } from "react";
import {
  StyleSheet,
  Text,
  TextInput,
  TouchableOpacity,
  View,
} from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";

import { useAuth } from "@/contexts/auth-context";

export default function LoginScreen() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [secure, setSecure] = useState(true);
  const [errorMessage, setErrorMessage] = useState("");

  const {login, isLoginPending} = useAuth()

  const handleLogin = () => {
    login(email, password)
  };

  return (
    <LinearGradient
      colors={["#9B5DE5", "#6A4CFF", "#4EA8DE"]}
      start={[0, 0]}
      end={[1, 1]}
      style={styles.container}
    >
      <SafeAreaView style={styles.safe}>
        <View style={styles.content}>
          <Text style={styles.title}>Planova</Text>
          <Text style={styles.subtitle}>Alışkanlıklarını Oyunlaştır</Text>

          <View style={styles.form}>
            <View style={styles.inputWrap}>
              <TextInput
                value={email}
                onChangeText={setEmail}
                placeholder="E-posta"
                placeholderTextColor="rgba(255,255,255,0.8)"
                keyboardType="email-address"
                autoCapitalize="none"
                style={styles.input}
              />
            </View>

            <View style={styles.inputWrap}>
              <TextInput
                value={password}
                onChangeText={setPassword}
                placeholder="Şifre"
                placeholderTextColor="rgba(255,255,255,0.8)"
                secureTextEntry={secure}
                autoCapitalize="none"
                style={styles.input}
              />

              <TouchableOpacity
                onPress={() => setSecure(!secure)}
                style={styles.eyeBtn}
              >
                <Ionicons
                  name={secure ? "eye-off-outline" : "eye-outline"}
                  size={22}
                  color="#fff"
                />
              </TouchableOpacity>
            </View>

            {errorMessage ? (
              <Text style={styles.errorText}>{errorMessage}</Text>
            ) : null}

            <TouchableOpacity
              style={styles.primaryButton}
              onPress={handleLogin}
              disabled={isLoginPending}
            >
              <Text style={styles.primaryButtonText}>
                {isLoginPending ? "Giriş Yapılıyor..." : "Giriş Yap"}
              </Text>
            </TouchableOpacity>

            <View style={styles.footerRow}>
              <Text style={styles.footerText}>Hesabın yok mu? </Text>
              <TouchableOpacity onPress={() => router.push("/register")}>
                <Text style={styles.linkText}>Kayıt Ol</Text>
              </TouchableOpacity>
            </View>
          </View>
        </View>
      </SafeAreaView>
    </LinearGradient>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, justifyContent: "center" },
  safe: { flex: 1, justifyContent: "center" },
  content: {
    flex: 1,
    alignItems: "center",
    paddingHorizontal: 28,
    paddingTop: 80,
  },
  title: { color: "#fff", fontSize: 40, fontWeight: "800", marginTop: 6 },
  subtitle: {
    color: "rgba(255,255,255,0.9)",
    marginTop: 6,
    marginBottom: 18,
  },
  form: { width: "100%", marginTop: 6 },
  inputWrap: {
    backgroundColor: "rgba(255,255,255,0.12)",
    borderRadius: 14,
    paddingHorizontal: 14,
    paddingVertical: 8,
    marginBottom: 12,
    position: "relative",
  },
  input: { color: "#fff", height: 44 },
  eyeBtn: { position: "absolute", right: 16, top: 18 },
  primaryButton: {
    backgroundColor: "#fff",
    borderRadius: 14,
    paddingVertical: 12,
    marginTop: 6,
    alignItems: "center",
    justifyContent: "center",
  },
  primaryButtonText: { color: "#7B2CBF", fontWeight: "700", fontSize: 16 },
  footerRow: { flexDirection: "row", justifyContent: "center", marginTop: 6 },
  footerText: { color: "rgba(255,255,255,0.9)" },
  linkText: {
    color: "#E6F0FF",
    textDecorationLine: "underline",
    fontWeight: "700",
  },
  errorText: {
    color: "#ffbaba",
    marginBottom: 8,
    marginLeft: 4,
  },
});
