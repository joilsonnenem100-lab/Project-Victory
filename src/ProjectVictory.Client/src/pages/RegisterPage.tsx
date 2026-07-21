import { FormEvent, useState } from "react";

import { register } from "../services/accountService";

import Container from "../components/ui/Container";
import Card from "../components/ui/Card";
import Button from "../components/ui/Button";
import FeedbackMessage from "../components/ui/FeedbackMessage";


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
        <Container>

            <Card>

                <form
                    onSubmit={handleSubmit}
                    style={{
                        width:
                            "100%",


                        display:
                            "flex",


                        flexDirection:
                            "column",


                        gap:
                            "var(--pv-spacing-md)"
                    }}
                >

                    <h1
                        style={{
                            textAlign:
                                "center",

                            fontSize:
                                "var(--pv-font-size-title)",

                            fontWeight:
                                "var(--pv-font-weight-title)"
                        }}
                    >
                        Criar Conta
                    </h1>


                    <input
                        type="email"
                        placeholder="E-mail"
                        value={email}
                        onChange={
                            event =>
                                setEmail(
                                    event.target.value
                                )
                        }
                        required
                        style={{
                            width:
                                "100%",


                            boxSizing:
                                "border-box",


                            padding:
                                "var(--pv-spacing-sm)",


                            fontFamily:
                                "var(--pv-font-family-primary)"
                        }}
                    />


                    <input
                        type="password"
                        placeholder="Senha"
                        value={password}
                        onChange={
                            event =>
                                setPassword(
                                    event.target.value
                                )
                        }
                        required
                        style={{
                            width:
                                "100%",


                            boxSizing:
                                "border-box",


                            padding:
                                "var(--pv-spacing-sm)",


                            fontFamily:
                                "var(--pv-font-family-primary)"
                        }}
                    />


                    <Button
                        onClick={
                            () =>
                            {
                                return;
                            }
                        }
                    >
                        {
                            loading
                                ? "Criando..."
                                : "Criar Conta"
                        }
                    </Button>


                    <Button
                        onClick={onBack}
                    >
                        Voltar para Login
                    </Button>


                    {
                        successMessage &&
                        (
                            <FeedbackMessage
                                message={successMessage}
                                type="success"
                            />
                        )
                    }


                    {
                        errorMessage &&
                        (
                            <FeedbackMessage
                                message={errorMessage}
                                type="error"
                            />
                        )
                    }

                </form>

            </Card>

        </Container>
    );
}