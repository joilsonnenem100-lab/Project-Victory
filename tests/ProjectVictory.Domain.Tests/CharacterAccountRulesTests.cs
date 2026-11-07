using ProjectVictory.Domain.Character;
using Xunit;

namespace ProjectVictory.Domain.Tests;

// PV-CHAR-013 (PO-36 a PO-39): regras puras de nome, slots e exclusão.
public sealed class CharacterAccountRulesTests
{
    [Theory]
    [InlineData("Eli")]
    [InlineData("Sir Galen")]
    [InlineData("Ana Del Mar")]
    [InlineData("Ab")]
    [InlineData("Abcdefghijklmnopqrstuvwxyzab")]
    public void ValidNames_AreAccepted(string name)
    {
        Assert.Null(CharacterNameRules.Validate(name));
    }

    [Theory]
    [InlineData("")]                               // vazio
    [InlineData("A")]                              // curto
    [InlineData("Abcdefghijklmnopqrstuvwxyzabcd")] // 30 caracteres
    [InlineData("Hero1")]                          // dígito
    [InlineData("Ana-Maria")]                      // símbolo (PROVISÓRIO: só letras e espaço)
    [InlineData("eli")]                            // sem maiúscula inicial
    [InlineData("SmokeHero")]                      // maiúscula no meio da palavra
    [InlineData("Sir A Galen")]                    // palavra de uma letra
    [InlineData("Sir  Galen")]                     // espaço duplo
    [InlineData(" Galen")]                         // espaço no início
    [InlineData("Galen ")]                         // espaço no fim
    [InlineData("Ab Cd Ef Gh")]                    // 4 palavras
    public void InvalidNames_AreRefused(string name)
    {
        Assert.NotNull(CharacterNameRules.Validate(name));
    }

    [Fact]
    public void Slots_FiveInTotal_ThreeUnlocked()
    {
        Assert.Equal(5, CharacterSlotRules.MaximumSlots);
        Assert.Equal(3, CharacterSlotRules.UnlockedSlots);
    }

    [Fact]
    public void Deletion_WaitsSevenDays()
    {
        var requested = new DateTimeOffset(2026, 11, 7, 10, 0, 0, TimeSpan.Zero);
        var scheduled = CharacterDeletionRules.ScheduledFor(requested);

        Assert.Equal(requested.AddDays(7), scheduled);
        Assert.False(CharacterDeletionRules.IsDue(scheduled, scheduled.AddTicks(-1)));
        Assert.True(CharacterDeletionRules.IsDue(scheduled, scheduled));
    }

    [Fact]
    public void AppearanceOptions_AreDataAndCaseInsensitive()
    {
        Assert.Equal(["Male", "Female"], CharacterAppearanceOptions.Genders.Select(g => g.ToString()));
        Assert.Equal("Scout", CharacterAppearanceOptions.FindOutfit("scout"));
        Assert.Null(CharacterAppearanceOptions.FindOutfit("Dragon"));
    }
}
