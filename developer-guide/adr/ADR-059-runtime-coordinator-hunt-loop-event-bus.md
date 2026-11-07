# ADR-059 - Runtime Coordinator = Hunt Loop + Event Bus; Game Session cuida da presença

Status: ACCEPTED
Version: 1.0
Data: 2026-11-07
Autor: Arquitetura (Chefe de Arquitetura) — decisão de arquitetura (ADR-016); sem efeito de Game Design
Contrato: `PV-GOV-019` (Fila 9). Responde à pergunta 2 do `RELATORIO-FILA-8.md`; item 1 do "o que falta para o LOCK" do Game Session (`REVISAO-FECHAMENTO-FILA-8.md`).
Relaciona: Handbook V3 §9–10 (emendado por esta ADR), ADR-045 e ADR-051 (orquestradores não implementam regra de outro domínio), ADR-049/050/058 (Event Bus), ADR-057 v1.2 (sessão, queda, retorno), `DEC-20261106-39/40`.

---

# Contexto

O Handbook V3 §10 descreve um "Runtime Coordinator" que coordena Hunt → Encounter → Combat → Reward → Progression. No código, o `GameSessionCoordinator` (`src/ProjectVictory.Application/GameSession/GameSession.cs`) só cuida de presença e queda; a cadeia é feita pelo Hunt Loop (`AttackEncounterHandler`: resolve o turno, publica `CombatCompletedEvent` e pede ao Spawn o próximo encounter) e pelo Event Bus (`RewardOnCombatCompleted` → Reward Pipeline → Experience/Level). A revisão da Fila 8 deixou duas saídas: refatorar o código para um coordenador central ou emendar o documento.

# Decisão

1. **Emendar o documento, não refatorar o código.** O papel do "Runtime Coordinator" do Handbook §10 é cumprido pela **composição** de dois mecanismos já LOCKED/revisados:
   - **Hunt Loop** (`PV-HUNT-011`, `DEC-20261106-40`): Hunt → Encounter → Combat (turno, vitória, próximo encounter pelo Spawn), com serialização por Hunt.
   - **Event Bus** (ADR-049/050/058, LOCKED): Combat → Reward → Progression por `CombatCompletedEvent` aguardado. O Combat não conhece o Reward.
2. **Game Session = presença** (ADR-057 v1.2, `DEC-20261106-39/41/47/49`): heartbeat, detecção de queda, encerramento pelo pipeline único, retorno à última Hunt, logout e marca de deslogado. Ele **não** encadeia combate.
3. Um coordenador central que chame Hunt, Combat e Reward em sequência **não** deve ser criado: duplicaria o Event Bus e violaria ADR-045/ADR-051 (orquestrador que conhece regra de outro domínio).
4. A regra do §10 que continua valendo sem mudança: "o Coordinator não contém regras profundas" — vale para o Hunt Loop e para o Game Session.

# Consequências

- O Handbook §10 recebe nota de emenda apontando para esta ADR (texto original mantido).
- O item 1 da lista de LOCK do Game Session fica cumprido. Os itens 2 (documento de domínio `game-session.md`), 3 (R-051 resto) e 4 (R-062) continuam pendentes; o módulo segue em **REVIEW** (`DEC-20261107-03`).
- Nenhuma mudança em `src/` ou testes.

# Evidência

- `GameSessionCoordinator` só depende de `IPresenceTracker`, `GameSessionSettings`, `TimeProvider`, `IDisconnectHandler` e `IDisconnectScopeFactory` (presença e queda).
- `AttackEncounterHandler` (Hunt Loop): publica `CombatCompletedEvent` pelo `IEventBus.PublishAsync` e gera o próximo encounter pelo `ISpawnSystem` ("PV-HUNT-011: Hunt Loop contínuo").
- `RewardOnCombatCompleted : IAsyncEventHandler<CombatCompletedEvent>` → `RewardResolutionHandler` (ADR-024).
- Testes de referência: `SmokeTests/EventBusTests`, `SmokeTests/EventIsolationAndDropScopeTests`, `Api.Tests/ContinuousHuntEndToEndTests`, `Api.Tests/Fila8EndToEndTests` (verdes no fim da Fila 8, 410 testes; não reexecutados nesta fila, que não altera código).
