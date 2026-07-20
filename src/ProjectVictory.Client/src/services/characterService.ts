import type { Character } from "../models/character";

import { apiRequest } from "../api/client";



export async function getCharacters()
: Promise<Character[]>
{
    const response =
        await apiRequest(
            "/api/character",
            {
                method: "GET"
            }
        );


    if (!response.ok)
    {
        throw new Error(
            "Failed loading characters"
        );
    }


    return await response.json();
}



export async function createCharacter(
    name: string
)
: Promise<string>
{
    const response =
        await apiRequest(
            "/api/character",
            {
                method: "POST",

                body:
                    JSON.stringify(
                    {
                        name
                    })
            }
        );


    if (!response.ok)
    {
        const message =
            await response.text();


        throw new Error(
            message ||
            "Failed creating character"
        );
    }


    return await response.text();
}