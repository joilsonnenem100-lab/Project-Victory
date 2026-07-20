import Container from "../components/ui/Container";
import Card from "../components/ui/Card";
import Button from "../components/ui/Button";

import EntrySection from "../components/sections/EntrySection";

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
        <EntrySection>

            <Container>

                <Card>

                    <h1>
                        Welcome to Project Victory
                    </h1>


                    <SectionTitle>
                        Player Entry
                    </SectionTitle>


                    <p>
                        Preparing your world entry...
                    </p>


                    <Button
                        onClick={onContinue}
                    >
                        Enter World
                    </Button>

                </Card>

            </Container>

        </EntrySection>
    );
}