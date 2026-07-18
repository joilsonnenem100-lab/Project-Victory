using System.Security.Claims;
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


            var claim =
                httpContext.User.FindFirst(
                    ClaimTypes.NameIdentifier);


            if (claim is null)
            {
                throw new InvalidOperationException(
                    "Player identity unavailable.");
            }


            return Guid.Parse(
                claim.Value);
        }
    }
}