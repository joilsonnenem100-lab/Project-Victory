import type { GameState } from "../../models/game";


interface Props
{
    player: GameState;
}


export default function CharacterProgressionPanel(
    {
        player
    }: Props
)
{
    return (
        <section>

            <h3>
                Progression
            </h3>


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