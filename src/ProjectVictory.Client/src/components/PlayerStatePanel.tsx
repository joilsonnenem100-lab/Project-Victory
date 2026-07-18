import type { GameState } from "../models/game";


interface Props
{
    player: GameState;
}


export default function PlayerStatePanel(
    {
        player
    }: Props
)
{
    return (
        <section>

            <h2>
                Player Status
            </h2>


            <p>
                Character ID:
                {" "}
                {player.characterId}
            </p>


            <p>
                Level:
                {" "}
                {player.level}
            </p>


            <p>
                Experience:
                {" "}
                {player.experience}
            </p>


        </section>
    );
}