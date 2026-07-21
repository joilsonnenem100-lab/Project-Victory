interface Props
{
    children: React.ReactNode;
}


export default function Container(
    {
        children
    }: Props
)
{
    return (
        <div
            style={{
                width:
                    "var(--pv-responsive-container-width)",


                maxWidth:
                    "100%",


                margin:
                    "0 auto",


                padding:
                    "var(--pv-responsive-page-padding)",


                display:
                    "flex",


                flexDirection:
                    "column",


                gap:
                    "var(--pv-spacing-lg)"
            }}
        >
            {children}
        </div>
    );
}