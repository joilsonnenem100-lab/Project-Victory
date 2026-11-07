# PV-QA-007 — Ponta a ponta da Fila 8 pela API real

Origem: MODO FILA 8, item 6 (2026-11-06). Lastro: `DEC-20261106-46/47/48/49`, `PV-HUNT-013`, `PV-RUNTIME-012/013`, padrão do `PV-QA-006` (API real, SQLite temporário, JWT aleatório, relógio manual, varredura disparada pelo teste — nunca o banco do PO).

## Cenários (`tests/ProjectVictory.Api.Tests/Fila8EndToEndTests.cs`)
1. `SecondHuntRefused_RestartReturnsToTheLastHunt_WithFullHealthAndMana` — Hunt na área escolhida (`huntArea`, sem diferenciar maiúsculas) → 2ª Hunt **409** e a 1ª continua (heartbeat) → checkpoint com vida 0 → **reinício** do host sobre o mesmo banco → login → 10 s exatos: sem retorno; 11 s: **nova** Hunt na "Spider Cave" → saída manual informa vida e Mana **no máximo** (oráculo: máximos lidos do repositório).
2. `Logout_IsAManualExit_NoReturn_EvenWithALateHeartbeatOrARestart` — logout = `Logout`/`Completed` → heartbeat atrasado → `LoggedOut` → 5 min com a página fechada: nenhuma queda → reinício → continua `LoggedOut` → `enter` → online 30 s: **sem** Hunt automática.

## Ressalvas do `PV-EVENT-012` aplicadas nesta branch
- (1) Retorno automático × logout: `ExecuteHuntCommand.AutomaticReturn` (aditivo); sob o portão de início, retorno automático de personagem deslogado → cancelado (`AutomaticReturnCancelledException`; o heartbeat responde `LoggedOut`), sem Hunt e sem limpar a marca. Teste `AutomaticReturnAfterLogout_IsCancelled_MarkKept`.
- (2)(3) Limites do isolamento (falha de resolução na DI e cancelamento voltam ao publisher) escritos no ADR-058.
- (4) O coordenador da composição oficial não resolve mais handlers do escopo de fora (lista vazia; os handlers vêm do escopo de cada queda).
- (5) Ordem de parâmetros do `ClearAsync`: cosmético, não alterado.
