using ProjectVictory.Application.Progression;

namespace ProjectVictory.Application.Reward;

public sealed class RefineRewardProgressionFlowHandler
{
    private readonly IProgressionProcessor _progressionProcessor;

    public RefineRewardProgressionFlowHandler(
        IProgressionProcessor progressionProcessor)
    {
        _progressionProcessor = progressionProcessor;
    }

    public RefineRewardProgressionFlowResult Handle(
        RefineRewardProgressionFlowCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.CharacterId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "CharacterId is required.");
        }

        var request = new ProgressionRequest(
            command.CharacterId,
            0);

        _progressionProcessor.Process(request);

        return new RefineRewardProgressionFlowResult(
            command.CharacterId,
            true);
    }
}