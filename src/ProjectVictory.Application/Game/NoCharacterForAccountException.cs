namespace ProjectVictory.Application.Game;

// PV-FIX-001 (R-069): "entrar no mundo" sem personagem na conta (nunca criado ou exclusão finalizada).
// A API traduz para HTTP 404 (o recurso pedido — um personagem da conta — não existe), não 500.
public sealed class NoCharacterForAccountException : InvalidOperationException
{
    public NoCharacterForAccountException()
        : base("No character found for account.")
    {
    }
}
