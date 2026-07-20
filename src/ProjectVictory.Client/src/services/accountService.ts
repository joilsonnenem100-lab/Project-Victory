import type { AccountLoginResult } from "../models/account";

import { setToken } from "../api/client";

const API_URL = "http://localhost:5250";

export async function register(
    email: string,
    password: string
): Promise<void>
{
    const response =
        await fetch(
            `${API_URL}/api/account`,
            {
                method: "POST",

                headers:
                {
                    "Content-Type": "application/json"
                },

                body:
                    JSON.stringify(
                    {
                        email,
                        password
                    })
            }
        );

    if (!response.ok)
    {
        let message =
            "Account creation failed";

        try
        {
            const result =
                await response.json();

            if (result?.message)
            {
                message =
                    result.message;
            }
        }
        catch
        {
            // mantém a mensagem padrão
        }

        throw new Error(message);
    }
}

export async function login(
    email: string,
    password: string
): Promise<AccountLoginResult>
{
    const response =
        await fetch(
            `${API_URL}/api/account/login`,
            {
                method: "POST",

                headers:
                {
                    "Content-Type": "application/json"
                },

                body:
                    JSON.stringify(
                    {
                        email,
                        password
                    })
            }
        );

    if (!response.ok)
    {
        throw new Error(
            "Account authentication failed"
        );
    }

    const result =
        await response.json();

    setToken(
        result.token
    );

    return {
        accountId:
            result.accountId,

        token:
            result.token
    };
}