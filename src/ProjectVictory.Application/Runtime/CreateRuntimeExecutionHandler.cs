using System;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Runtime;

namespace ProjectVictory.Application.VerticalSlice;

public sealed class CreateRuntimeExecutionHandler
{
    private readonly IRuntimeExecutionStateStore _runtimeExecutionStateStore;

    private readonly ICharacterRepository _characterRepository;


    public CreateRuntimeExecutionHandler(
        IRuntimeExecutionStateStore runtimeExecutionStateStore,
        ICharacterRepository characterRepository)
    {
        _runtimeExecutionStateStore =
            runtimeExecutionStateStore;

        _characterRepository =
            characterRepository;
    }


    public async Task<CreateRuntimeExecutionResult> Handle(
        CreateRuntimeExecutionCommand command,
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


        var characterState =
            new CharacterRuntimeState(
                character.MaximumHealth.Value,
                character.MaximumMana.Value);


        var context =
            new RuntimeExecutionContext(
                command.RuntimeId,
                command.CharacterId,
                true,
                characterState);

        Console.WriteLine(
                $"STORE Runtime={context.RuntimeId} Character={context.CharacterId}");

        _runtimeExecutionStateStore.Store(
            context);



        return new CreateRuntimeExecutionResult(
            context.RuntimeId,
            context.CharacterId,
            context.Initialized);
    }
}