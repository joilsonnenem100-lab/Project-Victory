import { useState } from "react";

import CharacterCreationPage from "./CharacterCreationPage";

import Container from "../components/ui/Container";
import Card from "../components/ui/Card";
import Button from "../components/ui/Button";

import CharacterSection from "../components/sections/CharacterSection";



interface Props
{
    onCharacterCreated(
        name: string,
        characterClass: string
    ): Promise<void>;

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
                                Character Identity
                            </h1>


                            <p
                                className="pv-content-decoration"
                                style={{
                                    textAlign:
                                        "center"
                                }}
                            >
                                Your journey begins with your character.
                            </p>


                            <section
                                className="pv-panel-decoration"
                            >

                                <strong>
                                    Character Profile
                                </strong>


                                <span>
                                    Name
                                </span>


                                <span>
                                    Character ID
                                </span>


                                <span>
                                    Level
                                </span>


                                <span>
                                    Experience
                                </span>

                            </section>


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
                                onClick={
                                    onEnterWorld
                                }
                            >
                                Enter World
                            </Button>

                        </Card>

                    </div>

                </section>

            </Container>

        </CharacterSection>
    );
}