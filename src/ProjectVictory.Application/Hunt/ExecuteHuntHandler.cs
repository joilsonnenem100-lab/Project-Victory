using ProjectVictory.Application.Combat;
using ProjectVictory.Application.Progression;
using ProjectVictory.Application.Reward;
using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Hunt;

public sealed class ExecuteHuntHandler
{
    private readonly IHuntRepository _huntRepository;
    private readonly ICombatResolver _combatResolver;
    private readonly IRewardProcessor _rewardProcessor;
    private readonly IProgressionProcessor _progressionProcessor;

    public ExecuteHuntHandler(
        IHuntRepository huntRepository,
        ICombatResolver combatResolver,
        IRewardProcessor rewardProcessor,
        IProgressionProcessor progressionProcessor)
    {
        _huntRepository = huntRepository;
        _combatResolver = combatResolver;
        _rewardProcessor = rewardProcessor;
        _progressionProcessor = progressionProcessor;
    }

    public async Task<ExecuteHuntResult> Handle(
        ExecuteHuntCommand command,
        CancellationToken cancellationToken = default)
    {
        var hunt = new Domain.Hunt.Hunt(
            Guid.NewGuid(),
            command.CharacterId);

        hunt.Start();

        await _huntRepository.AddAsync(
            hunt,
            cancellationToken);

        var combatResult = _combatResolver.Resolve(
            new CombatRequest(
                command.CharacterId,
                command.EnemyId));

        if (!combatResult.Completed)
        {
            hunt.Fail();

            return new ExecuteHuntResult(
                hunt.Id,
                false,
                0,
                0,
                0);
        }

        var rewardResult = _rewardProcessor.Process(
            new RewardRequest(
                command.CharacterId,
                combatResult));

        var progressionResult = _progressionProcessor.Process(
            new ProgressionRequest(
                command.CharacterId,
                rewardResult.Experience));

        hunt.Complete();

        return new ExecuteHuntResult(
            hunt.Id,
            true,
            progressionResult.Experience,
            rewardResult.Gold,
            progressionResult.Level);
    }
}