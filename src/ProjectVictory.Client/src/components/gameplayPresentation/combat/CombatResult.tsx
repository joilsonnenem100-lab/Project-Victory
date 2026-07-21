interface Props
{
    children: React.ReactNode;
}


export default function CombatResult(
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