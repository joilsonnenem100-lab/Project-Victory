using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Hunt;

public sealed class FinalizeHuntAfterCombatHandler
{
    private readonly IHuntRepository _huntRepository;

    private readonly ICombatResultStore _combatResultStore;

    private readonly CompleteHuntRewardFlowHandler _completeHuntRewardFlowHandler;


    public FinalizeHuntAfterCombatHandler(
        IHuntRepository huntRepository,
        ICombatResultStore combatResultStore,
        CompleteHuntRewardFlowHandler completeHuntRewardFlowHandler)
    {
        _huntRepository = huntRepository;

        _combatResultStore = combatResultStore;

        _completeHuntRewardFlowHandler =
            completeHuntRewardFlowHandler;
    }


    public async Task<FinalizeHuntAfterCombatResult> Handle(
        FinalizeHuntAfterCombatCommand command,
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


        var combatResult =
            _combatResultStore.Get(
                command.HuntId);


        hunt.CompleteAfterCombat();


        await _completeHuntRewardFlowHandler.Handle(
            combatResult,
            cancellationToken);


        return new FinalizeHuntAfterCombatResult(
            hunt.Id,
            true);
    }
}