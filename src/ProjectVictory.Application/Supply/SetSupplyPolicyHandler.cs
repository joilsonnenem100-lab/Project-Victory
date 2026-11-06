using ProjectVictory.Application.Session;

namespace ProjectVictory.Application.Supply;

public sealed record SetSupplyPolicyCommand(
    Guid CharacterId,
    int? HealthPotionBelowPercent,
    int? ManaPotionBelowPercent);

public sealed record SetSupplyPolicyResult(
    Guid CharacterId,
    int? HealthPotionBelowPercent,
    int? ManaPotionBelowPercent,
    string AppliesFrom);

public sealed class InvalidSupplyPolicyException : Exception
{
    public InvalidSupplyPolicyException()
        : base("Supply thresholds must be between 1 and 100, or null to disable.")
    {
    }
}

// PV-SUPPLY-010 (DEC-20261106-03): o jogador configura o uso de poções em tela própria, inclusive
// durante a Hunt. O cliente só SOLICITA; o servidor valida (posse do personagem pelo token e limites)
// e aplica a partir do PRÓXIMO turno, sem alterar um turno em andamento.
public sealed class SetSupplyPolicyHandler
{
    private readonly PlayerOwnershipGuard _ownershipGuard;
    private readonly ISupplyPolicyStore _store;

    public SetSupplyPolicyHandler(
        PlayerOwnershipGuard ownershipGuard,
        ISupplyPolicyStore store)
    {
        _ownershipGuard = ownershipGuard;
        _store = store;
    }

    public async Task<SetSupplyPolicyResult> Handle(
        SetSupplyPolicyCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        await _ownershipGuard.EnsureCharacterAsync(command.CharacterId, cancellationToken);

        if (!SupplyPolicy.IsValidPercent(command.HealthPotionBelowPercent)
            || !SupplyPolicy.IsValidPercent(command.ManaPotionBelowPercent))
        {
            throw new InvalidSupplyPolicyException();
        }

        _store.SetPending(
            command.CharacterId,
            new SupplyPolicy(command.HealthPotionBelowPercent, command.ManaPotionBelowPercent));

        return new SetSupplyPolicyResult(
            command.CharacterId,
            command.HealthPotionBelowPercent,
            command.ManaPotionBelowPercent,
            "NextTurn");
    }
}
