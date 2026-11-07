# PV-LEGACY-001 — Retirada do caminho legado `POST api/gameplay/action` (R-053)

Origem: Fila Noturna 1 (2026-11-07), executada sem o PO. Lastro: **`DEC-20261107-04`** (remoção autorizada do `api/gameplay/action` e da cadeia `GameplayActionDispatcher` → `StartEncounterCombatHandler` → `CreateCombatContextHandler`, **começando por teste funcional**; lista exata do que sai no contrato e no `CLEANUP-MANIFEST`), mesmo rito do `DEC-20261106-25/31` (commit isolado e reversível, grep sem referência, build e testes verdes, registro no manifesto); R-053, R-027 e R-029 em `project-state.md`; Regra Aditiva (`PROJECT-VICTORY-ARCHITECTURE-RULES.md`: a remoção só existe por decisão documentada — o `DEC-20261107-04`). Ordem da fila: "nunca apague arquivo fora da cadeia"; "sem migration".

## 1. O que o caminho legado faz (mapeado no código, antes de qualquer mudança)
`POST api/gameplay/action` `{ actionId, characterId }` (`[Authorize]`):
1. `GameplayController.Execute` → `GameplayActionExecutionHandler.Handle`: confere a posse do personagem (`PlayerOwnershipGuard`, 403 se for de outra conta).
2. `GameplayActionDispatcher.Handle(ExecuteLocationActionCommand)`: `actionId` vazio → erro; pega o runtime do personagem e chama o **`ExecuteHuntHandler`** (o MESMO do `api/hunt/execute`) → cria a Hunt `Running` e o Encounter. Desde o `PV-HUNT-013`, uma segunda Hunt em andamento → 409.
3. `StartEncounterCombatHandler` → `CreateCombatContextHandler`: cria um `CombatContext` (ativo) no `CombatContextRepository` (singleton) — **nunca limpo** (R-053) — e `BindEncounterToCombatHandler` liga o Encounter ao contexto.
4. `ResolveCombatHandler` → `ICombatResolver.Resolve`: o resolver legado só devolve "atacante vence" **sem efeito** (sem dano, sem XP, sem loot); o contexto passa a resolvido.
5. Resposta 200 `{ actionId, gameplaySystem: "Hunt" }`.

**Resultado de jogo:** o personagem fica com uma Hunt `Running` e um Encounter — exatamente o que o `api/hunt/execute` produz (que, além disso, devolve a Hunt/Encounter e deixa lutar de verdade por `api/hunt/attack` e sair por `api/hunt/leave`, sob o lock por Hunt do `PV-HUNT-011`). O legado não entrega nada que o `api/hunt/*` não entregue.

## 2. Quem usa
- **Cliente** (`src/ProjectVictory.Client`, só leitura): **não chama**. Rotas do cliente: `api/account`, `api/account/login`, `api/character`, `api/inventory/{id}`, `api/game/enter`, `api/hunt/execute`, `api/runtime/create`. As ocorrências de "gameplay" no cliente são pastas de componentes (`components/gameplay/…`), não o endpoint.
- **Testes:** `Api.Tests/AuthorizationHttpTests` (401 sem token na lista `ProtectedEndpoints`; 403 com o personagem de outra conta). Nenhum teste funcional antes deste contrato.
- **Servidor:** só o `GameplayController`.

## 3. Cada tipo da cadeia e onde mais é usado (grep `*.cs` em `src` e `tests`, sem `bin/obj`)
| Tipo | Usado fora da cadeia? | Destino |
|---|---|---|
| `API/Controllers/GameplayController` | não | **sai** |
| `Application/Gameplay/GameplayActionExecutionHandler` (+ `…Command`, `…Result`) | não (só o controller e o registro de DI) | **sai** (e a linha de DI) |
| `Application/World/GameplayActionDispatcher` | **sim** — `World/ExplorationDecisionHandler` (fora da cadeia; código morto: não registrado no DI, sem chamador) | **fica** |
| `World/ExecuteLocationActionCommand` | **sim** — `ExplorationDecisionHandler` | fica |
| `World/ExecuteLocationActionResult` | só o dispatcher (que fica) | fica |
| `Encounter/StartEncounterCombatHandler` (+ `…Command`, `…Result`) | só o dispatcher (que fica) | fica (apagar quebraria o dispatcher) |
| `Encounter/BindEncounterToCombatHandler` (+ `…Command`, `…Result`) | só o `StartEncounterCombatHandler` | fica |
| `Combat/CreateCombatContextHandler` (+ `…Command`, `…Result`) | **sim** — `SmokeTests/CoreGameplayFlowSmokeTest` (fluxo central, não só o legado) | fica |
| `Combat/ResolveCombatHandler` (+ `…Command`, `…Result`) | só o dispatcher | fica |
| `ExecuteHuntHandler` | **sim** — `api/hunt/execute` (fluxo validado) | fica (fora da remoção) |
| `ICombatContextRepository` / `CombatContextRepository` / `CombatRequest` / `ICombatResolver.Resolve` | **sim** — vários handlers e testes | ficam |

## 4. Decisão (leitura mais segura, registrada)
- **Retira-se o ENDPOINT** (`GameplayController` + `GameplayActionExecutionHandler/Command/Result` + o registro de DI do handler): nenhum desses tipos é usado fora da cadeia, o cliente não chama a rota, e o `api/hunt/*` cobre o mesmo resultado de jogo (teste funcional, §5). Com isso some a **única porta de entrada** que criava Hunts fora do lock por Hunt e acumulava `CombatContext` sem limpeza (o risco do R-053).
- **NÃO se apaga** `GameplayActionDispatcher` → `StartEncounterCombatHandler` → `CreateCombatContextHandler` (nem `ExecuteLocationAction*`, `BindEncounterToCombat*`, `ResolveCombat*`): o dispatcher é referenciado por `ExplorationDecisionHandler` (arquivo **fora** da cadeia — a ordem proíbe apagá-lo) e o `CreateCombatContextHandler` pelo `CoreGameplayFlowSmokeTest`. Ficam **sem caminho HTTP** (código inalcançável pela API). Os registros de DI desses tipos ficam (inofensivos; apagar não é necessário para o objetivo).
- **Pendência (pergunta ao PO/Arquiteto):** autorizar, num contrato próprio (`PV-LEGACY-002`), a remoção dos órfãos restantes — `ExplorationDecisionHandler/Command/Result` (código morto) e, por consequência, o dispatcher, `ExecuteLocationAction*`, `StartEncounterCombat*`, `BindEncounterToCombat*`, `ResolveCombat*` — e o ajuste do `CoreGameplayFlowSmokeTest` para não depender do `CreateCombatContextHandler`.
- **Sem migration:** nada disso toca banco (`CombatContextRepository` é em memória).

## 5. Testes
- **Antes da remoção** (commit próprio): `Api.Tests/LegacyGameplayActionHttpTests` prova pela API real o comportamento atual — `gameplay/action` responde 200 `{ actionId, gameplaySystem: "Hunt" }`, deixa uma Hunt em andamento (o `api/hunt/execute` seguinte → 409) e a 2ª chamada → 409 (a mudança do `PV-HUNT-013` que não tinha teste) — e que o `api/hunt/*` cobre o mesmo resultado (execute → Hunt + Encounter; 2ª → 409; `attack` luta de verdade; `leave` encerra).
- **Depois da remoção:** o mesmo arquivo passa a provar que `POST api/gameplay/action` responde **404** (com token válido) e que o `api/hunt/*` segue funcionando.
- **Ajustados:** `AuthorizationHttpTests` — sai a linha `POST /api/gameplay/action` do `ProtectedEndpoints` (401) e a asserção 403 do `gameplay/action` (só testavam o caminho removido).

## Fora do escopo
Cliente (não alterado); a Hunt Session persistida `api/hunt-session/*` (R-027); limpeza dos `CombatContext` já criados (em memória: somem no reinício).

## Resultado (Fila Noturna 1)
- Commits: `694217c` (contrato + teste funcional do legado, 482 verdes) → `be9e094` (remoção isolada).
- **Apagados:** `API/Controllers/GameplayController.cs`; `Application/Gameplay/GameplayActionExecutionHandler.cs`, `…Command.cs`, `…Result.cs`; registro de DI do handler (trocado por comentário). Grep sem referência restante; lista no `CLEANUP-MANIFEST-2026-11-06.md`.
- **Testes removidos/ajustados:** `AuthorizationHttpTests` — linha `POST /api/gameplay/action` do 401 (1 caso de teoria) e asserção 403 do `gameplay/action`; `ControllerAuthorizationConventionTests` — a linha do `GameplayController` some sozinha (teoria por reflexão); `LegacyGameplayActionHttpTests.LegacyAction_*` passa a provar 404 e que a rota não cria Hunt; `HuntFlow_CoversTheSameGameResult` continua.
- Build 0 avisos; **480 testes** (`Application.Tests` 34; `Domain.Tests` 136; `Runtime.Tests` 17; `SmokeTests` 169; `Api.Tests` 124), 0 falhas. Sem migration. Cliente não alterado.
- **Pendência:** `PV-LEGACY-002` (autorização para apagar `ExplorationDecisionHandler/Command/Result` e o resto da cadeia interna; ajustar o `CoreGameplayFlowSmokeTest`).
