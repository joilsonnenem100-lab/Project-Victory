using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ProjectVictory.API.Session;
using Xunit;

namespace ProjectVictory.Api.Tests;

// PV-SEC-002 (R-018): defesa em profundidade — mesmo que um principal sem identidade chegue ao
// contexto do jogador, ele lança PlayerIdentityUnavailableException (→ 401), nunca 500.
public sealed class HttpCurrentPlayerContextTests
{
    private static HttpCurrentPlayerContext ContextWith(params Claim[] claims)
    {
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(claims, "test"))
        };

        return new HttpCurrentPlayerContext(new HttpContextAccessor { HttpContext = httpContext });
    }

    [Fact]
    public void ValidClaim_ReturnsAccountId()
    {
        var accountId = Guid.NewGuid();

        Assert.Equal(accountId, ContextWith(new Claim(ClaimTypes.NameIdentifier, accountId.ToString())).AccountId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("nao-e-um-guid")]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public void MissingOrInvalidClaim_Throws401Exception(string? value)
    {
        var context = value is null
            ? ContextWith()
            : ContextWith(new Claim(ClaimTypes.NameIdentifier, value));

        Assert.Throws<PlayerIdentityUnavailableException>(() => context.AccountId);
    }
}
