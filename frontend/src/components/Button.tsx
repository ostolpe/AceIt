import type { ButtonHTMLAttributes } from "react";

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: "solid" | "ghost";
}

const Button = ({ variant = "solid", className = "", children, ...props }: ButtonProps) => (
  <button className={["btn", `btn-${variant}`, className].filter(Boolean).join(" ")} {...props}>
    {children}
  </button>
);

export default Button;
