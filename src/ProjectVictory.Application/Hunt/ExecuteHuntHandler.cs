using ProjectVictory.Application.Combat;
using ProjectVictory.Application.Progression;
using ProjectVictory.Application.Reward;
using ProjectVictory.Application.Session;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Hunt;

public sealed class ExecuteHuntHandler
{
    private readonly IHuntRepository _huntRepository;

    private readonly ICharacterRepository _characterRepository;

    private readonly IEncounterStateStore _encounterStateStore;

    private readonly EncounterGenerator _encounterGenerator;



    public ExecuteHuntHandler(
        IHuntRepository huntRepository,
        ICharacterRepository characterRepository,
        IEncounterStateStore encounterStateStore,
        EncounterGenerator encounterGenerator)
    {
        _huntRepository = huntRepository;

        _characterRepository = characterRepository;

        _encounterStateStore = encounterStateStore;

        _encounterGenerator = encounterGenerator;
    }



    public async Task<ExecuteHuntResult> Handle(
        ExecuteHuntCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);



        var character =
            await _characterRepository.GetByIdAsync(
                command.CharacterId,
                cancellationToken);



        if (character is null)
        {
            throw new InvalidOperationException(
                "Character not found.");
        }



        var hunt =
            new Domain.Hunt.Hunt(
                Guid.NewGuid(),
                character.Id,
                command.RuntimeId);



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