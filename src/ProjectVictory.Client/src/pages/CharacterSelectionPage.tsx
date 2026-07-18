import { useEffect, useState } from "react";

import type { Character } from "../models/character";

import { getCharacters } from "../services/characterService";

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


    const [gameState, setGameState] =
        useState(null);



    useEffect(
        () =>
        {
            getCharacters()
                .then(setCharacters);

        },
        []
    );



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
                    <p>
                        Nenhum personagem encontrado.
                    </p>
                )
            }


            {
                characters.map(
                    character =>
                    (
                        <button
                            key={character.characterId}
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

        </main>
    );
}