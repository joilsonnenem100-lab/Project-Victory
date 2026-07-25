import { apiRequest } from "../api/client";


export async function createRuntime(
    characterId: string
)
{
    const runtimeId =
        crypto.randomUUID();


    const response =
        await apiRequest(
            "/api/runtime/create",
            {
                method: "POST",

                headers:
                {
                    "Content-Type": "application/json"
                },

                body: JSON.stringify(
                {
                    runtimeId,

                    characterId
                })
            }
        );


    if (!response.ok)
    {
        throw new Error(
            "Failed to create runtime"
        );
    }


    return await response.json();
}