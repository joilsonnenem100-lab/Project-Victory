import { apiRequest } from "../api/client";


export interface Character
{
    id: string;
    name: string;
    level?: number;
    experience?: number;
    // PV-CLIENT-020 (DEC-20261106-30): título e tier vêm do servidor, derivados do Level; o cliente só exibe.
    title?: string;
    titleTier?: number;
}


export interface InventoryResponse
{
    success: boolean;
    items: string[];
}


export async function getCharacters()
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
            "Failed to load characters"
        );
    }


    return await response.json() as Character[];
}


export async function createCharacter(
    name: string
)
{
    const response =
        await apiRequest(
            "/api/character",
            {
                method: "POST",
                headers:
                {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(
                {
                    name
                })
            }
        );


    if (!response.ok)
    {
        throw new Error(
            "Failed to create character"
        );
    }


    return await response.json();
}


export async function getInventory(
    characterId: string
)
{
    const response =
        await apiRequest(
            `/api/inventory/${characterId}`,
            {
                method: "GET"
            }
        );


    if (!response.ok)
    {
        throw new Error(
            "Failed to load inventory"
        );
    }


    return await response.json() as InventoryResponse;
}