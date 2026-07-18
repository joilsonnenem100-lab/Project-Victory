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