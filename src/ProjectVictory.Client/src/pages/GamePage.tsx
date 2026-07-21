import WorldScenePage from "./WorldScenePage";

import type { GameState } from "../models/game";


interface Props
{
    gameState: GameState;
}


export default function GamePage(
    {
        gameState
    }: Props
)
{
    return (
        <WorldScenePage
            world={{
                player:
                    gameState,

                sceneName:
                    "FirstWorld"
            }}
        />
    );
}