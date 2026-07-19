import { apiRequest } from "../api/client";

import type { HuntResult } from "../models/hunt";


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


    return await response.json();
}