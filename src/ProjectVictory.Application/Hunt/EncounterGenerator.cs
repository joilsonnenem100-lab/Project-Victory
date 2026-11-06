using System;
using System.Collections.Generic;
using System.Linq;

using ProjectVictory.Domain.Hunt;
using ProjectVictory.Domain.Primitives;

namespace ProjectVictory.Application.Hunt;

public sealed class EncounterGenerator
{
    private static readonly Guid RatId =
        Guid.Parse("10000000-0000-0000-0000-000000000001");


    private static readonly Guid CaveRatId =
        Guid.Parse("10000000-0000-0000-0000-000000000002");

    private readonly IHuntAreaCatalog? _areas;


    // Composição antiga fixa (Rat, Cave Rat, Rat), mantida pela Regra Aditiva para quem cria o
    // gerador sem catálogo.
    public EncounterGenerator()
    {
    }

    // PV-CONTENT-011: com catálogo (DI), o encounter vem da área de caça (Spider Cave), por dados.
    public EncounterGenerator(IHuntAreaCatalog areas)
    {
        _areas = areas;
    }


    public ActiveEncounter Generate(
        Guid huntId)
    {
        return Generate(huntId, 0);
    }

    // PV-COMBAT-011: o encounter guarda a semente do seu combate.
    // PV-CONTENT-011: a composição é sorteada entre os encounters da área com a MESMA semente
    // (um gerador próprio, para não consumir números do combate).
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
            _areas is null
                ? new List<EncounterMonsterState> { CreateRat(), CreateCaveRat(), CreateRat() }
                : FromArea(_areas.GetDefault(), seed);


        return new ActiveEncounter(
            Guid.NewGuid(),
            huntId,
            monsters,
            seed);
    }


    private static List<EncounterMonsterState> FromArea(
        HuntAreaDefinition area,
        int seed)
    {
        if (area.EncounterTemplates.Count == 0)
        {
            throw new InvalidOperationException("Hunt area has no encounters.");
        }

        var pick = new SeededRandomSource(seed)
            .NextInclusive(0, area.EncounterTemplates.Count - 1);

        return area.EncounterTemplates[pick]
            .Select(spawn => new EncounterMonsterState(
                Guid.NewGuid(),
                spawn.MonsterId,
                spawn.Name,
                spawn.MaximumHealth))
            .ToList();
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
