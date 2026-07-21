// src/ProjectVictory.Client/src/pages/LoginPage.tsx

import { useState } from "react";

import { login } from "../services/accountService";

import background from "../assets/backgrounds/login/PV-ASSET-001-login-world-background-v001.webp";
import loginInterface from "../assets/interfaces/login/PV-ASSET-002-login-interface-system-v001.png";


interface Props
{
    onLoginSuccess(accountId: string): void;
    onRegister?: () => void;
    onForgotPassword?: () => void;
}


const DEBUG_HOTSPOTS = false;

const HOTSPOT_OFFSET_Y = -75;


interface HotspotProps
{
    name: string;
    left: number;
    top: number;
    width: number;
    height: number;
    onClick?: () => void;
}



function Hotspot(
{
    name,
    left,
    top,
    width,
    height,
    onClick
}: HotspotProps)
{
    return (
        <div
            onClick={onClick}

            style={{
                position: "absolute",

                left,

                top,

                width,

                height,

                border:
                    DEBUG_HOTSPOTS
                        ? "2px solid red"
                        : "none",

                background:
                    DEBUG_HOTSPOTS
                        ? "rgba(255,0,0,0.15)"
                        : "transparent",

                color: "white",

                fontSize: "12px",

                fontWeight: "bold",

                zIndex: 50,

                cursor:
                    onClick
                        ? "pointer"
                        : "default",

                pointerEvents:
                    onClick
                        ? "auto"
                        : "none"
            }}
        >
            {
                DEBUG_HOTSPOTS &&
                (
                    <span
                        style={{
                            background:
                                "rgba(0,0,0,0.85)",

                            display:
                                "inline-block",

                            padding:
                                "3px"
                        }}
                    >
                        {name}

                        <br />

                        X:{left}

                        <br />

                        Y:{top}

                        <br />

                        W:{width}

                        <br />

                        H:{height}

                    </span>
                )
            }

        </div>
    );
}



export default function LoginPage(
{
    onLoginSuccess,
    onRegister,
    onForgotPassword
}: Props)
{
    const [email, setEmail] =
        useState("");

    const [password, setPassword] =
        useState("");

    const [errorMessage, setErrorMessage] =
        useState("");



    async function handleLogin()
    {
        try
        {
            setErrorMessage("");

            const result =
                await login(
                    email,
                    password
                );


            onLoginSuccess(
                result.accountId
            );
        }
        catch
        {
            setErrorMessage(
                "Login failed."
            );
        }
    }



    return (
        <main
            style={{
                position: "fixed",

                inset: 0,

                width: "100vw",

                height: "100vh",

                overflow: "hidden",

                backgroundImage:
                    `url(${background})`,

                backgroundSize:
                    "cover",

                backgroundPosition:
                    "center",

                backgroundRepeat:
                    "no-repeat"
            }}
        >

            <div
                style={{
                    position: "absolute",

                    inset: 0,

                    display: "flex",

                    justifyContent: "center",

                    alignItems: "center"
                }}
            >

                <div
                    style={{
                        position: "relative",

                        width: "1536px",

                        height: "864px",

                        transform:
                            "scale(calc(min(100vw / 1536, 100vh / 864)))",

                        transformOrigin:
                            "center"
                    }}
                >

                    <img
                        src={loginInterface}

                        alt=""

                        draggable={false}

                        style={{
                            position: "absolute",

                            inset: 0,

                            width: "1536px",

                            height: "864px",

                            pointerEvents: "none"
                        }}
                    />



                    <input
                        type="email"

                        autoComplete="off"

                        value={email}

                        onChange={
                            e =>
                                setEmail(
                                    e.target.value
                                )
                        }

                        style={{
                            position: "absolute",

                            left: 520,

                            top:
                                435 + HOTSPOT_OFFSET_Y,

                            width: 496,

                            height: 70,

                            opacity: 0,

                            background: "transparent",

                            border: "none",

                            outline: "none",

                            zIndex: 20
                        }}
                    />



                    <input
                        type="password"

                        autoComplete="new-password"

                        value={password}

                        onChange={
                            e =>
                                setPassword(
                                    e.target.value
                                )
                        }

                        style={{
                            position: "absolute",

                            left: 520,

                            top:
                                525 + HOTSPOT_OFFSET_Y,

                            width: 496,

                            height: 70,

                            opacity: 0,

                            background: "transparent",

                            border: "none",

                            outline: "none",

                            zIndex: 20
                        }}
                    />



                    <Hotspot
                        name="EmailHotspot"

                        left={520}

                        top={
                            435 + HOTSPOT_OFFSET_Y
                        }

                        width={496}

                        height={70}
                    />



                    <Hotspot
                        name="PasswordHotspot"

                        left={520}

                        top={
                            525 + HOTSPOT_OFFSET_Y
                        }

                        width={496}

                        height={70}
                    />



                    <Hotspot
                        name="LoginHotspot"

                        left={520}

                        top={
                            650 + HOTSPOT_OFFSET_Y
                        }

                        width={496}

                        height={90}

                        onClick={handleLogin}
                    />



                    <Hotspot
                        name="ForgotPasswordHotspot"

                        left={840}

                        top={
                            555 + HOTSPOT_OFFSET_Y
                        }

                        width={190}

                        height={45}

                        onClick={
                            onForgotPassword
                        }
                    />



                    <Hotspot
                        name="RegisterHotspot"

                        left={790}

                        top={
                            725 + HOTSPOT_OFFSET_Y
                        }

                        width={190}

                        height={45}

                        onClick={
                            onRegister
                        }
                    />



                    {
                        errorMessage &&
                        (
                            <div
                                style={{
                                    position: "absolute",

                                    top:100,

                                    left:520,

                                    color:"red",

                                    zIndex:100
                                }}
                            >
                                {errorMessage}
                            </div>
                        )
                    }

                </div>

            </div>

        </main>
    );
}