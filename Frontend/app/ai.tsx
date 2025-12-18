import { useAskAi } from "@/hooks/use-tanstack-query";
import { Ionicons } from "@expo/vector-icons";
import { LinearGradient } from "expo-linear-gradient";
import { router } from "expo-router";
import React, { useEffect, useRef, useState } from "react";
import {
  Animated,
  FlatList,
  KeyboardAvoidingView,
  Platform,
  StyleSheet,
  Text,
  TextInput,
  TouchableOpacity,
  View
} from "react-native";

type Message = {
  id: string;
  sender: "user" | "ai";
  text: string;
};

export default function AiScreen() {
  const [messages, setMessages] = useState<Message[]>([
    {
      id: "1",
      sender: "ai",
      text: "Merhaba! Alışkanlıklarını geliştirmek için buradayım 🤖",
    },
  ]);

  const [input, setInput] = useState("");
  const [isTyping, setIsTyping] = useState(false);

  const flatListRef = useRef<FlatList>(null);

  const { mutate: askAi } = useAskAi({
    onSuccess: (response) => {
      setMessages((prev) => [
        ...prev,
        {
          id: Date.now().toString(),
          sender: "ai",
          text: response,
        },
      ]);
      setIsTyping(false);
    },
    onError: () => {
      setMessages((prev) => [
        ...prev,
        {
          id: Date.now().toString(),
          sender: "ai",
          text: "Şu an cevap veremedim, biraz sonra tekrar deneyelim.",
        },
      ]);
      setIsTyping(false);
    },
  });

  const sendMessage = () => {
    if (!input.trim()) return;

    const text = input;
    setInput("");

    setMessages((prev) => [
      ...prev,
      { id: Date.now().toString(), sender: "user", text },
    ]);

    setIsTyping(true);
    askAi(text);
  };

  useEffect(() => {
    flatListRef.current?.scrollToEnd({ animated: true });
  }, [messages, isTyping]);

  const dot1 = useRef(new Animated.Value(0.3)).current;
  const dot2 = useRef(new Animated.Value(0.3)).current;
  const dot3 = useRef(new Animated.Value(0.3)).current;

  useEffect(() => {
    const animate = (dot: Animated.Value, delay: number) => {
      Animated.loop(
        Animated.sequence([
          Animated.timing(dot, {
            toValue: 1,
            duration: 400,
            delay,
            useNativeDriver: true,
          }),
          Animated.timing(dot, {
            toValue: 0.3,
            duration: 400,
            useNativeDriver: true,
          }),
        ])
      ).start();
    };

    animate(dot1, 0);
    animate(dot2, 200);
    animate(dot3, 400);
  }, []);

  const renderMessage = ({ item }: { item: Message }) => {
    const isUser = item.sender === "user";

    return (
      <View
        style={[
          styles.row,
          { justifyContent: isUser ? "flex-end" : "flex-start" },
        ]}
      >
        {!isUser && <Text style={styles.aiEmoji}>🤖</Text>}

        <View
          style={[
            styles.bubble,
            isUser ? styles.userBubble : styles.aiBubble,
          ]}
        >
          {!isUser && <Text style={styles.aiName}>AI Asistan</Text>}
          <Text style={[styles.text, isUser && { color: "#fff" }]}>
            {item.text}
          </Text>
        </View>

        {isUser && <Ionicons name="person" size={18} color="#8E44FF" />}
      </View>
    );
  };

  return (
    <KeyboardAvoidingView
      style={{ flex: 1, backgroundColor: "#F5F5FF" }}
      behavior={Platform.OS === "ios" ? "padding" : "height"}
      keyboardVerticalOffset={Platform.OS === "ios" ? 90 : 0}
    >
      {/* HEADER */}
      <LinearGradient colors={["#8E44FF", "#5B86E5"]} style={styles.header}>
        <TouchableOpacity onPress={() => router.back()}>
          <Ionicons name="arrow-back" size={26} color="#fff" />
        </TouchableOpacity>
        <Text style={styles.headerTitle}>AI Asistan</Text>
        <View style={{ width: 26 }} />
      </LinearGradient>

      {/* MESSAGES */}
      <FlatList
        ref={flatListRef}
        data={messages}
        renderItem={renderMessage}
        keyExtractor={(item) => item.id}
        contentContainerStyle={{ padding: 16 }}
      />

      {/* TYPING */}
      {isTyping && (
        <View style={styles.typingRow}>
          <Text style={styles.aiEmoji}>🤖</Text>
          <View style={styles.typingBubble}>
            <Animated.View style={[styles.dot, { opacity: dot1 }]} />
            <Animated.View style={[styles.dot, { opacity: dot2 }]} />
            <Animated.View style={[styles.dot, { opacity: dot3 }]} />
          </View>
        </View>
      )}

      {/* INPUT */}
      <View style={styles.inputWrapper}>
        <TextInput
          value={input}
          onChangeText={setInput}
          placeholder="Mesaj yaz..."
          style={styles.input}
        />

        <TouchableOpacity onPress={sendMessage}>
          <LinearGradient
            colors={["#8E44FF", "#5B86E5"]}
            style={styles.sendBtn}
          >
            <Ionicons name="send" size={18} color="#fff" />
          </LinearGradient>
        </TouchableOpacity>
      </View>
    </KeyboardAvoidingView>
  );
}

const styles = StyleSheet.create({
  header: {
    height: 90,
    paddingTop: 45,
    paddingHorizontal: 16,
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    borderBottomLeftRadius: 24,
    borderBottomRightRadius: 24,
  },
  headerTitle: {
    color: "#fff",
    fontSize: 20,
    fontWeight: "700",
  },
  row: {
    flexDirection: "row",
    alignItems: "flex-end",
    marginBottom: 12,
  },
  aiEmoji: {
    fontSize: 20,
    marginRight: 6,
  },
  bubble: {
    maxWidth: "75%",
    padding: 14,
    borderRadius: 18,
  },
  aiBubble: {
    backgroundColor: "#fff",
    borderTopLeftRadius: 4,
  },
  userBubble: {
    backgroundColor: "#8E44FF",
    borderTopRightRadius: 4,
  },
  aiName: {
    fontSize: 12,
    fontWeight: "700",
    color: "#8E44FF",
    marginBottom: 4,
  },
  text: {
    fontSize: 15,
    color: "#333",
  },
  typingRow: {
    flexDirection: "row",
    alignItems: "center",
    paddingHorizontal: 16,
    marginBottom: 8,
  },
  typingBubble: {
    flexDirection: "row",
    backgroundColor: "#E9E9FF",
    padding: 10,
    borderRadius: 18,
  },
  dot: {
    width: 6,
    height: 6,
    borderRadius: 3,
    backgroundColor: "#6B6B7A",
    marginHorizontal: 2,
  },
  inputWrapper: {
    margin: 16,
    padding: 12,
    backgroundColor: "#fff",
    borderRadius: 30,
    flexDirection: "row",
    alignItems: "center",
    elevation: 8,
  },
  input: {
    flex: 1,
    fontSize: 16,
    paddingHorizontal: 12,
  },
  sendBtn: {
    width: 42,
    height: 42,
    borderRadius: 21,
    alignItems: "center",
    justifyContent: "center",
  },
});
