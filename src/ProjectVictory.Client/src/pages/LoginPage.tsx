// src/ProjectVictory.Client/src/pages/LoginPage.tsx

import { useState } from "react";
import { login } from "../services/accountService";

interface Props
{
    onLoginSuccess(accountId: string): void;
    onRegister?: () => void;
    onForgotPassword?: () => void;
}

export default function LoginPage({ onLoginSuccess, onRegister, onForgotPassword }: Props)
{
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [rememberMe, setRememberMe] = useState(false);
    const [showPassword, setShowPassword] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");

    async function handleLogin(e: React.FormEvent)
    {
        e.preventDefault();
        try
        {
            setErrorMessage("");
            const result = await login(email, password);
            onLoginSuccess(result.accountId);
        }
        catch
        {
            setErrorMessage("Login failed.");
        }
    }

    return (
        <main
            style={{
                position: "fixed",
                inset: 0,
                width: "100vw",
                height: "100vh",
                backgroundColor: "#0d0f12",
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
                fontFamily: "Arial, sans-serif",
                color: "#e2e8f0"
            }}
        >
            <div
                style={{
                    width: "420px",
                    padding: "40px",
                    backgroundColor: "#161b22",
                    border: "1px solid #30363d",
                    borderRadius: "12px",
                    boxShadow: "0 8px 24px rgba(0, 0, 0, 0.5)",
                    display: "flex",
                    flexDirection: "column",
                    gap: "20px"
                }}
            >
                <div style={{ textAlign: "center" }}>
                    <h1 style={{ fontSize: "24px", margin: "0 0 8px 0", color: "#f0b429" }}>Project Victory</h1>
                    <p style={{ fontSize: "14px", color: "#8b949e", margin: 0 }}>Entre na sua conta para continuar</p>
                </div>

                <form onSubmit={handleLogin} style={{ display: "flex", flexDirection: "column", gap: "16px" }}>
                    <div style={{ display: "flex", flexDirection: "column", gap: "6px" }}>
                        <label style={{ fontSize: "13px", fontWeight: "bold" }}>E-mail</label>
                        <input
                            type="email"
                            required
                            autoComplete="off"
                            value={email}
                            onChange={e => setEmail(e.target.value)}
                            placeholder="seu@email.com"
                            style={{
                                padding: "12px 14px",
                                backgroundColor: "#0d0f12",
                                border: "1px solid #30363d",
                                borderRadius: "6px",
                                color: "#fff",
                                fontSize: "14px",
                                outline: "none"
                            }}
                        />
                    </div>

                    <div style={{ display: "flex", flexDirection: "column", gap: "6px" }}>
                        <label style={{ fontSize: "13px", fontWeight: "bold" }}>Senha</label>
                        <div style={{ position: "relative", display: "flex", alignItems: "center" }}>
                            <input
                                type={showPassword ? "text" : "password"}
                                required
                                autoComplete="current-password"
                                value={password}
                                onChange={e => setPassword(e.target.value)}
                                placeholder="••••••••"
                                style={{
                                    width: "100%",
                                    padding: "12px 40px 12px 14px",
                                    backgroundColor: "#0d0f12",
                                    border: "1px solid #30363d",
                                    borderRadius: "6px",
                                    color: "#fff",
                                    fontSize: "14px",
                                    outline: "none"
                                }}
                            />
                            <button
                                type="button"
                                onClick={() => setShowPassword(!showPassword)}
                                style={{
                                    position: "absolute",
                                    right: "12px",
                                    background: "none",
                                    border: "none",
                                    color: "#8b949e",
                                    cursor: "pointer",
                                    fontSize: "12px"
                                }}
                            >
                                {showPassword ? "Ocultar" : "Mostrar"}
                            </button>
                        </div>
                    </div>

                    <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", fontSize: "13px" }}>
                        <label style={{ display: "flex", alignItems: "center", gap: "6px", cursor: "pointer" }}>
                            <input
                                type="checkbox"
                                checked={rememberMe}
                                onChange={e => setRememberMe(e.target.checked)}
                                style={{ cursor: "pointer" }}
                            />
                            Lembrar de mim
                        </label>
                        <button
                            type="button"
                            onClick={onForgotPassword}
                            style={{ background: "none", border: "none", color: "#58a6ff", cursor: "pointer", padding: 0, fontSize: "13px" }}
                        >
                            Esqueceu a senha?
                        </button>
                    </div>

                    {errorMessage && (
                        <div style={{ color: "#f85149", fontSize: "13px", textAlign: "center" }}>
                            {errorMessage}
                        </div>
                    )}

                    <button
                        type="submit"
                        style={{
                            marginTop: "8px",
                            padding: "12px",
                            backgroundColor: "#238636",
                            color: "#fff",
                            border: "none",
                            borderRadius: "6px",
                            fontSize: "15px",
                            fontWeight: "bold",
                            cursor: "pointer"
                        }}
                    >
                        Entrar
                    </button>
                </form>

                <div style={{ textAlign: "center", fontSize: "13px", color: "#8b949e", borderTop: "1px solid #30363d", paddingTop: "16px" }}>
                    Não tem uma conta?{" "}
                    <button
                        type="button"
                        onClick={onRegister}
                        style={{ background: "none", border: "none", color: "#58a6ff", cursor: "pointer", fontWeight: "bold", padding: 0 }}
                    >
                        Cadastre-se
                    </button>
                </div>
            </div>
        </main>
    );
}