using System;
using System.Collections.Generic;
using System.Linq;

using ProjectVictory.Domain.Account;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Primitives;
using ProjectVictory.Domain.Progression;
using ProjectVictory.Domain.ValueObjects;

using CharacterExperience = ProjectVictory.Domain.ValueObjects.Experience;
using CharacterLevel = ProjectVictory.Domain.ValueObjects.Level;

namespace ProjectVictory.Domain.Entities;

public sealed class Character : AggregateRoot
{
    public AccountId AccountId { get; private set; }

    public string Name { get; private set; }

    public CharacterClass CharacterClass { get; private set; }

    public CombatAbility InitialAbility { get; private set; }

    public IReadOnlyList<CombatAbility> AvailableAbilities =>
        InitialAbility is null
            ? Array.Empty<CombatAbility>()
            : new[]
            {
                InitialAbility
            };

    public Health Health { get; private set; }

    public Mana Mana { get; private set; }

    public Health MaximumHealth { get; private set; }

    public Mana MaximumMana { get; private set; }

    public CharacterExperience Experience { get; private set; }

    public CharacterLevel Level { get; private set; }

    public Inventory Inventory { get; }

    public Equipment Equipment { get; }

    public Attributes Attributes { get; }

    public Skills Skills { get; }

    private readonly CharacterClassDefinition? _classDefinition;

    public Character(
        Guid id,
        AccountId accountId,
        string name,
        Health health,
        Mana mana,
        CharacterExperience experience,
        CharacterLevel level)
        : base(id)
    {
        ArgumentNullException.ThrowIfNull(accountId);
        ArgumentNullException.ThrowIfNull(health);
        ArgumentNullException.ThrowIfNull(mana);
        ArgumentNullException.ThrowIfNull(experience);
        ArgumentNullException.ThrowIfNull(level);

        AccountId = accountId;
        Name = name;
        Health = health;
        Mana = mana;
        MaximumHealth = health;
        MaximumMana = mana;
        Experience = experience;
        Level = level;

        Inventory = new Inventory();
        Equipment = new Equipment();
        Attributes = new Attributes();
        Skills = new Skills();

        CharacterClass = default;
        InitialAbility = null!;
        _classDefinition = null;
    }

    public Character(
        Guid id,
        AccountId accountId,
        string name,
        CharacterClass characterClass,
        CharacterClassDefinition definition,
        Health health,
        Mana mana,
        CharacterExperience experience,
        CharacterLevel level)
        : base(id)
    {
        ArgumentNullException.ThrowIfNull(accountId);
        ArgumentNullException.ThrowIfNull(definition);
        ArgumentNullException.ThrowIfNull(health);
        ArgumentNullException.ThrowIfNull(mana);
        ArgumentNullException.ThrowIfNull(experience);
        ArgumentNullException.ThrowIfNull(level);

        AccountId = accountId;
        Name = name;
        CharacterClass = characterClass;
        InitialAbility = definition.InitialAbility;
        _classDefinition = definition;

        Health = health;
        Mana = mana;
        MaximumHealth = health;
        MaximumMana = mana;

        Experience = experience;
        Level = level;

        Inventory = new Inventory();
        Equipment = new Equipment();
        Attributes = new Attributes();
        Skills = new Skills();
    }

    public void GainExperience(CharacterExperience experience)
    {
        ArgumentNullException.ThrowIfNull(experience);

        Experience =
            new CharacterExperience(
                Experience.Value + experience.Value);
    }

    public LevelValidationResult ValidateLevel(int requestedLevel)
    {
        if (Id == Guid.Empty || requestedLevel <= 0 || Level is null)
        {
            return new LevelValidationResult(Id, false);
        }

        return new LevelValidationResult(Id, true);
    }

    public void ApplyLevel(int requestedLevel)
    {
        var validation = ValidateLevel(requestedLevel);

        if (!validation.Accepted)
        {
            throw new InvalidOperationException(
                "Character level validation failed.");
        }

        var levelsGained =
            requestedLevel - Level.Value;

        if (levelsGained > 0 && _classDefinition is not null)
        {
            MaximumHealth =
                new Health(
                    MaximumHealth.Value +
                    (_classDefinition.HealthGrowth * levelsGained));

            MaximumMana =
                new Mana(
                    MaximumMana.Value +
                    (_classDefinition.ManaGrowth * levelsGained));
        }

        Level = new CharacterLevel(requestedLevel);
    }

    public EvolutionValidationResult ValidateEvolution()
    {
        if (Id == Guid.Empty || Experience is null || Level is null)
        {
            return new EvolutionValidationResult(Id, false);
        }

        return new EvolutionValidationResult(Id, true);
    }

    public void ApplyEvolution()
    {
        var validation = ValidateEvolution();

        if (!validation.Accepted)
        {
            throw new InvalidOperationException(
                "Character evolution validation failed.");
        }
    }

    public EvolutionApplicationValidationResult ValidateCompositeEvolution(
        IReadOnlyCollection<EvolutionEffect> effects)
    {
        if (effects is null || effects.Count == 0)
        {
            return new EvolutionApplicationValidationResult(Id, false);
        }

        return new EvolutionApplicationValidationResult(Id, true);
    }

    public void ApplyCompositeEvolution(
        IReadOnlyCollection<EvolutionEffect> effects)
    {
        var validation = ValidateCompositeEvolution(effects);

        if (!validation.Accepted)
        {
            throw new InvalidOperationException(
                "Composite evolution validation failed.");
        }
    }

    public void ApplyProgressionGrowth(
        GrowthResult growthResult)
    {
        ArgumentNullException.ThrowIfNull(growthResult);

        if (growthResult.CharacterId != Id)
        {
            throw new InvalidOperationException(
                "Growth result does not belong to this Character.");
        }

        ApplyLevel(growthResult.CurrentLevel);
    }
}
