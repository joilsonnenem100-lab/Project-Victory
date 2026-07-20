interface Props
{
    children: React.ReactNode;
}


export default function Card(
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