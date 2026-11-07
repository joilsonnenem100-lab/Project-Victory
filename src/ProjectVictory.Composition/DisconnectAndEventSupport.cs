using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProjectVictory.Application.Events;
using ProjectVictory.Application.GameSession;
using ProjectVictory.Domain.Events;

namespace ProjectVictory.Composition;

// PV-EVENT-012 (fecha o R-060): cada queda é tratada num escopo de DI próprio (DbContext próprio).
public sealed class ServiceScopeDisconnectScopeFactory : IDisconnectScopeFactory
{
    private readonly IServiceScopeFactory _scopes;

    public ServiceScopeDisconnectScopeFactory(IServiceScopeFactory scopes)
    {
        _scopes = scopes;
    }

    public IDisconnectScope Create() => new Scope(_scopes.CreateAsyncScope());

    private sealed class Scope(AsyncServiceScope scope) : IDisconnectScope
    {
        public IEnumerable<IDisconnectHandler> Handlers =>
            scope.ServiceProvider.GetServices<IDisconnectHandler>();

        public ValueTask DisposeAsync() => scope.DisposeAsync();
    }
}

// PV-EVENT-012 (ADR-058): falhas de assinantes isolados (notificações) vão para o log do host e ficam
// registradas em memória (observabilidade e testes). Sem log configurado, só o registro em memória.
public sealed class LoggingEventFailureSink : IEventFailureSink
{
    private readonly ILogger<LoggingEventFailureSink>? _logger;

    public LoggingEventFailureSink(ILogger<LoggingEventFailureSink>? logger = null)
    {
        _logger = logger;
    }

    public RecordingEventFailureSink Recorded { get; } = new();

    public void Report(EngineEvent domainEvent, Type subscriber, Exception error)
    {
        Recorded.Report(domainEvent, subscriber, error);
        _logger?.LogError(error, "Assinante {Subscriber} falhou ao tratar {Event} ({EventId}); falha isolada (notificação).",
            subscriber.Name, domainEvent.GetType().Name, domainEvent.EventId);
    }
}
