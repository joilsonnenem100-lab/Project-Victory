interface Props
{
    message: string;

    type?: "success" | "warning" | "error";
}


export default function FeedbackMessage(
    {
        message,
        type = "success"
    }: Props
)
{
    const color =
        type === "success"
            ? "var(--pv-color-success)"
            :
        type === "warning"
            ? "var(--pv-color-warning)"
            :
            "var(--pv-color-error)";


    return (
        <div
            style={{
                width:
                    "100%",


                boxSizing:
                    "border-box",


                padding:
                    "var(--pv-spacing-sm) var(--pv-spacing-md)",


                background:
                    "var(--pv-color-background-secondary)",


                border:
                    `1px solid ${color}`,


                borderRadius:
                    "8px",


                color,


                fontFamily:
                    "var(--pv-font-family-primary)",


                fontSize:
                    "var(--pv-font-size-body)",


                lineHeight:
                    "var(--pv-line-height-normal)"
            }}
        >
            {message}
        </div>
    );
}