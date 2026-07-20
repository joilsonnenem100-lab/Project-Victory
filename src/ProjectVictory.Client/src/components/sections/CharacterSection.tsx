interface Props
{
    children: React.ReactNode;
}


export default function CharacterSection(
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