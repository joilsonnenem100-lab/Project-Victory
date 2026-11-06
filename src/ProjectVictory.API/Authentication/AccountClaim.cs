using System.Security.Claims;

namespace ProjectVictory.API.Authentication;

// PV-SEC-002 (R-018): a identidade da conta vem só da claim NameIdentifier do token
// (Server Authority). Token sem essa claim, ou com valor que não é um Guid válido, não autentica.
public static class AccountClaim
{
    public const string Type = ClaimTypes.NameIdentifier;

    public static bool TryGetAccountId(
        ClaimsPrincipal? principal,
        out Guid accountId)
    {
        accountId = Guid.Empty;

        var value =
            principal?.FindFirst(Type)?.Value;

        return Guid.TryParse(value, out accountId)
            && accountId != Guid.Empty;
    }
}
