const API_URL = "http://localhost:5250";


export function setToken(
    token: string
)
{
    localStorage.setItem(
        "pv_token",
        token
    );
}


export function getToken()
: string | null
{
    return localStorage.getItem(
        "pv_token"
    );
}


export async function apiRequest(
    url: string,
    options: RequestInit = {}
)
{
    const token =
        getToken();


    return fetch(
        `${API_URL}${url}`,
        {
            ...options,

            headers:
            {
                "Content-Type": "application/json",

                ...(token
                    ? {
                        Authorization:
                            `Bearer ${token}`
                    }
                    : {}),

                ...options.headers
            }
        }
    );
}


export { API_URL };