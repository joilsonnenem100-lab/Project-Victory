# Relatório do Modo Noite — 2026-11-06

Ordem do Product Owner e do Arquiteto-Chefe: executar a fila de contratos em sequência, com branches empilhadas (cada uma nasce da anterior), push de cada branch e **um único merge da última branch** (`chief/m1-gov-005`) pela manhã. Nenhuma alteração na `main`, nenhum `push --force`, nenhuma migration editada, nenhum segredo escrito no repositório.

## Resumo

| # | Contrato | Branch | Status |
|---|---|---|---|
| 1 | PV-SEC-001 — Autorização e posse de recursos | `chief/m1-sec-001` | ✅ Concluído |
| 2 | PV-GOV-003 — Hunt Analyzer (DEC-20261106-01) | `chief/m1-gov-003` | ✅ Concluído |
| 2 | PV-GOV-004 — Decisões do PO (Spider Cave, suprimentos, desconexão B1 + Offline Training) | `chief/m1-gov-004` | ✅ Concluído (a revisão reprovou por um bloqueio de documentação, corrigido) |
| 3 | PV-QA-001 parte 2 — Testes de JWT, autorização, DI, Domain e Runtime | `chief/m1-qa-001b` | ✅ Concluído |
| 4 | PV-PLATFORM-003 — NU1903 e o "7º aviso" | `chief/m1-platform-003` | ✅ Concluído |
| 5 | PV-QA-002 — Baseline do cliente | `chief/m1-qa-002` | ✅ Concluído (registro, sem corrigir código do cliente) |
| 6 | PV-GOV-005 — Relatório somente leitura | `chief/m1-gov-005` | ✅ Concluído (relatório; nada aplicado) |

Nenhum contrato foi pulado. Nada travou por decisão de PO, ação destrutiva ou conflito com ADR/LOCKED sem solução documental.

**Merge da manhã:** `chief/m1-gov-005` contém todos os commits acima, em ordem. Todas as branches estão no `origin`.

Observação de empilhamento: as ressalvas de revisão de um contrato foram aplicadas na branch **em andamento** quando a revisão chegou (a branch anterior já tinha filhas). Por isso alguns commits `PV-SEC-001`, `PV-GOV-004`, `PV-QA-001` e `PV-PLATFORM-003` estão em branches posteriores — o conteúdo final está completo na última branch. As branches antigas `chief/gov-003` e `chief/gov-004` (de antes da ordem de empilhamento) **não foram apagadas**; ficaram obsoletas, substituídas por `chief/m1-gov-003` e `chief/m1-gov-004`.

## Build e testes (última branch, `chief/m1-gov-005`, executados)

- `dotnet build ProjectVictory.sln --no-incremental`: **verde, 0 erros, 0 avisos** (antes: 10 linhas de aviso na `main`).
- `dotnet test ProjectVictory.sln`: **92 testes — 91 passam, 1 ignorado, 0 falhas.** Application 1 · Domain 22 · Runtime 17 · SmokeTests 11 (1 Skip aprovado pelo PO até o `PV-REWARD-010`) · Api.Tests 41.
- Nenhum processo `dotnet` ou `ProjectVictory.API` restou ao fim de cada contrato (verificado com `Get-Process dotnet,ProjectVictory.API` após `dotnet build-server shutdown`).

## Por contrato

### 1. PV-SEC-001 (`chief/m1-sec-001`)
Commits: `bb60035`, `20a41ab`, `f0cb498`, `79d2acf`, `1dd2614` (este último na pilha seguinte).
- `[Authorize]` em Hunt, HuntSession, Inventory, Runtime, Gameplay, Location, **Playable** e Game (na classe); só `Account` público.
- `PlayerOwnershipGuard` (Application): personagem, runtime, hunt session e encounter precisam pertencer à conta do token → senão **403**. A identidade da conta vem só do token (`ICurrentPlayerContext`).
- API real: 401 sem token, 200 para o dono, 403 para outra conta em todos os endpoints do escopo.
- Revisões: APROVADO COM RESSALVAS (duas vezes); todas aplicadas.
- Decisão do Arquiteto a confirmar pelo PO (**R-011**): o `characterId` continua no corpo/rota como **seletor de recurso conferido contra a conta do token** (a conta tem vários personagens e o cliente usa a rota). A leitura literal de "nunca pelo corpo ou rota" exigiria um conceito de "personagem selecionado" no servidor.

### 2. PV-GOV-003 e PV-GOV-004 (`chief/m1-gov-003`, `chief/m1-gov-004`)
Commits: `55e6607` (GOV-003); `f05b638`, `c92d443`, `056fc80` (GOV-004).
- `DEC-20261106-01` a `-09` no `decision-log.md`; ADR-057 **ACCEPTED** (B1 + Offline Training); notas de emenda em ADR-011, ADR-012 e **ADR-004**; R-001 resolvida; roadmap: M3 Spider Cave + poções, M6 replanejado, dependências do Hunt Analyzer.
- Revisão: **REPROVADO** por um bloqueio — o ADR-004 (ACCEPTED) diz que a Hunt "não depende da permanência do jogador conectado", em conflito com B1 e não registrado. Corrigido em `056fc80` (nota de emenda no ADR-004, conflito explícito no ADR-057, R-001 cita ADR-004, R-013 para `ADR-002`/`loot.md`, Handbook §17/18 atendido só para o Runtime).

### 3. PV-QA-001 parte 2 (`chief/m1-qa-001b`)
Commits: `2220209`, `e087040` (ressalvas, na pilha seguinte).
- Projeto novo `tests/ProjectVictory.Api.Tests` (`WebApplicationFactory`, Development, SQLite temporário, **chave JWT aleatória gerada em execução**): 401 sem token em 15 endpoints; token com issuer/audience/chave errados → 401; outra conta → 403; `JwtSettings.Validate`; **convenção por reflexão**: todo controller exceto `Account` tem `[Authorize]`.
- `DependencyInjectionTests` (SmokeTests, `ValidateOnBuild`) **encontrou defasagem real**: `AttackEncounterHandler` sem `ICombatResultStore` e a cadeia de reward/loot no DI de teste — corrigido só em `tests/`.
- `Domain.Tests` (22) e `Runtime.Tests` (17) com testes reais do que já existe.
- **Nenhum arquivo de `src/` alterado** por este contrato.
- Revisão: APROVADO COM RESSALVAS; aplicadas. Registrado **R-014**: composição de DI duplicada entre `Program.cs` e o teste (contrato futuro para um método de composição compartilhado).

### 4. PV-PLATFORM-003 (`chief/m1-platform-003`)
Commits: `d3e7c13`, `6ab915e`, `97af19d` (ressalvas, na pilha seguinte).
- **Decisão:** EF Core **10.0.0 → 10.0.14** (patch da mesma linha) em vez de fixar a dependência transitiva. Resolve **CVE-2025-6965** (GHSA-2m69-gcr7-jv3q, SQLite nativo em `SQLitePCLRaw.lib.e_sqlite3 <= 2.1.11`; agora 2.1.13).
- **O "7º aviso":** `System.Security.Cryptography.Pkcs` 9.0.0 no projeto da API, via `EntityFrameworkCore.Design` 10.0.0 → `Microsoft.Build.Tasks.Core`, com **CVE-2026-84008** (alta, NU1903) e **CVE-2026-70162** (moderada, NU1902). Também resolvido pelo `.Design` 10.0.14. A contagem varia com build incremental e com o momento da consulta à base de advisories; a contagem auditável na `main` é de 10 linhas.
- Ferramentas fora do `.sln` também corrigidas: `tools/PvDatabaseCheck` e `ItemCheck/` (achado da revisão). `PatchEngine` sem vulneráveis.
- `has-pending-model-changes` sem mudanças; clone limpo aplica as 14 migrations; nenhuma migration editada.
- Revisão: APROVADO COM RESSALVAS; aplicadas.

### 5. PV-QA-002 (`chief/m1-qa-002`)
Commits: `5c8629e`, `814cfbe`.
- `npm install` OK (lock inalterado); `npm run build` **verde**; **não existe lint**; `npm audit`: 6 vulnerabilidades de build (4 altas); `tsc --noEmit` (diagnóstico): 443 erros, 426 por falta de `@types/react*`, 2 imports quebrados, código inalcançável (`GamePage`, `WorldScenePage`, `PlayerStatePanel` e outros).
- O cliente **não quebra** com o PV-SEC-001: todas as chamadas protegidas passam por `apiRequest`, que envia o token.
- Nenhum código do cliente alterado. Proposto o contrato **`PV-PLATFORM-005`** (M1): `npm audit fix`, `@types/react*`, script de typecheck.
- Revisão: APROVADO COM RESSALVAS; aplicadas.

### 6. PV-GOV-005 (`chief/m1-gov-005`)
Commit: `4831dc6`. Relatório em `docs/architecture/PV-GOV-005-RELATORIO-2026-11-06.md`; **nada aplicado**.
- ADR-026/044/048 **nunca existiram** no histórico; ADR-055 sempre vazio. Propostas: 026 = **Ascension Model** (evidência indireta; mecânica é do PO), 044 e 048 = **número não utilizado** (sem evidência), 055 = **Event-Driven Quests** (texto proposto a partir de ADR-049/050/056).
- **Não existe nenhum arquivo com `#U`** no repositório, no disco ou no histórico. Hipótese: `#U2014` é como ferramentas de zip no Windows mostram o travessão "—"; há 4 arquivos com travessão (2 em `docs/Visual-Identity`) e 1 `.docx` com acentos. Mapa de renomeação proposto, com as citações a atualizar.
- `CURRENT_CONTEXT.md`: 4 documentos, **5 linhas** (2 em `AI_WORKFLOW.md`, que é LOCKED; 1 em `PROJECT_CONTEXT.md`, LOCKED; `README_DEVELOPMENT.md`; `architecture-checklist.md`), com texto proposto.
- Revisão: APROVADO COM RESSALVAS. Corrigidos no relatório: a coluna "quem cita" do mapa de renomeação (os arquivos listados citavam só o ID do contrato, não o nome do arquivo; `PV-ASSET-001-INTEGRATION-CONTRACT.md:36` já usa o nome-alvo, e o renomeio conserta esse link quebrado), a lista de commits do ADR-055 e uma frase do ADR-026 proposto que decidia sem respaldo em ADR (virou "em aberto"). Informativo: `CLAUDE.md:18` lista 3 dos 4 documentos com referência antiga a `CURRENT_CONTEXT` — ajustar quando o contrato do R-006 for aplicado.

## Achados do revisor ainda abertos (registrados, não bloqueiam)

- **R-011** — confirmação do PO sobre a leitura "dono pelo token; id como seletor".
- **R-012** — `ExecuteGameplayLoopCommand` recebe `AccountId` (latente; nenhum controller o expõe).
- **R-013** — `ADR-002` e `knowledge-base/loot.md` (LOCKED) ainda citam "Hunt Offline".
- **R-014** — composição de DI duplicada (API × testes).
- Testes futuros: 403 por HTTP em runtime/hunt-session/encounter; token expirado e sem claim de conta.
- Versões desalinhadas entre famílias de pacotes (sem vulnerabilidade); `dotnet-ef` global em 10.0.10.
- Cliente: `PV-PLATFORM-005` (dependências) e `PV-UI-0xx` (código).

## Decisões de PO pendentes

1. **R-011** — confirma que o `characterId` no corpo/rota, conferido contra a conta do token, atende "verificação de dono pelo token"? Ou quer um "personagem selecionado" no servidor (contrato novo)?
2. **Valor do loot no lucro do Hunt Analyzer** — confirma o preço de venda ao NPC (`DEC-20261106-08`, provisório)?
3. **Monstros da Spider Cave** — confirma Spider, Poison Spider e Giant Spider (recomendados pelo Arquiteto) e o status do `DEC-20261106-09`?
4. **Hunt Analyzer "durante e depois"** — o "depois da Hunt" do `DEC-20261106-01` está correto (o texto do PO dizia "em tempo real")?
5. **Offline Training (antes do `PV-TRAIN-010`, M6):** como se obtém o item de Training; se o Training começa sozinho ao desconectar; skill do Monk (Fist Fighting?); velocidade e teto de horas offline; Free × Premium.
6. **Janela de reconexão:** uma queda curta de rede encerra a Hunt ou há tolerância de alguns segundos? (afeta a emenda do ADR-004)
7. **ADR-026 (Ascension):** o que é uma Ascension no jogo (quantas, o que dispara, o que muda).

## Próximos contratos sugeridos (M1)

`PV-PLATFORM-004` (schema fora de Development) · `PV-PLATFORM-005` (dependências do cliente) · `PV-CLEAN-001` (quarentena de `Application/Architecture|Playtest|Production`, R-007 — o build está verde) · revisão documental de R-006/R-008 a partir do `PV-GOV-005`.
