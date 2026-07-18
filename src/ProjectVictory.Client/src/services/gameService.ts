import type { GameState } from "../models/game";

import { apiRequest } from "../api/client";


export async function enterGame()
: Promise<GameState>
{
    const response =
        await apiRequest(
            "/api/game/enter",
            {
                method: "POST"
            }
        );


    if (!response.ok)
    {
        throw new Error(
            "Game entry failed"
        );
    }


    return await response.json();
}