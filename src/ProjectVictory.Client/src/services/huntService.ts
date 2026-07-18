import { apiRequest } from "../api/client";

import type { HuntResult } from "../models/hunt";
import type { CombatResult } from "../models/combat";
import type { RewardResult } from "../models/reward";


export async function startHunt()
: Promise<HuntResult>
{
    const response =
        await apiRequest(
            "/api/hunt/execute",
            {
                method: "POST",

                body: JSON.stringify(
                    {
                        characterId:
                            undefined
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


    return await response.json();
}


export async function getCombatResult()
: Promise<CombatResult>
{
    const response =
        await apiRequest(
            "/api/combat/result",
            {
                method: "GET"
            }
        );


    if (!response.ok)
    {
        throw new Error(
            "Combat result unavailable"
        );
    }


    return await response.json();
}


export async function getRewardResult()
: Promise<RewardResult>
{
    const response =
        await apiRequest(
            "/api/reward/result",
            {
                method: "GET"
            }
        );


    if (!response.ok)
    {
        throw new Error(
            "Reward result unavailable"
        );
    }


    return await response.json();
}