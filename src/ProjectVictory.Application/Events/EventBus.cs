using ProjectVictory.Domain.Events;

namespace ProjectVictory.Application.Events;

// PV-EVENT-010 (ADR-049): handlers tipados. Síncrono (para quem publica de código síncrono, como o
// Spawn System) ou assíncrono (ex.: Reward, que grava no banco).
public interface IEventHandler<in TEvent>
    where TEvent : EngineEvent
{
    void Handle(TEvent domainEvent);
}

public interface IAsyncEventHandler<in TEvent>
    where TEvent : EngineEvent
{
    Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken = default);
}

// PV-EVENT-010 (ADR-049/050): Event Bus fortemente tipado — o despacho é pelo TIPO genérico do
// evento (nunca por string) e só transporta Domain Events (EngineEvent).
public interface IEventBus
{
    // Despacho síncrono: só handlers síncronos. Se houver handler assíncrono para o tipo, é erro de
    // composição (ele seria ignorado) — use PublishAsync.
    void Publish<TEvent>(TEvent domainEvent)
        where TEvent : EngineEvent;

    // Despacho assíncrono: handlers síncronos e depois os assíncronos, na ordem de registro, aguardados.
    // ADR-058: "síncrono" = o publisher só continua depois de TODOS os assinantes (aguardados; nunca
    // fire-and-forget). A falha de um assinante volta para o publisher.
    Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default)
        where TEvent : EngineEvent;

    // PV-EVENT-012 (R-060; ADR-058): NOTIFICAÇÃO de um fato já gravado (ex.: fim da Hunt, depois do
    // commit). Também aguarda todos os assinantes, mas a falha de um assinante é ISOLADA: vai para o
    // IEventFailureSink e não volta para o publisher nem impede os demais assinantes.
    Task PublishNotificationAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default)
        where TEvent : EngineEvent;
}

// PV-EVENT-012: destino das falhas de assinantes isolados (o host registra no log). A Application
// não conhece log; a composição fornece a implementação.
public interface IEventFailureSink
{
    void Report(EngineEvent domainEvent, Type subscriber, Exception error);
}

// Padrão em memória (testes e composição sem host): guarda as últimas falhas.
public sealed class RecordingEventFailureSink : IEventFailureSink
{
    public const int Maximum = 64;

    private readonly System.Collections.Concurrent.ConcurrentQueue<(EngineEvent Event, Type Subscriber, Exception Error)> _failures = new();

    public IReadOnlyList<(EngineEvent Event, Type Subscriber, Exception Error)> Failures => _failures.ToList();

    public void Report(EngineEvent domainEvent, Type subscriber, Exception error)
    {
        _failures.Enqueue((domainEvent, subscriber, error));

        while (_failures.Count > Maximum && _failures.TryDequeue(out _))
        {
        }
    }
}

// Implementação em processo: resolve os handlers do TIPO pela DI do escopo atual.
public sealed class InProcessEventBus : IEventBus
{
    private readonly IServiceProvider _services;

    public InProcessEventBus(IServiceProvider services)
    {
        _services = services;
    }

    // Resolução pelo TIPO (IEnumerable<THandler>) — sem dicionário de nomes.
    private IEnumerable<THandler> Handlers<THandler>() =>
        _services.GetService(typeof(IEnumerable<THandler>)) as IEnumerable<THandler> ?? [];

    public void Publish<TEvent>(TEvent domainEvent)
        where TEvent : EngineEvent
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        if (Handlers<IAsyncEventHandler<TEvent>>().Any())
        {
            throw new InvalidOperationException(
                $"{typeof(TEvent).Name} has asynchronous handlers; publish it with PublishAsync.");
        }

        foreach (var handler in Handlers<IEventHandler<TEvent>>())
        {
            handler.Handle(domainEvent);
        }
    }

    public async Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default)
        where TEvent : EngineEvent
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        foreach (var handler in Handlers<IEventHandler<TEvent>>())
        {
            handler.Handle(domainEvent);
        }

        foreach (var handler in Handlers<IAsyncEventHandler<TEvent>>())
        {
            await handler.HandleAsync(domainEvent, cancellationToken);
        }
    }

    public async Task PublishNotificationAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default)
        where TEvent : EngineEvent
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        // Sem destino para as falhas, isolar seria engolir o erro: nesse caso o comportamento é o do
        // PublishAsync (a falha volta ao publisher).
        if (_services.GetService(typeof(IEventFailureSink)) is not IEventFailureSink sink)
        {
            await PublishAsync(domainEvent, cancellationToken);
            return;
        }

        foreach (var handler in Handlers<IEventHandler<TEvent>>())
        {
            try
            {
                handler.Handle(domainEvent);
            }
            catch (Exception error) when (!cancellationToken.IsCancellationRequested)
            {
                sink.Report(domainEvent, handler.GetType(), error);
            }
        }

        foreach (var handler in Handlers<IAsyncEventHandler<TEvent>>())
        {
            try
            {
                await handler.HandleAsync(domainEvent, cancellationToken);
            }
            catch (Exception error) when (!cancellationToken.IsCancellationRequested)
            {
                sink.Report(domainEvent, handler.GetType(), error);
            }
        }
    }
}
