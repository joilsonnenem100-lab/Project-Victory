import WorldScenePage from "./WorldScenePage";

import type { GameState } from "../models/game";


interface Props
{
    gameState: GameState;

    onCharacterProgressionUpdate:
        (
            experience: number,
            level: number
        ) => void;
}



export default function GamePage(
    {
        gameState,
        onCharacterProgressionUpdate
    }: Props
)
{
    console.log(
        "GAME PAGE STATE:",
        gameState
    );


    return (
        <WorldScenePage
            world={{
                player:
                    gameState,

                sceneName:
                    "FirstWorld"
            }}

            onCharacterProgressionUpdate={
                onCharacterProgressionUpdate
            }
        />
    );
}