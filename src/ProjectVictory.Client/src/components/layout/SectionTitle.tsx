interface Props
{
    children: React.ReactNode;
}


export default function SectionTitle(
    {
        children
    }: Props
)
{
    return (
        <h2>
            {children}
        </h2>
    );
}