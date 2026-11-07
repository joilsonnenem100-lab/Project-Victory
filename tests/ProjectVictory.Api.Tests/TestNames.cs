namespace ProjectVictory.Api.Tests;

// PV-CHAR-013 (PO-38, formato do Tibia): nomes de personagem únicos e VÁLIDOS para os testes — só letras,
// uma palavra, maiúscula só no início. Substitui os nomes com dígitos do Guid (recusados pela regra nova).
internal static class TestNames
{
    public static string Unique(string prefix) =>
        prefix + new string(Guid.NewGuid().ToString("N")[..12].Select(c => (char)('a' + Convert.ToInt32(c.ToString(), 16))).ToArray());
}
