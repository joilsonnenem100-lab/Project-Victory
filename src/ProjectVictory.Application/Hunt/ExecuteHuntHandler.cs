using ProjectVictory.Application.Combat;
using ProjectVictory.Application.Progression;
using ProjectVictory.Application.Reward;
using ProjectVictory.Application.Session;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Hunt;

public sealed class ExecuteHuntHandler
{
    private readonly IHuntRepository _huntRepository;

    private readonly ICharacterRepository _characterRepository;

    private readonly IEncounterStateStore _encounterStateStore;

    private readonly EncounterGenerator _encounterGenerator;

    private readonly IRuntimeExecutionStateStore _runtimeExecutionStateStore;

    private readonly PlayerOwnershipGuard _ownershipGuard;

    public ExecuteHuntHandler(
        IHuntRepository huntRepository,
        ICharacterRepository characterRepository,
        IEncounterStateStore encounterStateStore,
        EncounterGenerator encounterGenerator,
        IRuntimeExecutionStateStore runtimeExecutionStateStore,
        PlayerOwnershipGuard ownershipGuard)
    {
        _ownershipGuard = ownershipGuard;

        _huntRepository = huntRepository;

        _characterRepository = characterRepository;

        _encounterStateStore = encounterStateStore;

        _encounterGenerator = encounterGenerator;

        _runtimeExecutionStateStore = runtimeExecutionStateStore;
    }

    public async Task<ExecuteHuntResult> Handle(
        ExecuteHuntCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        // PV-SEC-001: o personagem precisa pertencer à conta autenticada.
        await _ownershipGuard.EnsureCharacterAsync(
            command.CharacterId,
            cancellationToken);

        var character =
            await _characterRepository.GetByIdAsync(
                command.CharacterId,
                cancellationToken);

        if (character is null)
        {
            throw new InvalidOperationException(
                "Character not found.");
        }

        var runtimeContext =
            _runtimeExecutionStateStore.GetByCharacterId(
                character.Id);

        var hunt =
            new Domain.Hunt.Hunt(
                Guid.NewGuid(),
                character.Id,
                runtimeContext.RuntimeId);

        hunt.Start();

        await _huntRepository.AddAsync(
            hunt,
            cancellationToken);

        var encounter =
            _encounterGenerator.Generate(
                hunt.Id);

        _encounterStateStore.Add(
            encounter);

        var target =
            encounter.GetCurrentTarget();

        return new ExecuteHuntResult(
            hunt.Id,
            false,
            0,
            0,
            character.Level.Value,
            new EncounterResult(
                encounter.Id,
                encounter.HuntId,
                encounter.CurrentTargetId,
                target.Name,
                target.CurrentHealth,
                target.MaximumHealth));
    }
}