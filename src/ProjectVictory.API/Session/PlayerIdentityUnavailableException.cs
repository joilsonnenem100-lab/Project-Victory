namespace ProjectVictory.API.Session;

// PV-SEC-002 (R-018): requisição sem identidade de conta utilizável → 401 (ApiExceptionHandler).
// Defesa em profundidade: o JwtBearer já recusa esses tokens em OnTokenValidated.
public sealed class PlayerIdentityUnavailableException : Exception
{
    public PlayerIdentityUnavailableException()
        : base("Player identity unavailable.")
    {
    }
}
