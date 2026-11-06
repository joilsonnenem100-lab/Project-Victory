using ProjectVictory.Domain.Runtime;

namespace ProjectVictory.Application.World;

// PV-SUPPLY-011 (DEC-20261106-24): a cidade de retorno — a "tela inicial" — em dados.
// *** PROVISÓRIO *** — nome "Victoria" (nome provisório, DEC-20261106-15). A localização usada é a
// única ativa do mundo semeado (WorldDefinitionSeed). Cidades de verdade são Fase 7.
public static class HomeCity
{
    public const string Name = "Victoria";

    public static RuntimeWorldContext Location { get; } =
        new(
            Guid.Parse("10000000-0000-0000-0000-000000000001"),
            Guid.Parse("20000000-0000-0000-0000-000000000001"),
            Guid.Parse("30000000-0000-0000-0000-000000000001"));
}

// PV-SUPPLY-011 (DEC-20261106-22): dados da mensagem mostrada na tela inicial depois de uma morte,
// orientando o jogador a conferir antes de voltar. Códigos estáveis; o texto é do cliente (M8).
public sealed record DeathNotice(
    string ReasonCode,
    string ReturnCity,
    IReadOnlyList<string> Checks)
{
    public const string DeathReasonCode = "Death";

    public static IReadOnlyList<string> DefaultChecks { get; } = ["Potions", "Runes", "HuntLevel"];

    public static DeathNotice ForDeath() =>
        new(DeathReasonCode, HomeCity.Name, DefaultChecks);
}
