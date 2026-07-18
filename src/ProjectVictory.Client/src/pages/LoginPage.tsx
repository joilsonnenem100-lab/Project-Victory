import { useState } from "react";

import { login } from "../services/accountService";


interface Props
{
    onLoginSuccess(accountId: string): void;
}



export default function LoginPage(
    {
        onLoginSuccess
    }: Props
)
{
    const [email, setEmail] =
        useState("");

    const [password, setPassword] =
        useState("");

    const [error, setError] =
        useState<string | null>(null);



    async function handleLogin()
    {
        try
        {
            setError(null);


            const result =
                await login(
                    email,
                    password
                );


            onLoginSuccess(
                result.accountId
            );
        }
        catch
        {
            setError(
                "Falha no login"
            );
        }
    }



    return (
        <main>

            <h1>
                Project Victory
            </h1>


            <h2>
                Login
            </h2>


            <input
                placeholder="Email"
                value={email}
                onChange={
                    e => setEmail(
                        e.target.value
                    )
                }
            />


            <input
                placeholder="Password"
                type="password"
                value={password}
                onChange={
                    e => setPassword(
                        e.target.value
                    )
                }
            />


            <button
                onClick={handleLogin}
            >
                Entrar
            </button>


            {
                error &&
                (
                    <p>
                        {error}
                    </p>
                )
            }

        </main>
    );
}