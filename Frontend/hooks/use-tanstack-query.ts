import api from "@/axios";
import { useMutation, useQuery, UseQueryOptions, type UseMutationOptions } from "@tanstack/react-query";

export const useLogin = (
    options?: Omit<
        UseMutationOptions<any, Error, any, unknown>,
        'mutationFn'
    >
) => {
    return useMutation({
        mutationFn: async (credentials) => {
            const response = await api.post("/api/Auth/login", credentials);
            return response.data;
        },
        ...options
    })
}


export const useRegister = (
    options?: Omit<
        UseMutationOptions<any, Error, any, unknown>,
        'mutationFn'
    >
) => {
    return useMutation({
        mutationFn: async (credentials) => {
            const response = await api.post("/api/Auth/register", credentials);
            return response;
        },
        ...options
    })
}


export const useLogout = (
    options?: Omit<
        UseMutationOptions<any, Error, any, unknown>,
        'mutationFn'
    >
) => {
    return useMutation({
        mutationFn: async (credentials) => {
            const response = await api.post("/api/Auth/logout", credentials);
            return response;
        },
        ...options
    })
}

export const useGetMe = (options?: Omit<UseQueryOptions<any, Error, any, ["me"]>, 'queryKey' | 'queryFn'>) => {
    return useQuery({
        queryKey: ["me"],
        queryFn: async () =>  {
            const response = await api.get("/api/User/me");
            return response.data;
        },
        ...options
    })
}

export const useAddHabit = (
    options?: Omit<
        UseMutationOptions<any, Error, any, unknown>,
        'mutationFn'
    >
) => {
    return useMutation({
        mutationFn: async (credentials) => {
            const response = await api.post("/api/Habits/add", credentials);
            return response.data;
        },
        ...options
    })
}
export const useAskAi = (
  options?: Omit<
    UseMutationOptions<string, Error, string, unknown>,
    "mutationFn"
  >
) => {
  return useMutation({
    mutationFn: async (message: string) => {
      const response = await api.post(
        "/api/chatbot/ask",
        JSON.stringify(message), 
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );

      return response.data;
    },
    ...options,
  });
};


export const useTickHabit = (
  options?: Omit<
    UseMutationOptions<any, Error, number, unknown>,
    "mutationFn"
  >
) => {
  return useMutation({
    mutationFn: async (habitId: number) => {
      const response = await api.post(`/api/Habits/tick/${habitId}`);
      return response.data;
    },
    ...options,
  });
};


export const useCalendarStats = (
  start: string,
  end: string
) => {
  return useQuery({
    queryKey: ["calendar-stats", start, end],
    queryFn: async () => {
      const res = await api.get(
        `/api/Statistics/calendar?start=${start}&end=${end}`
      );
      return res.data.data;
    },
  });
};

export const useWeeklyStats = () => {
  return useQuery({
    queryKey: ["weekly-stats"],
    queryFn: async () => {
      const res = await api.get("/api/Statistics/weekly");
      return res.data.data;
    },
  });
};


export const useHabitWeeklyStats = () => {
  return useQuery({
    queryKey: ["habit-weekly-stats"],
    queryFn: async () => {
      const res = await api.get("/api/Statistics/habits/weekly");
      return res.data.data;
    },
  });
};


export const usePrizes = () => {
  return useQuery({
    queryKey: ["prizes"],
    queryFn: async () => {
      const res = await api.get("/api/Prize/getall");
      return res.data.data;
    },
  });
};

export const useDeleteHabit = (
  options?: Omit<
    UseMutationOptions<any, Error, number, unknown>,
    "mutationFn"
  >
) => {
  return useMutation({
    mutationFn: async (habitId: number) => {
      const response = await api.delete(`/api/Habits/delete/${habitId}`);
      return response.data;
    },
    ...options,
  });
};
