using ProjectVictory.Application.Supply;
using ProjectVictory.Persistence.Database;

namespace ProjectVictory.Persistence.Supply;

// PV-PERSIST-010: linha da política de poções de um personagem (tabela SupplyPolicies).
// Ativa = a que vale nos turnos; pendente = a gravada pelo jogador, que vale a partir do próximo turno.
public sealed class PersistentSupplyPolicyState
{
    public Guid CharacterId { get; set; }

    public int? ActiveHealthPotionBelowPercent { get; set; }

    public int? ActiveManaPotionBelowPercent { get; set; }

    public bool HasPending { get; set; }

    public int? PendingHealthPotionBelowPercent { get; set; }

    public int? PendingManaPotionBelowPercent { get; set; }
}

// PV-PERSIST-010 (Handbook §17; classificação do PV-STATE-001 estendida): a política de poções é
// CONFIGURAÇÃO DO JOGADOR → Permanente. Antes vivia só em memória (SupplyPolicyStore, singleton) e
// se perdia ao reiniciar a API. Mesma semântica do DEC-20261106-03 (pendente → ativa no próximo turno).
// Grava só quando o jogador muda a configuração (SetPending) ou quando a pendente é ativada (uma vez
// por mudança) — nunca a cada turno (PV-PERSIST-011).
public sealed class PersistentSupplyPolicyStore : ISupplyPolicyStore
{
    private readonly ProjectVictoryDbContext _context;

    public PersistentSupplyPolicyStore(ProjectVictoryDbContext context)
    {
        _context = context;
    }

    public void SetPending(Guid characterId, SupplyPolicy policy)
    {
        ArgumentNullException.ThrowIfNull(policy);

        var row = Find(characterId) ?? Add(characterId);
        row.HasPending = true;
        row.PendingHealthPotionBelowPercent = policy.HealthPotionBelowPercent;
        row.PendingManaPotionBelowPercent = policy.ManaPotionBelowPercent;

        _context.SaveChanges();
    }

    public SupplyPolicy ActivatePending(Guid characterId)
    {
        var row = Find(characterId);

        if (row is { HasPending: true })
        {
            row.ActiveHealthPotionBelowPercent = row.PendingHealthPotionBelowPercent;
            row.ActiveManaPotionBelowPercent = row.PendingManaPotionBelowPercent;
            row.HasPending = false;
            row.PendingHealthPotionBelowPercent = null;
            row.PendingManaPotionBelowPercent = null;

            _context.SaveChanges();
        }

        return ToActive(row);
    }

    public SupplyPolicy GetActive(Guid characterId) => ToActive(Find(characterId));

    private PersistentSupplyPolicyState? Find(Guid characterId) =>
        _context.SupplyPolicies.Find(characterId);

    private PersistentSupplyPolicyState Add(Guid characterId)
    {
        var row = new PersistentSupplyPolicyState { CharacterId = characterId };
        _context.SupplyPolicies.Add(row);
        return row;
    }

    private static SupplyPolicy ToActive(PersistentSupplyPolicyState? row) =>
        row is null
            ? SupplyPolicy.None
            : new SupplyPolicy(row.ActiveHealthPotionBelowPercent, row.ActiveManaPotionBelowPercent);
}
