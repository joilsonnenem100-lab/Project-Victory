using System.Reflection;
using ProjectVictory.Domain.Character;
using ProjectVictory.Domain.Entities;
using Xunit;

namespace ProjectVictory.Domain.Tests;

// PV-CHAR-012 (DEC-20261106-26/27/29): atributos do personagem = só os do Tibia; identidade das classes
// em dados (descritiva).
public sealed class TibiaAttributeTests
{
    private static readonly string[] Forbidden =
    [
        "Strength", "Dexterity", "Intelligence", "Vitality", "Luck", "AttackSpeed", "Capacity", "Attributes"
    ];

    [Fact]
    public void Character_HasNoInventedAttributes()
    {
        var properties = typeof(ProjectVictory.Domain.Entities.Character).GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(p => p.Name);

        Assert.Empty(properties.Intersect(Forbidden));
        Assert.Null(typeof(ProjectVictory.Domain.Entities.Character).Assembly.GetType("ProjectVictory.Domain.Entities.Attributes"));
    }

    [Fact]
    public void Skills_AreExactlyTheTibiaSkills()
    {
        var skillProperties = typeof(Skills).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => p.Name).OrderBy(n => n).ToArray();

        Assert.Equal(
            new[] { "Axe", "Club", "Distance", "Fishing", "Fist", "MagicLevel", "Shielding", "Sword" },
            skillProperties);
        Assert.Equal(8, Enum.GetValues<TibiaSkill>().Length);
    }

    [Fact]
    public void ClassIdentity_MatchesPoDecision()
    {
        Assert.Equal(Enum.GetValues<CharacterClass>().Length, ClassIdentityTable.All.Count);

        Assert.Equal(new[] { ElementTheme.Earth, ElementTheme.Ice }, ClassIdentityTable.For(CharacterClass.Druid).Elements);
        Assert.Equal(new[] { ElementTheme.Energy, ElementTheme.Fire }, ClassIdentityTable.For(CharacterClass.Sorcerer).Elements);
        Assert.Equal(new[] { TibiaSkill.FistFighting }, ClassIdentityTable.For(CharacterClass.Monk).MainSkills);
        Assert.Contains(TibiaSkill.MagicLevel, ClassIdentityTable.For(CharacterClass.Druid).MainSkills);
        Assert.Contains(TibiaSkill.DistanceFighting, ClassIdentityTable.For(CharacterClass.Paladin).MainSkills);
        Assert.Contains(TibiaSkill.SwordFighting, ClassIdentityTable.For(CharacterClass.Knight).MainSkills);
        Assert.All(ClassIdentityTable.All, identity => Assert.False(string.IsNullOrWhiteSpace(identity.Description)));
    }
}
