using System;
using System.Threading.Tasks;
using ProjectVictory.Domain.Character;

namespace ProjectVictory.Application.Character;

public sealed class ValidateCharacterEvolutionFlowHandler
{
    private readonly ExecuteCharacterEvolutionFlowHandler _handler;

    public ValidateCharacterEvolutionFlowHandler(
        ICharacterRepository characterRepository)
    {
        _handler =
            new ExecuteCharacterEvolutionFlowHandler(
                characterRepository);
    }

    public async Task<ValidateCharacterEvolutionFlowResult> Handle(
        ValidateCharacterEvolutionFlowCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var result =
            await _handler.Handle(
                new ExecuteCharacterEvolutionFlowCommand(
                    command.CharacterId));

        return new ValidateCharacterEvolutionFlowResult(
            result.CharacterId,
            result.Completed);
    }
}