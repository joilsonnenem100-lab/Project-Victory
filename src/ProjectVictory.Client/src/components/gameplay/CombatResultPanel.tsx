import type { CombatResult } from "../models/combat";


interface Props
{
    result: CombatResult | null;
}


export default function CombatResultPanel(
    {
        result
    }: Props
)
{
    if (!result)
    {
        return null;
    }


    return (
        <section>

            <h2>
                Combat Result
            </h2>


            <p>
                Combat:
                {" "}
                {result.combatId}
            </p>


            <p>
                Victory:
                {" "}
                {result.victory ? "Yes" : "No"}
            </p>

        </section>
    );
}