import { useState } from "react";

import Container from "../components/ui/Container";
import Card from "../components/ui/Card";
import Button from "../components/ui/Button";
import FeedbackMessage from "../components/ui/FeedbackMessage";



interface Props
{
    onCreate(
        name: string
    ): Promise<void>;

    onBack(): void;
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
        catch
        {
            setMessage(
                "Unable to create character."
            );
        }
    }



    return (
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
                                    "center",

                                fontSize:
                                    "var(--pv-font-size-title)"
                            }}
                        >
                            Character Creation
                        </h1>


                        <p
                            className="pv-content-decoration"
                            style={{
                                textAlign:
                                    "center"
                            }}
                        >
                            Create your identity in PROJECT-VICTORY.
                        </p>


                        <section
                            className="pv-panel-decoration"
                            style={{
                                display:
                                    "flex",

                                flexDirection:
                                    "column",

                                gap:
                                    "var(--pv-spacing-sm)",

                                padding:
                                    "var(--pv-spacing-md)"
                            }}
                        >

                            <strong>
                                Character Identity
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


                        <input
                            value={
                                name
                            }

                            placeholder="Character Name"

                            onChange={
                                event =>
                                    setName(
                                        event.target.value
                                    )
                            }

                            style={{
                                width:
                                    "100%",

                                boxSizing:
                                    "border-box",

                                padding:
                                    "var(--pv-spacing-sm)"
                            }}
                        />


                        <Button
                            onClick={
                                handleCreate
                            }
                        >
                            Create Character
                        </Button>


                        <Button
                            onClick={
                                onBack
                            }
                        >
                            Back
                        </Button>


                        {
                            message &&
                            (
                                <FeedbackMessage
                                    message={
                                        message
                                    }

                                    type={
                                        message.includes(
                                            "successfully"
                                        )
                                            ? "success"
                                            : "error"
                                    }
                                />
                            )
                        }

                    </Card>

                </div>

            </section>

        </Container>
    );
}