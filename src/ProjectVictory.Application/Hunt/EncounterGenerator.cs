using System;
using System.Collections.Generic;
using System.Linq;

using ProjectVictory.Application.Spawn;
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

    private readonly ISpawnSystem? _spawnSystem;


    // Composição antiga fixa (Rat, Cave Rat, Rat), mantida pela Regra Aditiva para quem cria o
    // gerador sem catálogo.
    public EncounterGenerator()
    {
    }

    // PV-CONTENT-011: com catálogo (DI), o encounter vem da área de caça (Spider Cave), por dados.
    public EncounterGenerator(IHuntAreaCatalog areas)
        : this(areas, new StatelessSpawnSystem())
    {
    }

    // PV-SPAWN-010 (ADR-009/010/047): os monstros do encounter vêm do Spawn System, a partir da
    // configuração de spawn da Hunt (área de caça). O gerador só monta o encounter.
    public EncounterGenerator(IHuntAreaCatalog areas, ISpawnSystem spawnSystem)
    {
        _areas = areas;
        _spawnSystem = spawnSystem ?? throw new ArgumentNullException(nameof(spawnSystem));
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
                : FromSpawn(huntId, _areas.GetDefault(), seed);


        return new ActiveEncounter(
            Guid.NewGuid(),
            huntId,
            monsters,
            seed);
    }


    // PV-SPAWN-010: substitui o sorteio que ficava aqui (FromArea). Mesmo resultado para a mesma
    // semente. As Spawn Instances são efêmeras: o encounter guarda só o estado dos monstros.
    private List<EncounterMonsterState> FromSpawn(
        Guid huntId,
        HuntAreaDefinition area,
        int seed)
    {
        var result = _spawnSystem!.SpawnPack(
            new SpawnContext(huntId, area, seed));

        return result.Monsters.ToList();
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
