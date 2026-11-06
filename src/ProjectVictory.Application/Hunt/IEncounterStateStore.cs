using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Hunt;

public interface IEncounterStateStore
{
    void Add(
        ActiveEncounter encounter);


    ActiveEncounter Get(
        Guid encounterId);


    void Remove(
        Guid encounterId);

    // PV-HUNT-011 (PV-STATE-001 D2): descarta os encounters de uma Hunt encerrada; devolve os ids.
    IReadOnlyList<Guid> RemoveByHunt(
        Guid huntId);

    bool Contains(
        Guid encounterId);
}