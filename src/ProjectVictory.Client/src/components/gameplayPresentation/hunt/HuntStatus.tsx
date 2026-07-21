interface Props
{
    children: React.ReactNode;
}


export default function HuntStatus(
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