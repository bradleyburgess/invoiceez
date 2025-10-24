// apiClient.ts
import axios from "axios";

export const getApiClient = () => {
  const apiClient = axios.create({
    baseURL: useRuntimeConfig().public.apiBaseUrl,
    withCredentials: true,
    validateStatus: (status) => status >= 200 && status < 500,
  });

  let refreshPromise: Promise<void> | null = null;

  apiClient.interceptors.request.use(
    async (config) => {
      const auth = useAuth();
      if (auth.isTokenExpired()) {
        if (!refreshPromise) {
          refreshPromise = auth.refreshToken().finally(() => {
            refreshPromise = null;
          });
        }
        await refreshPromise;
      }

      const token = auth.getAccessToken();
      if (token && config.headers) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      return config;
    },
    (error) => Promise.reject(error)
  );

  return apiClient;
};
