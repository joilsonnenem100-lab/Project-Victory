# ADR-058 - Event Bus Dispatch Semantics (aguardado, assinante obrigatório de composição, notificação isolada)

Status: ACCEPTED
Version: 1.0
Data: 2026-11-06
Autor: Arquitetura (Chefe de Arquitetura) — decisão de arquitetura (ADR-016); sem efeito de Game Design
Contrato: `PV-EVENT-012` (Fila 8). Resolve o R-058; parte do R-060.
Relaciona: ADR-049 (Typed Event Bus), ADR-050 (Domain Events), `developer-guide/domain/events.md` (DRAFT)

---

# Contexto

O `events.md` (DRAFT) pede um Event Bus **síncrono** ("o Publisher somente continua sua execução após todos os Subscribers concluírem") e diz que Subscribers "nunca são obrigatórios". O código (`PV-EVENT-010/011`) aceita handlers **assíncronos** (`PublishAsync`), necessários para o Reward, que grava no banco; e o `RewardOnCombatCompleted` é **obrigatório** para o jogo (teste de composição garante o registro). Além disso, o `HuntEndedEvent` é publicado depois do commit do fim da Hunt: um assinante que falhe gerava erro depois de o fato já estar gravado (R-060).

# Decisão

1. **Síncrono = aguardado.** "Síncrono" no `events.md` significa: o publisher só continua depois que **todos** os assinantes concluírem, na ordem de registro. Handlers assíncronos **aguardados** (`await`) cumprem a regra. É proibido despacho em segundo plano (fire-and-forget) no Event Bus de Domain Events.
2. **Publisher nunca depende de assinante.** A regra "nunca são obrigatórios" vale para o **publisher**: ele publica sem saber quem assina e funciona com zero assinantes. Um assinante pode ser **obrigatório para a composição** (ex.: `RewardOnCombatCompleted` → sem ele o jogo não entrega recompensa); essa obrigação é da **composição** (`AddProjectVictoryCore`) e é garantida por **teste de composição**, nunca pelo publisher nem pelo bus.
3. **Dois modos de publicação:**
   - `PublishAsync` — fato cuja reação faz parte do mesmo caso de uso (ex.: `CombatCompletedEvent` → Reward). A falha de um assinante **volta** ao publisher.
   - `PublishNotificationAsync` — **notificação** de um fato já gravado (ex.: `HuntEndedEvent`). Também aguarda todos, mas a falha de um assinante é **isolada**: vai para o `IEventFailureSink` (log do host) e não impede os demais. Sem destino configurado, comporta-se como `PublishAsync` (nunca engole erro em silêncio).
4. `Publish` (sem `Async`) continua para publicadores de código síncrono (Spawn) e recusa tipos com handler assíncrono (erro de composição).

# Consequências

- `events.md` recebe nota de emenda apontando para este ADR (sem apagar texto).
- Novos eventos pós-commit usam `PublishNotificationAsync`.
- Pendente (fora deste ADR): `HuntEndedEvent.Reason` é texto (o enum está na Application) — trocar por tipo do Domain num contrato futuro.
