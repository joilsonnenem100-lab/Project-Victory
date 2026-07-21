interface Props
{
    children: React.ReactNode;
}



export default function GameplayPresentation(
    {
        children
    }: Props
)
{
    return (
        <section
            className="pv-panel-decoration"
            style={{
                display:
                    "flex",

                flexDirection:
                    "column",

                gap:
                    "var(--pv-spacing-md)",

                padding:
                    "var(--pv-spacing-md)",

                width:
                    "100%",

                boxSizing:
                    "border-box"
            }}
        >
            <header
                className="pv-title-decoration"
                style={{
                    textAlign:
                        "center"
                }}
            >
                PROJECT-VICTORY
            </header>


            <div
                className="pv-content-decoration"
                style={{
                    display:
                        "flex",

                    flexDirection:
                        "column",

                    gap:
                        "var(--pv-spacing-md)"
                }}
            >
                {children}
            </div>

        </section>
    );
}