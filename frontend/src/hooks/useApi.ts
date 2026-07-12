import { useCallback } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "./useAuth";

export const useApi = () => {
  const { token, logout } = useAuth();
  const navigate = useNavigate();
  const baseUrl = import.meta.env.VITE_API_BASE_URL;

  const apiFetch = useCallback(
    async <T>(path: string, options?: RequestInit): Promise<T> => {
      const res = await fetch(`${baseUrl}${path}`, {
        ...options,
        headers: {
          "Content-Type": "application/json",
          ...(token ? { Authorization: `Bearer ${token}` } : {}),
          ...options?.headers,
        },
      });

      if (res.status === 401 && token) {
        logout();
        navigate("/login");
        throw new Error("Session expired");
      }
      if (!res.ok) {
        throw new Error(`API error: ${res.status}`);
      }

      return res.json() as Promise<T>;
    },
    [token, logout, navigate, baseUrl],
  );

  return { apiFetch };
};
