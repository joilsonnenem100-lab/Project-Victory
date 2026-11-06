using ProjectVictory.Domain.Combat.TurnBased;

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
