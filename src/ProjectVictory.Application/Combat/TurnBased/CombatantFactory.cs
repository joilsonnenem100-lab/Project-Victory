using ProjectVictory.Domain.Combat.Rules;
using ProjectVictory.Domain.Combat.TurnBased;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Domain.Runtime;
using CharacterEntity = ProjectVictory.Domain.Entities.Character;

namespace ProjectVictory.Application.Combat.TurnBased;

// PV-COMBAT-012: monta os participantes do combate a partir do personagem, do estado de runtime
// (vida e mana atuais) e do encounter. Não contém regra de dano.
public sealed class CombatantFactory
{
    private readonly IMonsterCombatProfileProvider _monsterProfiles;

    public CombatantFactory(IMonsterCombatProfileProvider monsterProfiles)
    {
        _monsterProfiles = monsterProfiles;
    }

    public Combatant CreatePlayer(
        CharacterEntity character,
        CharacterRuntimeState runtimeState)
    {
        ArgumentNullException.ThrowIfNull(character);
        ArgumentNullException.ThrowIfNull(runtimeState);

        // Vida/Mana máximas derivadas do Level (PV-CHAR-010); atuais vêm do runtime.
        return new Combatant(
            Guid.NewGuid(),
            character.Id,
            CombatSide.Player,
            character.Name,
            character.Level.Value,
            character.MaximumHealth.Value,
            runtimeState.CurrentHealth,
            character.MaximumMana.Value,
            runtimeState.CurrentMana,
            ClassCombatProfileTable.ForCharacter(character.CharacterClass, character.Skills));
    }

    public Combatant CreateMonster(EncounterMonsterState monster)
    {
        ArgumentNullException.ThrowIfNull(monster);

        var definition = _monsterProfiles.Get(monster.MonsterId);

        // O Id do combatente é o Id do monstro no encounter, para sincronizar a vida de volta.
        return new Combatant(
            monster.Id,
            monster.MonsterId,
            CombatSide.Monster,
            monster.Name,
            definition.Level,
            monster.MaximumHealth,
            monster.CurrentHealth,
            0,
            0,
            definition.Profile);
    }
}
