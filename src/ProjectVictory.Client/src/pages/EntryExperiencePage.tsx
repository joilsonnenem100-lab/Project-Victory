import PageContainer from "../components/layout/PageContainer";
import Panel from "../components/layout/Panel";
import SectionTitle from "../components/layout/SectionTitle";


interface Props
{
    onContinue: () => void;
}


export default function EntryExperiencePage(
    {
        onContinue
    }: Props
)
{
    return (
        <PageContainer>

            <Panel>

                <h1>
                    Welcome to Project Victory
                </h1>


                <SectionTitle>
                    Player Entry
                </SectionTitle>


                <p>
                    Preparing your world entry...
                </p>


                <button
                    onClick={onContinue}
                >
                    Enter World
                </button>

            </Panel>

        </PageContainer>
    );
}