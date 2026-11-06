namespace ProjectVictory.Application.Hunt;

// PV-CONTENT-011: um monstro a nascer num encounter.
public sealed record EncounterSpawn(
    Guid MonsterId,
    string Name,
    int MaximumHealth);

// PV-CONTENT-011: área de caça e seus encounters possíveis, definidos por dados.
public sealed record HuntAreaDefinition(
    string Name,
    IReadOnlyList<IReadOnlyList<EncounterSpawn>> EncounterTemplates);

public interface IHuntAreaCatalog
{
    // A área padrão da primeira Hunt (Spider Cave, DEC-20261106-09).
    HuntAreaDefinition GetDefault();
}
