interface Props
{
    children: React.ReactNode;
}


export default function PlayerIdentityArea(
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