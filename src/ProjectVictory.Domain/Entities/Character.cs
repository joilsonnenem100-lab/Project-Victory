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

    // PV-CHAR-010: Vida e Mana máximas derivadas de Class + Level pela Attribute Curve (ADR-029),
    // pelo mesmo padrão do Level derivado (ADR-027). Personagem sem definição de classe (construtor
    // antigo) mantém o máximo recebido na criação.
    public Health MaximumHealth =>
        _classDefinition is null
            ? _fixedMaximumHealth
            : new Health(
                AttributeCurveTable.ForClass(CharacterClass)
                    .MaximumHealthAt(Level.Value));

    public Mana MaximumMana =>
        _classDefinition is null
            ? _fixedMaximumMana
            : new Mana(
                AttributeCurveTable.ForClass(CharacterClass)
                    .MaximumManaAt(Level.Value));

    private readonly Health _fixedMaximumHealth;

    private readonly Mana _fixedMaximumMana;

    public CharacterExperience Experience { get; private set; }

    public const int FirstAscension = ExperienceTable.FirstAscension;

    // PV-PROG-010/012: Level derivado da Experience pela curva do Tibia, SEM teto (DEC-20261106-19).
    // A curva é a mesma em todas as Ascensions (DEC-20261106-11). Não é fonte de verdade.
    public CharacterLevel Level =>
        new(ExperienceTable.Standard.GetLevel(Experience.Value));

    // PV-PROG-013 (DEC-20261106-30; substitui a Ascension da Fila 4): título e tier DERIVADOS do
    // Level (não persistidos). Tabela padrão: Novice (0), Ascension 1–4 (1–4), Legend (5). Com a
    // tabela da configuração do servidor, use TitleUnder(table) (ILevelTitleProvider).
    public LevelTitle Title => TitleUnder(LevelTitleTable.Default);

    public LevelTitle TitleUnder(LevelTitleTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        return table.For(Level.Value);
    }

    public Inventory Inventory { get; }

    public Equipment Equipment { get; }

    // PV-CHAR-012 (DEC-20261106-26): as skills do Tibia (Magic Level, Fist, Club, Sword, Axe, Distance,
    // Shielding, Fishing). Não há outros atributos (a classe Attributes foi removida, DEC-20261106-27).
    public Skills Skills { get; }

    private readonly CharacterClassDefinition? _classDefinition;

    public Character(
        Guid id,
        AccountId accountId,
        string name,
        Health health,
        Mana mana,
        CharacterExperience experience)
        : base(id)
    {
        ArgumentNullException.ThrowIfNull(accountId);
        ArgumentNullException.ThrowIfNull(health);
        ArgumentNullException.ThrowIfNull(mana);
        ArgumentNullException.ThrowIfNull(experience);

        AccountId = accountId;
        Name = name;
        Health = health;
        Mana = mana;
        _fixedMaximumHealth = health;
        _fixedMaximumMana = mana;
        Experience = experience;

        Inventory = new Inventory();
        Equipment = new Equipment();
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
        CharacterExperience experience)
        : base(id)
    {
        ArgumentNullException.ThrowIfNull(accountId);
        ArgumentNullException.ThrowIfNull(definition);
        ArgumentNullException.ThrowIfNull(health);
        ArgumentNullException.ThrowIfNull(mana);
        ArgumentNullException.ThrowIfNull(experience);

        AccountId = accountId;
        Name = name;
        CharacterClass = characterClass;
        InitialAbility = definition.InitialAbility;
        _classDefinition = definition;

        Health = health;
        Mana = mana;
        _fixedMaximumHealth = health;
        _fixedMaximumMana = mana;

        Experience = experience;

        Inventory = new Inventory();
        Equipment = new Equipment();
        Skills = new Skills();
    }

    public void GainExperience(CharacterExperience experience)
    {
        ArgumentNullException.ThrowIfNull(experience);

        Experience =
            new CharacterExperience(
                checked(Experience.Value + experience.Value));
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

        // PV-PROG-010: o Level é derivado da Experience (ADR-027) e não pode ser definido.
        // ApplyLevel só confirma o Level atual. O crescimento de Vida/Mana máximas por Level
        // passa a ser atributo derivado (ADR-029), fora do escopo do M2 (ver project-state.md).
        if (requestedLevel != Level.Value)
        {
            throw new InvalidOperationException(
                "Level is derived from Experience (ADR-027) and cannot be set directly.");
        }
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
