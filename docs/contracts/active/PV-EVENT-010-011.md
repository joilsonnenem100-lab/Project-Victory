# PV-EVENT-010 e PV-EVENT-011 — Typed Event Bus e Domain Events

- **Status:** Implementado (Fila 7, branch `chief/q7-event-010`)
- **Origem:** ordem "MODO FILA 7", item 5; M7 do `roadmap-to-playable.md`.
- **Lastro:**
  - **ADR-049:** Event Bus fortemente tipado; todo evento deriva de `EngineEvent`; nunca despacha por nome em texto.
  - **ADR-050:** o bus transporta **só Domain Events**; eventos técnicos usam infraestrutura própria.
  - `PRINCIPLES` ("Eventos futuros"): `CombatCompletedEvent → Reward → (futuro) Achievement/Quest`.
  - `spawn.md`: "toda alteração relevante publica Engine Events". **Fecha o R-046.**
- **Restrição da ordem:** desacoplar **sem alterar contratos públicos**. Nenhum endpoint, DTO ou campo mudou.

## Implementação

- **Domain** (`Domain/Events/EngineEvent.cs`):
  - `EngineEvent` (base, com `EventId`);
  - `CombatCompletedEvent`;
  - `SpawnInstanceActivatedEvent` e `SpawnInstanceDespawnedEvent`;
  - `HuntEndedEvent` (motivo, tempo e XP da Hunt).
- **Application** (`Application/Events/EventBus.cs`):
  - `IEventHandler<T>` (síncrono) e `IAsyncEventHandler<T>`;
  - `IEventBus` / `InProcessEventBus`: resolve os handlers pelo **tipo** (`IEnumerable<IEventHandler<T>>` na DI do escopo).
    - `Publish` (síncrono) é usado pelo Spawn System, que é síncrono. Ele **recusa** o despacho se houver handler assíncrono para o tipo, para nada ser ignorado em silêncio.
    - `PublishAsync` executa os síncronos e depois os assíncronos, aguardados, na ordem de registro.
  - `RewardOnCombatCompleted`: **`CombatCompletedEvent` → Reward Pipeline** (ADR-024).
  - `SpawnLifecycleCounter`: observa o ciclo de vida do Spawn (contadores; memória fixa).
- **Combat → Reward desacoplado:**
  - o `AttackEncounterHandler` **não conhece mais o Reward**; publica um `CombatCompletedEvent` por monstro morto;
  - a XP da rodada na resposta é a diferença de Experience do personagem antes e depois dos eventos. É o mesmo valor de antes, porque o Reward Pipeline é o único que altera Experience;
  - o parâmetro `RewardResolutionHandler` saiu do construtor do handler, que só é criado pela DI.
- **Spawn:** o `StatelessSpawnSystem` publica a ativação e o despawn (construtor novo e aditivo; registrado como scoped para usar o bus do escopo).
- **Fim da Hunt:** o `EndHuntHandler` (pipeline único) publica o `HuntEndedEvent` (ressalva 8 da revisão do PV-HUNT-012).

## Testes

- Novo `SmokeTests/EventBusTests`:
  - o despacho é por tipo (só o handler do tipo recebe), e o `Publish` síncrono recusa um tipo com handler assíncrono;
  - matar monstros publica `CombatCompletedEvent` e o **Reward reage ao evento**: a XP do personagem é a XP da Hunt;
  - todo Spawn ativado é despawnado no fim da Hunt (contadores iguais);
  - o `HuntEndedEvent` é publicado com o motivo `Manual`;
  - a varredura com falha numa queda tenta de novo na varredura seguinte (ressalva 2 do PV-HUNT-012).
- `Api.Tests/HuntConflictMappingTests`: `HuntAlreadyFinishedException` → **409**.
- **Todos os testes existentes passam sem mudança**: a XP das vitórias, pelo evento, é a mesma de antes.
- **Nota:** numa primeira execução, o teste de eventos (com semente aleatória) não recebeu nenhum `CombatCompletedEvent`, e isso não se repetiu. O teste passou a usar semente fixa e a suíte inteira foi executada 3 vezes seguidas, verde. Fica registrado para observação.

Resultado: build com 0 avisos; **375 aprovados, 0 ignorados, 0 falhas** (Application 31, Domain 120, Runtime 17, Smoke 109, Api 98), em 3 execuções seguidas.

## Também nesta branch

**Ressalvas da revisão do `PV-HUNT-012`:**
1. Teste do **409** (mapeamento) e teste HTTP renomeado: a segunda saída de uma Hunt já descartada responde 403.
2. Varredura resiliente: um erro numa queda não para as outras, e a queda é **desmarcada** para a próxima varredura (`UnmarkDropped`, `Failures`).
3. **R-054:** heartbeat depois do logout (requisito do cliente; o logout é de jogo e o JWT continua válido).
4. **R-027** atualizado: a Hunt Session persistida continua fora do pipeline.
5. Logout = saída manual → `Completed` (comentário do pipeline).
6. Invariante do `IHuntLock` documentada.
7. `RunningHunts.EndAllAsync`: encerramento de todas as Hunts num lugar só. **Bug corrigido aqui:** a primeira versão usava `first ??= await ...` e só encerrava a primeira Hunt; agora todas são encerradas, com teste.
8. `HuntEndedEvent` publicado.
9. R-030 (Idle gravado direto no store) segue como débito herdado.

**Ressalvas da revisão do `PV-RUNTIME-011`:** ver o contrato `PV-RUNTIME-011` (R-055, R-056, teste da mana, comentário).
