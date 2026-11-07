# PV-EVENT-012 — Bus síncrono × handlers assíncronos, assinante obrigatório (R-058) e DbContext por queda (R-060)

Origem: MODO FILA 8, item 5 (2026-11-06). Decisão: `DEC-20261106-50`, **ADR-058** (novo, ACCEPTED pelo Arquiteto — arquitetura, ADR-016). Lastro: ADR-049, ADR-050, `developer-guide/domain/events.md` (DRAFT).

## Escopo
- **D1 (R-058):** ADR-058 + nota de emenda no `events.md`: "síncrono" = aguardado; publisher nunca depende de assinante; assinante obrigatório é da composição (teste de composição existente do Reward).
- **D2 (R-060):** `IEventBus.PublishNotificationAsync` (aditivo) — aguarda todos, isola a falha de cada assinante no `IEventFailureSink` (composição: `LoggingEventFailureSink`, log do host + registro em memória). Sem sink: igual ao `PublishAsync`. O `EndHuntHandler` publica o `HuntEndedEvent` assim (depois do commit).
- **D3 (R-060):** `GameSessionCoordinator` trata cada queda num escopo de DI próprio (`IDisconnectScopeFactory`; composição: `ServiceScopeDisconnectScopeFactory`). O construtor antigo (sem fábrica) continua para testes de unidade.
- **Ressalvas do `PV-RUNTIME-013`:** marca de deslogado antes de remover a presença; `Touch` ignorado e sobra de presença descartada (não vira queda) para deslogados; logout sob o portão de início do personagem; `enter` limpa a marca que só o banco tem.

## Fora do escopo
`HuntEndedEvent.Reason` como tipo do Domain; eventos fora do processo.

## Testes
Novo `SmokeTests/EventIsolationAndDropScopeTests` (6): notificação isola falha e os demais rodam; sem sink propaga; falha de assinante do `HuntEndedEvent` não quebra a saída manual; um DbContext por queda; deslogado sem presença por `Touch` e sem queda; `enter` limpa marca só do banco.
