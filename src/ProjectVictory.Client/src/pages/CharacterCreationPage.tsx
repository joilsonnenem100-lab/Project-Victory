import { useState } from "react";


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
        <main>

            <h1>
                Create Character
            </h1>


            <section>

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


                <button
                    onClick={handleCreate}
                    disabled={
                        !name.trim()
                    }
                >
                    Create Character
                </button>


                <button
                    onClick={onBack}
                >
                    Back
                </button>

            </section>


            {
                message &&
                <p>
                    {message}
                </p>
            }

        </main>
    );
}