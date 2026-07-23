// src/ProjectVictory.Client/src/App.tsx

import { useState, useEffect } from "react";
import LoginPage from "./pages/LoginPage";
import CharacterSelectionPage from "./pages/CharacterSelectionPage";
import EntryExperiencePage from "./pages/EntryExperiencePage";
import HuntPage from "./pages/HuntPage";
import GamePage from "./pages/GamePage";

interface CharacterInfo
{
    id?: string;
    name?: string;
    level?: number;
    experience?: number;
    characterClass?: string;
}

export default function App()
{
    const [currentStep, setCurrentStep] = useState<"login" | "character-selection" | "entry" | "hunt" | "game">("login");
    const [characters, setCharacters] = useState<CharacterInfo[]>([]);
    const [selectedCharacter, setSelectedCharacter] = useState<CharacterInfo | undefined>(undefined);

    useEffect(() =>
    {
        if (currentStep === "character-selection")
        {
            async function loadCharacters()
            {
                try
                {
                    const response = await fetch('/api/characters');
                    if (response.ok)
                    {
                        const data = await response.json();
                        setCharacters(data);
                        if (data && data.length > 0)
                        {
                            setSelectedCharacter(data[0]);
                        }
                    }
                }
                catch (error)
                {
                    console.error("Erro ao carregar personagens do banco de dados:", error);
                }
            }
            loadCharacters();
        }
    }, [currentStep]);

    return (
        <>
            {currentStep === "login" && (
                <LoginPage
                    onLoginSuccess={() => setCurrentStep("character-selection")}
                    onRegister={() => {}}
                    onForgotPassword={() => {}}
                />
            )}

            {currentStep === "character-selection" && (
                <CharacterSelectionPage
                    characters={characters}
                    onCharacterCreated={async (name, characterClass) =>
                    {
                        const newChar: CharacterInfo = {
                            id: "char_" + Date.now(),
                            name,
                            level: 1,
                            experience: 0,
                            characterClass
                        };
                        setCharacters(prev => [...prev, newChar]);
                        setSelectedCharacter(newChar);
                    }}
                    onSelectCharacter={(char) => setSelectedCharacter(char)}
                    onEnterWorld={() =>
                    {
                        if (selectedCharacter || characters.length > 0)
                        {
                            setCurrentStep("entry");
                        }
                    }}
                />
            )}

            {currentStep === "entry" && (
                <EntryExperiencePage
                    character={selectedCharacter || characters[0]}
                    onEnterWorld={() => setCurrentStep("hunt")}
                />
            )}

            {currentStep === "hunt" && (
                <HuntPage
                    character={selectedCharacter || characters[0]}
                    onBackToTown={() => setCurrentStep("game")}
                    onStartHunt={() => setCurrentStep("game")}
                />
            )}

            {currentStep === "game" && (
                <GamePage />
            )}
        </>
    );
}