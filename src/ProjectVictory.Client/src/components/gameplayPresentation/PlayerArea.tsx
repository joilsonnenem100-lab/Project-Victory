interface Props
{
    children: React.ReactNode;
}


export default function PlayerArea(
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