import { useState } from "react";

import { login } from "../services/accountService";

import PageContainer from "../components/layout/PageContainer";
import Container from "../components/ui/Container";
import Card from "../components/ui/Card";
import Button from "../components/ui/Button";
import FeedbackMessage from "../components/ui/FeedbackMessage";
import SectionTitle from "../components/layout/SectionTitle";


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
        <PageContainer>

            <Container>

                <Card>

                    <h1>
                        Project Victory
                    </h1>


                    <SectionTitle>
                        Login
                    </SectionTitle>


                    <input
                        placeholder="Email"
                        value={email}
                        onChange={
                            e =>
                                setEmail(
                                    e.target.value
                                )
                        }
                    />


                    <input
                        placeholder="Password"
                        type="password"
                        value={password}
                        onChange={
                            e =>
                                setPassword(
                                    e.target.value
                                )
                        }
                    />


                    <Button
                        onClick={handleLogin}
                    >
                        Entrar
                    </Button>


                    {
                        error &&
                        (
                            <FeedbackMessage
                                message={error}
                                type="error"
                            />
                        )
                    }

                </Card>

            </Container>

        </PageContainer>
    );
}