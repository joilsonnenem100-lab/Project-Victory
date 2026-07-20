import type { GameState } from "../../models/game";

import CharacterStatusPanel from "../character/CharacterStatusPanel";


interface Props
{
    player: GameState;
}


export default function PlayerIdentityArea(
    {
        player
    }: Props
)
{
    return (
        <section>

            <CharacterStatusPanel
                player={player}
            />

        </section>
    );
}