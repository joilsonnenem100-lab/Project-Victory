using ProjectVictory.Application.Combat;
using ProjectVictory.Application.Combat.TurnBased;
using ProjectVictory.Domain.Combat.TurnBased;
using ProjectVictory.Domain.Items;

namespace ProjectVictory.Application.Supply;

// PV-SUPPLY-010: os únicos suprimentos desta fase são poção de vida e poção de mana
// (DEC-20261106-02). Definições como dados.
// *** PROVISÓRIO *** — preço de compra no NPC (usado no débito do Hunt Analyzer, DEC-20261106-01)
// e preço de venda (DEC-20261106-08) aguardam o PO. A cura está em ISupplyEffects (75 HP / 50 Mana).
public static class SupplyCatalog
{
    public static readonly Guid HealthPotionId = Guid.Parse("00000000-0000-0000-0000-0000000000a1");

    public static readonly Guid ManaPotionId = Guid.Parse("00000000-0000-0000-0000-0000000000a2");

    public static ItemDefinition HealthPotion { get; } =
        new(HealthPotionId, "Health Potion", new ItemCharacteristics("Restaura vida (suprimento de Hunt)."), npcSellPrice: 10, npcBuyPrice: 50);

    public static ItemDefinition ManaPotion { get; } =
        new(ManaPotionId, "Mana Potion", new ItemCharacteristics("Restaura mana (suprimento de Hunt)."), npcSellPrice: 10, npcBuyPrice: 50);

    public static bool IsPotion(Guid itemDefinitionId) =>
        itemDefinitionId == HealthPotionId || itemDefinitionId == ManaPotionId;
}

// PV-SUPPLY-010: configuração do jogador (DEC-20261106-03). "Usar a poção quando a vida/mana ficar
// abaixo de X% do máximo"; null = não usar aquela poção.
public sealed record SupplyPolicy(
    int? HealthPotionBelowPercent,
    int? ManaPotionBelowPercent)
{
    public static SupplyPolicy None { get; } = new(null, null);

    public bool UsesAnyPotion => HealthPotionBelowPercent is not null || ManaPotionBelowPercent is not null;

    public static bool IsValidPercent(int? value) => value is null or (>= 1 and <= 100);
}

// PV-SUPPLY-010: política por personagem. O jogador grava a política PENDENTE a qualquer momento,
// inclusive durante a Hunt; ela só vale a partir do PRÓXIMO turno (Activate é chamado no início
// de cada rodada de combate, nunca no meio de um turno) — DEC-20261106-03.
public interface ISupplyPolicyStore
{
    void SetPending(Guid characterId, SupplyPolicy policy);

    SupplyPolicy ActivatePending(Guid characterId);

    SupplyPolicy GetActive(Guid characterId);
}

public sealed class SupplyPolicyStore : ISupplyPolicyStore
{
    private readonly System.Collections.Concurrent.ConcurrentDictionary<Guid, SupplyPolicy> _pending = new();
    private readonly System.Collections.Concurrent.ConcurrentDictionary<Guid, SupplyPolicy> _active = new();

    public void SetPending(Guid characterId, SupplyPolicy policy)
    {
        ArgumentNullException.ThrowIfNull(policy);
        _pending[characterId] = policy;
    }

    public SupplyPolicy ActivatePending(Guid characterId)
    {
        if (_pending.TryRemove(characterId, out var pending))
        {
            _active[characterId] = pending;
        }

        return GetActive(characterId);
    }

    public SupplyPolicy GetActive(Guid characterId) =>
        _active.TryGetValue(characterId, out var policy) ? policy : SupplyPolicy.None;
}

// PV-SUPPLY-010: suprimentos disponíveis na rodada de combate em andamento (escopo da requisição).
public sealed class CombatSupplyContext
{
    public SupplyPolicy Policy { get; private set; } = SupplyPolicy.None;

    public int HealthPotions { get; private set; }

    public int ManaPotions { get; private set; }

    public int HealthPotionsUsed { get; private set; }

    public int ManaPotionsUsed { get; private set; }

    public void Load(SupplyPolicy policy, int healthPotions, int manaPotions)
    {
        Policy = policy ?? SupplyPolicy.None;
        HealthPotions = healthPotions;
        ManaPotions = manaPotions;
        HealthPotionsUsed = 0;
        ManaPotionsUsed = 0;
    }

    public void Consume(CombatActionKind kind)
    {
        if (kind == CombatActionKind.UseHealthPotion && HealthPotions > 0)
        {
            HealthPotions--;
            HealthPotionsUsed++;
        }
        else if (kind == CombatActionKind.UseManaPotion && ManaPotions > 0)
        {
            ManaPotions--;
            ManaPotionsUsed++;
        }
    }

    // PV-SUPPLY-010: "acabou uma poção que a política manda usar". PV-SUPPLY-011 (DEC-20261106-22):
    // NÃO encerra mais a Hunt; mantido só como informação (ex.: aviso na interface, M8).
    public bool IsOutOfSupplies =>
        (Policy.HealthPotionBelowPercent is not null && HealthPotions == 0)
        || (Policy.ManaPotionBelowPercent is not null && ManaPotions == 0);
}

// PV-SUPPLY-010: Secondary Action do jogador conforme a política (ADR-006: a Secondary é opcional).
// Ordem: poção de vida (se abaixo do limite e houver poção) → poção de mana → habilidade da classe.
// Sem poção não há cura (DEC-20261106-04): não existe outra fonte de vida durante a Hunt.
public sealed class SupplyAwareCombatActionSelector : ICombatActionSelector
{
    private readonly CombatSupplyContext _supplies;
    private readonly DefaultCombatActionSelector _fallback;

    public SupplyAwareCombatActionSelector(CombatSupplyContext supplies)
        : this(supplies, new DataDrivenMonsterAI(BasicAttackOnlyBehaviorProvider.Instance))
    {
    }

    // PV-MONSTER-010: monstros escolhem pela Monster AI orientada a dados (ADR-005).
    public SupplyAwareCombatActionSelector(CombatSupplyContext supplies, IMonsterAI monsterAI)
    {
        _supplies = supplies;
        _fallback = new DefaultCombatActionSelector(
            ProjectVictory.Domain.Combat.Rules.SkillSelectionPolicy.Default,
            monsterAI);
    }

    public CombatActionKind? SelectSecondaryAction(CombatSession session, Combatant actor) =>
        SelectSecondaryChoice(session, actor)?.Kind;

    // PV-COMBAT-013: poção (se a política mandar) ou, senão, a skill escolhida pelo seletor padrão.
    public CombatTurnChoice? SelectSecondaryChoice(CombatSession session, Combatant actor)
    {
        ArgumentNullException.ThrowIfNull(actor);

        if (actor.Side == CombatSide.Player)
        {
            var policy = _supplies.Policy;

            if (policy.HealthPotionBelowPercent is { } hp
                && _supplies.HealthPotions > 0
                && actor.CurrentHealth * 100 < hp * actor.MaximumHealth)
            {
                return new CombatTurnChoice(CombatActionKind.UseHealthPotion);
            }

            if (policy.ManaPotionBelowPercent is { } mp
                && _supplies.ManaPotions > 0
                && actor.MaximumMana > 0
                && actor.CurrentMana * 100 < mp * actor.MaximumMana)
            {
                return new CombatTurnChoice(CombatActionKind.UseManaPotion);
            }
        }

        return _fallback.SelectSecondaryChoice(session, actor);
    }
}

// PV-SUPPLY-010: conta a poção usada (o item sai do inventário ao fim da rodada).
public sealed class SupplyConsumptionObserver : ICombatActionObserver
{
    private readonly CombatSupplyContext _supplies;

    public SupplyConsumptionObserver(CombatSupplyContext supplies)
    {
        _supplies = supplies;
    }

    public void OnActionExecuted(CombatSession session, CombatTurnAction action, CombatActionOutcome outcome)
    {
        if (action.Kind is CombatActionKind.UseHealthPotion or CombatActionKind.UseManaPotion)
        {
            _supplies.Consume(action.Kind);
        }
    }
}

// PV-SUPPLY-010: configuração do servidor — poções iniciais dadas na criação do personagem.
// *** PROVISÓRIO *** — o PO ainda não definiu como o jogador obtém poções (compra no NPC é Fase 7/8).
// Padrão 0: sem mudança de comportamento; ambientes de teste podem ligar.
public sealed class SupplySettings
{
    public const string StarterHealthPotionsKey = "Supplies:StarterHealthPotions";

    public const string StarterManaPotionsKey = "Supplies:StarterManaPotions";

    public int StarterHealthPotions { get; }

    public int StarterManaPotions { get; }

    public SupplySettings(int starterHealthPotions = 0, int starterManaPotions = 0)
    {
        if (starterHealthPotions < 0 || starterManaPotions < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(starterHealthPotions), "Starter potions cannot be negative.");
        }

        StarterHealthPotions = starterHealthPotions;
        StarterManaPotions = starterManaPotions;
    }
}
