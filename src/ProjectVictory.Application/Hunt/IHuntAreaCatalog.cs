namespace ProjectVictory.Application.Hunt;

// PV-CONTENT-011: um monstro a nascer num encounter.
public sealed record EncounterSpawn(
    Guid MonsterId,
    string Name,
    int MaximumHealth);

// PV-CONTENT-011: área de caça e seus encounters possíveis, definidos por dados.
// PV-PROG-013 (DEC-20261106-30): cada área declara o TIER mínimo (dados; 0 = Novice … 5 = Legend).
// O jogador pode caçar em qualquer área com RequiredTitleTier ≤ o seu tier (o novo e os anteriores).
// Substitui o RequiredAscension da Fila 4 (PV-PROG-012).
public sealed record HuntAreaDefinition(
    string Name,
    IReadOnlyList<IReadOnlyList<EncounterSpawn>> EncounterTemplates,
    int RequiredTitleTier = 0);

public interface IHuntAreaCatalog
{
    // A área padrão da primeira Hunt (Spider Cave, DEC-20261106-09).
    HuntAreaDefinition GetDefault();

    // PV-PROG-012/013: todas as áreas de caça (conteúdo por dados; expansível por tier).
    IReadOnlyList<HuntAreaDefinition> GetAll() => [GetDefault()];

    // PV-PROG-013: áreas disponíveis para o tier do jogador (o novo e os anteriores).
    IReadOnlyList<HuntAreaDefinition> GetAvailable(int tier) =>
        GetAll().Where(area => area.RequiredTitleTier <= tier).ToList();
}
