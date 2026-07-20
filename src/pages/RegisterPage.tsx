import { useState } from "react";

import { register } from "../services/accountService";


interface Props
{
    onBack(): void;
}


export default function RegisterPage(
    {
        onBack
    }: Props
)
{
    const [email, setEmail] =
        useState("");

    const [password, setPassword] =
        useState("");

    const [message, setMessage] =
        useState<string | null>(null);


    async function handleRegister()
    {
        try
        {
            await register(
                email,
                password
            );

            setMessage(
                "Conta criada com sucesso."
            );
        }
        catch
        {
            setMessage(
                "Falha ao criar conta."
            );
        }
    }


    return (
        <main>

            <h1>
                Project Victory
            </h1>

            <h2>
                Cadastro
            </h2>


            <input
                placeholder="Email"
                value={email}
                onChange={
                    event =>
                        setEmail(
                            event.target.value
                        )
                }
            />


            <input
                placeholder="Senha"
                type="password"
                value={password}
                onChange={
                    event =>
                        setPassword(
                            event.target.value
                        )
                }
            />


            <button
                onClick={handleRegister}
            >
                Criar conta
            </button>


            <button
                onClick={onBack}
            >
                Voltar
            </button>


            {
                message &&
                (
                    <p>
                        {message}
                    </p>
                )
            }

        </main>
    );
}