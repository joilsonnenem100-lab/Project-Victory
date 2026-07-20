interface Props
{
    children: React.ReactNode;
}


export default function PlayerInterface(
    {
        children
    }: Props
)
{
    return (
        <main>

            {children}

        </main>
    );
}