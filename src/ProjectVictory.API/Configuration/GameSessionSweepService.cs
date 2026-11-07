using ProjectVictory.Application.GameSession;

namespace ProjectVictory.API.Configuration;

// PV-RUNTIME-010: varredura periódica de quedas (Runtime Coordinator). Cada varredura usa um escopo
// próprio de DI. Intervalo em configuração ("GameSession:SweepIntervalSeconds"; 0 = desligada).
public sealed class GameSessionSweepService : BackgroundService
{
    private readonly IServiceScopeFactory _scopes;
    private readonly GameSessionSettings _settings;
    private readonly ILogger<GameSessionSweepService> _logger;

    public GameSessionSweepService(
        IServiceScopeFactory scopes,
        GameSessionSettings settings,
        ILogger<GameSessionSweepService> logger)
    {
        _scopes = scopes;
        _settings = settings;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_settings.SweepInterval <= TimeSpan.Zero)
        {
            return;
        }

        using var timer = new PeriodicTimer(_settings.SweepInterval);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                using var scope = _scopes.CreateScope();
                var coordinator = scope.ServiceProvider.GetRequiredService<GameSessionCoordinator>();
                await coordinator.SweepAsync(stoppingToken);

                // Revisão do PV-EVENT-010/011 (ressalva 1): cada queda com erro vai para o log (ela é
                // desmarcada e tentada de novo na próxima varredura).
                foreach (var (presence, error) in coordinator.Errors)
                {
                    _logger.LogError(error, "Falha ao encerrar a Hunt do personagem {CharacterId} após queda.", presence.CharacterId);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception exception)
            {
                // Uma varredura com erro não derruba o servidor; a próxima tenta de novo.
                _logger.LogError(exception, "Falha na varredura de quedas de conexão.");
            }
        }
    }
}
