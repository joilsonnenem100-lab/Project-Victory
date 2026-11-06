using ProjectVictory.Application.Progression;
using ProjectVictory.Domain.Progression;

namespace ProjectVictory.API.Configuration;

// PV-PROG-013 (DEC-20261106-30): títulos por Level lidos da configuração (seção "LevelTitles") A CADA
// consulta. O appsettings.json é recarregado pelo ASP.NET quando o arquivo muda, então trocar um
// rótulo ou uma faixa não exige recompilar nem reiniciar. Exemplo de seção:
//   "LevelTitles": [ { "Tier": 0, "Label": "Novice", "MinimumLevel": 1 }, ... ]
// Sem a seção, vale a tabela padrão. Configuração inválida não derruba o jogo: mantém a última
// tabela válida e avisa no log.
public sealed class ConfigurationLevelTitleProvider : ILevelTitleProvider
{
    private readonly IConfiguration _configuration;

    private readonly ILogger<ConfigurationLevelTitleProvider>? _logger;

    private readonly object _gate = new();

    private LevelTitleTable _lastValid = LevelTitleTable.Default;

    public ConfigurationLevelTitleProvider(
        IConfiguration configuration,
        ILogger<ConfigurationLevelTitleProvider>? logger = null)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger;
    }

    public LevelTitleTable Current
    {
        get
        {
            var section = _configuration.GetSection(LevelTitleTable.SectionKey);
            var entries = section.GetChildren().ToList();

            if (entries.Count == 0)
            {
                return LevelTitleTable.Default;
            }

            try
            {
                var table = new LevelTitleTable(entries.Select(entry => new LevelTitle(
                    entry.GetValue<int>("Tier"),
                    entry.GetValue<string>("Label") ?? string.Empty,
                    entry.GetValue<int>("MinimumLevel"))));

                lock (_gate)
                {
                    _lastValid = table;
                }

                return table;
            }
            catch (Exception exception) when (exception is ArgumentException or FormatException or InvalidOperationException)
            {
                _logger?.LogWarning(exception, "Configuração LevelTitles inválida; usando a última tabela válida.");

                lock (_gate)
                {
                    return _lastValid;
                }
            }
        }
    }
}
