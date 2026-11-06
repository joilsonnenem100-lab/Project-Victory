# Project-Victory

# Project State

Versão do Documento: 2.0
Status: Oficial
Data da verificação: 2026-11-05
Substitui: v1.2 (2026-07-14), preservada no histórico git.
Método: estado **verificado contra o código** (hierarquia da fonte da verdade: código > contratos/ADRs > documentação). Build e testes **não foram executados** (ambiente sem `dotnet`) nesta verificação; o baseline foi executado depois na máquina do dono (2026-11-05) e está registrado em Pendências de Validação.

---

# Resumo

O código está **à frente** da documentação de estado anterior: o Vertical Slice (conta → login → personagem → entrada no mundo → hunt → combate → recompensa → XP) existe ponta a ponta, porém **sobre implementações provisórias** (stubs) em Combate, Recompensa e Progressão, e com divergências em relação a ADRs ACCEPTED. O projeto **não está pronto para jogo**; está pronto para a fase de **conformidade e substituição de stubs**.

---

# Sprint Atual

Nome: **M0 — Baseline e Governança** (ver `roadmap-to-playable.md`)
Objetivo: reconciliar documentação e código, decidir as revisões pendentes, estabelecer equipe e baseline executável.

---

# Estado dos Sistemas (verificado no código)

| Sistema | Estado real | Evidência |
|---|---|---|
| Account / Authentication | Funcional (JWT, SQLite, hash de senha) | `AccountController`, `LoginHandler`, `PasswordHasher` |
| Character | Funcional e persistido | `CharacterRepository`, `PersistentCharacterState` |
| World Entry / Runtime | Funcional; Runtime persistido (estado base) | `EnterGameHandler`, `RuntimeRepository` |
| World (definições) | Seed em memória | `WorldDefinitionSeed`, `WorldDefinitionCatalog` |
| Hunt | Funcional; **repositório em memória** | `HuntRepository` (`List<Hunt>`, singleton) |
| Hunt Session | Persistida em SQLite — **conflita com ADR-011/012** | `HuntSessionRepository`, `PersistentHuntSessionState` |
| Encounter | Funcional; estado em memória | `EncounterStateStore` |
| **Combat** | **STUB**: `CombatResolver` sempre devolve vitória do atacante | `Infrastructure/Services/CombatResolver.cs` |
| **Reward** | **STUB**: `RewardProcessor` devolve XP 0, ouro 0 e item fixo "Smoke Test Item" | `Infrastructure/Services/RewardProcessor.cs` |
| **Progression** | **Provisório**: tabela fixa, nível máx. 5, `Level` persistido — **conflita com ADR-025/027/028** | `ProgressionProcessor`, `PersistentCharacterState.Level` |
| Loot / Inventory | Parcial (Loot por definição de monstro, inventário do personagem) | `MonsterLootDefinitionProvider`, `InventoryController` |
| Event System | **Inexistente** (ADR-049/050 não implementados) | Known Gaps |
| Game Session / Runtime Coordinator / Hunt Loop contínuo | **Inexistentes** | Known Gaps |
| Projetos `ProjectVictory.Runtime` e `ProjectVictory.Shared` | **Vazios** (somente `.csproj`) | 0 arquivos `.cs` |
| Client | Telas de login, cadastro, personagem, entrada, jogo, inventário, recompensa | `src/ProjectVictory.Client/src/pages` |

Testes automatizados: **3 escritos** (`Application.Tests`: 1; `SmokeTests`: 2); **1 executável e verde** (baseline 2026-11-05); os 2 do `SmokeTests` **não compilam** (ver Pendências de Validação). `Domain.Tests` e `Runtime.Tests`: 0. `SmokeTests` **fora** do `.sln`.

---

# Roadmap oficial de módulos (`roadmap.md`) — situação confrontada com o código

| Fase | Declarado | Observação verificada |
|---|---|---|
| 0–5 | LOCKED | Arquitetura LOCKED; **implementação em código divergente/parcial** (ver revisões) |
| 6 Gameplay Foundation | IN PROGRESS (Monsters LOCKED; Monster AI, Experience Distribution, Spawn PLANNED) | Mantido: Monster AI continua sendo o próximo módulo **de arquitetura**. O código avançou por Vertical Slice (contratos `PV-VS-*`) sem esses módulos |
| 7–10 | PLANNED | Inalterado |

---

# Revisões Arquiteturais Pendentes

(substitui "Nenhuma" da v1.2 — a verificação encontrou divergências reais)

| ID | Conflito | Fontes | Resolução proposta | Gate |
|---|---|---|---|---|
| R-001 | Hunt Session **nunca persistida / sem recovery** × Handbook V3 §17–19 (Hunt Session "semi-permanente", Recovery, Checkpoint) × contratos `PV-RUNTIME-002/002-A` × código que persiste | ADR-011, ADR-012 × Handbook V3 × código | **ADR-057 (PROPOSED)**: separar *Hunt Session* (efêmera, ADR-011) de *Runtime State* (persistível) e definir checkpoint/recovery de Hunt para um jogo Idle. Ver `docs/architecture/PV-ANALYSIS-2026-11-05.md` §4 | **Product Owner** decide a política de desconexão (Game Design, `decision-log` ADR-016); emenda reverte restrições de ADR ACCEPTED |
| R-002 | `Level` persistido × Level derivado | ADR-027, ADR-028 × `PersistentCharacterState.Level` | Código deve se conformar ao ADR (hierarquia: ADR > código não aprovado por contrato). Contrato M2 | Nenhum — ADR prevalece |
| R-003 | Tabela de nível fixa (máx. 5) × Experience Table por Ascension | ADR-025 × `ProgressionProcessor` | Idem R-002 | Nenhum |
| R-004 | Event Bus tipado exigido × inexistente | ADR-049/050 × Known Gaps | Implementar em M7 (já planejado) | Nenhum |
| R-005 | Duas famílias de roadmap (`developer-guide/architecture/roadmap.md` Fases 0–10 × `docs/roadmap/phase-3..5`, ambos "Aguardando validação") | docs | `roadmap-to-playable.md` torna-se o roadmap de **execução**; `roadmap.md` permanece a ordem de **módulos**; `phase-3..5` ficam subordinados. Registrar em decision-log | Nenhum |
| R-006 | Estado oficial declarado em `project-state.md`, mas 4 documentos ainda apontam `CURRENT_CONTEXT.md` (histórico) | `PROJECT_CONTEXT.md`, `AI_WORKFLOW.md`, `architecture-checklist.md`, `README_DEVELOPMENT.md` | Ordem de leitura fixada no `CLAUDE.md`. Edição dos documentos LOCKED aguarda revisão formal (M0) | Nenhum |
| R-007 | Pastas `Application/Architecture`, `Playtest`, `Production` (42 arquivos) modelam processos de governança como handlers que ecoam booleanos; **sem referência** em nenhum outro ponto | Código × Clean Architecture (Application = casos de uso do jogo) | Remoção **somente após `dotnet build` verde**; contrato de limpeza em M1 | Build |
| R-009 | Duas séries de ADR com a mesma numeração (`decision-log.md` ADR-001…018 × `developer-guide/adr/` ADR-001…057) | decision-log × pasta adr | Novas decisões usam prefixo `DEC-`; unificação aguarda decisão | Dono/Arquiteto |
| R-010 | Higiene do repositório: `node_modules`/`dist` (2.440 arquivos; corrigido em PV-GOV-002), 3 binários/listagens e `projectvictory.db` versionados; `.gitignore` incompleto | Clone do GitHub | `node_modules/` e `dist/` ignorados e removidos do controle; binários removidos (ver `CLEANUP-MANIFEST`). `projectvictory.db` sai **depois** do `PV-PLATFORM-001` (migração no startup) | Nenhum |
| R-008 | Numeração de ADR: faltam 026, 044, 048; ADR-055 existe vazio; `docs/history/Sprint-054..056.md` vazios | `developer-guide/adr/` | Preencher ou declarar formalmente como reservados/descartados | Dono |

---

# Dívida técnica e risco verificados

Detalhe e evidências em `docs/architecture/PV-ANALYSIS-2026-11-05.md`. Itens críticos:
- **Segurança (Handbook §23–24):** só `Character`, `Game/enter` e `Item` exigem `[Authorize]`; `Hunt`, `HuntSession`, `Inventory`, `Runtime`, `Gameplay` e `Location` não exigem. Chave JWT padrão fixa no código; SQLite com caminho fixo; log com dados sensíveis habilitado.
- **Inicialização:** o `Program.cs` não aplica migrations; existem 14 migrations (uma com nome-placeholder `NomeDaMigration`, já aplicada — não renomear). O banco `projectvictory.db` versionado é o que faz um clone rodar hoje.
- **Concorrência/estado:** `HuntRepository` é `List<>` singleton (não thread-safe); `CombatContextRepository` idem; `CombatResultStore` está registrado como *scoped* guardando estado (padrão do FIX-001).

---

# Módulos LOCKED (inalterado em relação à v1.2)

Foundation (Aggregate Root, Value Objects, Entities, Character) · Character Domain (Health, Mana, Experience, Level, Inventory, Equipment, Skills) · Progression (Skill Progression, Trainer, Offline Training) · Item System · Loot · Engine Foundation (Vocations, Resources, Combat Architecture) · Gameplay Foundation: Monsters.

> LOCKED descreve a **arquitetura aprovada**. Onde o código diverge, o código é que deve ser corrigido (R-002, R-003), salvo decisão em contrário via revisão formal.

---

# Pendências de Validação

## Baseline executado na máquina do dono — 2026-11-05 (registrado em PV-GOV-002)

| Verificação | Resultado |
|---|---|
| SDK | `dotnet` 10.0.301 |
| `dotnet build ProjectVictory.sln` | **Verde**, com 4 avisos **NU1903**: `SQLitePCLRaw.lib.e_sqlite3` 2.1.11 (vulnerabilidade de severidade alta) |
| `dotnet test ProjectVictory.sln` | **1/1 passou** (`Application.Tests`). `Domain.Tests` e `Runtime.Tests` com 0 testes |
| `dotnet test tests/ProjectVictory.SmokeTests` | **NÃO compila**: CS1503 em `RewardInventoryIntegrationTest.cs` linha 70 — `Assert.Contains` de `string` contra `Inventory.Items`, que é coleção de `ItemInstance` |

Encaminhamento: correção do SmokeTests dentro de `PV-QA-001` e atualização do pacote SQLitePCLRaw (NU1903), ambos no M1 (`roadmap-to-playable.md`).

## Ainda pendentes

- Que o `ModelSnapshot` e as 14 migrations refletem o modelo atual do `DbContext` (6 DbSets: Accounts, Characters, Monsters, Items, HuntSessions, RuntimeStates).
- Execução do fluxo no navegador (`COMO USAR O SMOKE TEST.txt`).

---

# Próximo Objetivo

Executar **M0** (decisões R-001, R-005, R-006, R-008; baseline executável na máquina do dono) e em seguida **M1**. Ver `roadmap-to-playable.md`.

---

# Regras Permanentes (mantidas da v1.2)

- A documentação oficial é a única fonte de verdade, **subordinada à hierarquia código > contratos/ADRs > documentação**.
- Todo novo módulo consulta o Roadmap e este documento.
- Nenhum módulo LOCKED é alterado sem revisão arquitetural.
- Toda decisão permanente é documentada.
- Toda Sprint atualiza este documento.
