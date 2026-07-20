interface Props
{
    children: React.ReactNode;
}


export default function PlayerCombatArea(
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