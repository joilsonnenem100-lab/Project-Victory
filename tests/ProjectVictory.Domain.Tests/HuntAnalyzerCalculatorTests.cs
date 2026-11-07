using ProjectVictory.Domain.Hunt;
using Xunit;

namespace ProjectVictory.Domain.Tests;

// PV-HUD-ANALYZER-010 (DEC-20261106-01/08): regra pura do Hunt Analyzer.
public sealed class HuntAnalyzerCalculatorTests
{
    private static readonly Guid Potion = Guid.NewGuid();
    private static readonly Guid Fang = Guid.NewGuid();
    private static readonly Guid Silk = Guid.NewGuid();

    [Fact]
    public void Debit_Profit_NetProfit_And_ExperiencePerHour()
    {
        var report = HuntAnalyzerCalculator.Calculate(
            experienceGained: 1500,
            elapsedSeconds: 1800,
            suppliesUsed: [new HuntAnalyzerLine(Potion, 4, 50)],
            loot: [new HuntAnalyzerLine(Fang, 10, 8), new HuntAnalyzerLine(Silk, 2, 25)],
            gold: 70);

        Assert.Equal(1500, report.ExperienceGained);
        Assert.Equal(3000, report.ExperiencePerHour);   // 1500 em meia hora
        Assert.Equal(200, report.Debit);                // 4 x 50 (compra no NPC)
        Assert.Equal(130, report.LootValue);            // 10 x 8 + 2 x 25 (venda ao NPC)
        Assert.Equal(200, report.Profit);               // loot + ouro
        Assert.Equal(0, report.NetProfit);
        Assert.Equal(4, report.SuppliesUsed);
        Assert.Equal(12, report.ItemsLooted);
    }

    [Fact]
    public void LessThanOneSecond_ExperiencePerHourIsZero_AndNetProfitCanBeNegative()
    {
        var report = HuntAnalyzerCalculator.Calculate(10, 0, [new HuntAnalyzerLine(Potion, 1, 50)], [], 5);

        Assert.Equal(0, report.ExperiencePerHour);
        Assert.Equal(-45, report.NetProfit);
    }

    [Fact]
    public void ExperiencePerHour_IsFloored()
    {
        // 1 XP em 1199 s = 3,0025 XP/h -> 3
        Assert.Equal(3, HuntAnalyzerCalculator.Calculate(1, 1199, [], [], 0).ExperiencePerHour);
    }

    [Fact]
    public void NegativeInputs_AreRejected()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => HuntAnalyzerCalculator.Calculate(-1, 1, [], [], 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => HuntAnalyzerCalculator.Calculate(1, -1, [], [], 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => HuntAnalyzerCalculator.Calculate(1, 1, [], [], -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => HuntAnalyzerCalculator.Calculate(1, 1, [new HuntAnalyzerLine(Potion, -1, 50)], [], 0));
    }
}
