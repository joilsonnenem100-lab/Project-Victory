import { useState } from "react";

import Container from "../components/ui/Container";
import Card from "../components/ui/Card";
import Button from "../components/ui/Button";
import FeedbackMessage from "../components/ui/FeedbackMessage";


interface Props
{
    onCreate: (
        name: string
    ) => Promise<void>;

    onBack: () => void;
}


export default function CharacterCreationPage(
    {
        onCreate,
        onBack
    }: Props
)
{
    const [name, setName] =
        useState("");


    const [message, setMessage] =
        useState("");



    async function handleCreate()
    {
        if (!name.trim())
        {
            setMessage(
                "Character name is required."
            );

            return;
        }


        try
        {
            await onCreate(
                name.trim()
            );


            setMessage(
                "Character created successfully."
            );
        }
        catch(error)
        {
            setMessage(
                error instanceof Error
                    ? error.message
                    : "Character creation failed."
            );
        }
    }



    return (
        <Container>

            <Card>

                <h1>
                    Create Character
                </h1>


                <input
                    type="text"
                    placeholder="Character Name"
                    value={name}
                    onChange={
                        event =>
                            setName(
                                event.target.value
                            )
                    }
                />


                <Button
                    onClick={handleCreate}
                    disabled={
                        !name.trim()
                    }
                >
                    Create Character
                </Button>


                <Button
                    onClick={onBack}
                >
                    Back
                </Button>


                {
                    message &&
                    (
                        <FeedbackMessage
                            message={message}
                            type={
                                message.includes("successfully")
                                    ? "success"
                                    : "error"
                            }
                        />
                    )
                }

            </Card>

        </Container>
    );
}