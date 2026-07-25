import { useState } from "react";

import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import CharacterSelectionPage from "./pages/CharacterSelectionPage";
import PlayerInformationPage from "./pages/PlayerInformationPage";
import InventoryPage from "./pages/InventoryPage";
import RewardFeedbackPage from "./pages/RewardFeedbackPage";

import GameplayPresentation from "./components/gameplayPresentation/GameplayPresentation";
import HuntArea from "./components/gameplayPresentation/HuntArea";
import CombatArea from "./components/gameplayPresentation/CombatArea";
import RewardArea from "./components/gameplayPresentation/RewardArea";

import { startHunt } from "./services/huntService";
import { enterGame } from "./services/gameService";

import type { Character } from "./models/character";
import type { GameState } from "./models/game";


function App()
{
    const [page, setPage] =
        useState("login");


    const [selectedCharacter, setSelectedCharacter] =
        useState<Character | null>(null);


    const [gameState, setGameState] =
        useState<GameState | null>(null);


    const [huntResult, setHuntResult] =
        useState<any>(null);


    const [reward, setReward] =
        useState<any>(null);



    if (page === "login")
    {
        return (
            <LoginPage

                onLoginSuccess={() =>
                {
                    setPage("characters");
                }}

                onRegister={() =>
                {
                    setPage("register");
                }}

            />
        );
    }



    if (page === "register")
    {
        return (
            <RegisterPage

                onBack={() =>
                {
                    setPage("login");
                }}

            />
        );
    }



    if (page === "characters")
    {
        return (
            <CharacterSelectionPage

                onCharacterSelected={(character) =>
                {
                    setSelectedCharacter(character);

                    setPage("player");
                }}

            />
        );
    }



    if (page === "player")
    {
        if (!selectedCharacter)
        {
            return null;
        }


        return (
            <PlayerInformationPage

                character={
                    selectedCharacter
                }


                onOpenInventory={() =>
                {
                    setPage("inventory");
                }}


                onStartHunt={async () =>
                {
                    const state =
                        await enterGame();


                    setGameState(state);


                    const result =
                        await startHunt(
                            state.characterId
                        );


                    setHuntResult(result);

                    setPage("gameplay");
                }}

            />
        );
    }



    if (page === "gameplay")
    {
        return (
            <GameplayPresentation>


                <HuntArea>

                    <h2>
                        Hunt
                    </h2>


                    <p>
                        Character:
                        {" "}
                        {
                            gameState?.characterName
                            ??
                            "-"
                        }
                    </p>


                    <p>
                        Level:
                        {" "}
                        {
                            gameState?.level
                            ??
                            "-"
                        }
                    </p>


                    <p>
                        Monster:
                        {" "}
                        {
                            huntResult?.encounter?.monsterName
                            ??
                            "-"
                        }
                    </p>


                    <p>
                        HP:
                        {" "}
                        {
                            huntResult?.encounter?.currentHealth
                            ??
                            "-"
                        }

                        /

                        {
                            huntResult?.encounter?.maximumHealth
                            ??
                            "-"
                        }
                    </p>


                </HuntArea>



                <CombatArea>

                    <h2>
                        Combat Simulation
                    </h2>


                    <p>
                        Combat is automatic.
                    </p>


                    <p>
                        Monster:
                        {" "}
                        {
                            huntResult?.encounter?.monsterName
                            ??
                            "-"
                        }
                    </p>


                </CombatArea>



            </GameplayPresentation>
        );
    }



    if (page === "inventory")
    {
        if (!selectedCharacter)
        {
            return null;
        }


        return (
            <InventoryPage

                characterId={
                    selectedCharacter.characterId
                }

            />
        );
    }



    if (page === "reward")
    {
        return (
            <RewardFeedbackPage

                reward={
                    reward
                }

            />
        );
    }



    return null;
}


export default App;