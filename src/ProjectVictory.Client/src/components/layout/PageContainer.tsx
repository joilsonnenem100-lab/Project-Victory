interface Props
{
    children: React.ReactNode;
}


export default function PageContainer(
    {
        children
    }: Props
)
{
    return (
        <main
            style={{
                minHeight: "100vh",
                padding: "32px",
                display: "flex",
                flexDirection: "column",
                gap: "24px"
            }}
        >
            {children}
        </main>
    );
}