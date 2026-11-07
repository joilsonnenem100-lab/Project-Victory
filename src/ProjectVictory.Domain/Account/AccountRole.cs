namespace ProjectVictory.Domain.Account;

// PV-ADMIN-010 (DEC-20261106-12): papel da conta. Hoje o papel NÃO é gravado na conta: vem da
// configuração do servidor (Admin:Accounts), resolvida na Application (PROVISÓRIO).
public enum AccountRole
{
    Player = 0,
    Administrator = 1
}
