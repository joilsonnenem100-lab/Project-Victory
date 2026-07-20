import { useState } from "react";

import CharacterCreationPage from "./CharacterCreationPage";

import {
    createCharacter
}
from "../services/characterService";


interface Props
{
    onCharacterCreated: () => Promise<void>;

    onEnterWorld: () => void;
}


export default function CharacterSelectionPage(
    {
        onCharacterCreated,
        onEnterWorld
    }: Props
)
{
    const [creatingCharacter, setCreatingCharacter] =
        useState(false);



    async function handleCreateCharacter(
        name: string
    )
    {
        await createCharacter(
            name
        );


        await onCharacterCreated();
    }



    if (creatingCharacter)
    {
        return (
            <CharacterCreationPage
                onCreate={
                    handleCreateCharacter
                }

                onBack={
                    async () =>
                    {
                        setCreatingCharacter(
                            false
                        );
                    }
                }
            />
        );
    }



    return (
        <main>

            <h1>
                Character
            </h1>


            <section>

                <button
                    onClick={
                        () =>
                        {
                            setCreatingCharacter(
                                true
                            );
                        }
                    }
                >
                    Create Character
                </button>


                <button
                    onClick={onEnterWorld}
                >
                    Enter World
                </button>

            </section>

        </main>
    );
}