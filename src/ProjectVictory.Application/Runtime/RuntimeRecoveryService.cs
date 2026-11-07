using ProjectVictory.Application.GameSession;
using ProjectVictory.Application.VerticalSlice;

namespace ProjectVictory.Application.Runtime;

public sealed record RuntimeRecoveryReport(
    int Recovered,
    int SkippedWithoutCharacter,
    // PV-RUNTIME-012 (aditivo): retornos automáticos agendados (DEC-20261106-47) e Runtimes extras
    // do mesmo personagem ignorados (R-056).
    int ReturnsScheduled = 0,
    int SkippedDuplicates = 0);

// PV-RUNTIME-011 (Handbook §18; ADR-057, Parte A simplificada): ao iniciar o servidor, carrega o
// checkpoint do PERSONAGEM e do RUNTIME e reconstrói o contexto em memória. NÃO recupera Hunt Session
// (a Hunt nunca é retomada): todo Runtime volta à tela inicial "Victoria", aguardando comando (Idle).
// PV-RUNTIME-012:
// - reconstrução única com o EnterGame (RuntimeReconstruction; R-055; vida 0 → cheia, DEC-20261106-48);
// - reinício = QUEDA (DEC-20261106-47): quem tem "última Hunt escolhida" fica marcado como caído; ao
//   voltar, depois de MAIS que o ReturnDelay online, o heartbeat inicia uma NOVA Hunt (mesma regra da queda);
// - R-056: personagens lidos em LOTE (sem N+1) e ordem determinística (um Runtime por personagem: o
//   primeiro por RuntimeId, o mesmo que o EnterGame usa).
public sealed class RuntimeRecoveryService
{
    public const int CharacterBatchSize = 200;

    private readonly IRuntimeRepository _runtimes;
    private readonly ICharacterRepository _characters;
    private readonly IRuntimeExecutionStateStore _store;
    private readonly IPresenceTracker _presence;
    private readonly TimeProvider _time;

    public RuntimeRecoveryService(
        IRuntimeRepository runtimes,
        ICharacterRepository characters,
        IRuntimeExecutionStateStore store,
        IPresenceTracker presence,
        TimeProvider time)
    {
        _runtimes = runtimes;
        _characters = characters;
        _store = store;
        _presence = presence;
        _time = time;
    }

    public async Task<RuntimeRecoveryReport> RecoverAsync(CancellationToken cancellationToken = default)
    {
        var recovered = 0;
        var skipped = 0;
        var duplicates = 0;
        var returns = 0;
        var now = _time.GetUtcNow();

        // GetAllAsync vem ordenado (CharacterId, RuntimeId): o primeiro de cada personagem vence.
        var perCharacter = new List<RuntimeRecoveryData>();
        var seen = new HashSet<Guid>();

        foreach (var runtime in await _runtimes.GetAllAsync(cancellationToken))
        {
            if (seen.Add(runtime.CharacterId))
            {
                perCharacter.Add(runtime);
            }
            else
            {
                duplicates++;
            }
        }

        foreach (var batch in perCharacter.Chunk(CharacterBatchSize))
        {
            var characters = (await _characters.GetByIdsAsync(
                    batch.Select(runtime => runtime.CharacterId).ToList(),
                    cancellationToken))
                .ToDictionary(character => character.Id);

            foreach (var runtime in batch)
            {
                // Runtime órfão (personagem inexistente) não é reconstruído.
                if (!characters.TryGetValue(runtime.CharacterId, out var character))
                {
                    skipped++;
                    continue;
                }

                _store.Store(RuntimeReconstruction.FromCheckpoint(runtime, character));
                recovered++;

                // PV-RUNTIME-013 (R-054): quem fez logout antes do reinício continua deslogado e NÃO
                // recebe o retorno automático (DEC-20261106-49 prevalece sobre o DEC-20261106-47).
                if (runtime.LoggedOut)
                {
                    _presence.MarkLoggedOut(character.Id);
                }
                else if (runtime.LastHuntArea is not null)
                {
                    _presence.MarkDroppedByRestart(character.AccountId.Value, character.Id, now);
                    returns++;
                }
            }
        }

        return new RuntimeRecoveryReport(recovered, skipped, returns, duplicates);
    }
}
