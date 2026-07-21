interface Props
{
    children: React.ReactNode;
}


export default function CombatStatus(
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