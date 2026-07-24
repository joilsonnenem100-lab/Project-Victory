import { useState } from "react";

import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import CharacterSelectionPage from "./pages/CharacterSelectionPage";
import PlayerInformationPage from "./pages/PlayerInformationPage";
import InventoryPage from "./pages/InventoryPage";
import RewardFeedbackPage from "./pages/RewardFeedbackPage";

import { startHunt } from "./services/huntService";

import type { Character } from "./models/character";


function App()
{
    const [page, setPage] =
        useState("login");

    const [selectedCharacter, setSelectedCharacter] =
        useState<Character | null>(null);

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
                character={selectedCharacter}

                onOpenInventory={() =>
                {
                    setPage("inventory");
                }}

                onStartHunt={async () =>
                {
                    const result =
                        await startHunt(
                            selectedCharacter.characterId
                        );


                    setReward(result);

                    setPage("reward");
                }}
            />
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
                reward={reward}
            />
        );
    }


    return null;
}


export default App;