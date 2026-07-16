using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Hunt;

public sealed class FinalizeHuntHandler
{
    private readonly IHuntRepository _huntRepository;

    public FinalizeHuntHandler(
        IHuntRepository huntRepository)
    {
        _huntRepository = huntRepository;
    }

    public async Task<FinalizeHuntResult> Handle(
        FinalizeHuntCommand command,
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

        if (hunt.State != HuntState.Completed &&
            hunt.State != HuntState.Failed)
        {
            throw new InvalidOperationException(
                "Only resolved hunts can be finalized.");
        }

        return new FinalizeHuntResult(
            hunt.Id,
            true);
    }
}