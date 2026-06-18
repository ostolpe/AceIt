import { useAuth } from "./useAuth";

export const useApi = () => {
  const { token } = useAuth();
  const baseUrl = import.meta.env.VITE_API_BASE_URL;

  const apiFetch = async (path: string, options?: RequestInit) => {
    const res = await fetch(`${baseUrl}${path}`, {
      ...options,
      headers: {
        "Content-Type": "application/json",
        ...(token ? { Authorization: `Bearer ${token}` } : {}),
        ...options?.headers,
      },
    });
    if (!res.ok) {
      throw new Error(`API error: ${res.status}`);
    }

    return res.json();
  };

  return { apiFetch };
};
