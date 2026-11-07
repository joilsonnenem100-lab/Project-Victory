using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Items;

namespace ProjectVictory.Application.Reward;

public sealed class ApplyRewardHandler
{
    private readonly IRewardProcessor _rewardProcessor;
    private readonly ICharacterRepository _characterRepository;
    private readonly RewardSettings _settings;
    private readonly IExperienceDistributor _experienceDistributor;
    private readonly IExperienceRateProvider? _experienceRateProvider;

    public ApplyRewardHandler(
        IRewardProcessor rewardProcessor,
        ICharacterRepository characterRepository,
        RewardSettings settings)
        : this(rewardProcessor, characterRepository, settings, EvenSplitExperienceDistributor.Instance)
    {
    }

    // PV-XP-010 (ADR-007): a Experience final passa pela Experience Distribution.
    public ApplyRewardHandler(
        IRewardProcessor rewardProcessor,
        ICharacterRepository characterRepository,
        RewardSettings settings,
        IExperienceDistributor experienceDistributor,
        // PV-ADMIN-010 (aditivo, opcional): XP rate efetivo; sem provedor, o da configuração (como antes).
        IExperienceRateProvider? experienceRateProvider = null)
    {
        _experienceRateProvider = experienceRateProvider;
        _experienceDistributor = experienceDistributor ?? throw new ArgumentNullException(nameof(experienceDistributor));
        _rewardProcessor = rewardProcessor;
        _characterRepository = characterRepository;
        _settings = settings;
    }

    public async Task<ApplyRewardResult> Handle(
        ApplyRewardCommand command,
        CancellationToken cancellationToken = default)
    {
        var character =
            await _characterRepository.GetByIdAsync(
                command.CharacterId,
                cancellationToken);

        if (character is null)
        {
            throw new InvalidOperationException(
                "Character not found.");
        }

        var reward =
            _rewardProcessor.Process(
                command.Request);

        // PV-REWARD-010: XP × XP rate do servidor (DEC-20261106-12); o Reward Pipeline é o
        // único que muda Experience (ADR-024).
        var rate = _experienceRateProvider is null
            ? _settings.ExperienceRate
            : await _experienceRateProvider.GetCurrentAsync(cancellationToken);

        var experience =
            rate.Apply(
                Math.Max(0, reward.Experience));

        // PV-XP-010: este handler aplica a recompensa a UM personagem; a distribuição com 1 participante
        // devolve o total (mesmo XP de antes).
        if (experience > 0)
        {
            foreach (var share in _experienceDistributor.Distribute(experience, [character.Id]))
            {
                // Revisão do PV-XP-010: este handler serve a UM participante; parte de outro personagem
                // é erro de configuração (nunca aplicar a Experience no personagem errado).
                if (share.CharacterId != character.Id)
                {
                    throw new InvalidOperationException(
                        "ApplyRewardHandler applies experience to a single participant.");
                }

                // Parte zero (ex.: 1 XP dividido entre 3) não altera nada.
                if (share.Amount > 0)
                {
                    character.GainExperience(
                        new ProjectVictory.Domain.ValueObjects.Experience(
                            share.Amount));
                }
            }
        }

        // PV-REWARD-010: drops e ouro chegam ao Inventory do personagem (lacuna do PV-QA-001).
        foreach (var drop in reward.Drops)
        {
            character.Inventory.AddItem(
                new ItemInstance(
                    Guid.NewGuid(),
                    drop.ItemDefinitionId));
        }

        if (reward.Gold > 0)
        {
            character.Inventory.AddGold(
                reward.Gold);
        }

        await _characterRepository.UpdateAsync(
            character,
            cancellationToken);

        return new ApplyRewardResult(
            character.Id,
            true);
    }
}
