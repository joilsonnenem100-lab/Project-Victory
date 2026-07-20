import { useState } from "react";

import type { WorldState } from "../models/world";
import type { GameState } from "../models/game";
import type { CombatResult } from "../models/combat";
import type { RewardResult } from "../models/reward";

import CharacterStatusPanel from "../components/character/CharacterStatusPanel";

import HuntPanel from "../components/gameplay/HuntPanel";
import CombatResultPanel from "../components/gameplay/CombatResultPanel";
import RewardPanel from "../components/gameplay/RewardPanel";

import {
    startHunt
}
from "../services/huntService";


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
    const [player] =
        useState<GameState>(
            world.player
        );


    const [combatResult, setCombatResult] =
        useState<CombatResult | null>(null);


    const [rewardResult, setRewardResult] =
        useState<RewardResult | null>(null);


    const [loading, setLoading] =
        useState(false);


    const [error, setError] =
        useState<string | null>(null);



    async function executeHunt()
    {
        if (loading)
        {
            return;
        }


        try
        {
            setLoading(true);

            setError(null);


            const result =
                await startHunt();


            setCombatResult(
                {
                    combatId: result.huntId,
                    characterId: player.characterId,
                    victory: result.completed
                }
            );


            setRewardResult(
                {
                    experience: result.experience,
                    gold: result.gold
                }
            );
        }
        catch
        {
            setError(
                "Unable to complete hunt."
            );
        }
        finally
        {
            setLoading(false);
        }
    }



    return (
        <main>

            <h1>
                Project Victory World
            </h1>


            <section>

                <h2>
                    Player Entry Screen
                </h2>


                <p>
                    World:
                    {" "}
                    {world.sceneName}
                </p>

            </section>


            <CharacterStatusPanel
                player={player}
            />


            <section>

                <h2>
                    Available Actions
                </h2>


                <HuntPanel
                    onStart={executeHunt}
                />

            </section>


            {
                loading &&
                <p>
                    Executing Hunt...
                </p>
            }


            {
                error &&
                <p>
                    {error}
                </p>
            }


            <CombatResultPanel
                result={combatResult}
            />


            <RewardPanel
                reward={rewardResult}
            />

        </main>
    );
}