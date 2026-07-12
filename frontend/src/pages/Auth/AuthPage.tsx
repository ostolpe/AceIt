import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useApi } from "../../hooks/useApi";
import { useAuth } from "../../hooks/useAuth";
import type { LoginResponse } from "../../types";
import Button from "../../components/Button";
import "./AuthPage.css";

const config = {
  login: {
    title: "Welcome back",
    subtitle: "Sign in to continue your sessions.",
    endpoint: "/api/auth/login",
    submitLabel: "Sign in",
    footer: {
      text: "Don't have an account?",
      linkTo: "/register",
      linkLabel: "Create one",
    },
  },
  register: {
    title: "Create account",
    subtitle: "Start acing your sessions today.",
    endpoint: "/api/auth/register",
    submitLabel: "Create account",
    footer: {
      text: "Already have an account?",
      linkTo: "/login",
      linkLabel: "Sign in",
    },
  },
} as const;

interface AuthPageProps {
  mode: "login" | "register";
}

const AuthPage = ({ mode }: AuthPageProps) => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const { login } = useAuth();
  const { apiFetch } = useApi();
  const navigate = useNavigate();
  const { title, subtitle, endpoint, submitLabel, footer } = config[mode];

  const handleSubmit = async (e: React.SyntheticEvent) => {
    e.preventDefault();
    setError("");
    try {
      const res = await apiFetch<LoginResponse>(endpoint, {
        method: "POST",
        body: JSON.stringify({ email, password }),
      });
      login(res.token);
      navigate("/profile");
    } catch {
      setError(
        mode === "login"
          ? "Invalid email or password."
          : "Could not create your account. Try a different email.",
      );
    }
  };

  return (
    <div className="auth-wrap">
      <div className="auth-card">
        <h1 className="auth-title">{title}</h1>
        <p className="auth-subtitle">{subtitle}</p>

        <form className="auth-form" onSubmit={handleSubmit}>
          <div className="auth-field">
            <label className="auth-label" htmlFor="email">
              Email
            </label>
            <input
              id="email"
              className="auth-input"
              type="email"
              value={email}
              required
              placeholder="you@example.com"
              onChange={(e) => setEmail(e.target.value)}
            />
          </div>

          <div className="auth-field">
            <label className="auth-label" htmlFor="password">
              Password
            </label>
            <input
              id="password"
              className="auth-input"
              type="password"
              value={password}
              required
              placeholder="••••••••"
              onChange={(e) => setPassword(e.target.value)}
            />
          </div>

          {error && <p className="auth-error">{error}</p>}

          <Button type="submit" className="auth-submit">
            {submitLabel}
          </Button>
        </form>

        <p className="auth-footer">
          {footer.text} <Link to={footer.linkTo}>{footer.linkLabel}</Link>
        </p>
      </div>
    </div>
  );
};

export default AuthPage;
