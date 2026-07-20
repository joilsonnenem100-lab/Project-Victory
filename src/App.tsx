import { useState } from "react";

import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import CharacterSelectionPage from "./pages/CharacterSelectionPage";

export default function App()
{
    const [authenticated, setAuthenticated] =
        useState<boolean>(
            false
        );

    const [registering, setRegistering] =
        useState<boolean>(
            false
        );

    if (!authenticated)
    {
        if (registering)
        {
            return (
                <RegisterPage
                    onBack={
                        () =>
                            setRegistering(false)
                    }
                />
            );
        }

        return (
            <main>

                <LoginPage
                    onLoginSuccess={
                        () =>
                            setAuthenticated(true)
                    }
                />

                <button
                    onClick={
                        () =>
                            setRegistering(true)
                    }
                >
                    Criar conta
                </button>

            </main>
        );
    }

    return (
        <CharacterSelectionPage />
    );
}