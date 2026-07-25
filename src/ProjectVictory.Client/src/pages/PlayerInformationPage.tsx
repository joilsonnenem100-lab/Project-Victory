import type { Character } from "../models/character";

import Container from "../components/ui/Container";
import Card from "../components/ui/Card";
import HuntPanel from "../components/gameplay/HuntPanel";

interface Props
{
    character: Character;

    onStartHunt: () => void;

    onOpenInventory: () => void;
}

export default function PlayerInformationPage(
{
    character,

    onStartHunt,

    onOpenInventory

}: Props)
{
    return (
        <Container>

            <Card>

                <section className="pv-panel-decoration">

                    <h1 className="pv-title-decoration">
                        Player Information
                    </h1>

                    <p className="pv-content-decoration">
                        Name: {character.name}
                    </p>

                    <p className="pv-content-decoration">
                        Level: {character.level}
                    </p>

                    <p className="pv-content-decoration">
                        Experience: {character.experience}
                    </p>

                    <button
                        onClick={onOpenInventory}
                    >
                        Inventory
                    </button>

                    <HuntPanel
                        onStart={onStartHunt}
                    />

                </section>

            </Card>

        </Container>
    );
}
