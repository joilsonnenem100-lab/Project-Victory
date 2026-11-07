# Relatório — MODO FILA 7 (M6 sem treino e M7), 2026-11-06

Ordem do PO e do Arquiteto-Chefe: "MODO FILA 7". Branches empilhadas a partir da `main` (`ee6f0f3`, que contém a Fila 6) e trabalho no worktree `C:/pv7`, com push de cada branch. Não houve `push --force` nem alteração na `main`. Nenhum arquivo foi apagado, nenhum segredo foi escrito e nenhuma migration existente foi editada. **Uma** migration nova foi criada, só aditiva. O banco real do PO **não foi tocado**: os testes usam bancos temporários.

**Não feito, por ordem:** `PV-TRAIN-010`, combate elemental, balanceamento, UI.

## Branches (o merge da última traz todas)

| # | Branch | Conteúdo |
|---|---|---|
| 1 | `chief/q7-runtime-010` | PV-RUNTIME-010 |
| 2 | `chief/q7-hunt-011` | PV-HUNT-011 + ressalvas do RUNTIME-010 |
| 3 | `chief/q7-hunt-012` | PV-HUNT-012 + ressalvas do HUNT-011 |
| 4 | `chief/q7-runtime-011` | PV-RUNTIME-011 |
| 5 | `chief/q7-event-010` | PV-EVENT-010/011 + ressalvas do HUNT-012 e do RUNTIME-011 |
| 6 | `chief/q7-qa-006` | PV-QA-006 |
| 7 | `chief/q7-gov-011` | PV-GOV-011 (fechamento) + ressalvas do EVENT e do QA-006 + este relatório (**última**) |

## Status

| Item | Status | Revisão |
|---|---|---|
| 1. PV-RUNTIME-010 — sessão de jogo, heartbeat, presença, queda, última Hunt | ✅ | APROVADO COM RESSALVAS (aplicadas) |
| 2. PV-HUNT-011 — Hunt Loop contínuo, XP/hora, limpeza, lock por Hunt | ✅ | APROVADO COM RESSALVAS (aplicadas) |
| 3. PV-HUNT-012 — encerramento por queda e logout | ✅ | APROVADO COM RESSALVAS (aplicadas) |
| 4. PV-RUNTIME-011 — Recovery no startup | ✅ | APROVADO COM RESSALVAS (aplicadas ou registradas) |
| 5. PV-EVENT-010/011 — Event Bus tipado e Domain Events | ✅ | APROVADO COM RESSALVAS (aplicadas/registradas) |
| 6. PV-QA-006 — ponta a ponta do M6 pela API real | ✅ | APROVADO COM RESSALVAS (aplicadas; o revisor executou os 3 E2E: verdes) |
| 7. PV-GOV-011 — fechamento (critério de LOCK) | ✅ | revisão documentada; nenhum LOCK |

Nada travou nem foi pulado.

## Build e testes (executados)

| Momento | Build | Testes |
|---|---|---|
| Início (main, Fila 6) | 0 avisos | 348 |
| RUNTIME-010 | 0 avisos | 354 |
| HUNT-011 | 0 avisos | 359 |
| HUNT-012 | 0 avisos | 369 |
| RUNTIME-011 | 0 avisos | 371 |
| EVENT-010/011 | 0 avisos | 375 (3 execuções seguidas) |
| QA-006 | 0 avisos | 378 |
| **Final** | **0 avisos, 0 erros** | **380 aprovados, 0 ignorados, 0 falhas** (Application 31, Domain 120, Runtime 17, Smoke 111, Api 101), em 2 execuções seguidas |

## Migrations criadas

| Migration | Tipo | Conteúdo |
|---|---|---|
| `20261106224154_RuntimeLastHuntArea` | **nova, aditiva** | `AddColumn<string>("LastHuntArea", "RuntimeStates", nullable: true)`, a "última Hunt escolhida" (checkpoint mínimo do Runtime, ADR-057 v1.2). O `Down` remove a coluna |

Foi gerada com `dotnet ef migrations add`, usando chave JWT aleatória só no ambiente do comando e banco temporário.

## O que mudou no jogo (regras do M6)

- **Heartbeat HTTP** (`POST api/game/heartbeat`, sem WebSocket); presença em memória thread-safe; os tempos ficam em configuração (`GameSession`: 60 s, 10 s, varredura a cada 5 s).
- **Hunt contínua:** vencer um encounter gera o próximo (Spawn); a Hunt termina **só** por morte, saída manual (`POST api/hunt/leave`, novo) ou queda. Fim de suprimento não encerra.
- **Queda de mais de 1 min** encerra pelo pipeline único (motivo `Disconnect`), volta à "Victoria" e não perde nada. **No primeiro minuto** (inclusive exatamente 60 s), a Hunt segue.
- **Ao voltar:** tela inicial; depois de **mais de 10 s** online, uma **nova** Hunt na última Hunt escolhida.
- **Logout** (`POST api/game/logout`, novo): encerra a Hunt (motivo `Logout`, forma de saída manual), grava o checkpoint e não gera retorno automático.
- **Reinício do servidor:** personagem e Runtime voltam (Recovery no startup, na tela inicial); a Hunt não volta (ADR-057).
- **Medição** para o XP/hora: tempo e XP da Hunt nas respostas.
- **Eventos (M7):** `CombatCompletedEvent` → Reward (o Combat não conhece mais o Reward); ciclo de vida do Spawn e fim da Hunt publicados.

## Testes alterados (comportamento antigo → `DEC-20261106-22`)

A vitória de um encounter deixou de encerrar a Hunt (fecha o R-036). Seis testes foram ajustados, com a tabela completa no contrato `PV-HUNT-011`:
- `HuntEndingPipelineTests.Victory…` foi substituído (vitória → próximo encounter → saída manual encerra);
- `HuntEndingPipelineTests.Death…`: a Hunt agora sai da memória;
- `SupplyTests.Potions…`;
- `SaveStrategyTests.Victory…`;
- os E2E `Victory_RewardsByMonster…` e `Potion_Policy…`.

## Achados dos revisores (principais)

- **RUNTIME-010:**
  - corrida que podia criar duas Hunts → `TryConsumeReturn` atômico;
  - retorno pendente consumido com Hunt em andamento;
  - "NoCharacter" no lugar do 500;
  - R-051.
- **HUNT-011:**
  - Hunts duplicadas → R-052 (decisão do PO); a queda e o logout encerram todas;
  - caminho legado `gameplay/action` → R-053;
  - saída manual: 403/409 no lugar do 500;
  - lock liberado nos caminhos "já terminou";
  - próximo encounter gerado antes do descarte.
- **HUNT-012:**
  - varredura resiliente (uma falha não prende a Hunt);
  - teste do 409;
  - R-054 (heartbeat depois do logout);
  - R-027 atualizado;
  - logout documentado como saída manual;
  - invariante do lock;
  - `RunningHunts.EndAllAsync`, onde **eu tinha introduzido um bug** (`??=` encerrava só a primeira Hunt), corrigido e coberto por teste;
  - `HuntEndedEvent`.
- **RUNTIME-011:**
  - o `EnterGame` desfaz a reconstrução do Recovery → R-055 (contrato próprio);
  - escalabilidade e ordem → R-056;
  - teste da mana;
  - comentário.
- **EVENT-010/011:**
  - a varredura engolia os erros → voltaram ao log;
  - DbContext compartilhado entre quedas → R-060;
  - invariante da XP documentada;
  - assinantes movidos para Reward/Spawn; `OccurredAt` no `EngineEvent`; teste de composição do assinante obrigatório;
  - **a causa da falha intermitente do teste de eventos:** o `UpdateHealth(10_000)` dos meus testes não tinha efeito (a vida é limitada ao máximo), então o Knight podia morrer antes de matar um monstro; corrigido com semente fixa e asserção de vitória;
  - teste de logout com duas Hunts.
- **QA-006:**
  - o teste contínuo aceitava dois desfechos → o desfecho real (vitória) foi conferido e é afirmado, e o 3º encounter e a saída `Manual` são sempre exercitados;
  - a varredura do teste devolve as quedas (nenhuma aos 60 s, uma aos 61 s);
  - status exato do ataque depois do descarte;
  - senha como constante;
  - relógio a partir da hora real.

## Fechamento (`docs/architecture/REVISAO-FECHAMENTO-FILA-7.md`)

Critério único: LOCK só quando o documento de domínio está cumprido. **Nenhum módulo foi travado.**

| Módulo | Estado | Falta |
|---|---|---|
| Monster AI | REVIEW | R-050, R-043 |
| Experience Distribution | REVIEW | R-045 |
| Spawn System | REVIEW | R-057 (Respawn Policy, Spawn Delay, Spawn Definition versionada); R-044/R-046 fechados |
| Game Session + Runtime Coordinator | REVIEW | R-051, R-054, R-055 |
| Hunt Loop + queda | REVIEW | R-059, R-052, R-053, R-027, R-030, R-060 |
| Recovery | REVIEW | R-055, R-056 |
| Event System | REVIEW | R-058 (Timestamp já acrescentado; falta o bus síncrono × handlers assíncronos e o assinante obrigatório) |

**Fechados nesta fila:** R-036, R-044, R-046, R-048, R-029 (para `api/hunt/*`).

## PROVISÓRIO e onde trocar

| Item | Onde |
|---|---|
| Queda (60 s), retorno (10 s), varredura (5 s) | `appsettings.json` → `GameSession` |
| Área da última Hunt (sempre a padrão, R-051) | `ExecuteHuntHandler` / `IHuntAreaCatalog` |
| Logout como saída manual (`Completed`) | `EndHuntHandler` (switch do motivo) |

## Perguntas ao PO

1. **R-052:** iniciar uma Hunt com outra já em andamento deve ser **recusado** ou deve **encerrar a anterior**?
2. **Reinício do servidor:** deve disparar o retorno automático à última Hunt, como uma queda? Hoje não dispara. O Recovery já deixa o Runtime pronto; só falta tratar o reinício como queda.
3. **R-055:** um personagem com vida 0 no checkpoint deve voltar com vida cheia na cidade?
4. **Logout:** confirmar que conta como saída manual (Hunt `Completed`).
5. **Pendentes de filas anteriores:**
   - habilidades de monstro (R-043/R-050);
   - XP em grupo (R-045);
   - risco da Save Strategy;
   - Critical, Dodge e Healing Power;
   - `BasicCombatAttributes`;
   - R-040;
   - "Arcane Lance";
   - R-035;
   - fórmula de dano;
   - poções;
   - balanceamento;
   - Treino (Free × Premium).

## Notas de processo

- Os contratos foram derivados da ordem do PO e escritos junto com o código, como o modo fila autoriza; o `roadmap-to-playable.md` foi marcado ✅ antes de cada revisão, e as ressalvas entraram na branch seguinte.
- Um comando meu travou (um `cat` esperando entrada) e foi interrompido antes de qualquer edição; refiz o passo.
- Ao fim de cada contrato rodei `dotnet build-server shutdown` e conferi os processos `dotnet`.
