interface Props
{
    onStart: () => void;
}


export default function HuntPanel(
    {
        onStart
    }: Props
)
{
    return (
        <section>

            <h2>
                Hunt
            </h2>

            <button
                onClick={onStart}
            >
                Start Hunt
            </button>

        </section>
    );
}