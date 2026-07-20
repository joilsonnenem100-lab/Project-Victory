interface Props
{
    children: React.ReactNode;
}


export default function EntrySection(
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