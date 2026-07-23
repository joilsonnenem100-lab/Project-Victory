import { apiRequest } from "../api/client";

import type { HuntResult } from "../models/hunt";

import type { RewardDrop } from "../models/reward";



export interface AttackEncounterResult
{
    encounterId: string;

    monsterName: string;

    currentHealth: number;

    maximumHealth: number;

    monsterDefeated: boolean;

    playerHealth: number;

    playerMaximumHealth: number;

    playerDefeated: boolean;

    combatFinished: boolean;

    combatState: string;

    experience: number;

    gold: number;

    level: number;

    drops: RewardDrop[];
}



export async function startHunt(
    characterId: string
)
: Promise<HuntResult>
{
    console.log(
        "START HUNT CHARACTER ID:",
        characterId
    );


    const response =
        await apiRequest(
            "/api/hunt/execute",
            {
                method: "POST",

                body:
                    JSON.stringify(
                    {
                        characterId
                    }
                )
            }
        );


    if (!response.ok)
    {
        throw new Error(
            "Hunt execution failed"
        );
    }


    const data =
        await response.json();


    return data.result ?? data;
}



export async function attackEncounter(
    encounterId: string
)
: Promise<AttackEncounterResult>
{
    const response =
        await apiRequest(
            "/api/hunt/attack",
            {
                method: "POST",

                body:
                    JSON.stringify(
                    {
                        encounterId
                    }
                )
            }
        );


    if (!response.ok)
    {
        throw new Error(
            "Encounter attack failed"
        );
    }


    const data =
        await response.json();


    return data.result ?? data;
}