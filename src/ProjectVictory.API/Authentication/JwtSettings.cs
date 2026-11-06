namespace ProjectVictory.API.Authentication;

// PV-PLATFORM-002: configuração JWT vinda de appsettings, variáveis de ambiente
// ou user-secrets (Development). Não existe valor padrão para a chave.
public sealed class JwtSettings
{
    public const string SectionName = "Jwt";

    // HMAC-SHA256 exige chave de pelo menos 256 bits.
    public const int MinimumKeyLength = 32;

    public string Key { get; init; } = string.Empty;

    public string Issuer { get; init; } = string.Empty;

    public string Audience { get; init; } = string.Empty;


    // Lança InvalidOperationException com mensagem clara se a configuração
    // estiver ausente ou fraca. Chamado no startup, antes de a API subir.
    public void Validate(
        bool isDevelopment)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(Key))
        {
            errors.Add("Jwt:Key não configurada.");
        }
        else if (Key.Length < MinimumKeyLength)
        {
            errors.Add(
                $"Jwt:Key fraca: tem {Key.Length} caracteres; o mínimo é {MinimumKeyLength}.");
        }

        if (string.IsNullOrWhiteSpace(Issuer))
        {
            errors.Add("Jwt:Issuer não configurado.");
        }

        if (string.IsNullOrWhiteSpace(Audience))
        {
            errors.Add("Jwt:Audience não configurado.");
        }

        if (errors.Count == 0)
        {
            return;
        }

        var howToFix =
            isDevelopment
                ? "Em Development, configure com: dotnet user-secrets set \"Jwt:Key\" \"<chave aleatória com 32+ caracteres>\" --project src/ProjectVictory.API"
                : "Fora de Development, defina a variável de ambiente Jwt__Key (32+ caracteres) e, se necessário, Jwt__Issuer e Jwt__Audience.";

        throw new InvalidOperationException(
            "Configuração JWT inválida — a API não pode subir. "
            + string.Join(" ", errors)
            + " "
            + howToFix
            + " Ver developer-guide/guides/configuracao-local.md.");
    }
}
