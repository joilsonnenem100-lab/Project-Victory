import type { RewardResult } from "../models/reward";


interface Props
{
    reward: RewardResult | null;
}


export default function RewardPanel(
    {
        reward
    }: Props
)
{
    if (!reward)
    {
        return null;
    }


    return (
        <section>

            <h2>
                Rewards
            </h2>


            <p>
                Experience:
                {" "}
                {reward.experience}
            </p>


            <p>
                Gold:
                {" "}
                {reward.gold}
            </p>

        </section>
    );
}