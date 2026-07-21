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
                width:
                    "100%",


                maxWidth:
                    "100%",


                padding:
                    "var(--pv-spacing-lg)",


                background:
                    "var(--pv-color-panel)",


                border:
                    "1px solid var(--pv-color-panel-border)",


                borderRadius:
                    "12px",


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