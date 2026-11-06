using System.Security.Claims;
using ProjectVictory.API.Authentication;
using ProjectVictory.Application.Session;

namespace ProjectVictory.API.Session;

public sealed class HttpCurrentPlayerContext : ICurrentPlayerContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;


    public HttpCurrentPlayerContext(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }


    public Guid AccountId
    {
        get
        {
            var httpContext =
                _httpContextAccessor.HttpContext;


            if (httpContext is null)
            {
                throw new InvalidOperationException(
                    "HTTP context unavailable.");
            }


            // PV-SEC-002 (R-018): antes, claim ausente → InvalidOperationException (500) e
            // valor inválido → FormatException (500). Agora as duas viram 401.
            if (!AccountClaim.TryGetAccountId(
                    httpContext.User,
                    out var accountId))
            {
                throw new PlayerIdentityUnavailableException();
            }

            return accountId;
        }
    }
}