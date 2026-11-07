using ProjectVictory.Domain.Items;

namespace ProjectVictory.Application.Supply;

// PV-CONTENT-012 (PO-15): poções em três níveis — pequena, média, grande.
public enum PotionKind
{
    Health,
    Mana
}

public enum PotionSize
{
    Small = 1,
    Medium = 2,
    Large = 3
}

// PV-CONTENT-012: um nível de poção, como dado. Amount = vida curada (Health) ou Mana restaurada (Mana).
// Key = nome usado na configuração do kit inicial ("Supplies:StarterKit:<Key>").
public sealed record PotionDefinition(
    string Key,
    ItemDefinition Item,
    PotionKind Kind,
    PotionSize Size,
    int Amount)
{
    public Guid ItemId => Item.Id;
}

// PV-CONTENT-012 (PO-15/PO-16; DEC-20261106-02 com nota de superação): ÚNICA fonte dos níveis de poção.
// Regra Aditiva: as poções que já existiam (Health Potion a1, Mana Potion a2) continuam válidas, com os
// mesmos Ids, nomes, preços e valores — agora são o nível "pequena".
//
// *** PROVISÓRIO *** — nomes (estilo Tibia: Health / Strong Health / Great Health; idem Mana), valores
// de cura/Mana (Tibia ÷ 2, mantendo os 75/50 já validados no nível pequeno) e preços de compra/venda no
// NPC (compra = Tibia; venda ≈ 20% da compra). Trocar AQUI (e na migration nova que grava os preços
// na tabela Items, se o preço mudar).
public static class PotionCatalog
{
    public static readonly Guid StrongHealthPotionId = Guid.Parse("00000000-0000-0000-0000-0000000000a3");

    public static readonly Guid GreatHealthPotionId = Guid.Parse("00000000-0000-0000-0000-0000000000a4");

    public static readonly Guid StrongManaPotionId = Guid.Parse("00000000-0000-0000-0000-0000000000a5");

    public static readonly Guid GreatManaPotionId = Guid.Parse("00000000-0000-0000-0000-0000000000a6");

    public static ItemDefinition StrongHealthPotion { get; } =
        new(StrongHealthPotionId, "Strong Health Potion", new ItemCharacteristics("Restaura mais vida (suprimento de Hunt)."), npcSellPrice: 20, npcBuyPrice: 100);

    public static ItemDefinition GreatHealthPotion { get; } =
        new(GreatHealthPotionId, "Great Health Potion", new ItemCharacteristics("Restaura muita vida (suprimento de Hunt)."), npcSellPrice: 38, npcBuyPrice: 190);

    public static ItemDefinition StrongManaPotion { get; } =
        new(StrongManaPotionId, "Strong Mana Potion", new ItemCharacteristics("Restaura mais mana (suprimento de Hunt)."), npcSellPrice: 18, npcBuyPrice: 93);

    public static ItemDefinition GreatManaPotion { get; } =
        new(GreatManaPotionId, "Great Mana Potion", new ItemCharacteristics("Restaura muita mana (suprimento de Hunt)."), npcSellPrice: 28, npcBuyPrice: 144);

    // Valores do nível pequeno = os já validados no PV-SUPPLY-010 (ProvisionalSupplyEffects lê daqui).
    public const int SmallHealthAmount = 75;

    public const int SmallManaAmount = 50;

    public static IReadOnlyList<PotionDefinition> All { get; } =
    [
        new("HealthPotion", SupplyCatalog.HealthPotion, PotionKind.Health, PotionSize.Small, SmallHealthAmount),
        new("StrongHealthPotion", StrongHealthPotion, PotionKind.Health, PotionSize.Medium, 150),
        new("GreatHealthPotion", GreatHealthPotion, PotionKind.Health, PotionSize.Large, 250),
        new("ManaPotion", SupplyCatalog.ManaPotion, PotionKind.Mana, PotionSize.Small, SmallManaAmount),
        new("StrongManaPotion", StrongManaPotion, PotionKind.Mana, PotionSize.Medium, 75),
        new("GreatManaPotion", GreatManaPotion, PotionKind.Mana, PotionSize.Large, 100)
    ];

    public static IReadOnlyList<ItemDefinition> NewItems { get; } =
        [StrongHealthPotion, GreatHealthPotion, StrongManaPotion, GreatManaPotion];

    public static PotionDefinition? Find(Guid itemDefinitionId) =>
        All.FirstOrDefault(p => p.ItemId == itemDefinitionId);

    public static PotionDefinition? FindByKey(string key) =>
        All.FirstOrDefault(p => string.Equals(p.Key, key, StringComparison.OrdinalIgnoreCase));

    public static PotionDefinition Small(PotionKind kind) =>
        All.First(p => p.Kind == kind && p.Size == PotionSize.Small);

    // PV-CONTENT-012 — regra de escolha do nível (*** PROVISÓRIA ***): entre os níveis do tipo que o
    // personagem TEM, usa a MENOR poção que resolve (valor ≥ o que falta para o máximo); se nenhuma
    // resolve, a MAIOR disponível. Sem poção do tipo → null. Economiza as poções caras e não desperdiça
    // cura quando falta pouco. Trocar AQUI.
    public static PotionDefinition? Choose(
        PotionKind kind,
        int missing,
        IReadOnlyDictionary<Guid, int> available)
    {
        ArgumentNullException.ThrowIfNull(available);

        var owned = All
            .Where(p => p.Kind == kind && available.TryGetValue(p.ItemId, out var count) && count > 0)
            .OrderBy(p => p.Amount)
            .ToList();

        if (owned.Count == 0)
        {
            return null;
        }

        return owned.FirstOrDefault(p => p.Amount >= missing) ?? owned[^1];
    }
}
