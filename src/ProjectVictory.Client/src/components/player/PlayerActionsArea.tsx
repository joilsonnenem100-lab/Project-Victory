interface Props
{
    children: React.ReactNode;
}


export default function PlayerActionsArea(
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