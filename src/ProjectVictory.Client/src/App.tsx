import { useState } from "react";

import LoginPage from "./pages/LoginPage";

import CharacterSelectionPage from "./pages/CharacterSelectionPage";


export default function App()
{
    const [authenticated, setAuthenticated] =
        useState<boolean>(
            false
        );


    if (!authenticated)
    {
        return (
            <LoginPage
                onLoginSuccess={
                    () =>
                        setAuthenticated(true)
                }
            />
        );
    }


    return (
        <CharacterSelectionPage />
    );
}