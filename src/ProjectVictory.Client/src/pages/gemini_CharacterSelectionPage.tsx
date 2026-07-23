// src/ProjectVictory.Client/src/pages/CharacterSelectionPage.tsx

import { useState } from "react";
import CharacterCreationPage from "./CharacterCreationPage";

interface CharacterInfo
{
    id?: string;
    name?: string;
    level?: number;
    experience?: number;
    characterClass?: string;
}

interface Props
{
    characters: CharacterInfo[];
    onCharacterCreated(name: string, characterClass: string): Promise<void>;
    onSelectCharacter: (char: CharacterInfo) => void;
    onEnterWorld: () => void;
}

export default function CharacterSelectionPage({
    characters,
    onCharacterCreated,
    onSelectCharacter,
    onEnterWorld
}: Props)
{
    const [creatingCharacter, setCreatingCharacter] = useState(false);
    const [selectedId, setSelectedId] = useState<string | undefined>(characters[0]?.id);

    async function handleCreateCharacter(name: string, characterClass: string)
    {
        await onCharacterCreated(name, characterClass);
        setCreatingCharacter(false);
    }

    if (creatingCharacter)
    {
        return (
            <CharacterCreationPage
                onCreate={handleCreateCharacter}
                onBack={async () => setCreatingCharacter(false)}
            />
        );
    }

    return (
        <main
            style={{
                position: "fixed",
                inset: 0,
                width: "100vw",
                height: "100vh",
                backgroundColor: "#090a0f",
                backgroundImage: "radial-gradient(circle at center, #1a1f2c 0%, #090a0f 100%)",
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
                fontFamily: "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif",
                color: "#e2e8f0",
                overflow: "hidden"
            }}
        >
            <div
                style={{
                    width: "100%",
                    maxWidth: "540px",
                    margin: "0 20px",
                    padding: "48px",
                    backgroundColor: "#131722",
                    border: "2px solid #30363d",
                    borderRadius: "16px",
                    boxShadow: "0 16px 40px rgba(0, 0, 0, 0.7)",
                    display: "flex",
                    flexDirection: "column",
                    gap: "28px",
                    textAlign: "center"
                }}
            >
                <div>
                    <h1 style={{ fontSize: "32px", margin: "0 0 10px 0", color: "#f0b429", letterSpacing: "1.5px", textTransform: "uppercase" }}>
                        Character Selection
                    </h1>
                    <p style={{ fontSize: "15px", color: "#8b949e", margin: 0 }}>
                        Selecione seu herói para entrar no mundo.
                    </p>
                </div>

                <div
                    style={{
                        backgroundColor: "#090a0f",
                        border: "1px solid #30363d",
                        borderRadius: "10px",
                        padding: "16px",
                        display: "flex",
                        flexDirection: "column",
                        gap: "10px",
                        maxHeight: "220px",
                        overflowY: "auto",
                        textAlign: "left"
                    }}
                >
                    <strong style={{ fontSize: "14px", color: "#f0b429", borderBottom: "1px solid #30363d", paddingBottom: "8px", letterSpacing: "0.5px" }}>
                        Seus Personagens
                    </strong>
                    {characters && characters.length > 0 ? (
                        characters.map((char) =>
                        {
                            const isSelected = selectedId === char.id;
                            return (
                                <div
                                    key={char.id}
                                    onClick={() =>
                                    {
                                        setSelectedId(char.id);
                                        onSelectCharacter(char);
                                    }}
                                    style={{
                                        padding: "12px",
                                        borderRadius: "8px",
                                        backgroundColor: isSelected ? "#1f2937" : "#131722",
                                        border: isSelected ? "1px solid #f0b429" : "1px solid #30363d",
                                        cursor: "pointer",
                                        display: "flex",
                                        justifyContent: "space-between",
                                        alignItems: "center",
                                        transition: "all 0.2s"
                                    }}
                                >
                                    <div>
                                        <div style={{ fontWeight: "bold", color: "#fff", fontSize: "15px" }}>{char.name}</div>
                                        <div style={{ fontSize: "12px", color: "#8b949e" }}>{char.characterClass || "Guerreiro"}</div>
                                    </div>
                                    <div style={{ fontSize: "13px", color: "#f0b429", fontWeight: "bold" }}>Level {char.level ?? 1}</div>
                                </div>
                            );
                        })
                    ) : (
                        <div style={{ textAlign: "center", color: "#8b949e", padding: "10px 0" }}>
                            Nenhum personagem encontrado. Crie um para continuar.
                        </div>
                    )}
                </div>

                <div style={{ display: "flex", flexDirection: "column", gap: "14px" }}>
                    <button
                        onClick={() => setCreatingCharacter(true)}
                        style={{
                            padding: "14px",
                            backgroundColor: "transparent",
                            color: "#58a6ff",
                            border: "1px solid #58a6ff",
                            borderRadius: "8px",
                            fontSize: "15px",
                            fontWeight: "bold",
                            cursor: "pointer",
                            transition: "all 0.2s"
                        }}
                    >
                        Create Character
                    </button>

                    {characters && characters.length > 0 && (
                        <button
                            onClick={onEnterWorld}
                            style={{
                                padding: "14px",
                                backgroundColor: "#238636",
                                color: "#fff",
                                border: "none",
                                borderRadius: "8px",
                                fontSize: "16px",
                                fontWeight: "bold",
                                cursor: "pointer",
                                boxShadow: "0 4px 12px rgba(35, 134, 54, 0.4)",
                                transition: "all 0.2s"
                            }}
                        >
                            Enter World
                        </button>
                    )}
                </div>
            </div>
        </main>
    );
}