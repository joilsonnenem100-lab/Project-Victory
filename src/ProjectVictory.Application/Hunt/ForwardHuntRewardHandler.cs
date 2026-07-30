using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Hunt;

public sealed class ForwardHuntRewardHandler
{
    private readonly IHuntRepository _huntRepository;

    private readonly ICombatResultStore _combatResultStore;


    public ForwardHuntRewardHandler(
        IHuntRepository huntRepository,
        ICombatResultStore combatResultStore)
    {
        _huntRepository = huntRepository;

        _combatResultStore = combatResultStore;
    }


    public async Task<ForwardHuntRewardResult> Handle(
        ForwardHuntRewardCommand command,
        CancellationToken cancellationToken = default)
    {
        var hunt =
            await _huntRepository.GetByIdAsync(
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


        _combatResultStore.Get(
            command.HuntId);


        return new ForwardHuntRewardResult(
            hunt.Id,
            true);
    }
}