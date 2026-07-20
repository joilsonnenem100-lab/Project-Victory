import { useState } from "react";

import { login } from "../services/accountService";

import PageContainer from "../components/layout/PageContainer";
import Panel from "../components/layout/Panel";
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

            <Panel>

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

            </Panel>

        </PageContainer>
    );
}