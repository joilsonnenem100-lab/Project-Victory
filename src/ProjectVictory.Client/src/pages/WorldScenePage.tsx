// src/ProjectVictory.Client/src/pages/WorldScenePage.tsx

import { useState } from "react";

import type { WorldState } from "../models/world";
import type { HuntResult } from "../models/hunt";
import type { RewardResult } from "../models/reward";

import PlayerInterface from "../components/player/PlayerInterface";

import PlayerIdentityArea from "../components/player/PlayerIdentityArea";
import PlayerActionsArea from "../components/player/PlayerActionsArea";
import PlayerCombatArea from "../components/player/PlayerCombatArea";
import PlayerRewardArea from "../components/player/PlayerRewardArea";

import GameplayPresentation from "../components/gameplayPresentation/GameplayPresentation";

import PlayerArea from "../components/gameplayPresentation/PlayerArea";
import HuntArea from "../components/gameplayPresentation/HuntArea";
import CombatArea from "../components/gameplayPresentation/CombatArea";
import RewardArea from "../components/gameplayPresentation/RewardArea";

import HuntPanel from "../components/gameplay/HuntPanel";

import {
    startHunt,
    attackEncounter,
    type AttackEncounterResult
}
from "../services/huntService";

import Container from "../components/ui/Container";
import Card from "../components/ui/Card";



interface Props
{
    world: WorldState;

    onCharacterProgressionUpdate:
    (
        experience: number,
        level: number
    ) => void;
}



export default function WorldScenePage(
{
    world,
    onCharacterProgressionUpdate

}: Props)
{
    const [huntResult, setHuntResult] =
        useState<HuntResult | null>(null);


    const [encounterState, setEncounterState] =
        useState<AttackEncounterResult | null>(null);



    const [reward, setReward] =
        useState<RewardResult | null>(null);



    async function handleStartHunt()
    {
        const result =
            await startHunt(
                world.player.characterId
            );


        setHuntResult(
            result
        );


        if (result.encounter)
        {
            setEncounterState(
                {
                    encounterId:
                        result.encounter.id,

                    monsterName:
                        result.encounter.monsterName,

                    currentHealth:
                        result.encounter.currentHealth,

                    maximumHealth:
                        result.encounter.maximumHealth,

                    monsterDefeated:
                        false,

                    experience: 0,

                    gold: 0,

                    level:
                        result.level,

                    drops: []
                }
            );
        }
    }



    async function handleAttack()
    {
        if (!encounterState)
        {
            return;
        }


        const result =
            await attackEncounter(
                encounterState.encounterId
            );


        setEncounterState(
            {
                ...encounterState,

                ...result,

                encounterId:
                    encounterState.encounterId
            }
        );


        if (result.monsterDefeated)
        {
            setReward(
                {
                    experience:
                        result.experience,

                    gold:
                        result.gold,

                    drops:
                        result.drops ?? []
                }
            );


            onCharacterProgressionUpdate(
                world.player.experience + result.experience,
                result.level
            );
        }
    }



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
                                                handleStartHunt
                                            }
                                        />

                                    </PlayerActionsArea>

                                </Card>

                            </HuntArea>



                            <CombatArea>

                                <Card>

                                    <PlayerCombatArea>

                                        {
                                            encounterState &&
                                            (
                                                <>
                                                    <h2>
                                                        Combat
                                                    </h2>


                                                    <h3>
                                                        {
                                                            encounterState.monsterName
                                                        }
                                                    </h3>


                                                    <div>
                                                        HP Monster:
                                                        {" "}
                                                        {
                                                            encounterState.currentHealth
                                                        }
                                                        /
                                                        {
                                                            encounterState.maximumHealth
                                                        }
                                                    </div>


                                                    <progress
                                                        value={
                                                            encounterState.currentHealth
                                                        }
                                                        max={
                                                            encounterState.maximumHealth
                                                        }
                                                    />


                                                    <div>
                                                        Player HP:
                                                        {" "}
                                                        {
                                                            encounterState.playerHealth
                                                        }
                                                        /
                                                        {
                                                            encounterState.playerMaximumHealth
                                                        }
                                                    </div>


                                                    <progress
                                                        value={
                                                            encounterState.playerHealth
                                                        }
                                                        max={
                                                            encounterState.playerMaximumHealth
                                                        }
                                                    />


                                                    {
                                                        !encounterState.combatFinished &&
                                                        (
                                                            <button
                                                                onClick={
                                                                    handleAttack
                                                                }
                                                            >
                                                                Attack
                                                            </button>
                                                        )
                                                    }


                                                    {
                                                        encounterState.combatFinished &&
                                                        (
                                                            <h2>
                                                                {
                                                                    encounterState.combatState
                                                                }
                                                            </h2>
                                                        )
                                                    }

                                                </>
                                            )
                                        }

                                    </PlayerCombatArea>

                                </Card>

                            </CombatArea>



                            <RewardArea>

                                <Card>

                                    <PlayerRewardArea>

                                        {
                                            reward &&
                                            (
                                                <>
                                                    <h2>
                                                        Rewards
                                                    </h2>


                                                    <p>
                                                        Experience:
                                                        {" "}
                                                        {
                                                            reward.experience
                                                        }
                                                    </p>


                                                    <p>
                                                        Gold:
                                                        {" "}
                                                        {
                                                            reward.gold
                                                        }
                                                    </p>


                                                    {
                                                        reward.drops.length > 0 &&
                                                        (
                                                            <div>

                                                                <h3>
                                                                    Items:
                                                                </h3>


                                                                {
                                                                    reward.drops.map(
                                                                        drop =>
                                                                        (
                                                                            <p
                                                                                key={
                                                                                    drop.itemId
                                                                                }
                                                                            >
                                                                                {
                                                                                    drop.itemName
                                                                                }
                                                                            </p>
                                                                        )
                                                                    )
                                                                }

                                                            </div>
                                                        )
                                                    }

                                                </>
                                            )
                                        }

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