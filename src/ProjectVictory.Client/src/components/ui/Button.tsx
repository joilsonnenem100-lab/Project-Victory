interface Props
{
    children: React.ReactNode;

    onClick?: () => void;

    disabled?: boolean;
}


export default function Button(
    {
        children,
        onClick,
        disabled
    }: Props
)
{
    return (
        <button
            onClick={onClick}
            disabled={disabled}
        >
            {children}
        </button>
    );
}