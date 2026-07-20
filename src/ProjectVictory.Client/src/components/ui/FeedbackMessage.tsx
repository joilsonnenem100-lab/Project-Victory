interface Props
{
    message: string;

    type?: "error" | "success" | "info";
}


export default function FeedbackMessage(
    {
        message,
        type = "info"
    }: Props
)
{
    return (
        <p data-type={type}>
            {message}
        </p>
    );
}