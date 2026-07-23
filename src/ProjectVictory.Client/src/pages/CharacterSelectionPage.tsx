import { useState } from "react";

import CharacterCreationPage from "./CharacterCreationPage";

import type { Character } from "../models/character";

import Container from "../components/ui/Container";
import Card from "../components/ui/Card";
import Button from "../components/ui/Button";

import CharacterSection from "../components/sections/CharacterSection";



interface Props
{
    characters: Character[];


    onCharacterCreated(
        name: string,
        characterClass: string
    ): Promise<void>;


    onEnterWorld(
        character: Character
    ): void;
}



export default function CharacterSelectionPage(
{
    characters,

    onCharacterCreated,

    onEnterWorld

}: Props)
{
    const [creatingCharacter, setCreatingCharacter] =
        useState(false);



    const [selectedCharacter, setSelectedCharacter] =
        useState<Character | null>(null);



    async function handleCreateCharacter(
        name: string,
        characterClass: string
    )
    {
        await onCharacterCreated(
            name,
            characterClass
        );
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
                        setCreatingCharacter(false);
                    }
                }
            />
        );
    }



    return (
        <CharacterSection>

            <Container>

                <section
                    className="pv-background-layer"
                    style={{
                        minHeight:
                            "100vh"
                    }}
                >

                    <div
                        className="pv-background-content"
                    >

                        <Card>

                            <h1
                                className="pv-title-decoration"
                                style={{
                                    textAlign:
                                        "center"
                                }}
                            >
                                Character Selection
                            </h1>



                            <p
                                className="pv-content-decoration"
                                style={{
                                    textAlign:
                                        "center"
                                }}
                            >
                                Select your character.
                            </p>



                            <section
                                className="pv-panel-decoration"
                            >

                                {
                                    characters.length === 0 &&
                                    (
                                        <p>
                                            No characters found.
                                        </p>
                                    )
                                }



                                {
                                    characters.map(
                                        character =>
                                        (
                                            <button
                                                key={
                                                    character.characterId
                                                }

                                                onClick={
                                                    () =>
                                                    {
                                                        setSelectedCharacter(
                                                            character
                                                        );
                                                    }
                                                }
                                            >

                                                <strong>
                                                    {
                                                        character.name
                                                    }
                                                </strong>


                                                <br />


                                                Level:
                                                {" "}
                                                {
                                                    character.level
                                                }


                                                <br />


                                                Experience:
                                                {" "}
                                                {
                                                    character.experience
                                                }

                                            </button>
                                        )
                                    )
                                }

                            </section>



                            {
                                selectedCharacter &&
                                (
                                    <section
                                        className="pv-panel-decoration"
                                    >

                                        <h3>
                                            Selected Character
                                        </h3>


                                        <p>
                                            {
                                                selectedCharacter.name
                                            }
                                        </p>


                                        <Button
                                            onClick={
                                                () =>
                                                {
                                                    onEnterWorld(
                                                        selectedCharacter
                                                    );
                                                }
                                            }
                                        >
                                            Enter World
                                        </Button>

                                    </section>
                                )
                            }



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


                        </Card>

                    </div>

                </section>

            </Container>

        </CharacterSection>
    );
}