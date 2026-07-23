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

                <section
                    className="pv-background-layer"
                    style={{
                        minHeight:
                            "100vh"
                    }}
                >

                    <div
                        className="pv-background-content"
                    >

                        <Card>

                            <h1
                                className="pv-title-decoration"
                                style={{
                                    textAlign:
                                        "center",

                                    fontSize:
                                        "var(--pv-font-size-title)"
                                }}
                            >
                                PROJECT-VICTORY
                            </h1>


                            <SectionTitle>
                                World Entry
                            </SectionTitle>


                            <p
                                className="pv-content-decoration"
                                style={{
                                    textAlign:
                                        "center"
                                }}
                            >
                                Your character is ready to enter the world.
                            </p>


                            <section
                                className="pv-panel-decoration"
                                style={{
                                    display:
                                        "flex",

                                    flexDirection:
                                        "column",

                                    gap:
                                        "var(--pv-spacing-sm)",

                                    padding:
                                        "var(--pv-spacing-md)"
                                }}
                            >

                                <strong>
                                    Character Context
                                </strong>


                                <span>
                                    Name
                                </span>


                                <span>
                                    Character ID
                                </span>


                                <span>
                                    Level
                                </span>


                                <span>
                                    Experience
                                </span>

                            </section>


                            <Button
                                onClick={
                                    onContinue
                                }
                            >
                                Enter World
                            </Button>

                        </Card>

                    </div>

                </section>

            </Container>

        </EntrySection>
    );
}