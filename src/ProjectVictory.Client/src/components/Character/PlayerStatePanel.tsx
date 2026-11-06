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
                Character Status
            </h2>


            <section>

                <h3>
                    Identity
                </h3>


                <p>
                    Name:
                    {" "}
                    {player.characterName}
                </p>


                <p>
                    Character ID:
                    {" "}
                    {player.characterId}
                </p>

            </section>



            <section>

                <h3>
                    Progression
                </h3>


                <p>
                    Current Level:
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
                    Current Experience:
                    {" "}
                    {player.experience}
                </p>

            </section>

        </section>
    );
}