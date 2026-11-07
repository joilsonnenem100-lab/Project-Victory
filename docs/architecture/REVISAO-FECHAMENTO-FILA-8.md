# Revisão de LOCK — Game Session, Recovery e Event System (PV-GOV-013, Fila 8)

Anexo do `RELATORIO-FILA-8.md`. Critério único (o mesmo da Fila 7, `DEC-20261106-44`): **LOCK só quando o documento que rege o módulo está cumprido**, com evidência no código e em teste. Decisão: `DEC-20261106-51`.

| Módulo | Documento que rege | Resultado |
|---|---|---|
| Event System (Typed Event Bus + Domain Events) | `developer-guide/domain/events.md` (+ emenda `ADR-058`), ADR-049, ADR-050 | **LOCKED** |
| Recovery do Runtime | Handbook V3 §18 (Recovery), §19 (Checkpoint), ADR-057 Parte A, `DEC-20261106-47/48` | **LOCKED** |
| Game Session + Runtime Coordinator | Handbook V3 §9–10, ADR-057 v1.2 (Parte B) — **sem documento de domínio próprio** | **REVIEW** |

## Event System — evidências (`events.md`, item a item)
- Só publica/distribui Domain Events; não conhece Character/Combat/Hunt/Inventory/Loot/Quest: `src/ProjectVictory.Application/Events/EventBus.cs` só referencia `ProjectVictory.Domain.Events` (restrição `where TEvent : EngineEvent`).
- `EngineEvent` com `EventId`, Timestamp (`OccurredAt`), imutável (`record`, `init`): `src/ProjectVictory.Domain/Events/EngineEvent.cs`. CorrelationId/CausationId são opcionais no documento.
- Bus síncrono (= aguardado, `ADR-058` e emenda no `events.md`), fortemente tipado (despacho por `IEnumerable<THandler>`), stateless (só o `IServiceProvider`).
- Subscribers por tipo, não conhecidos pelos publishers (Combat publica `CombatCompletedEvent` sem conhecer o Reward — `AttackEncounterHandler`); "nunca obrigatórios" para o publisher, obrigatoriedade só da composição (teste de composição do `RewardOnCombatCompleted`).
- Testes: `SmokeTests/EventBusTests`, `SmokeTests/EventIsolationAndDropScopeTests`.
- Revisões: R-046 e R-058 fechadas. Dívida fora do documento (não impede o LOCK): `HuntEndedEvent.Reason` como texto.

## Recovery — evidências (Handbook §18: "servidor inicia → carrega estado → reconstrói contexto → continua")
- Carrega: `RuntimeRecoveryService` lê os checkpoints de Runtime (ordem determinística) e os personagens em lote, antes do `app.Run` (`Program.cs`).
- Reconstrói: `RuntimeReconstruction.FromCheckpoint` — tela inicial "Victoria", Idle, vida/mana limitadas; vida 0 → cheia (`DEC-48`, PROVISÓRIO). O `EnterGame` usa a mesma reconstrução e não desfaz o Recovery (R-055).
- Continua: reinício = queda (`DEC-47`) — retorno à última Hunt escolhida após > 10 s online, com nova Hunt Session; quem fez logout não volta (`DEC-49`, coluna `LoggedOut`). Hunt Session nunca recuperada (ADR-057).
- Checkpoints (§19): início, recompensa, conclusão, logout (`ICheckpointService`).
- Testes: `SmokeTests/RuntimeRecoveryTests`, `RestartAsDropTests`, `LogoutMarkAndHuntAreaTests`; E2E `Api.Tests/Fila8EndToEndTests` e `ContinuousHuntEndToEndTests` (reinício real do host).
- Revisões: R-055 e R-056 fechadas. Dívida aceita (não contradiz o documento): R-063 (contexto em memória não recalculado no enter; leitura em lote inclui inventário).

## Notas da revisão final (ressalvas do revisor)
- **DEC-48 PROVISÓRIO dentro do LOCK do Recovery:** o valor "vida 0 → vida e Mana cheias" é **dado de regra** num ponto único (`RuntimeReconstruction`); se o PO mudar a regra, troca-se esse ponto por contrato, **sem** revisão do LOCK (a arquitetura — carregar, reconstruir, continuar — não muda).
- **Por que Recovery trava sem documento de domínio próprio e Game Session não:** o Recovery é regido por documentos que o código cumpre sem conflito (Handbook §18/§19, ADR-057 Parte A). O Game Session tem **conflito** com o seu documento (Handbook §10); sem decisão sobre esse conflito, o documento de domínio próprio é o caminho para fixar o que vale.
- O oráculo da vida 0 no E2E lê o checkpoint gravado (prova indireta); a reconstrução em si é provada no smoke (`RestartAsDropTests.HealthZeroInTheCheckpoint_ComesBackWithFullHealthAndMana`).

## Game Session + Runtime Coordinator — o que falta para o LOCK
1. **Handbook §10 × código:** o documento diz que o Runtime Coordinator coordena Hunt → Encounter → Combat → Reward. No código, o `GameSessionCoordinator` só detecta quedas; a cadeia do Hunt Loop está no `AttackEncounterHandler` + Event Bus. Precisa de decisão documentada (emenda/ADR: o Hunt Loop é o coordenador da cadeia e o Game Session cuida da presença) ou de ajuste no código.
2. **Documento de domínio do módulo** (`developer-guide/domain/game-session.md`) não existe: escrever com as regras do ADR-057 v1.2 e da Fila 8 (presença, queda, retorno, logout, deslogado).
3. **R-051 (resto):** gerar os encounters pela área escolhida (hoje só existe a Spider Cave; depende de conteúdo).
4. **R-062:** premissa de servidor único (portão, locks, presença em memória) — registrada; decidir antes de escalar.

Não reavaliados nesta fila (fora do item): Hunt Loop + queda (R-059, R-053, R-027, R-030, R-061), Monster AI, Experience Distribution, Spawn System.
