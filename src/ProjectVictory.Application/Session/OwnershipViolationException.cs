namespace ProjectVictory.Application.Session;

// PV-SEC-001: o recurso pedido não pertence à conta autenticada (ou não está visível a ela).
// A API traduz para HTTP 403 sem expor detalhes do recurso.
public sealed class OwnershipViolationException : Exception
{
    public OwnershipViolationException(
        string resource)
        : base($"Access to {resource} denied for the current account.")
    {
        Resource = resource;
    }

    public string Resource { get; }
}
