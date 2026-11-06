namespace ProjectVictory.Domain.Character;

// PV-CHAR-010/011: InitialHealth/InitialMana (Level 1) e HealthGrowth/ManaGrowth (por Level, a partir
// do Level 8) alimentam a Attribute Curve (AttributeCurveTable). DEC-20261106-18: seguem o Tibia —
// Level 1 = 150 de vida / 55 de mana para todas as classes; ganhos do PO: Knight +15/+5,
// Paladin +10/+15, Druid e Sorcerer +5/+30, Monk +12/+15 (Monk PROVISÓRIO quanto ao início).
// Antes (até o PV-CHAR-010): Knight 185/55, Paladin 155/95, Sorcerer e Druid 125/140, Monk 150/80.
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
                    150,
                    55,
                    15,
                    5,
                    new CombatAbility("Ground Slam")),

            CharacterClass.Paladin =>
                new CharacterClassDefinition(
                    CharacterClass.Paladin,
                    150,
                    55,
                    10,
                    15,
                    new CombatAbility("Exori Con")),

            CharacterClass.Sorcerer =>
                new CharacterClassDefinition(
                    CharacterClass.Sorcerer,
                    150,
                    55,
                    5,
                    30,
                    new CombatAbility("Exori Vis")),

            CharacterClass.Druid =>
                new CharacterClassDefinition(
                    CharacterClass.Druid,
                    150,
                    55,
                    5,
                    30,
                    new CombatAbility("Exori Tera")),

            CharacterClass.Monk =>
                new CharacterClassDefinition(
                    CharacterClass.Monk,
                    150,
                    55,
                    12,
                    15,
                    new CombatAbility("Power Punch")),

            _ =>
                throw new InvalidOperationException(
                    "Invalid character class.")
        };
    }
}