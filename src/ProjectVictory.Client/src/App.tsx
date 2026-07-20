import { useState } from "react";

import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import CharacterSelectionPage from "./pages/CharacterSelectionPage";
import GamePage from "./pages/GamePage";
import EntryExperiencePage from "./pages/EntryExperiencePage";

import {
    getCharacters
}
from "./services/characterService";


import type { GameState } from "./models/game";


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


    const [entryExperience, setEntryExperience] =
        useState<boolean>(
            false
        );


    const [gameState, setGameState] =
        useState<GameState | null>(
            null
        );



    async function loadCharacter()
    {
        const characters =
            await getCharacters();


        if (characters.length === 0)
        {
            setGameState(
                null
            );

            return;
        }


        const character =
            characters[0];


        setGameState(
            {
                accountId:
                    character.accountId,

                characterId:
                    character.id,

                level:
                    character.level,

                experience:
                    character.experience,

                characterName:
                    character.name
            }
        );
    }



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
                        async () =>
                        {
                            setAuthenticated(true);

                            await loadCharacter();

                            setEntryExperience(true);
                        }
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



    if (entryExperience)
    {
        return (
            <EntryExperiencePage
                onContinue={
                    () =>
                    {
                        setEntryExperience(false);
                    }
                }
            />
        );
    }



    if (gameState)
    {
        return (
            <GamePage
                gameState={
                    gameState
                }
            />
        );
    }



    return (
        <CharacterSelectionPage

            onCharacterCreated={
                loadCharacter
            }

            onEnterWorld={
                loadCharacter
            }

        />
    );
}