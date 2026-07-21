interface Props
{
    children: React.ReactNode;
}


export default function CombatFeedback(
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