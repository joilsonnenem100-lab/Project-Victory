namespace ProjectVictory.Application.Combat;

// PV-COMBAT-012 / PV-SUPPLY-010: efeito das poções, como dados (usado pelo ICombatResolver).
public interface ISupplyEffects
{
    int HealthPotionHealing { get; }

    int ManaPotionRestore { get; }

    // PV-CONTENT-012 (PO-15): efeito por nível. Sem nível (null) ou nível pequeno = os valores acima
    // (compatibilidade com implementações antigas); médio/grande = valor do PotionCatalog.
    int HealthRestoreFor(Guid? potionItemId) =>
        potionItemId is { } id
        && ProjectVictory.Application.Supply.PotionCatalog.Find(id) is { Kind: ProjectVictory.Application.Supply.PotionKind.Health } potion
        && potion.Size != ProjectVictory.Application.Supply.PotionSize.Small
            ? potion.Amount
            : HealthPotionHealing;

    int ManaRestoreFor(Guid? potionItemId) =>
        potionItemId is { } id
        && ProjectVictory.Application.Supply.PotionCatalog.Find(id) is { Kind: ProjectVictory.Application.Supply.PotionKind.Mana } potion
        && potion.Size != ProjectVictory.Application.Supply.PotionSize.Small
            ? potion.Amount
            : ManaPotionRestore;
}
