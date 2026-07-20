import PlayerInterface from "../components/player/PlayerInterface";

import PlayerIdentityArea from "../components/player/PlayerIdentityArea";
import PlayerActionsArea from "../components/player/PlayerActionsArea";
import PlayerCombatArea from "../components/player/PlayerCombatArea";
import PlayerRewardArea from "../components/player/PlayerRewardArea";


import CharacterStatusPanel from "../components/character/CharacterStatusPanel";

import HuntPanel from "../components/gameplay/HuntPanel";
import CombatResultPanel from "../components/gameplay/CombatResultPanel";
import RewardPanel from "../components/gameplay/RewardPanel";


interface Props
{
    onStartHunt: () => void;

    onExitHunt: () => void;
}



export default function WorldScenePage(
    {
        onStartHunt,
        onExitHunt
    }: Props
)
{
    return (
        <PlayerInterface>


            <PlayerIdentityArea>

                <CharacterStatusPanel />

            </PlayerIdentityArea>



            <PlayerActionsArea>

                <HuntPanel
                    onStart={
                        onStartHunt
                    }
                />

            </PlayerActionsArea>



            <PlayerCombatArea>

                <CombatResultPanel />

            </PlayerCombatArea>



            <PlayerRewardArea>

                <RewardPanel />

            </PlayerRewardArea>



            <button
                onClick={
                    onExitHunt
                }
            >
                Exit
            </button>


        </PlayerInterface>
    );
}