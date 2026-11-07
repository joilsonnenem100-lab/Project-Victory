using ProjectVictory.Application.VerticalSlice;
using ProjectVictory.Domain.Runtime;
using CharacterEntity = ProjectVictory.Domain.Entities.Character;

namespace ProjectVictory.Application.Runtime;

// PV-RUNTIME-012 (fecha o R-055): reconstrução ÚNICA do contexto do Runtime a partir do checkpoint,
// usada pelo Recovery do startup e pelo "entrar no jogo" (antes o EnterGame regravava o Runtime com
// WorldContext vazio e vida/mana sem limite, desfazendo o Recovery).
// - Tela inicial: cidade principal ("Victoria", HomeCity.Location), aguardando comando (Idle).
// - Vida/Mana do checkpoint limitadas a [0, máximo do personagem].
// - *** PROVISÓRIO *** (DEC-20261106-48): vida 0 no checkpoint → vida E Mana cheias na cidade (mesma
//   regra da morte em knowledge-base/character.md, LOCKED: "Morte → Cidade Principal → HP cheio → Mana cheia").
public static class RuntimeReconstruction
{
    public static RuntimeExecutionContext FromCheckpoint(RuntimeRecoveryData runtime, CharacterEntity character)
    {
        ArgumentNullException.ThrowIfNull(runtime);
        ArgumentNullException.ThrowIfNull(character);

        var maximumHealth = character.MaximumHealth.Value;
        var maximumMana = character.MaximumMana.Value;
        var health = Math.Clamp(runtime.Health, 0, maximumHealth);
        var mana = Math.Clamp(runtime.Mana, 0, maximumMana);

        if (health == 0)
        {
            health = maximumHealth;
            mana = maximumMana;
        }

        return new RuntimeExecutionContext(
            runtime.RuntimeId,
            runtime.CharacterId,
            runtime.Initialized,
            new CharacterRuntimeState(health, mana),
            new RuntimeGameplayContext(RuntimeActivity.Idle),
            ProjectVictory.Application.World.HomeCity.Location);
    }
}
