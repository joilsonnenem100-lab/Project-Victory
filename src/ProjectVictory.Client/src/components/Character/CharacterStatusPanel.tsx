import type { GameState } from "../../models/game";

import CharacterIdentityPanel from "./CharacterIdentityPanel";
import CharacterProgressionPanel from "./CharacterProgressionPanel";


interface Props
{
    player: GameState;
}


export default function CharacterStatusPanel(
    {
        player
    }: Props
)
{
    return (
        <section>

            <h2>
                Character
            </h2>


            <CharacterIdentityPanel
                player={player}
            />


            <CharacterProgressionPanel
                player={player}
            />

        </section>
    );
}