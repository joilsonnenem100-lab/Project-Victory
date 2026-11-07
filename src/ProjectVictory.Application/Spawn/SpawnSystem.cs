using ProjectVictory.Application.Hunt;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Domain.Primitives;
using ProjectVictory.Domain.Spawn;

namespace ProjectVictory.Application.Spawn;

// PV-SPAWN-010 (spawn.md "Spawn Context"): contexto IMUTÁVEL de uma operação de spawn.
// A configuração de spawn é da Hunt (ADR-010): a área de caça define o Monster Pool / packs.
public sealed record SpawnContext(
    Guid HuntId,
    HuntAreaDefinition HuntArea,
    int Seed);

// PV-SPAWN-010 (spawn.md "Spawn Result"): resultado TEMPORÁRIO (DTO) — as instâncias criadas e o
// estado de encounter de cada monstro. Nada disso é persistido (ADR-009/047).
public sealed record SpawnResult(
    IReadOnlyList<SpawnInstance> Instances,
    IReadOnlyList<EncounterMonsterState> Monsters);

// PV-SPAWN-010 (ADR-009/010/047): Spawn System — STATELESS, dirigido pela Hunt. Só interpreta a
// configuração de spawn da Hunt e executa o ciclo de vida; não faz combate, IA, loot nem XP.
public interface ISpawnSystem
{
    // Cria um pack sob demanda a partir da configuração da Hunt (Created → Spawning → Active).
    SpawnResult SpawnPack(SpawnContext context);

    // Encerra as instâncias (Active → Despawning → Despawned); depois são descartadas.
    void Despawn(IEnumerable<SpawnInstance> instances);
}

public sealed class StatelessSpawnSystem : ISpawnSystem
{
    private readonly TimeProvider _time;
    private readonly ProjectVictory.Application.Events.IEventBus? _events;

    public StatelessSpawnSystem()
        : this(TimeProvider.System)
    {
    }

    public StatelessSpawnSystem(TimeProvider time)
    {
        _time = time ?? throw new ArgumentNullException(nameof(time));
    }

    // PV-EVENT-011 (spawn.md "Eventos"; fecha o R-046): publica o ciclo de vida como Domain Events.
    public StatelessSpawnSystem(TimeProvider time, ProjectVictory.Application.Events.IEventBus events)
        : this(time)
    {
        _events = events ?? throw new ArgumentNullException(nameof(events));
    }

    // O pack é escolhido entre os packs da área com um gerador PRÓPRIO criado da semente do encounter
    // (o mesmo sorteio de antes, no EncounterGenerator — composição idêntica para a mesma semente, e
    // nenhum número do combate é consumido).
    public SpawnResult SpawnPack(SpawnContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context.HuntId == Guid.Empty)
        {
            throw new ArgumentException("HuntId is required.", nameof(context));
        }

        var packs = context.HuntArea.EncounterTemplates;

        if (packs.Count == 0)
        {
            throw new InvalidOperationException("Hunt area has no encounters.");
        }

        var pick = new SeededRandomSource(context.Seed).NextInclusive(0, packs.Count - 1);
        var now = _time.GetUtcNow().UtcDateTime;
        var instances = new List<SpawnInstance>();
        var monsters = new List<EncounterMonsterState>();

        foreach (var spawn in packs[pick])
        {
            var instance = new SpawnInstance(Guid.NewGuid(), spawn.MonsterId);
            instance.BeginSpawning();

            // Revisão do PV-SPAWN-010: o monstro do encounter usa o MESMO id da Spawn Instance, para o
            // Hunt Loop (M6) despawnar a instância do monstro certo.
            monsters.Add(new EncounterMonsterState(
                instance.SpawnInstanceId,
                spawn.MonsterId,
                spawn.Name,
                spawn.MaximumHealth));

            instance.Activate(now);
            instances.Add(instance);
            _events?.Publish(new ProjectVictory.Domain.Events.SpawnInstanceActivatedEvent(
                instance.SpawnInstanceId, instance.SpawnDefinitionId, context.HuntId, now));
        }

        return new SpawnResult(instances, monsters);
    }

    public void Despawn(IEnumerable<SpawnInstance> instances)
    {
        ArgumentNullException.ThrowIfNull(instances);

        var now = _time.GetUtcNow().UtcDateTime;

        foreach (var instance in instances)
        {
            instance.BeginDespawning();
            instance.Despawn(now);
            _events?.Publish(new ProjectVictory.Domain.Events.SpawnInstanceDespawnedEvent(
                instance.SpawnInstanceId, instance.SpawnDefinitionId, now));
        }
    }
}
