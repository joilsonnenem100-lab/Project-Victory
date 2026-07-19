using ProjectVictory.Application.Combat;
using ProjectVictory.Application.Progression;
using ProjectVictory.Application.Reward;
using ProjectVictory.Application.Session;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Hunt;

public sealed class ExecuteHuntHandler
{
    private readonly IHuntRepository _huntRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly ICurrentPlayerContext _currentPlayerContext;
    private readonly ICombatResolver _combatResolver;
    private readonly IRewardProcessor _rewardProcessor;
    private readonly IProgressionProcessor _progressionProcessor;


    public ExecuteHuntHandler(
        IHuntRepository huntRepository,
        ICharacterRepository characterRepository,
        ICurrentPlayerContext currentPlayerContext,
        ICombatResolver combatResolver,
        IRewardProcessor rewardProcessor,
        IProgressionProcessor progressionProcessor)
    {
        _huntRepository = huntRepository;
        _characterRepository = characterRepository;
        _currentPlayerContext = currentPlayerContext;
        _combatResolver = combatResolver;
        _rewardProcessor = rewardProcessor;
        _progressionProcessor = progressionProcessor;
    }


    public async Task<ExecuteHuntResult> Handle(
        ExecuteHuntCommand command,
        CancellationToken cancellationToken = default)
    {
        var accountId =
            _currentPlayerContext.AccountId;


        var character =
            await _characterRepository.GetByAccountIdAsync(
                accountId,
                cancellationToken);


        if (character is null)
        {
            throw new InvalidOperationException(
                "Character not found.");
        }


        var hunt = new Domain.Hunt.Hunt(
            Guid.NewGuid(),
            character.Id);


        hunt.Start();


        await _huntRepository.AddAsync(
            hunt,
            cancellationToken);


        var encounterId =
            Guid.NewGuid();


        var combatResult =
            _combatResolver.Resolve(
                new CombatRequest(
                    character.Id,
                    encounterId));


        if (!combatResult.Completed)
        {
            hunt.Fail();

            return new ExecuteHuntResult(
                hunt.Id,
                false,
                0,
                0,
                character.Level.Value);
        }


        var rewardResult =
            _rewardProcessor.Process(
                new RewardRequest(
                    character.Id,
                    combatResult));


        var progressionResult =
            _progressionProcessor.Process(
                new ProgressionRequest(
                    character.Id,
                    rewardResult.Experience));


        character.GainExperience(
            new ProjectVictory.Domain.ValueObjects.Experience(
                progressionResult.Experience));


        await _characterRepository.UpdateAsync(
            character,
            cancellationToken);


        hunt.Complete();


        return new ExecuteHuntResult(
            hunt.Id,
            true,
            progressionResult.Experience,
            rewardResult.Gold,
            progressionResult.Level);
    }
}