import RewardStatus from "./reward/RewardStatus";
import ExperienceReward from "./reward/ExperienceReward";
import GoldReward from "./reward/GoldReward";


interface Props
{
    children: React.ReactNode;
}


export default function RewardArea(
    {
        children
    }: Props
)
{
    return (
        <section>


            <RewardStatus>

                {children}

            </RewardStatus>



            <ExperienceReward>

                {children}

            </ExperienceReward>



            <GoldReward>

                {children}

            </GoldReward>


        </section>
    );
}