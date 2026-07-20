import type { GameState } from "../models/game";

import PlayerInterface from "../components/player/PlayerInterface";

import PlayerIdentityArea from "../components/player/PlayerIdentityArea";
import PlayerActionsArea from "../components/player/PlayerActionsArea";
import PlayerCombatArea from "../components/player/PlayerCombatArea";
import PlayerRewardArea from "../components/player/PlayerRewardArea";

import HuntPanel from "../components/gameplay/HuntPanel";
import CombatResultPanel from "../components/gameplay/CombatResultPanel";
import RewardPanel from "../components/gameplay/RewardPanel";


interface Props
{
    player: GameState;

    onStartHunt: () => void;

    onExitHunt: () => void;
}



export default function WorldScenePage(
    {
        player,
        onStartHunt,
        onExitHunt
    }: Props
)
{
    return (
        <PlayerInterface>


            <PlayerIdentityArea
                player={player}
            />



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