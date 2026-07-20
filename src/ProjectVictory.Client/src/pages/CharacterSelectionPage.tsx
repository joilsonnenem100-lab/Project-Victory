import { useState } from "react";

import CharacterCreationPage from "./CharacterCreationPage";

import {
    createCharacter
}
from "../services/characterService";

import Container from "../components/ui/Container";
import Card from "../components/ui/Card";
import Button from "../components/ui/Button";


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
        <Container>

            <Card>

                <h1>
                    Character
                </h1>


                <Button
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
                </Button>


                <Button
                    onClick={onEnterWorld}
                >
                    Enter World
                </Button>

            </Card>

        </Container>
    );
}