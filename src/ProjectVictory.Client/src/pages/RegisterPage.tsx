import { FormEvent, useState } from "react";
import { register } from "../services/accountService";

interface RegisterPageProps
{
    onBack: () => void;
}

export default function RegisterPage(
    {
        onBack
    }: RegisterPageProps
)
{
    const [email, setEmail] =
        useState("");

    const [password, setPassword] =
        useState("");

    const [loading, setLoading] =
        useState(false);

    const [successMessage, setSuccessMessage] =
        useState("");

    const [errorMessage, setErrorMessage] =
        useState("");

    async function handleSubmit(
        event: FormEvent<HTMLFormElement>
    )
    {
        event.preventDefault();

        setLoading(true);
        setSuccessMessage("");
        setErrorMessage("");

        try
        {
            await register(
                email,
                password
            );

            setSuccessMessage(
                "Conta criada com sucesso."
            );

            setEmail("");
            setPassword("");
        }
        catch (error)
        {
            if (error instanceof Error)
            {
                setErrorMessage(
                    error.message
                );
            }
            else
            {
                setErrorMessage(
                    "Não foi possível criar a conta."
                );
            }
        }
        finally
        {
            setLoading(false);
        }
    }

    return (
        <main
            style={{
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
                minHeight: "100vh"
            }}
        >
            <form
                onSubmit={handleSubmit}
                style={{
                    width: 360,
                    display: "flex",
                    flexDirection: "column",
                    gap: 16
                }}
            >
                <h1>
                    Criar Conta
                </h1>

                <input
                    type="email"
                    placeholder="E-mail"
                    value={email}
                    onChange={
                        (event) =>
                            setEmail(
                                event.target.value
                            )
                    }
                    required
                />

                <input
                    type="password"
                    placeholder="Senha"
                    value={password}
                    onChange={
                        (event) =>
                            setPassword(
                                event.target.value
                            )
                    }
                    required
                />

                <button
                    type="submit"
                    disabled={loading}
                >
                    {
                        loading
                            ? "Criando..."
                            : "Criar Conta"
                    }
                </button>

                <button
                    type="button"
                    onClick={onBack}
                    disabled={loading}
                >
                    Voltar para Login
                </button>

                {
                    successMessage &&
                    (
                        <p
                            style={{
                                color: "green"
                            }}
                        >
                            {successMessage}
                        </p>
                    )
                }

                {
                    errorMessage &&
                    (
                        <p
                            style={{
                                color: "red"
                            }}
                        >
                            {errorMessage}
                        </p>
                    )
                }
            </form>
        </main>
    );
}