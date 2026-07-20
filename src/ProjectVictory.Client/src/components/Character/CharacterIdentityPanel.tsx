import type { GameState } from "../../models/game";


interface Props
{
    player: GameState;
}


export default function CharacterIdentityPanel(
    {
        player
    }: Props
)
{
    return (
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
    );
}