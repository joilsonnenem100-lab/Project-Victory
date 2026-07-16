namespace ProjectVictory.Application.Progression;

public sealed class ValidateProgressionPipelineHandler
{
    public ValidateProgressionPipelineResult Handle(
        ValidateProgressionPipelineCommand command)
    {
        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        return new ValidateProgressionPipelineResult(
            command.CharacterId,
            command.CharacterApplied);
    }
}