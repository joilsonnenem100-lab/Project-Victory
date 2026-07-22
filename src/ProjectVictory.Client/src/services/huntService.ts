import { apiRequest } from "../api/client";

import type { HuntResult } from "../models/hunt";



export interface AttackEncounterResult
{
    encounterId: string;

    monsterName: string;

    currentHealth: number;

    maximumHealth: number;

    monsterDefeated: boolean;

    experience: number;

    gold: number;

    level: number;
}



export async function startHunt()
: Promise<HuntResult>
{
    const response =
        await apiRequest(
            "/api/hunt/execute",
            {
                method: "POST",

                body: JSON.stringify({})
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

                body: JSON.stringify(
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