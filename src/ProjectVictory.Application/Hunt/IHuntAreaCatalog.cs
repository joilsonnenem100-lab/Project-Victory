namespace ProjectVictory.Application.Hunt;

// PV-CONTENT-011: um monstro a nascer num encounter.
public sealed record EncounterSpawn(
    Guid MonsterId,
    string Name,
    int MaximumHealth);

// PV-CONTENT-011: área de caça e seus encounters possíveis, definidos por dados.
// PV-PROG-012 (DEC-20261106-19): cada área declara a Ascension mínima (dados). O jogador pode caçar
// em qualquer área com RequiredAscension ≤ a sua Ascension (nova ou antigas).
public sealed record HuntAreaDefinition(
    string Name,
    IReadOnlyList<IReadOnlyList<EncounterSpawn>> EncounterTemplates,
    int RequiredAscension = 1);

public interface IHuntAreaCatalog
{
    // A área padrão da primeira Hunt (Spider Cave, DEC-20261106-09).
    HuntAreaDefinition GetDefault();

    // PV-PROG-012: todas as áreas de caça (conteúdo por dados; expansível por Ascension).
    IReadOnlyList<HuntAreaDefinition> GetAll() => [GetDefault()];

    // PV-PROG-012: áreas disponíveis para a Ascension do jogador (a nova e as antigas).
    IReadOnlyList<HuntAreaDefinition> GetAvailable(int ascension) =>
        GetAll().Where(area => area.RequiredAscension <= ascension).ToList();
}
