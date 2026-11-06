using System;
using System.Collections.Generic;

using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Hunt;

public sealed class EncounterGenerator
{
    private static readonly Guid RatId =
        Guid.Parse("10000000-0000-0000-0000-000000000001");


    private static readonly Guid CaveRatId =
        Guid.Parse("10000000-0000-0000-0000-000000000002");



    public ActiveEncounter Generate(
        Guid huntId)
    {
        return Generate(huntId, 0);
    }

    // PV-COMBAT-011: o encounter guarda a semente do seu combate.
    public ActiveEncounter Generate(
        Guid huntId,
        int seed)
    {
        if (huntId == Guid.Empty)
        {
            throw new ArgumentException(
                "HuntId is required.",
                nameof(huntId));
        }


        var monsters =
            new List<EncounterMonsterState>
            {
                CreateRat(),

                CreateCaveRat(),

                CreateRat()
            };


        return new ActiveEncounter(
            Guid.NewGuid(),
            huntId,
            monsters,
            seed);
    }



    private static EncounterMonsterState CreateRat()
    {
        return new EncounterMonsterState(
            Guid.NewGuid(),
            RatId,
            "Rat",
            20);
    }



    private static EncounterMonsterState CreateCaveRat()
    {
        return new EncounterMonsterState(
            Guid.NewGuid(),
            CaveRatId,
            "Cave Rat",
            50);
    }
}