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

    // PV-CONTENT-012: qualquer nível de poção (pequena, média, grande — PotionCatalog).
    public static bool IsPotion(Guid itemDefinitionId) =>
        PotionCatalog.Find(itemDefinitionId) is not null;
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
// PV-CONTENT-012: contagem POR NÍVEL de poção (Id do item); HealthPotions/ManaPotions e os "Used"
// continuam existindo como totais do tipo (Regra Aditiva).
public sealed class CombatSupplyContext
{
    private readonly Dictionary<Guid, int> _available = new();
    private readonly Dictionary<Guid, int> _used = new();

    public SupplyPolicy Policy { get; private set; } = SupplyPolicy.None;

    public int HealthPotions => Total(_available, PotionKind.Health);

    public int ManaPotions => Total(_available, PotionKind.Mana);

    public int HealthPotionsUsed => Total(_used, PotionKind.Health);

    public int ManaPotionsUsed => Total(_used, PotionKind.Mana);

    // PV-CONTENT-012: poções disponíveis / usadas na rodada, por Id do item.
    public IReadOnlyDictionary<Guid, int> Available => _available;

    public IReadOnlyDictionary<Guid, int> Used => _used;

    // Compatibilidade (PV-SUPPLY-010): contagens sem nível = nível pequeno.
    public void Load(SupplyPolicy policy, int healthPotions, int manaPotions) =>
        Load(policy, new Dictionary<Guid, int>
        {
            [SupplyCatalog.HealthPotionId] = healthPotions,
            [SupplyCatalog.ManaPotionId] = manaPotions
        });

    // PV-CONTENT-012: contagem por nível (itens que não são poção são ignorados).
    public void Load(SupplyPolicy policy, IReadOnlyDictionary<Guid, int> potionsByItem)
    {
        ArgumentNullException.ThrowIfNull(potionsByItem);

        Policy = policy ?? SupplyPolicy.None;
        _available.Clear();
        _used.Clear();

        foreach (var (itemId, count) in potionsByItem)
        {
            if (count > 0 && PotionCatalog.Find(itemId) is not null)
            {
                _available[itemId] = count;
            }
        }
    }

    // PV-CONTENT-012: o nível que o seletor usaria agora (regra em PotionCatalog.Choose).
    public PotionDefinition? Choose(PotionKind kind, int missing) =>
        PotionCatalog.Choose(kind, missing, _available);

    // Compatibilidade: sem nível informado = a menor poção do tipo que houver.
    public void Consume(CombatActionKind kind) => Consume(kind, null);

    public void Consume(CombatActionKind kind, Guid? potionItemId)
    {
        PotionKind? potionKind = kind switch
        {
            CombatActionKind.UseHealthPotion => PotionKind.Health,
            CombatActionKind.UseManaPotion => PotionKind.Mana,
            _ => null
        };

        if (potionKind is null)
        {
            return;
        }

        Guid? itemId = potionItemId is { } id && PotionCatalog.Find(id)?.Kind == potionKind
            ? id
            : PotionCatalog.All
                .Where(p => p.Kind == potionKind && _available.GetValueOrDefault(p.ItemId) > 0)
                .OrderBy(p => p.Size)
                .Select(p => (Guid?)p.ItemId)
                .FirstOrDefault();

        if (itemId is not { } chosen || _available.GetValueOrDefault(chosen) <= 0)
        {
            return;
        }

        _available[chosen]--;
        _used[chosen] = _used.GetValueOrDefault(chosen) + 1;
    }

    // PV-SUPPLY-010: "acabou uma poção que a política manda usar". PV-SUPPLY-011 (DEC-20261106-22):
    // NÃO encerra mais a Hunt; mantido só como informação (ex.: aviso na interface, M8).
    public bool IsOutOfSupplies =>
        (Policy.HealthPotionBelowPercent is not null && HealthPotions == 0)
        || (Policy.ManaPotionBelowPercent is not null && ManaPotions == 0);

    private static int Total(Dictionary<Guid, int> counts, PotionKind kind) =>
        counts.Where(x => PotionCatalog.Find(x.Key)?.Kind == kind).Sum(x => x.Value);
}

// PV-SUPPLY-010: Secondary Action do jogador conforme a política (ADR-006: a Secondary é opcional).
// Ordem: poção de vida (se abaixo do limite e houver poção) → poção de mana → habilidade da classe.
// Sem poção não há cura (DEC-20261106-04): não existe outra fonte de vida durante a Hunt.
// PV-COMBAT-014 (PO-53, emenda do DEC-20261106-04: "sem suprimento não há cura" vale só para poções):
// nova ordem = spell de cura (vida abaixo do limite da HealingSpellPolicy E Mana suficiente) → poção
// de vida (reserva: sem Mana, ou vida abaixo do limite da poção mas acima do da spell) → poção de
// Mana (repõe a Mana que a spell gasta) → skill da classe.
public sealed class SupplyAwareCombatActionSelector : ICombatActionSelector
{
    private readonly CombatSupplyContext _supplies;
    private readonly DefaultCombatActionSelector _fallback;
    private readonly ProjectVictory.Domain.Combat.Rules.HealingSpellPolicy _healing;

    // Compatibilidade/testes de unidade: monstros só com ataque básico. Produção (DI) usa o construtor
    // com IMonsterAI, que lê os dados do catálogo.
    public SupplyAwareCombatActionSelector(CombatSupplyContext supplies)
        : this(supplies, new DataDrivenMonsterAI(BasicAttackOnlyBehaviorProvider.Instance))
    {
    }

    // PV-MONSTER-010: monstros escolhem pela Monster AI orientada a dados (ADR-005).
    // PV-COMBAT-014: sem HealingSpellPolicy (testes de unidade antigos), a cura por spell fica DESLIGADA;
    // a composição (DI) registra a política configurada (padrão 50%).
    public SupplyAwareCombatActionSelector(
        CombatSupplyContext supplies,
        IMonsterAI monsterAI,
        ProjectVictory.Domain.Combat.Rules.HealingSpellPolicy? healingPolicy = null)
    {
        _supplies = supplies;
        _healing = healingPolicy ?? ProjectVictory.Domain.Combat.Rules.HealingSpellPolicy.Disabled;
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
            // PV-COMBAT-014: a spell de cura vem primeiro quando há Mana; a poção de vida é a reserva.
            if (actor.HealingSpell is { } spell
                && actor.CurrentMana >= spell.ManaCost
                && _healing.WantsHealing(actor.CurrentHealth, actor.MaximumHealth))
            {
                return new CombatTurnChoice(CombatActionKind.UseHealingSpell);
            }

            var policy = _supplies.Policy;

            // PV-CONTENT-012: o nível da poção sai de PotionCatalog.Choose (menor que resolve; senão a maior).
            if (policy.HealthPotionBelowPercent is { } hp
                && _supplies.HealthPotions > 0
                && actor.CurrentHealth * 100 < hp * actor.MaximumHealth
                && _supplies.Choose(PotionKind.Health, actor.MaximumHealth - actor.CurrentHealth) is { } healthPotion)
            {
                return new CombatTurnChoice(CombatActionKind.UseHealthPotion, PotionItemId: healthPotion.ItemId);
            }

            if (policy.ManaPotionBelowPercent is { } mp
                && _supplies.ManaPotions > 0
                && actor.MaximumMana > 0
                && actor.CurrentMana * 100 < mp * actor.MaximumMana
                && _supplies.Choose(PotionKind.Mana, actor.MaximumMana - actor.CurrentMana) is { } manaPotion)
            {
                return new CombatTurnChoice(CombatActionKind.UseManaPotion, PotionItemId: manaPotion.ItemId);
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
        // Revisão do PV-MONSTER-010 (ressalva 1): só poções usadas PELO JOGADOR saem do inventário dele.
        if (action.Actor.Side == CombatSide.Player
            && action.Kind is CombatActionKind.UseHealthPotion or CombatActionKind.UseManaPotion)
        {
            _supplies.Consume(action.Kind, action.PotionItemId);
        }
    }
}

// PV-SUPPLY-010: configuração do servidor — poções iniciais dadas na criação do personagem.
// *** PROVISÓRIO *** — o PO ainda não definiu como o jogador obtém poções (compra no NPC é Fase 7/8).
// Padrão 0: sem mudança de comportamento; ambientes de teste podem ligar.
// PV-CONTENT-012 (PO-16: kit inicial): kit por nível de poção em "Supplies:StarterKit:<Key>" (Key do
// PotionCatalog, ex.: "StrongHealthPotion"). As chaves antigas continuam valendo e SOMAM ao kit
// (nível pequeno). Os valores do kit ficam no appsettings.json (PROVISÓRIOS).
public sealed class SupplySettings
{
    public const string StarterHealthPotionsKey = "Supplies:StarterHealthPotions";

    public const string StarterManaPotionsKey = "Supplies:StarterManaPotions";

    public const string StarterKitKey = "Supplies:StarterKit";

    public int StarterHealthPotions { get; }

    public int StarterManaPotions { get; }

    // PV-CONTENT-012: quantidade por Id do item (só poções do PotionCatalog).
    public IReadOnlyDictionary<Guid, int> StarterKit { get; }

    public SupplySettings(int starterHealthPotions = 0, int starterManaPotions = 0)
        : this(starterHealthPotions, starterManaPotions, null)
    {
    }

    public SupplySettings(
        int starterHealthPotions,
        int starterManaPotions,
        IReadOnlyDictionary<Guid, int>? starterKit)
    {
        if (starterHealthPotions < 0 || starterManaPotions < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(starterHealthPotions), "Starter potions cannot be negative.");
        }

        var kit = new Dictionary<Guid, int>();

        foreach (var (itemId, quantity) in starterKit ?? new Dictionary<Guid, int>())
        {
            if (PotionCatalog.Find(itemId) is null)
            {
                throw new ArgumentException($"Starter kit item {itemId} is not a potion.", nameof(starterKit));
            }

            if (quantity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(starterKit), "Starter kit quantities cannot be negative.");
            }

            if (quantity > 0)
            {
                kit[itemId] = quantity;
            }
        }

        StarterHealthPotions = starterHealthPotions;
        StarterManaPotions = starterManaPotions;
        StarterKit = kit;
    }

    // PV-CONTENT-012: kit por nome de poção (Key do PotionCatalog; maiúsculas/minúsculas indiferentes).
    // Nome desconhecido ou quantidade negativa → erro (o startup falha, como as demais configurações).
    public static IReadOnlyDictionary<Guid, int> ParseStarterKit(IEnumerable<KeyValuePair<string, int>> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);

        var kit = new Dictionary<Guid, int>();

        foreach (var (key, quantity) in entries)
        {
            var potion = PotionCatalog.FindByKey(key)
                ?? throw new InvalidOperationException(
                    $"{StarterKitKey}:{key} não é uma poção conhecida ({string.Join(", ", PotionCatalog.All.Select(p => p.Key))}).");

            if (quantity < 0)
            {
                throw new InvalidOperationException($"{StarterKitKey}:{key} não pode ser negativo.");
            }

            kit[potion.ItemId] = kit.GetValueOrDefault(potion.ItemId) + quantity;
        }

        return kit;
    }

    // PV-CONTENT-012: tudo o que o personagem novo recebe, por Id do item (kit + chaves antigas).
    public IReadOnlyDictionary<Guid, int> StarterItems()
    {
        var items = new Dictionary<Guid, int>(StarterKit);

        if (StarterHealthPotions > 0)
        {
            items[SupplyCatalog.HealthPotionId] = items.GetValueOrDefault(SupplyCatalog.HealthPotionId) + StarterHealthPotions;
        }

        if (StarterManaPotions > 0)
        {
            items[SupplyCatalog.ManaPotionId] = items.GetValueOrDefault(SupplyCatalog.ManaPotionId) + StarterManaPotions;
        }

        return items;
    }
}
