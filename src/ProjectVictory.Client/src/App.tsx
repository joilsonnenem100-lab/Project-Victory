import { useState } from "react";

import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import CharacterSelectionPage from "./pages/CharacterSelectionPage";
import GamePage from "./pages/GamePage";

import {
    getCharacters,
    createCharacter
}
from "./services/characterService";

import type { GameState } from "./models/game";
import type { Character } from "./models/character";



export default function App()
{
    const [authenticated, setAuthenticated] =
        useState(false);


    const [registering, setRegistering] =
        useState(false);


    const [characters, setCharacters] =
        useState<Character[]>([]);


    const [gameState, setGameState] =
        useState<GameState | null>(null);



    async function loadCharacters()
    {
        const result =
            await getCharacters();


        setCharacters(
            result
        );
    }



    async function handleCreateCharacter(
        name: string,
        characterClass: string
    )
    {
        await createCharacter(
            name,
            characterClass
        );


        await loadCharacters();
    }



    function enterWorld(
        character: Character
    )
    {
        console.log(
            "ENTER WORLD CHARACTER:",
            character
        );


        setGameState(
            {
                accountId:
                    character.accountId,

                characterId:
                    character.characterId,

                level:
                    character.level,

                experience:
                    character.experience,

                characterName:
                    character.name
            }
        );
    }



    function updateCharacterProgression(
        experience: number,
        level: number
    )
    {
        setGameState(
            current =>
            {
                if (!current)
                {
                    return current;
                }


                return {
                    ...current,

                    experience,

                    level
                };
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
            <LoginPage
                onLoginSuccess={
                    async () =>
                    {
                        setAuthenticated(true);

                        await loadCharacters();
                    }
                }

                onRegister={
                    () =>
                    {
                        setRegistering(true);
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

                onCharacterProgressionUpdate={
                    updateCharacterProgression
                }
            />
        );
    }



    return (
        <CharacterSelectionPage

            characters={
                characters
            }

            onCharacterCreated={
                handleCreateCharacter
            }

            onEnterWorld={
                enterWorld
            }

        />
    );
}