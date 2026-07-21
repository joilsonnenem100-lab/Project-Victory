interface Props
{
    children: React.ReactNode;
}


export default function HuntAction(
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