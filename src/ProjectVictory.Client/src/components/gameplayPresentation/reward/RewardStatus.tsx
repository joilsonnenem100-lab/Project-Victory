interface Props
{
    children: React.ReactNode;
}


export default function RewardStatus(
    {
        children
    }: Props
)
{
    return (
        <section>

            {children}

        </section>
    );
}