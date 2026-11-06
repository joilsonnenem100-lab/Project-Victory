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
                Title:
                {" "}
                {player.title ?? "-"}
                {player.titleTier !== undefined ? ` (tier ${player.titleTier})` : ""}
            </p>


            <p>
                Experience:
                {" "}
                {player.experience}
            </p>

        </section>
    );
}