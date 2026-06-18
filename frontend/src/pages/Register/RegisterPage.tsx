import React, { useState } from "react";
import { useApi } from "../../hooks/useApi";
import { useAuth } from "../../hooks/useAuth";
import type { LoginResponse } from "../../types";

const RegisterPage = () => {
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const { login } = useAuth();
  const { apiFetch } = useApi();

  const handleSubmit = async (e: React.SyntheticEvent) => {
    e.preventDefault();
    try {
      const res: LoginResponse = await apiFetch("/api/auth/register", {
        method: "POST",
        body: JSON.stringify({ email, password }),
      });
      login(res.token);
    } catch (err) {
      console.log(err);
    }
  };

  return (
    <div>
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          value={email}
          placeholder="example@example.com"
          onChange={(e) => {
            setEmail(e.target.value);
          }}
        />
        <input
          type="password"
          value={password}
          placeholder="yourpassword"
          onChange={(e) => {
            setPassword(e.target.value);
          }}
        />
        <button type="submit">Register</button>
      </form>
    </div>
  );
};

export default RegisterPage;
