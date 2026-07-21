interface Props
{
    children: React.ReactNode;

    onClick?: () => void;

    disabled?: boolean;

    type?: "button" | "submit";
}



export default function Button(
    {
        children,
        onClick,
        disabled = false,
        type = "button"
    }: Props
)
{
    return (
        <button
            type={type}
            onClick={onClick}
            disabled={disabled}

            style={{
                width:
                    "100%",

                padding:
                    "var(--pv-spacing-sm) var(--pv-spacing-lg)",

                background:
                    "linear-gradient(180deg, rgba(176, 145, 70, 0.95), rgba(110, 82, 30, 0.95))",

                color:
                    "var(--pv-color-text-primary)",

                border:
                    "1px solid rgba(230, 200, 120, 0.65)",

                borderRadius:
                    "10px",

                fontFamily:
                    "var(--pv-font-family-primary)",

                fontSize:
                    "var(--pv-font-size-body)",

                fontWeight:
                    "var(--pv-font-weight-section)",

                letterSpacing:
                    "0.05em",

                textTransform:
                    "uppercase",

                cursor:
                    disabled
                        ? "not-allowed"
                        : "pointer",

                opacity:
                    disabled
                        ? 0.6
                        : 1,

                boxShadow:
                    "0 8px 20px rgba(0, 0, 0, 0.35)"
            }}
        >
            {children}
        </button>
    );
}