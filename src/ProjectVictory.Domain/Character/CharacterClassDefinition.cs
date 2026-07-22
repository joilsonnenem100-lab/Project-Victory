namespace ProjectVictory.Domain.Character;

public sealed record CharacterClassDefinition(
    CharacterClass Class,
    int InitialHealth,
    int InitialMana,
    int HealthGrowth,
    int ManaGrowth,
    CombatAbility InitialAbility)
{
    public static CharacterClassDefinition Get(
        CharacterClass characterClass)
    {
        return characterClass switch
        {
            CharacterClass.Knight =>
                new CharacterClassDefinition(
                    CharacterClass.Knight,
                    185,
                    55,
                    15,
                    5,
                    new CombatAbility("Ground Slam")),

            CharacterClass.Paladin =>
                new CharacterClassDefinition(
                    CharacterClass.Paladin,
                    155,
                    95,
                    10,
                    15,
                    new CombatAbility("Exori Con")),

            CharacterClass.Sorcerer =>
                new CharacterClassDefinition(
                    CharacterClass.Sorcerer,
                    125,
                    140,
                    5,
                    30,
                    new CombatAbility("Exori Vis")),

            CharacterClass.Druid =>
                new CharacterClassDefinition(
                    CharacterClass.Druid,
                    125,
                    140,
                    5,
                    30,
                    new CombatAbility("Exori Tera")),

            CharacterClass.Monk =>
                new CharacterClassDefinition(
                    CharacterClass.Monk,
                    150,
                    80,
                    10,
                    10,
                    new CombatAbility("Power Punch")),

            _ =>
                throw new InvalidOperationException(
                    "Invalid character class.")
        };
    }
}