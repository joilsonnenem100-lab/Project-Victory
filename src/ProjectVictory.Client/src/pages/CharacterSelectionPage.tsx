import { useEffect, useState } from "react";

import type { Character } from "../models/character";

import {
    getCharacters,
    createCharacter
} from "../services/characterService";

import { enterGame } from "../services/gameService";

import GamePage from "./GamePage";



interface Props
{
}



export default function CharacterSelectionPage(
    {
    }: Props
)
{
    const [characters, setCharacters] =
        useState<Character[]>([]);


    const [characterName, setCharacterName] =
        useState("");


    const [message, setMessage] =
        useState("");


    const [gameState, setGameState] =
        useState(null);



    async function loadCharacters()
    {
        const result =
            await getCharacters();


        setCharacters(
            result
        );
    }



    useEffect(
        () =>
        {
            loadCharacters();

        },
        []
    );



    async function handleCreateCharacter()
    {
        try
        {
            setMessage("");


            await createCharacter(
                characterName
            );


            setCharacterName("");


            await loadCharacters();


            setMessage(
                "Character criado com sucesso."
            );
        }
        catch(error)
        {
            setMessage(
                error instanceof Error
                    ? error.message
                    : "Falha ao criar Character."
            );
        }
    }



    async function selectCharacter()
    {
        const result =
            await enterGame();


        setGameState(
            result
        );
    }



    if (gameState)
    {
        return (
            <GamePage
                gameState={gameState}
            />
        );
    }



    return (
        <main>

            <h1>
                Select Character
            </h1>


            {
                characters.length === 0 &&
                (
                    <section>

                        <p>
                            Nenhum personagem encontrado.
                        </p>


                        <input
                            type="text"
                            placeholder="Nome do Character"
                            value={characterName}
                            onChange={
                                event =>
                                    setCharacterName(
                                        event.target.value
                                    )
                            }
                        />


                        <button
                            onClick={
                                handleCreateCharacter
                            }
                            disabled={
                                !characterName.trim()
                            }
                        >
                            Criar Character
                        </button>

                    </section>
                )
            }


            {
                characters.map(
                    character =>
                    (
                        <button
                            key={character.id}
                            onClick={selectCharacter}
                        >
                            {character.name}
                            {" "}
                            Lv.
                            {character.level}
                        </button>
                    )
                )
            }


            {
                message &&
                (
                    <p>
                        {message}
                    </p>
                )
            }

        </main>
    );
}