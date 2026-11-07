using System;
using System.Collections.Generic;

namespace ProjectVictory.Domain.Hunt;

// PV-HUD-ANALYZER-010 (DEC-20261106-01/08): uma linha valorada do Analyzer (quantidade × preço NPC).
public sealed record HuntAnalyzerLine(
    Guid ItemDefinitionId,
    int Quantity,
    int UnitPrice)
{
    public long Total => (long)Quantity * UnitPrice;
}

// PV-HUD-ANALYZER-010: números do Hunt Analyzer (calculados no servidor; o cliente só exibe, ADR-002).
public sealed record HuntAnalyzerReport(
    long ExperienceGained,
    long ElapsedSeconds,
    long ExperiencePerHour,
    long Debit,
    long LootValue,
    long Gold,
    long Profit,
    long NetProfit,
    int SuppliesUsed,
    int ItemsLooted);

// PV-HUD-ANALYZER-010: regra pura do Analyzer.
// - XP/hora = floor(XP × 3600 / segundos), 0 se a Hunt tem menos de 1 s (mesma fórmula do EndHuntResult);
// - Débito = Σ suprimentos usados × preço de COMPRA no NPC (DEC-20261106-01);
// - Lucro = Σ loot × preço de VENDA ao NPC (DEC-20261106-08) + ouro;
// - Lucro líquido = lucro − débito (pode ser negativo).
public static class HuntAnalyzerCalculator
{
    public static HuntAnalyzerReport Calculate(
        long experienceGained,
        long elapsedSeconds,
        IEnumerable<HuntAnalyzerLine> suppliesUsed,
        IEnumerable<HuntAnalyzerLine> loot,
        long gold)
    {
        ArgumentNullException.ThrowIfNull(suppliesUsed);
        ArgumentNullException.ThrowIfNull(loot);

        if (experienceGained < 0) throw new ArgumentOutOfRangeException(nameof(experienceGained));
        if (elapsedSeconds < 0) throw new ArgumentOutOfRangeException(nameof(elapsedSeconds));
        if (gold < 0) throw new ArgumentOutOfRangeException(nameof(gold));

        long debit = 0;
        var supplies = 0;

        foreach (var line in suppliesUsed)
        {
            Validate(line);
            debit = checked(debit + line.Total);
            supplies = checked(supplies + line.Quantity);
        }

        long lootValue = 0;
        var items = 0;

        foreach (var line in loot)
        {
            Validate(line);
            lootValue = checked(lootValue + line.Total);
            items = checked(items + line.Quantity);
        }

        var profit = checked(lootValue + gold);
        var experiencePerHour = elapsedSeconds >= 1
            ? (long)Math.Floor(experienceGained * 3600d / elapsedSeconds)
            : 0;

        return new HuntAnalyzerReport(
            experienceGained,
            elapsedSeconds,
            experiencePerHour,
            debit,
            lootValue,
            gold,
            profit,
            checked(profit - debit),
            supplies,
            items);
    }

    private static void Validate(HuntAnalyzerLine line)
    {
        ArgumentNullException.ThrowIfNull(line);

        if (line.Quantity < 0 || line.UnitPrice < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(line), "Quantity and price cannot be negative.");
        }
    }
}
