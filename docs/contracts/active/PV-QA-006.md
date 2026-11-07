# PV-QA-006 — Ponta a ponta do M6 pela API real

- **Status:** Implementado (Fila 7, branch `chief/q7-qa-006`)
- **Origem:** ordem "MODO FILA 7", item 6.
- **Lastro:** `DEC-20261106-20/22`, ADR-057 v1.2, ADR-012; contratos `PV-RUNTIME-010`, `PV-HUNT-011`, `PV-HUNT-012` e `PV-RUNTIME-011`.

**Escopo:** somente testes e documentação; nenhuma mudança em `src/`.

## Como

`tests/ProjectVictory.Api.Tests/ContinuousHuntEndToEndTests.cs`:
- **API real**: `WebApplicationFactory`, Development, SQLite **temporário** de teste, JWT aleatório. **Nunca o banco do PO.**
- O **tempo** vem de um relógio manual (`TimeProvider`) injetado no host.
- A varredura automática fica desligada (`GameSession__SweepIntervalSeconds=0`), e o teste dispara as varreduras pelo `GameSessionCoordinator` do próprio host.
- A semente de combate é fixa (`Combat__FixedSeed`, encounter [Rat, Cave Rat, Rat]).

## Cenários

| Pedido da ordem | Teste | O que confere |
|---|---|---|
| Hunt contínua com 2 ou mais encounters | `ContinuousHunt_TwoOrMoreEncounters_ThenLeave_KeepsEverything` | com a semente fixa, o desfecho é **determinístico**: vitória no 1º → `nextEncounterId` na **mesma** Hunt; **vitória no 2º** → o 3º já existe; a saída manual encerra (`Manual`) com a XP da Hunt; a XP do personagem é a XP da Hunt (nada perdido) |
| Queda curta não encerra | `ShortDrop_DoesNotEnd_…` | **exatamente 60 s** sem sinal → a Hunt segue (heartbeat devolve a mesma Hunt; o ataque funciona) |
| Queda de mais de 1 min encerra | (mesmo teste) | aos 60 s a varredura não trata nenhuma queda; com mais **61 s** ela trata **exatamente uma** (a do jogador); o encounter da Hunt foi descartado (o ataque responde **500**, como para encounter inexistente) |
| Retorno depois de 10 s cria uma nova Hunt na última Hunt | (mesmo teste) | volta: entrar no jogo (tela inicial); 1º heartbeat sem Hunt; **exatamente 10 s** ainda não; **11 s** → `returnedToLastHunt`, `lastHuntArea = "Spider Cave"`, `huntId` **novo**, e o encounter novo é atacável |
| Reinício do servidor | `ServerRestart_CharacterAndRuntimeBack_HuntNot_NoAutomaticReturn` | o primeiro host para no meio da Hunt; o segundo sobe sobre o mesmo banco de teste: login ok, XP mantida, nenhuma Hunt (ADR-057), sem retorno automático (o reinício não é uma "queda" detectada), o "entrar" devolve o **mesmo** Runtime e uma nova Hunt pode começar |

Resultado: build com 0 avisos; **378 aprovados, 0 ignorados, 0 falhas** (Application 31, Domain 120, Runtime 17, Smoke 109, Api 101).

## Revisão (`pv-architecture-reviewer`): APROVADO COM RESSALVAS — aplicadas na branch `chief/q7-gov-011`

1. Teste contínuo sem asserção condicional: o desfecho do 2º encounter (vitória) foi conferido e é afirmado; o 3º encounter e a saída `Manual` são sempre exercitados.
2. A varredura do teste devolve as quedas (nenhuma aos 60 s, uma aos 61 s) e o status exato do ataque depois do descarte é conferido.
3. A senha dos testes virou a constante `ApiFactory.TestPassword`.
4. O relógio manual começa na hora real.
5. Linha de escopo no contrato.

**Dependência conhecida (revisão do `PV-GOV-011`, ressalva 9):** este E2E e o `SmokeTests/EventBusTests` afirmam desfechos ("Victory") que dependem da semente fixa **e** do balanceamento atual (fórmula de dano e dados da Spider Cave, PROVISÓRIOS). Mudar o balanceamento pode exigir nova semente ou novo desfecho esperado. O 500 do ataque a um encounter descartado também está fixado aqui (**R-061**).
