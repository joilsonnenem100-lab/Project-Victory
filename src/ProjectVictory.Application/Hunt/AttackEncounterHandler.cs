using ProjectVictory.Application.Progression;
using ProjectVictory.Application.Reward;
using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Hunt;

namespace ProjectVictory.Application.Hunt;

public sealed class AttackEncounterHandler
{
    private readonly IEncounterStateStore _encounterStateStore;

    private readonly IHuntRepository _huntRepository;

    private readonly ICharacterRepository _characterRepository;

    private readonly IRewardProcessor _rewardProcessor;

    private readonly IProgressionProcessor _progressionProcessor;

    private readonly IRuntimeExecutionStateStore _runtimeExecutionStateStore;


    private const int AttackDamage = 5;



    public AttackEncounterHandler(
        IEncounterStateStore encounterStateStore,
        IHuntRepository huntRepository,
        ICharacterRepository characterRepository,
        IRewardProcessor rewardProcessor,
        IProgressionProcessor progressionProcessor,
        IRuntimeExecutionStateStore runtimeExecutionStateStore)
    {
        _encounterStateStore =
            encounterStateStore;

        _huntRepository =
            huntRepository;

        _characterRepository =
            characterRepository;

        _rewardProcessor =
            rewardProcessor;

        _progressionProcessor =
            progressionProcessor;

        _runtimeExecutionStateStore =
            runtimeExecutionStateStore;
    }



    public async Task<AttackEncounterResult> Handle(
        AttackEncounterCommand command,
        CancellationToken cancellationToken = default)
    {
        var encounter =
            _encounterStateStore.Get(
                command.EncounterId);



        var hunt =
            await _huntRepository.GetByIdAsync(
                encounter.HuntId,
                cancellationToken);



        if (hunt is null)
        {
            throw new InvalidOperationException(
                "Hunt not found.");
        }



        var runtimeContext =
            _runtimeExecutionStateStore.Get(
                hunt.RuntimeId);



        var characterState =
            runtimeContext.CharacterState;



        if (characterState is null)
        {
            throw new InvalidOperationException(
                "Character runtime state not initialized.");
        }



        if (encounter.IsFinished())
        {
            throw new InvalidOperationException(
                "Combat already finished.");
        }



        encounter.Activate();



        var target =
            encounter.GetCurrentTarget();



        target.TakeDamage(
            AttackDamage);



        var monsterDefeated =
            target.IsDead;



        int experience = 0;

        int gold = 0;

        int level = 0;

        IReadOnlyList<RewardDrop> drops =
            [];



        if (monsterDefeated)
        {
            encounter.Victory();



            var character =
                await _characterRepository.GetByIdAsync(
                    hunt.CharacterId,
                    cancellationToken);



            if (character is null)
            {
                throw new InvalidOperationException(
                    "Character not found.");
            }



            var rewardResult =
                _rewardProcessor.Process(
                    new RewardRequest(
                        character.Id,
                        target));



            experience =
                rewardResult.Experience;



            gold =
                rewardResult.Gold;



            drops =
                rewardResult.Drops;



            var progressionResult =
                _progressionProcessor.Process(
                    new ProgressionRequest(
                        character.Id,
                        experience));



            character.GainExperience(
                new ProjectVictory.Domain.ValueObjects.Experience(
                    progressionResult.Experience));



            await _characterRepository.UpdateAsync(
                character,
                cancellationToken);



            level =
                progressionResult.Level;
        }



        return new AttackEncounterResult(
            encounter.Id,
            target.Name,
            target.CurrentHealth,
            target.MaximumHealth,
            monsterDefeated,
            characterState.CurrentHealth,
            characterState.CurrentMana,
            false,
            encounter.IsFinished(),
            encounter.State.ToString(),
            experience,
            gold,
            level,
            drops);
    }
}