using System;
using System.Threading.Tasks;
using ProjectVictory.Domain.Character;

namespace ProjectVictory.Application.Character;

public sealed class ExecuteCharacterEvolutionFlowHandler
{
    private readonly ValidateEvolutionHandler _validateHandler;
    private readonly ApplyEvolutionHandler _applyHandler;

    public ExecuteCharacterEvolutionFlowHandler(
        ICharacterRepository characterRepository)
    {
        _validateHandler =
            new ValidateEvolutionHandler(
                characterRepository);

        _applyHandler =
            new ApplyEvolutionHandler(
                characterRepository);
    }

    public async Task<ExecuteCharacterEvolutionFlowResult> Handle(
        ExecuteCharacterEvolutionFlowCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var evolutionCommand =
            new CreateEvolutionCommand(
                command.CharacterId,
                Guid.Empty,
                true);

        var validation =
            await _validateHandler.Handle(
                evolutionCommand);

        if (!validation.Accepted)
        {
            return new ExecuteCharacterEvolutionFlowResult(
                command.CharacterId,
                false);
        }

        var result =
            await _applyHandler.Handle(
                evolutionCommand);

        return new ExecuteCharacterEvolutionFlowResult(
            result.CharacterId,
            result.Applied);
    }
}