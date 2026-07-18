import type { AccountLoginResult } from "../models/account";

import { setToken } from "../api/client";


const API_URL = "http://localhost:5250";



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