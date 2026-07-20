interface Props
{
    children: React.ReactNode;
}


export default function Panel(
    {
        children
    }: Props
)
{
    return (
        <section
            style={{
                padding: "20px",
                border: "1px solid #444",
                borderRadius: "8px",
                display: "flex",
                flexDirection: "column",
                gap: "12px"
            }}
        >
            {children}
        </section>
    );
}