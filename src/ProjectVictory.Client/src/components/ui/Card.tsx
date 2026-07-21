interface Props
{
    children: React.ReactNode;
}


export default function Card(
    {
        children
    }: Props
)
{
    return (
        <section
            className="pv-panel-decoration"
            style={{
                width:
                    "100%",

                maxWidth:
                    "480px",

                padding:
                    "var(--pv-spacing-lg)",

                background:
                    "rgba(10, 12, 18, 0.82)",

                border:
                    "1px solid rgba(180, 150, 70, 0.45)",

                borderRadius:
                    "18px",

                boxShadow:
                    "0 20px 60px rgba(0, 0, 0, 0.55), inset 0 0 40px rgba(255, 255, 255, 0.03)",

                backdropFilter:
                    "blur(12px)",

                display:
                    "flex",

                flexDirection:
                    "column",

                gap:
                    "var(--pv-spacing-md)"
            }}
        >
            {children}
        </section>
    );
}