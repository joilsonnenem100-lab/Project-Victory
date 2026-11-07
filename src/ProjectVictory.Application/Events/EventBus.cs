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
    Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default)
        where TEvent : EngineEvent;
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
}
