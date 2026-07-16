using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Hunt;

public sealed class ForwardHuntRewardHandler
{
    private readonly IHuntRepository _huntRepository;

    public ForwardHuntRewardHandler(
        IHuntRepository huntRepository)
    {
        _huntRepository = huntRepository;
    }

    public async Task<ForwardHuntRewardResult> Handle(
        ForwardHuntRewardCommand command,
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

        if (hunt.State != HuntState.Completed)
        {
            throw new InvalidOperationException(
                "Only completed hunts can be forwarded to reward.");
        }

        return new ForwardHuntRewardResult(
            hunt.Id,
            true);
    }
}