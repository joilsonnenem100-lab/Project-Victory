import type { WorldState } from "../models/world";

import PlayerInterface from "../components/player/PlayerInterface";

import PlayerIdentityArea from "../components/player/PlayerIdentityArea";
import PlayerActionsArea from "../components/player/PlayerActionsArea";
import PlayerCombatArea from "../components/player/PlayerCombatArea";
import PlayerRewardArea from "../components/player/PlayerRewardArea";

import GameplayPresentation from "../components/gameplayPresentation/GameplayPresentation";

import PlayerArea from "../components/gameplayPresentation/PlayerArea";
import HuntArea from "../components/gameplayPresentation/HuntArea";
import CombatArea from "../components/gameplayPresentation/CombatArea";
import ResultArea from "../components/gameplayPresentation/ResultArea";
import RewardArea from "../components/gameplayPresentation/RewardArea";

import HuntPanel from "../components/gameplay/HuntPanel";
import CombatResultPanel from "../components/gameplay/CombatResultPanel";
import RewardPanel from "../components/gameplay/RewardPanel";

import Container from "../components/ui/Container";
import Card from "../components/ui/Card";



interface Props
{
    world: WorldState;
}



export default function WorldScenePage(
    {
        world
    }: Props
)
{
    return (
        <PlayerInterface>

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

                        <GameplayPresentation>

                            <PlayerArea>

                                <Card>

                                    <PlayerIdentityArea
                                        player={
                                            world.player
                                        }
                                    />

                                </Card>

                            </PlayerArea>



                            <HuntArea>

                                <Card>

                                    <PlayerActionsArea>

                                        <HuntPanel
                                            onStart={
                                                () =>
                                                {
                                                    return;
                                                }
                                            }
                                        />

                                    </PlayerActionsArea>

                                </Card>

                            </HuntArea>



                            <CombatArea>

                                <Card>

                                    <PlayerCombatArea>

                                        <CombatResultPanel
                                            result={null}
                                        />

                                    </PlayerCombatArea>

                                </Card>

                            </CombatArea>



                            <ResultArea>

                                <Card>

                                    <CombatResultPanel
                                        result={null}
                                    />

                                </Card>

                            </ResultArea>



                            <RewardArea>

                                <Card>

                                    <PlayerRewardArea>

                                        <RewardPanel
                                            reward={null}
                                        />

                                    </PlayerRewardArea>

                                </Card>

                            </RewardArea>


                        </GameplayPresentation>

                    </div>

                </section>

            </Container>

        </PlayerInterface>
    );
}