using System;
using System.Collections.Generic;
using ProjectVictory.Domain.World;

namespace ProjectVictory.Application.World;

public static class WorldDefinitionSeed
{
    public static WorldDefinitionSeedResult Create()
    {
        var worldId = Guid.Parse("10000000-0000-0000-0000-000000000001");
        var regionId = Guid.Parse("20000000-0000-0000-0000-000000000001");
        var locationId = Guid.Parse("30000000-0000-0000-0000-000000000001");

        return new WorldDefinitionSeedResult(
            new[]
            {
                new WorldDefinition(
                    worldId,
                    "First World",
                    "Primeiro mundo do Project-Victory.",
                    WorldDefinitionStatus.Active)
            },
            new[]
            {
                new RegionDefinition(
                    regionId,
                    worldId,
                    "Starting Region",
                    "Região inicial do mundo.",
                    RegionDefinitionStatus.Active)
            },
            new[]
            {
                new LocationDefinition(
                    locationId,
                    regionId,
                    // PV-Q4 (DEC-20261106-24): a localização inicial é a cidade de retorno "Victoria"
                    // (nome PROVISÓRIO; antes "Starting Location").
                    HomeCity.Name,
                    "Cidade de retorno (tela inicial). Antes: Starting Location.",
                    LocationDefinitionStatus.Active)
            });
    }
}

public sealed record WorldDefinitionSeedResult(
    IReadOnlyList<WorldDefinition> Worlds,
    IReadOnlyList<RegionDefinition> Regions,
    IReadOnlyList<LocationDefinition> Locations);
