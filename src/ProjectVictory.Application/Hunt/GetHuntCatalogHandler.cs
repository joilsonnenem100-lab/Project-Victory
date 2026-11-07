using ProjectVictory.Application.Progression;

namespace ProjectVictory.Application.Hunt;

// PV-HUNT-014 (PO-09/PO-10/PO-55/PO-56): catálogo SOMENTE LEITURA de regiões e Hunts para a tela de
// escolha. O nível/tier recomendado é só informativo (não bloqueia a entrada). Sem dados do jogador.
public sealed record HuntCatalogMonsterView(Guid MonsterId, string Name);

public sealed record HuntCatalogAreaView(
    string Name,
    int RecommendedTitleTier,
    string RecommendedTitle,
    int RecommendedLevel,
    IReadOnlyList<HuntCatalogMonsterView> Monsters);

public sealed record HuntCatalogRegionView(string Name, IReadOnlyList<HuntCatalogAreaView> Hunts);

public sealed record HuntCatalogResult(IReadOnlyList<HuntCatalogRegionView> Regions);

public sealed class GetHuntCatalogHandler(IHuntAreaCatalog areas, ILevelTitleProvider levelTitles)
{
    public HuntCatalogResult Handle()
    {
        var titles = levelTitles.Current.Titles;

        var regions = areas.GetAll()
            .GroupBy(area => area.Region ?? area.Name)
            .Select(region => new HuntCatalogRegionView(
                region.Key,
                region.Select(area =>
                {
                    // Tier sem título configurado → o maior título existente (leitura informativa).
                    var title = titles.FirstOrDefault(t => t.Tier == area.RequiredTitleTier) ?? titles[^1];

                    return new HuntCatalogAreaView(
                        area.Name,
                        area.RequiredTitleTier,
                        title.Label,
                        title.MinimumLevel,
                        area.EncounterTemplates
                            .SelectMany(template => template)
                            .GroupBy(spawn => spawn.MonsterId)
                            .Select(group => new HuntCatalogMonsterView(group.Key, group.First().Name))
                            .ToList());
                }).ToList()))
            .ToList();

        return new HuntCatalogResult(regions);
    }
}
