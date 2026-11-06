using System.Collections.Concurrent;

using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Hunt;

public sealed class EncounterStateStore : IEncounterStateStore
{
    private readonly ConcurrentDictionary<Guid, ActiveEncounter> _encounters =
        new();



    public void Add(
        ActiveEncounter encounter)
    {
        ArgumentNullException.ThrowIfNull(
            encounter);


        _encounters[encounter.Id] =
            encounter;


        Console.WriteLine(
            $"[EncounterStateStore] Added Encounter: {encounter.Id}");


        Console.WriteLine(
            $"[EncounterStateStore] Total Encounters: {_encounters.Count}");
    }



    public ActiveEncounter Get(
        Guid encounterId)
    {
        Console.WriteLine(
            $"[EncounterStateStore] Requested Encounter: {encounterId}");


        Console.WriteLine(
            $"[EncounterStateStore] Available Encounters: {string.Join(", ", _encounters.Keys)}");



        if (!_encounters.TryGetValue(
                encounterId,
                out var encounter))
        {
            throw new InvalidOperationException(
                "Encounter not found.");
        }


        return encounter;
    }



    public void Remove(
        Guid encounterId)
    {
        _encounters.TryRemove(
            encounterId,
            out _);


        Console.WriteLine(
            $"[EncounterStateStore] Removed Encounter: {encounterId}");
    }

    public IReadOnlyList<Guid> RemoveByHunt(
        Guid huntId)
    {
        var removed = new List<Guid>();

        foreach (var (id, encounter) in _encounters)
        {
            if (encounter.HuntId == huntId && _encounters.TryRemove(id, out _))
            {
                removed.Add(id);
            }
        }

        return removed;
    }

    public bool Contains(
        Guid encounterId) =>
        _encounters.ContainsKey(encounterId);

    public int Count => _encounters.Count;
}
