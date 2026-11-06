using ProjectVictory.Domain.Combat.Rules;
using ProjectVictory.Domain.Combat.TurnBased;
using ProjectVictory.Domain.Hunt;
using ProjectVictory.Domain.Runtime;
using CharacterEntity = ProjectVictory.Domain.Entities.Character;

namespace ProjectVictory.Application.Combat.TurnBased;

// PV-COMBAT-012: atributos de combate de um monstro (Level + perfil), vindos de dados.
public sealed record MonsterCombatDefinition(
    int Level,
    CombatProfile Profile);

// PV-COMBAT-012 / PV-CONTENT-011: fonte dos atributos de combate dos monstros.
public interface IMonsterCombatProfileProvider
{
    MonsterCombatDefinition Get(Guid monsterId);
}

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

// PV-COMBAT-012: Secondary Action padrão (ADR-006).
// - Jogador: usa a habilidade inicial da classe se tiver mana. *** PROVISÓRIO *** — a política do
//   jogador vira configuração própria; poções entram no PV-SUPPLY-010.
// - Monstro: nenhuma (Monster AI é o PV-MONSTER-010, M4).
public sealed class DefaultCombatActionSelector : ICombatActionSelector
{
    public CombatActionKind? SelectSecondaryAction(
        CombatSession session,
        Combatant actor)
    {
        ArgumentNullException.ThrowIfNull(actor);

        if (actor.Side != CombatSide.Player)
        {
            return null;
        }

        var cost = actor.Profile.AbilityManaCost;

        return cost > 0 && actor.CurrentMana >= cost
            ? CombatActionKind.ClassAbility
            : null;
    }
}
