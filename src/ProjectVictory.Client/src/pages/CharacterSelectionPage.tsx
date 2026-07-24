import { useEffect, useState } from "react";

import {
    getCharacters,
    createCharacter
} from "../services/characterService";

import type { Character } from "../models/character";


interface Props
{
    onCharacterSelected: (
        character: Character
    ) => void;
}


export default function CharacterSelectionPage(
{
    onCharacterSelected

}: Props)
{
    const [characters, setCharacters] =
        useState<Character[]>([]);

    const [name, setName] =
        useState("");


    async function loadCharacters()
    {
        const result =
            await getCharacters();

        setCharacters(result);
    }


    useEffect(() =>
    {
        loadCharacters();

    }, []);


    async function handleCreate()
    {
        const character =
            await createCharacter(
                name
            );

        setCharacters(
            previous =>
            [
                ...previous,
                character
            ]
        );

        onCharacterSelected(
            character
        );
    }


    return (
        <section>

            <h1>
                Character Selection
            </h1>


            {
                characters.map(
                    character =>
                    (
                        <button
                            key={
                                character.id
                            }
                            onClick={() =>
                                onCharacterSelected(
                                    character
                                )
                            }
                        >
                            {
                                character.name
                            }
                        </button>
                    )
                )
            }


            <div>

                <input
                    value={
                        name
                    }
                    onChange={
                        event =>
                            setName(
                                event.target.value
                            )
                    }
                    placeholder="Character name"
                />


                <button
                    onClick={
                        handleCreate
                    }
                >
                    Create Character
                </button>

            </div>

        </section>
    );
}