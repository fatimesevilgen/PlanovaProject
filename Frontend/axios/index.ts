import axios from "axios"
import * as SecureStore from "expo-secure-store"

const baseURL = process.env.EXPO_PUBLIC_BASE_URL
const api = axios.create({baseURL})

api.interceptors.request.use(async(config) => {
    const token = await SecureStore.getItemAsync("token");
    if(token){
        config.headers.Authorization = `Bearer ${token}`
    }
    return config
})

export default api