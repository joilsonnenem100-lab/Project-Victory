interface Props
{
    children: React.ReactNode;
}


export default function PlayerRewardArea(
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