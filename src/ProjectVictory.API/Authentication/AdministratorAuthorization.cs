using Microsoft.AspNetCore.Authorization;
using ProjectVictory.Application.Admin;
using ProjectVictory.Domain.Account;

namespace ProjectVictory.API.Authentication;

// PV-ADMIN-010 (DEC-20261106-12): exigência de papel de administrador.
public sealed class AdministratorRequirement : IAuthorizationRequirement
{
}

// PV-ADMIN-010: a identidade vem só da claim da conta do token (AccountClaim, Server Authority);
// o papel vem do IAccountRoleResolver (configuração do servidor). Não satisfeito → 403.
public sealed class AdministratorAuthorizationHandler : AuthorizationHandler<AdministratorRequirement>
{
    public const string PolicyName = "Administrator";

    private readonly IAccountRoleResolver _roles;

    public AdministratorAuthorizationHandler(IAccountRoleResolver roles)
    {
        _roles = roles;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AdministratorRequirement requirement)
    {
        if (!AccountClaim.TryGetAccountId(context.User, out var accountId))
        {
            return;
        }

        if (await _roles.GetRoleAsync(accountId) == AccountRole.Administrator)
        {
            context.Succeed(requirement);
        }
    }
}
