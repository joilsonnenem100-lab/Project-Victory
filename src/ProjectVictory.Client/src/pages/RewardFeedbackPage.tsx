interface RewardFeedbackData
{
    experience?: number;

    gold?: number;

    drops?: string[];
}


interface RewardFeedbackPageProps
{
    reward?: RewardFeedbackData;
}


export default function RewardFeedbackPage(
    {
        reward
    }: RewardFeedbackPageProps
)
{
    if (!reward)
    {
        return (
            <div>
                <h1>Reward</h1>

                <p>
                    Nenhuma recompensa disponível.
                </p>
            </div>
        );
    }


    return (
        <div>

            <h1>
                Reward Received
            </h1>


            <p>
                Experience:
                {" "}
                {reward.experience ?? 0}
            </p>


            <p>
                Gold:
                {" "}
                {reward.gold ?? 0}
            </p>


            <h2>
                Items
            </h2>


            {
                !reward.drops ||
                reward.drops.length === 0
                ?
                (
                    <p>
                        Nenhum item recebido.
                    </p>
                )
                :
                (
                    <ul>
                        {
                            reward.drops.map(
                                (drop, index) =>
                                (
                                    <li key={index}>
                                        {drop}
                                    </li>
                                )
                            )
                        }
                    </ul>
                )
            }

        </div>
    );
}