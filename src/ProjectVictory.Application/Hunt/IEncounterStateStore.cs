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
}