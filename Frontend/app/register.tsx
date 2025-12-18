import { useRegister } from "@/hooks/use-tanstack-query";
import { Ionicons } from "@expo/vector-icons";
import { LinearGradient } from "expo-linear-gradient";
import { Stack, useRouter } from "expo-router";
import { useState } from "react";
import {
  StyleSheet,
  Text,
  TextInput,
  TouchableOpacity,
  View,
} from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";

export default function RegisterScreen() {
  return (
    <>
      <Stack.Screen
        options={{
          headerTransparent: true,
          headerTitle: "",
          headerBackButtonDisplayMode: "minimal",
          headerTintColor: "#fff",
        }}
      />
      <RegisterContent />
    </>
  );
}

function RegisterContent() {
  const [name, setName] = useState("");
  const [surname, setSurname] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [secure, setSecure] = useState(true);
  const [secure2, setSecure2] = useState(true);
  const [isAgreed, setIsAgreed] = useState(false);

  const router = useRouter();

  const {
    mutate,
    isPending,
  } = useRegister({
    onSuccess: (response) => {console.log(response), router.push("/login")},
    onError: (error) => {console.log(error.message)},
  });


  const handleRegister = () => {
    const dto = {
      name,
      surname,
      email,
      password,
      confirmPassword,
      isAgreedToPrivacyPolicy: isAgreed,
    };

    mutate(dto)
  };

  return (
    <LinearGradient
      colors={["#8E44FF", "#5B86E5"]}
      start={[0, 0]}
      end={[1, 1]}
      style={styles.container}
    >
      <SafeAreaView style={styles.safe}>
        <View style={styles.content}>
          <Text style={styles.title}>Kayıt Ol</Text>
          <Text style={styles.subtitle}>Planova'ya Hoş Geldin</Text>

          <View style={styles.form}>
            <View style={styles.inputWrap}>
              <TextInput
                value={name}
                onChangeText={setName}
                placeholder="Ad"
                placeholderTextColor="rgba(255,255,255,0.8)"
                style={styles.input}
              />
            </View>

            <View style={styles.inputWrap}>
              <TextInput
                value={surname}
                onChangeText={setSurname}
                placeholder="Soyad"
                placeholderTextColor="rgba(255,255,255,0.8)"
                style={styles.input}
              />
            </View>

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
                  size={20}
                  color="#fff"
                />
              </TouchableOpacity>
            </View>

            <View style={styles.inputWrap}>
              <TextInput
                value={confirmPassword}
                onChangeText={setConfirmPassword}
                placeholder="Şifre Tekrar"
                placeholderTextColor="rgba(255,255,255,0.8)"
                secureTextEntry={secure2}
                autoCapitalize="none"
                style={styles.input}
              />

              <TouchableOpacity
                onPress={() => setSecure2(!secure2)}
                style={styles.eyeBtn}
              >
                <Ionicons
                  name={secure2 ? "eye-off-outline" : "eye-outline"}
                  size={20}
                  color="#fff"
                />
              </TouchableOpacity>
            </View>

            <TouchableOpacity
              style={styles.checkboxRow}
              onPress={() => setIsAgreed(!isAgreed)}
              activeOpacity={0.8}
            >
              <Ionicons
                name={isAgreed ? "checkbox-outline" : "square-outline"}
                size={22}
                color="#fff"
              />
              <Text style={styles.checkboxText}>
                Gizlilik politikasını kabul ediyorum
              </Text>
            </TouchableOpacity>

            <TouchableOpacity
              style={styles.primaryButton}
              onPress={handleRegister}
              
            >
              <Text style={styles.primaryButtonText}>Kayıt Ol</Text>
            </TouchableOpacity>
          </View>
        </View>
      </SafeAreaView>
    </LinearGradient>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1 },
  safe: { flex: 1 },

  content: {
    flex: 1,
    alignItems: "center",
    paddingHorizontal: 28,
    paddingTop: 90,
  },

  title: { color: "#fff", fontSize: 36, fontWeight: "800", marginBottom: 6 },
  subtitle: {
    color: "rgba(255,255,255,0.9)",
    marginBottom: 24,
  },

  form: { width: "100%" },

  inputWrap: {
    backgroundColor: "rgba(255,255,255,0.12)",
    borderRadius: 14,
    paddingHorizontal: 14,
    paddingVertical: 8,
    marginBottom: 12,
    position: "relative",
  },

  input: {
    color: "#fff",
    height: 44,
  },

  eyeBtn: {
    position: "absolute",
    right: 12,
    top: 0,
    bottom: 0,
    justifyContent: "center",
  },

  checkboxRow: {
    flexDirection: "row",
    alignItems: "center",
    marginTop: 8,
    marginBottom: 20,
  },

  checkboxText: { color: "#fff", marginLeft: 8 },

  primaryButton: {
    backgroundColor: "#fff",
    borderRadius: 14,
    paddingVertical: 12,
    alignItems: "center",
  },

  primaryButtonText: {
    color: "#7B2CBF",
    fontWeight: "700",
    fontSize: 16,
  },
});
