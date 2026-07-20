import { api } from "../api/client";


export async function login(
    email: string,
    password: string
)
{
    const response =
        await api.post(
            "/api/account/login",
            {
                email,
                password
            }
        );

    return response.data;
}


export async function register(
    email: string,
    password: string
): Promise<void>
{
    await api.post(
        "/api/account",
        {
            email,
            password
        }
    );
}