using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Hunt;

public sealed class ProcessHuntExecutionHandler
{
    private readonly IHuntRepository _huntRepository;

    public ProcessHuntExecutionHandler(
        IHuntRepository huntRepository)
    {
        _huntRepository = huntRepository;
    }

    public async Task<ProcessHuntExecutionResult> Handle(
        ProcessHuntExecutionCommand command,
        CancellationToken cancellationToken = default)
    {
        var hunt = await _huntRepository.GetByIdAsync(
            command.HuntId,
            cancellationToken);

        if (hunt is null)
        {
            throw new InvalidOperationException(
                "Hunt not found.");
        }

        if (hunt.State != HuntState.Running)
        {
            throw new InvalidOperationException(
                "Only running hunts can be processed.");
        }

        hunt.Complete();

        return new ProcessHuntExecutionResult(
            hunt.Id,
            true);
    }
}