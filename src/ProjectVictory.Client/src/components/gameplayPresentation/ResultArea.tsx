interface Props
{
    children: React.ReactNode;
}


export default function ResultArea(
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