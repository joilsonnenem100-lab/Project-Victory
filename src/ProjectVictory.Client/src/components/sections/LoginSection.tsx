interface Props
{
    children: React.ReactNode;
}


export default function LoginSection(
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