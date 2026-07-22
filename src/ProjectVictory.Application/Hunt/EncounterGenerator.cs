using System;
using System.Collections.Generic;

using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Hunt;

public sealed class EncounterGenerator
{
    public ActiveEncounter Generate(
        Guid huntId)
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
            monsters);
    }



    private static EncounterMonsterState CreateRat()
    {
        return new EncounterMonsterState(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Rat",
            20);
    }



    private static EncounterMonsterState CreateCaveRat()
    {
        return new EncounterMonsterState(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Cave Rat",
            50);
    }
}