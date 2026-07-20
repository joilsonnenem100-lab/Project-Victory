interface Props
{
    children: React.ReactNode;
}


export default function Container(
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