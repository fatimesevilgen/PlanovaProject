import { useGetMe, useLogin, useLogout } from "@/hooks/use-tanstack-query";
import { useQueryClient } from "@tanstack/react-query";
import { useRouter } from "expo-router";
import * as SecureStore from "expo-secure-store";
import {
  createContext,
  useContext,
  useEffect,
  useState,
} from "react";

/* ================= TYPES ================= */

export interface AuthContextType {
  user: any | null;
  accessToken: string | null;
  isAuthenticated: boolean;
  isLoginPending: boolean;
  login: (email: string, password: string) => void;
  logout: () => void;
  refreshUser: () => Promise<void>;
}

export const AuthContext = createContext<AuthContextType | undefined>(
  undefined
);

interface AuthProviderProps {
  children: React.ReactNode;
}

/* ================= PROVIDER ================= */

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [accessToken, setAccessToken] = useState<string | null>(null);

  const router = useRouter();
  const queryClient = useQueryClient();

  /* ---- TOKEN LOAD (APP AÇILINCA) ---- */
  useEffect(() => {
    const loadToken = async () => {
      const token = await SecureStore.getItemAsync("token");
      if (token) {
        setAccessToken(token);
      }
    };
    loadToken();
  }, []);

  /* ---- LOGIN ---- */
  const { mutate: loginMutation, isPending: isLoginPending } = useLogin({
    onSuccess: async (res) => {
      const token = res.data.accessToken;

      // token state + storage
      setAccessToken(token);
      await SecureStore.setItemAsync("token", token);

      // 🔥 kullanıcı bilgisini yeniden çektir
      await queryClient.invalidateQueries({ queryKey: ["me"] });

      router.replace("/home");
    },
  });

  const login = (email: string, password: string) => {
    loginMutation({ email, password });
  };

  /* ---- LOGOUT ---- */
  const clearCredentials = async () => {
    setAccessToken(null);
    await SecureStore.deleteItemAsync("token");

    // tüm cache temizlenir
    queryClient.clear();

    router.replace("/login");
  };

  const { mutate: logoutMutation } = useLogout({
    onSuccess: clearCredentials,
    onError: clearCredentials,
  });

  const logout = () => logoutMutation({});

  /* ---- GET ME ---- */
  const { data } = useGetMe({
    enabled: !!accessToken,
  });

  /* ---- REFRESH USER (PUAN, ROZET, SEVİYE) ---- */
  const refreshUser = async () => {
    await queryClient.invalidateQueries({
      queryKey: ["me"],
    });
  };

  const user = data?.data ?? null;
  const isAuthenticated = !!accessToken && !!user;

  const value: AuthContextType = {
    user,
    accessToken,
    isAuthenticated,
    isLoginPending,
    login,
    logout,
    refreshUser,
  };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
};

/* ================= HOOK ================= */

export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within AuthProvider");
  }
  return context;
};
