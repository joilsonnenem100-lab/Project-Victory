using ProjectVictory.Application.Combat;
using ProjectVictory.Application.Combat.TurnBased;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Combat.Rules;
using ProjectVictory.Domain.Combat.TurnBased;
using ProjectVictory.Domain.Primitives;
using Xunit;

namespace ProjectVictory.Application.Tests;

// PV-COMBAT-013 (DEC-20261106-23): 4 skills por classe em dados, seletor automático PROVISÓRIO,
// skill em área atinge todos os inimigos vivos, determinismo com semente.
public sealed class ClassSkillTests
{
    public static TheoryData<CharacterClass> Classes => new()
    {
        CharacterClass.Knight, CharacterClass.Paladin, CharacterClass.Sorcerer, CharacterClass.Druid, CharacterClass.Monk
    };

    [Theory]
    [MemberData(nameof(Classes))]
    public void EachClass_HasTwoAreaAndTwoSingleTargetSkills_PhysicalDamage(CharacterClass characterClass)
    {
        var skills = ClassSkillTable.ForClass(characterClass);

        Assert.Equal(4, skills.Count);
        Assert.Equal(2, skills.Count(s => s.Targeting == SkillTargeting.Area));
        Assert.Equal(2, skills.Count(s => s.Targeting == SkillTargeting.SingleTarget));
        Assert.All(skills, s => Assert.Equal(DamageType.Physical, s.DamageType));
        Assert.All(skills, s => Assert.True(s.ManaCost > 0 && s.DamageMultiplier > 0 && s.UnlockLevel >= 1));
        Assert.Equal(skills.Count, skills.Select(s => s.Id).Distinct().Count());
    }

    [Fact]
    public void PoRules_AreReflectedInTheData()
    {
        var knight = ClassSkillTable.ForClass(CharacterClass.Knight);
        var sorcerer = ClassSkillTable.ForClass(CharacterClass.Sorcerer);

        // Knight: área de dano baixo < alvo único de dano médio; ataque básico forte.
        Assert.True(knight.Where(s => s.Targeting == SkillTargeting.Area).Max(s => s.DamageMultiplier)
                    < knight.Where(s => s.Targeting == SkillTargeting.SingleTarget).Min(s => s.DamageMultiplier));
        Assert.True(ClassSkillTable.BasicAttackMultiplier(CharacterClass.Knight) > 1m);

        // Paladin: ataque básico à distância MUITO forte — o maior de todas as classes.
        Assert.Equal(
            Enum.GetValues<CharacterClass>().Select(ClassSkillTable.BasicAttackMultiplier).Max(),
            ClassSkillTable.BasicAttackMultiplier(CharacterClass.Paladin));

        // Sorcerer: dano alto — maior multiplicador médio entre as classes; escala por Magic Level.
        var sorcererAverage = sorcerer.Average(s => s.DamageMultiplier);
        foreach (var other in new[] { CharacterClass.Knight, CharacterClass.Paladin, CharacterClass.Druid, CharacterClass.Monk })
        {
            Assert.True(sorcererAverage > ClassSkillTable.ForClass(other).Average(s => s.DamageMultiplier));
        }

        Assert.All(sorcerer, s => Assert.Equal(SkillScaling.MagicLevel, s.Scaling));
        Assert.All(ClassSkillTable.ForClass(CharacterClass.Druid), s => Assert.Equal(SkillScaling.MagicLevel, s.Scaling));
        Assert.All(ClassSkillTable.ForClass(CharacterClass.Monk), s => Assert.Equal(SkillScaling.FistFighting, s.Scaling));
    }

    [Fact]
    public void Unlock_ByLevel()
    {
        Assert.Single(ClassSkillTable.UnlockedFor(CharacterClass.Knight, 1));
        Assert.Equal(2, ClassSkillTable.UnlockedFor(CharacterClass.Knight, 5).Count);
        Assert.Equal(4, ClassSkillTable.UnlockedFor(CharacterClass.Knight, 20).Count);
    }

    [Fact]
    public void Policy_AreaWithTwoOrMoreEnemies_SingleWithOne_NoneWithoutMana()
    {
        var unlocked = ClassSkillTable.UnlockedFor(CharacterClass.Knight, 20);
        var policy = SkillSelectionPolicy.Default;

        Assert.Equal("Ground Shaker", policy.Choose(unlocked, aliveEnemies: 3, currentMana: 100)!.Name);
        Assert.Equal("Fierce Thrust", policy.Choose(unlocked, aliveEnemies: 1, currentMana: 100)!.Name);
        Assert.Equal("Whirl Strike", policy.Choose(unlocked, aliveEnemies: 2, currentMana: 12)!.Name); // só a mais barata cabe
        Assert.Null(policy.Choose(unlocked, aliveEnemies: 2, currentMana: 5));

        // Level 1 só tem uma skill em área: com 1 inimigo, a regra é alvo único → nenhuma.
        Assert.Null(policy.Choose(ClassSkillTable.UnlockedFor(CharacterClass.Knight, 1), aliveEnemies: 1, currentMana: 100));
    }

    private sealed class FixedDamageResolver : ICombatResolver
    {
        public CombatResult Resolve(CombatRequest request) => throw new NotSupportedException();

        public CombatActionOutcome ResolveAction(CombatTurnAction action, IRandomSource random) => action.Kind switch
        {
            CombatActionKind.BasicAttack => new CombatActionOutcome(1, 0, 0, 0, true),
            CombatActionKind.UseSkill => new CombatActionOutcome(7, 0, 0, action.Skill!.ManaCost, true),
            _ => CombatActionOutcome.NotExecuted
        };
    }

    private static CombatSession Session(int playerMana, int monsters, int seed = 1)
    {
        var player = new Combatant(Guid.NewGuid(), Guid.NewGuid(), CombatSide.Player, "P", 20, 500, 500, 100, playerMana,
            new CombatProfile(10, 14, 4, 6), ClassSkillTable.UnlockedFor(CharacterClass.Knight, 20));
        var enemies = Enumerable.Range(0, monsters)
            .Select(i => new Combatant(Guid.NewGuid(), Guid.NewGuid(), CombatSide.Monster, $"M{i}", 1, 100, 100, 0, 0, new CombatProfile(5, 5, 0, 0)))
            .ToList();

        return new CombatSession(Guid.NewGuid(), player, enemies, new SeededRandomSource(seed));
    }

    [Fact]
    public void AreaSkill_HitsAllAliveEnemies_ManaSpentOnce()
    {
        var session = Session(playerMana: 100, monsters: 3);
        var orchestrator = new CombatOrchestrator(new FixedDamageResolver(), new SequentialTurnScheduler(), new DefaultCombatActionSelector(), []);

        var report = orchestrator.ExecuteRound(session);

        var skillTurns = report.Log.Where(e => e.Kind == CombatActionKind.UseSkill).ToList();
        Assert.Equal(3, skillTurns.Count);
        Assert.All(skillTurns, e => Assert.Equal("Ground Shaker", e.SkillName));
        Assert.Equal(100 - 20, session.Player.CurrentMana); // custo 20, uma vez só
        // Ataque básico (1) só no primeiro alvo; a skill em área (7) em todos.
        Assert.Equal(100 - 1 - 7, session.Monsters[0].CurrentHealth);
        Assert.All(session.Monsters.Skip(1), m => Assert.Equal(100 - 7, m.CurrentHealth));
    }

    [Fact]
    public void SingleTargetSkill_WithOneEnemy()
    {
        var session = Session(playerMana: 100, monsters: 1);
        var orchestrator = new CombatOrchestrator(new FixedDamageResolver(), new SequentialTurnScheduler(), new DefaultCombatActionSelector(), []);

        var report = orchestrator.ExecuteRound(session);

        Assert.Equal("Fierce Thrust", Assert.Single(report.Log, e => e.Kind == CombatActionKind.UseSkill).SkillName);
        Assert.Equal(100 - 25, session.Player.CurrentMana);
    }

    [Fact]
    public void RealFormula_SameSeed_SameSkillCombat()
    {
        static List<(string?, int)> Run(int seed)
        {
            var session = Session(playerMana: 100, monsters: 3, seed);
            var orchestrator = new CombatOrchestrator(
                new ProjectVictory.Infrastructure.Services.CombatResolver(new ProvisionalTibiaLikeDamageFormula(), new NoSupplies()),
                new SequentialTurnScheduler(), new DefaultCombatActionSelector(), []);
            var log = new List<(string?, int)>();

            for (var i = 0; i < 50 && !session.IsFinished; i++)
            {
                log.AddRange(orchestrator.ExecuteRound(session).Log.Select(e => (e.SkillName, e.Damage)));
            }

            return log;
        }

        Assert.Equal(Run(9), Run(9));
        Assert.Contains(Run(9), e => e.Item1 is not null);
    }

    private sealed class NoSupplies : ISupplyEffects
    {
        public int HealthPotionHealing => 0;

        public int ManaPotionRestore => 0;
    }

    [Fact]
    public void Formula_SkillMultiplierScalesDamage()
    {
        var formula = new ProvisionalTibiaLikeDamageFormula();
        var attacker = new Combatant(Guid.NewGuid(), Guid.NewGuid(), CombatSide.Player, "S", 20, 200, 200, 300, 300,
            new CombatProfile(40, 13, 0, 0));

        Assert.True(formula.MaximumDamage(attacker, 2.4m) > formula.MaximumDamage(attacker, 1.6m));
        Assert.Equal(formula.MaximumDamage(attacker, 1m), formula.MaximumDamage(attacker, CombatActionKind.UseSkill));
    }
}
