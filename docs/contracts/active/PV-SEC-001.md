# PV-SEC-001 — Autorização e posse de recursos do jogador

Status: **IMPLEMENTADO — aguardando merge** (branch `chief/m1-sec-001`, validação em `project-state.md`)
Emitido por: Arquitetura (sessão Chefe), 2026-11-06, conforme `roadmap-to-playable.md` (M1).
Lastro: `PRINCIPLES` / Handbook V3 §23–24 (autenticação, autorização, integridade do estado); `CLAUDE.md` (Server Authority: identidade vem do token via `ICurrentPlayerContext`, nunca do corpo); `PV-ANALYSIS-2026-11-05.md` §4.3.

## Problema

`Hunt`, `HuntSession`, `Inventory`, `Runtime`, `Gameplay` e `Location` aceitam chamadas anônimas. Os comandos trazem `CharacterId`, `RuntimeId`, `HuntSessionId` ou `EncounterId` no corpo/rota e nenhum handler confere se o recurso pertence à conta autenticada: qualquer chamador age sobre o personagem de qualquer outro.

## Escopo

1. **Autenticação:** `[Authorize]` nos controllers `Hunt`, `HuntSession`, `Inventory`, `Runtime`, `Gameplay`, `Location` e `Playable` (este acrescentado pela ordem do PO de 2026-11-06, modo noite; não é usado pelo cliente). Só `Account` (cadastro/login) continua público.
2. **Posse (Application):** novo serviço `PlayerOwnershipGuard` (`Application/Session`) compara o dono do recurso com `ICurrentPlayerContext.AccountId`:
   - personagem → `Character.AccountId`;
   - runtime → personagem do `RuntimeExecutionContext`;
   - hunt session → `HuntSession.CharacterId`;
   - encounter → `Hunt.CharacterId` da hunt do encounter.
   Recurso de outra conta, ou personagem inexistente, → `OwnershipViolationException` → **HTTP 403**. Runtime/encounter inexistentes mantêm o comportamento atual dos stores.
3. **Handlers de entrada** chamam o guard antes de agir: `ExecuteHunt`, `AttackEncounter`, `StartHuntSession`, `StopHuntSession`, `GameplayActionExecution`, `CreateRuntimeExecution`, `UpdateRuntimeActivity`, `ChangeRuntimeLocation`, `GetCharacterInventory`. `ResolveLocationActions` não tem recurso do jogador (só `[Authorize]`).
4. **`ApiExceptionHandler`:** mapeia `OwnershipViolationException` para 403 (ProblemDetails, sem detalhes do recurso).

## Decisões do Arquiteto

- **D1 — Rota do Inventory mantida** (`GET api/inventory/{characterId}`), com verificação de dono. O roadmap previa "remover `characterId` livre da rota", mas a conta pode ter **vários personagens** (`AllowMultipleCharacters`) e não existe conceito de "personagem selecionado" no servidor; remover o parâmetro quebraria o cliente (`characterService.ts`) e violaria a Regra Aditiva. O objetivo de segurança (ninguém lê inventário alheio) é atingido pela verificação de dono. `characterId` passa a ser **seletor de recurso verificado**, não identidade.
- **D2 — `CharacterId` no corpo mantido** nos comandos, pelo mesmo motivo; a identidade continua vindo só do token. Leitura da ordem do PO "verificação de dono pelo token, nunca pelo corpo ou rota": o **dono** é sempre a conta do token (`ICurrentPlayerContext`); o id no corpo/rota é apenas o **seletor do recurso**, conferido contra essa conta.
- **D3 — `await` em `RuntimeController.Create` e `HuntController.Attack`.** Hoje chamam handlers assíncronos sem `await` e serializam o `Task` (defeito pré-existente). Sem `await`, a exceção de posse ficaria presa no `Task` e a resposta seria 200. `hunt/attack` não é chamado pelo cliente; `runtime/create` só é chamado por `runtimeService.createRuntime` (`src/ProjectVictory.Client/src/services/runtimeService.ts`), função que não é importada em nenhuma tela — nenhum consumidor ativo depende do formato antigo. Os handlers síncronos `UpdateRuntimeActivity` e `ChangeRuntimeLocation` passam a assíncronos (mesmo contrato HTTP).
- **D4 — 403 também para personagem inexistente**, para não revelar quais ids existem. Exceção mantida: `GET api/inventory/{characterId}` continua respondendo **404** quando o personagem não existe (o `InventoryController` já fazia isso antes; comportamento validado preservado pela Regra Aditiva). Como os ids são GUIDs aleatórios, o risco de enumeração é desprezível. Pela mesma regra, **hunt session inexistente** (`hunt-session/stop`) e **encounter cuja hunt não existe** (`hunt/attack`) passam de 500 para **403** (comportamento anterior era erro não tratado, não um contrato validado).

## Fora do escopo

Rate limiting, papéis/administração, refresh token, testes HTTP ponta a ponta com `WebApplicationFactory` (→ `PV-QA-001` parte 2), mudanças de domínio, migrations.

## Critérios de aceite

- Build verde; `dotnet test ProjectVictory.sln` sem falhas.
- Testes novos: dono é aceito; outra conta recebe `OwnershipViolationException` (personagem, hunt session, inventário, `ExecuteHunt`, runtime e encounter via guard). O mapeamento 403 na API é validado manualmente; testes HTTP por endpoint ficam no `PV-QA-001` parte 2.
- API real: endpoints do escopo respondem 401 sem token; 403 com token de outra conta; fluxo do dono (login → personagem → enter → hunt) continua 200.
- Nenhuma mudança em Domain nem em migrations; contrato de resposta dos endpoints inalterado (exceto D3, que passa a devolver o resultado real em vez do `Task` serializado).
