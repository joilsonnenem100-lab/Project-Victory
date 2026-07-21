interface Props
{
    children: React.ReactNode;
}


export default function HuntContext(
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