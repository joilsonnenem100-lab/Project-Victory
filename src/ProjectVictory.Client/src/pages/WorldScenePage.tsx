import { useState } from "react";

import type { WorldState } from "../models/world";
import type { CombatResult } from "../models/combat";
import type { RewardResult } from "../models/reward";

import PlayerStatePanel from "../components/PlayerStatePanel";
import HuntPanel from "../components/HuntPanel";
import CombatResultPanel from "../components/CombatResultPanel";
import RewardPanel from "../components/RewardPanel";

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
                    characterId: "",
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
                    First World Scene
                </h2>


                <p>
                    Scene:
                    {" "}
                    {world.sceneName}
                </p>

            </section>


            <section
                style={{
                    width: "120px",
                    height: "120px",
                    border: "2px solid black",
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center"
                }}
            >
                Player
            </section>


            <PlayerStatePanel
                player={world.player}
            />


            <HuntPanel
                onStart={executeHunt}
            />


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