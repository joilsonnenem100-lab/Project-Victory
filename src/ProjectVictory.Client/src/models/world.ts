import type { GameState } from "./game";


export interface WorldState
{
    player: GameState;

    sceneName: string;
}