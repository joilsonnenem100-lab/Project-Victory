# PV-QA-006 — Ponta a ponta do M6 pela API real

- **Status:** Implementado (Fila 7, branch `chief/q7-qa-006`)
- **Origem:** ordem "MODO FILA 7", item 6.
- **Lastro:** `DEC-20261106-20/22`, ADR-057 v1.2, ADR-012; contratos `PV-RUNTIME-010`, `PV-HUNT-011`, `PV-HUNT-012` e `PV-RUNTIME-011`.

## Como

`tests/ProjectVictory.Api.Tests/ContinuousHuntEndToEndTests.cs`:
- **API real**: `WebApplicationFactory`, Development, SQLite **temporário** de teste, JWT aleatório. **Nunca o banco do PO.**
- O **tempo** vem de um relógio manual (`TimeProvider`) injetado no host.
- A varredura automática fica desligada (`GameSession__SweepIntervalSeconds=0`), e o teste dispara as varreduras pelo `GameSessionCoordinator` do próprio host.
- A semente de combate é fixa (`Combat__FixedSeed`, encounter [Rat, Cave Rat, Rat]).

## Cenários

| Pedido da ordem | Teste | O que confere |
|---|---|---|
| Hunt contínua com 2 ou mais encounters | `ContinuousHunt_TwoOrMoreEncounters_ThenLeave_KeepsEverything` | vitória no 1º → `nextEncounterId` na **mesma** Hunt; o 2º encounter é lutado; se for vitória, o 3º já existe e a saída manual encerra (`Manual`) com a XP da Hunt; se for morte, encerra por `Death`. A XP do personagem é a XP da Hunt (nada perdido) |
| Queda curta não encerra | `ShortDrop_DoesNotEnd_…` | **exatamente 60 s** sem sinal → a Hunt segue (heartbeat devolve a mesma Hunt; o ataque funciona) |
| Queda de mais de 1 min encerra | (mesmo teste) | mais **61 s** → a varredura encerra; o encounter da Hunt foi descartado (o ataque falha) |
| Retorno depois de 10 s cria uma nova Hunt na última Hunt | (mesmo teste) | volta: entrar no jogo (tela inicial); 1º heartbeat sem Hunt; **exatamente 10 s** ainda não; **11 s** → `returnedToLastHunt`, `lastHuntArea = "Spider Cave"`, `huntId` **novo**, e o encounter novo é atacável |
| Reinício do servidor | `ServerRestart_CharacterAndRuntimeBack_HuntNot_NoAutomaticReturn` | o primeiro host para no meio da Hunt; o segundo sobe sobre o mesmo banco de teste: login ok, XP mantida, nenhuma Hunt (ADR-057), sem retorno automático (o reinício não é uma "queda" detectada), o "entrar" devolve o **mesmo** Runtime e uma nova Hunt pode começar |

Resultado: build com 0 avisos; **378 aprovados, 0 ignorados, 0 falhas** (Application 31, Domain 120, Runtime 17, Smoke 109, Api 101).
