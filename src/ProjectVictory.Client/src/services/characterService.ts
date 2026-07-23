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


    const data = await response.json();

console.log(
    "CHARACTERS FROM API:",
    data
);

return data;
}



export async function createCharacter(
    name: string,
    characterClass: string
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
                        Name: name,

                        CharacterClass: characterClass
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