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

Testes automatizados (após `PV-QA-001` parte 2, 2026-11-06): **92** no `.sln` — `Application.Tests` 1; `Domain.Tests` 22; `Runtime.Tests` 17; `SmokeTests` 11 (1 com Skip aprovado pelo PO até o `PV-REWARD-010`); `Api.Tests` 41 (HTTP com `WebApplicationFactory`, `JwtSettings` e convenção de `[Authorize]` por reflexão). **91 passam, 1 ignorado, 0 falhas.** Detalhe em Pendências de Validação.

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
| R-001 | Hunt Session **nunca persistida / sem recovery** × Handbook V3 §17–19 (Hunt Session "semi-permanente", Recovery, Checkpoint) × contratos `PV-RUNTIME-002/002-A` × código que persiste | ADR-011, ADR-012, **ADR-004** × Handbook V3 × código | **ADR-057 (ACCEPTED em 2026-11-06; PROPOSED na emissão)**: separar *Hunt Session* (efêmera, ADR-011) de *Runtime State* (persistível) e definir checkpoint/recovery de Hunt para um jogo Idle. Ver `docs/architecture/PV-ANALYSIS-2026-11-05.md` §4. ✅ **RESOLVIDA em 2026-11-06:** `ADR-057` **ACCEPTED** — Parte B (PO): offline a Hunt não rende nada; ao desconectar a Hunt termina pelo pipeline único (B1); progresso offline só pelo Offline Training de skills (`DEC-20261106-05`). Parte A simplificada: checkpoint/recovery só do personagem e do Runtime, sem recuperar Hunt Session. Notas de emenda em ADR-011/012 e **ADR-004** (cuja frase "não depende da permanência do jogador conectado" deixa de valer para a desconexão). Handbook §17/§18 atendido só para o Runtime | Nenhum |
| R-002 | `Level` persistido × Level derivado | ADR-027, ADR-028 × `PersistentCharacterState.Level` | Código deve se conformar ao ADR (hierarquia: ADR > código não aprovado por contrato). Contrato `PV-PROG-010` (M2): Level derivado; a coluna `Level` **fica** como cache gravado com o valor derivado e nunca lido como verdade (`PV-GOV-006` D2; `level.md`) | Nenhum — ADR prevalece |
| R-003 | Tabela de nível fixa (máx. 5) × Experience Table por Ascension | ADR-025 × `ProgressionProcessor` | Idem R-002. Curva decidida pelo PO em 2026-11-06 (`DEC-20261106-10/11`); teto de 500 por Ascension (`ADR-026`). ✅ **Resolvida em `PV-PROG-011`** (2026-11-06): Experience Table gerada pela curva, Levels 1–500, usada por `ProgressionProcessor`, `LevelCalculator` e `GrowthProcessor` | Nenhum |
| R-004 | Event Bus tipado exigido × inexistente | ADR-049/050 × Known Gaps | Implementar em M7 (já planejado) | Nenhum |
| R-005 | Duas famílias de roadmap (`developer-guide/architecture/roadmap.md` Fases 0–10 × `docs/roadmap/phase-3..5`, ambos "Aguardando validação") | docs | `roadmap-to-playable.md` torna-se o roadmap de **execução**; `roadmap.md` permanece a ordem de **módulos**; `phase-3..5` ficam subordinados. Registrar em decision-log | Nenhum |
| R-006 | Estado oficial declarado em `project-state.md`, mas 4 documentos ainda apontam `CURRENT_CONTEXT.md` (histórico) | `PROJECT_CONTEXT.md`, `AI_WORKFLOW.md`, `architecture-checklist.md`, `README_DEVELOPMENT.md` | Ordem de leitura fixada no `CLAUDE.md`. Edição dos documentos LOCKED aguarda revisão formal (M0) | Nenhum  · Levantamento e proposta: `PV-GOV-005-RELATORIO-2026-11-06.md`. |
| R-007 | Pastas `Application/Architecture`, `Playtest`, `Production` (42 arquivos) modelam processos de governança como handlers que ecoam booleanos; **sem referência** em nenhum outro ponto | Código × Clean Architecture (Application = casos de uso do jogo) | Remoção **somente após `dotnet build` verde**; contrato de limpeza em M1 | Build |
| R-009 | Duas séries de ADR com a mesma numeração (`decision-log.md` ADR-001…018 × `developer-guide/adr/` ADR-001…057) | decision-log × pasta adr | Novas decisões usam prefixo `DEC-`; unificação aguarda decisão | Dono/Arquiteto |
| R-010 | Higiene do repositório: `node_modules`/`dist` (2.440 arquivos; corrigido em PV-GOV-002), 3 binários/listagens e `projectvictory.db` versionados; `.gitignore` incompleto | Clone do GitHub | `node_modules/` e `dist/` ignorados e removidos do controle; binários removidos (ver `CLEANUP-MANIFEST`). `projectvictory.db` sai **depois** do `PV-PLATFORM-001` (migração no startup). ✅ **Concluído em `PV-PLATFORM-001`:** `projectvictory.db` fora do controle e no `.gitignore` (aviso de backup no `CLEANUP-MANIFEST`) | Nenhum |
| R-008 | Numeração de ADR: faltam 026, 044, 048; ADR-055 existe vazio; `docs/history/Sprint-054..056.md` vazios | `developer-guide/adr/` | Preencher ou declarar formalmente como reservados/descartados | Dono  · Levantamento e proposta: `PV-GOV-005-RELATORIO-2026-11-06.md`. ✅ **ADR-026 preenchido** (Ascension Model, ACCEPTED, `PV-GOV-006`); faltam 044, 048 e 055 |
| R-011 | Ordem do PO: "verificação de dono do personagem pelo token, nunca pelo corpo ou rota" × `PV-SEC-001` D1/D2, que mantêm `characterId` no corpo/rota como **seletor de recurso conferido contra a conta do token** | Ordem do PO (2026-11-06) × `docs/contracts/active/PV-SEC-001.md` | Leitura do Arquiteto: o dono vem sempre do token; o id só escolhe qual personagem da conta. A leitura literal exigiria um conceito de "personagem selecionado" no servidor (contrato novo) e mudaria o cliente | **PO confirma** a leitura ou pede o contrato novo |
| R-012 | `ExecuteGameplayLoopCommand` (`src/ProjectVictory.Application/Game/ExecuteGameplayLoopCommand.cs`) recebe `AccountId` como parâmetro; nenhum controller o expõe hoje | Server Authority (`CLAUDE.md`) | Se for exposto, a conta deve vir de `ICurrentPlayerContext`, nunca do corpo (risco latente, revisão do `PV-SEC-001`) | Nenhum enquanto não exposto |
| R-013 | `ADR-002` ("suporte ao modo Hunt Offline") e `knowledge-base/loot.md` (LOCKED, "regras de Hunt Offline previamente definidas") × decisão do PO de não haver Hunt Offline (`DEC-20261106-05`, ADR-057) | ADR-002, `loot.md` × ADR-057 | ADR-057 prevalece (decisão mais recente, com aceite do PO). Notas nos documentos só por revisão documentada (`loot.md` é LOCKED) | Revisão documental futura |
| R-014 | Composição de DI duplicada: `Program.cs` × `tests/ProjectVictory.SmokeTests/TestServiceCollectionExtensions.cs`. O `DependencyInjectionTests` valida só a cópia de teste; ~16 serviços do `Program.cs` não estão no DI de teste | Revisão do `PV-QA-001` parte 2 | Contrato futuro: extrair um método de composição compartilhado em `src` (ex.: `AddProjectVictoryApplication()`) usado pela API e pelos testes. Até lá, a validação do container real vem do `Api.Tests` (Development + `ValidateOnBuild`) | Contrato próprio (M1/M2) |

---

# Dívida técnica e risco verificados

Detalhe e evidências em `docs/architecture/PV-ANALYSIS-2026-11-05.md`. Itens críticos:
- **Segurança (Handbook §23–24):** ~~só `Character`, `Game/enter` e `Item` exigem `[Authorize]`; `Hunt`, `HuntSession`, `Inventory`, `Runtime`, `Gameplay` e `Location` não exigem.~~ **Resolvido em `PV-SEC-001`:** todos esses controllers exigem token; `PlayerOwnershipGuard` (Application) garante que personagem, runtime, hunt session e encounter pertencem à conta do token (outra conta → 403). Público por desenho: só cadastro/login (`Account`); `Playable/status` também passou a exigir token. Contrato: `docs/contracts/active/PV-SEC-001.md`. ~~Chave JWT padrão fixa no código; SQLite com caminho fixo; log com dados sensíveis habilitado.~~ **Resolvido em `PV-PLATFORM-002`:** chave JWT sem valor padrão (user-secrets em Development, `Jwt__Key` fora dele; ausente ou < 32 caracteres derruba o startup), `Issuer/Audience` validados, connection string e CORS por configuração, log sensível do EF só em Development. Guia: `developer-guide/guides/configuracao-local.md`. *A chave antiga continua no histórico do git (repositório público) e não deve ser reutilizada em nenhum ambiente.*
- **Inicialização:** ~~o `Program.cs` não aplica migrations; o banco `projectvictory.db` versionado é o que faz um clone rodar hoje.~~ **Resolvido em `PV-PLATFORM-001`:** em Development, o `Program.cs` aplica as 14 migrations (`Database.MigrateAsync()`) antes do seed; `projectvictory.db` não é mais versionado. `NomeDaMigration` (nome-placeholder, já aplicada) continua intocada — não renomear. **Restante:** fora de Development nada cria o schema (um banco vazio em Production falha no seed) → **`PV-PLATFORM-004`**. ~~O caminho do banco é relativo ao diretório de trabalho~~ → configurável via `ConnectionStrings:ProjectVictory` desde o `PV-PLATFORM-002` (o valor padrão continua relativo).
- **Concorrência/estado:** `HuntRepository` é `List<>` singleton (não thread-safe); `CombatContextRepository` idem; `CombatResultStore` está registrado como *scoped* guardando estado (padrão do FIX-001).
- **Reward não chega ao inventário** (encontrado em `PV-QA-001` parte 1): `ApplyRewardHandler` (`src/ProjectVictory.Application/Reward/ApplyRewardHandler.cs`) aplica só a experiência e ignora `RewardResult.Drops` e `RewardResult.Gold`; nenhum item ou ouro chega ao personagem. O teste `RewardInventoryIntegrationTest.ApplyReward_Should_Add_Item_To_Character_Inventory` documenta o comportamento esperado e está com Skip aprovado pelo dono até a correção. Resolução: **`PV-REWARD-010`** (M2, Reward Pipeline real; ADR-008, 024, 037, 038). Remover o Skip faz parte do aceite desse contrato.
- **`RuntimeId` escolhido pelo cliente** em `POST api/runtime/create` (achado da revisão do `PV-SEC-001`): a proteção contra sobrescrever o runtime de outra conta no `RuntimeExecutionStateStore` depende de a gravação no banco (`RuntimeRepository.AddAsync`, chave primária) falhar antes do `Store`. Resolução futura: gerar o id no servidor ou recusar id existente — candidato ao `PV-STATE-001` (M2).
- **Checagem de posse duplicada** em chamadas internas (`GameplayActionExecution` → `ExecuteHunt`; `EnterGame` → `CreateRuntimeExecution`): uma consulta extra ao banco, sem defeito funcional. Só tratar se virar gargalo.
- **DI de teste mantido à mão:** `tests/ProjectVictory.SmokeTests/TestServiceCollectionExtensions.cs` replica registros do `Program.cs` e já ficou defasado uma vez (faltava `IRuntimeRepository`; corrigido em `PV-QA-001` parte 1). Risco de nova defasagem quando handlers ganharem dependências.

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

## Baseline após `PV-QA-001` parte 1 — 2026-11-05

| Verificação | Resultado |
|---|---|
| `dotnet build ProjectVictory.sln` | **Verde**, 0 erros, 6 avisos — todos **NU1903** (os 4 anteriores + 2 do `SmokeTests`, que agora está no `.sln`). CS0105 eliminado |
| `dotnet test ProjectVictory.sln` | **0 falhas.** `Application.Tests` 1/1; `SmokeTests` 1 passou (`Core_Gameplay_Flow_Should_Complete`) e 1 ignorado (`ApplyReward_Should_Add_Item_To_Character_Inventory` → `PV-REWARD-010`); `Domain.Tests` e `Runtime.Tests` com 0 testes |

Nenhum arquivo de `src/` foi alterado. A falha do SmokeTests registrada na tabela anterior está resolvida.

## Validação de startup do banco — `PV-PLATFORM-001`, 2026-11-05

| Verificação | Resultado |
|---|---|
| `ModelSnapshot` × modelo do `DbContext` (6 DbSets + tabela owned `PersistentItemInstanceState`) | `dotnet ef migrations has-pending-model-changes` (dotnet-ef 10.0.10): **"No changes have been made to the model since the last migration."** Sem divergência; nenhuma migration nova gerada |
| `projectvictory.db` do repositório (lido de cópia) | 14 migrations no `__EFMigrationsHistory` (EF 10.0.0) |
| Clone limpo simulado (pasta temporária vazia, Development) | API sobe; 14 migrations aplicadas; 7 tabelas + `__EFMigrationsHistory` e `__EFMigrationsLock` criadas; seed `Training Sword`; `POST /api/account` 200 e `POST /api/account/login` 200 |
| Banco existente (cópia do `projectvictory.db`) | "No migrations were applied"; cadastro e login 200 |
| `projectvictory.db` do repositório | Hash MD5 inalterado durante os testes; mantido no disco após o `git rm --cached` |
| `dotnet build ProjectVictory.sln` | Verde, 0 erros, 6 avisos NU1903 |
| `dotnet test ProjectVictory.sln` (inclui SmokeTests) | 0 falhas: `Application.Tests` 1/1; `SmokeTests` 1 passou + 1 ignorado (`PV-REWARD-010`) |

## Validação de configuração e segredos — `PV-PLATFORM-002`, 2026-11-05

Execuções feitas com a DLL da API a partir de `bin/Debug/net10.0`, banco em pasta temporária (`ConnectionStrings__ProjectVictory`), sem tocar no `projectvictory.db` local.

| Verificação | Resultado |
|---|---|
| `dotnet build ProjectVictory.sln` | Verde, 0 erros, 6 avisos NU1903 |
| `dotnet test ProjectVictory.sln` (inclui SmokeTests) | 0 falhas: `Application.Tests` 1/1; `SmokeTests` 1 passou + 1 ignorado (`PV-REWARD-010`) |
| `has-pending-model-changes` após mexer no `OnConfiguring` | "No changes have been made to the model since the last migration." |
| Development, chave em user-secrets | Cadastro 200; login 200; token com `iss=ProjectVictory.API` e `aud=ProjectVictory.Client`; `GET /api/character` com token 200 e sem token 401; CORS libera `http://localhost:5173`; log do EF com valores de parâmetros (modo sensível ligado) |
| Production sem `Jwt__Key` | Startup encerra: `InvalidOperationException: Configuração JWT inválida — a API não pode subir. Jwt:Key não configurada...` |
| Production com chave de 12 caracteres | Startup encerra: `...Jwt:Key fraca: tem 12 caracteres; o mínimo é 32...` |
| Production com chave válida (via variável de ambiente), sobre **cópia do banco temporário já migrado** pela execução em Development (fora de Development nada cria o schema — `PV-PLATFORM-004`) | Login 200; CORS **não** libera `http://localhost:5173` (lista vazia); log do EF com parâmetros mascarados (`'?'`), modo sensível desligado |
| Busca pelo valor da chave antiga na árvore atual | 0 ocorrências (permanece só no histórico do git) |

## Validação de autorização — `PV-SEC-001`, 2026-11-06

API em Development (DLL de `bin/Debug/net10.0`, banco temporário via `ConnectionStrings__ProjectVictory`), duas contas: A (dono do personagem) e B.

| Verificação | Resultado |
|---|---|
| `dotnet build ProjectVictory.sln` | Verde, 0 erros, 6 avisos NU1903 |
| `dotnet test ProjectVictory.sln` | 0 falhas: `Application.Tests` 1/1; `SmokeTests` **9 passaram** (fluxo principal + 8 de posse: StartHuntSession, StopHuntSession dono/outra conta, inventário, ExecuteHunt, runtime, encounter, personagem inexistente) + 1 ignorado (`PV-REWARD-010`) |
| Sem token: `hunt/execute`, `hunt/attack`, `hunt-session/start|stop`, `runtime/create|activity|location`, `gameplay/action`, `location/actions`, `inventory/{id}` | **401** em todos |
| Dono A: `game/enter`, `hunt/execute`, `hunt/attack`, `hunt-session/start|stop`, `runtime/activity`, `inventory/{id}` | **200** em todos |
| Conta B sobre recursos de A: `hunt/execute`, `hunt/attack`, `hunt-session/start|stop`, `runtime/create|activity|location`, `gameplay/action`, `inventory/{id}` | **403** em todos; corpo `{"title":"Forbidden","status":403}` (sem detalhes do recurso) |
| Públicos | `account/login` 200 sem token. `playable/status`: 401 sem token e 200 com token (acrescentado depois — ver commit do Playable) |
| `hunt/attack` do dono | Agora devolve o resultado real do combate (antes, sem `await`, serializava o `Task`) |

## Baseline de testes — `PV-QA-001` parte 2, 2026-11-06

| Projeto | Testes | Resultado | O que cobre |
|---|---|---|---|
| `Application.Tests` | 1 | 1/1 | Pipeline de resultado de gameplay (pré-existente) |
| `Domain.Tests` (novo conteúdo) | 22 | 22/22 | Value objects (`Experience`, `Level`, `Health`, `Mana`, `AccountId`), `Inventory`, `Skills`, ciclo de vida de `HuntSession` |
| `Runtime.Tests` (novo conteúdo) | 17 | 17/17 | `RuntimeActivityTransitionRules` (Domain/Runtime). O projeto `ProjectVictory.Runtime` continua vazio; o teste passou a referenciar também `ProjectVictory.Domain`. Quando `ProjectVictory.Runtime` ganhar código, estes testes migram para `Domain.Tests` (ou o tipo migra para Runtime por contrato) |
| `SmokeTests` | 11 | 10 passam, 1 ignorado | Fluxo principal, 8 testes de posse (`PV-SEC-001`) e **`DependencyInjectionTests`**: todo serviço registrado no DI de teste precisa ser construível (`ValidateOnBuild` + `ValidateScopes`) |
| `Api.Tests` (projeto novo) | 41 | 41/41 | API real em memória (`WebApplicationFactory`, Development, SQLite temporário, chave JWT aleatória por execução): **401 sem token** em 15 endpoints (todos menos `Account`); `Account` público; token com issuer, audience ou chave errados → 401; **outra conta → 403** em inventory, hunt-session/start, hunt/execute, runtime/create e gameplay/action, sem expor o recurso; `JwtSettings.Validate` (chave ausente/curta/limite de 32, Issuer/Audience, mensagens Development × fora dele); **convenção por reflexão**: todo controller exceto `Account` tem `[Authorize]` e nenhuma ação tem `[AllowAnonymous]` (endpoint novo nasce protegido) |

`dotnet build ProjectVictory.sln`: verde, 0 erros, 8 avisos — todos NU1903 (`SQLitePCLRaw.lib.e_sqlite3` 2.1.11), 2 por projeto em API, Persistence, SmokeTests e Api.Tests.

**Defasagem encontrada pelo teste de DI:** o `SmokeTests` registrava `AttackEncounterHandler` sem `ICombatResultStore`, `FinalizeHuntAfterCombatHandler` e a cadeia de reward/loot. Corrigido só em `tests/` (mesmos registros do `Program.cs`). Nenhum arquivo de `src/` foi alterado neste contrato.

**Cobertura implícita:** o `WebApplicationFactory` sobe a API em Development, onde o host liga `ValidateOnBuild`/`ValidateScopes` — o container **real** do `Program.cs` é validado a cada execução do `Api.Tests`. Controllers não são serviços registrados: suas dependências só são resolvidas quando uma requisição chega.

**Não coberto:** 403 por HTTP nos caminhos de runtime/hunt session/encounter (`runtime/activity|location`, `hunt-session/stop`, `hunt/attack`) — cobertos no nível de handler (`OwnershipAuthorizationTests`); token expirado e token sem a claim de conta. Falha de startup em Production sem chave por teste de integração (o `Program.cs` lê a configuração antes do `Build()`, e variáveis de ambiente são globais ao processo de teste); coberto pelo teste unitário de `JwtSettings.Validate` e pela validação manual do `PV-PLATFORM-002`.

## Dependências vulneráveis — `PV-PLATFORM-003`, 2026-11-06

Contrato: ordem do PO de 2026-11-06 (modo noite, item 4) + linha do `PV-PLATFORM-003` em `roadmap-to-playable.md`.

**Decisão:** atualizar o EF Core de **10.0.0 para 10.0.14** (patch da mesma linha 10.0) em vez de fixar a dependência transitiva. O 10.0.14 depende de `SQLitePCLRaw` **2.1.13** (fora da faixa vulnerável), a correção vem pelo canal de manutenção oficial e não fica uma fixação manual para manter. Pacotes alinhados: `Microsoft.EntityFrameworkCore`, `.Sqlite`, `.Design` (API), `.InMemory` (SmokeTests), `tools/PvDatabaseCheck` e `ItemCheck/` (`Microsoft.Data.Sqlite`; os dois estavam em 10.0.10, que ainda puxava o 2.1.11 — ferramentas fora do `.sln`, a segunda do contrato ativo `PV-TOOLS-002`; o `ItemCheck` foi incluído pela revisão). Nenhuma migration alterada.

| Vulnerabilidade | Pacote (transitivo) | Origem | Situação |
|---|---|---|---|
| **CVE-2025-6965** / GHSA-2m69-gcr7-jv3q (alta, NU1903): SQLite nativo empacotado vulnerável; afeta `SQLitePCLRaw.lib.e_sqlite3` `<= 2.1.11` | `SQLitePCLRaw.lib.e_sqlite3` 2.1.11 | `Microsoft.EntityFrameworkCore.Sqlite` 10.0.0 | ✅ Resolvida: 2.1.13 com EF 10.0.14 |
| **CVE-2026-84008** / GHSA-846f-f764-76mx (alta, NU1903) e **CVE-2026-70162** / GHSA-2fmv-rv2c-59x7 (moderada, NU1902): negação de serviço no .NET; afeta `System.Security.Cryptography.Pkcs` 9.0.0–9.0.20 e 10.0.0–10.0.12 | `System.Security.Cryptography.Pkcs` 9.0.0 | `Microsoft.EntityFrameworkCore.Design` 10.0.0 → `Microsoft.Build.Tasks.Core` 17.14.28 (só no projeto da API) | ✅ Resolvida: com `.Design` 10.0.14 a API não depende mais do `Pkcs` (`dotnet nuget why`) |

**O "7º aviso":** além dos NU1903 do SQLite (um por projeto que usa SQLite, cada um impresso duas vezes pelo restore/build), o projeto da API mostrava avisos do `System.Security.Cryptography.Pkcs` 9.0.0 (NU1903 e NU1902). Esses alertas da base do GitHub Advisory podem aparecer depois de um baseline já registrado, porque a auditoria do NuGet consulta a base no momento do restore; por isso o número de avisos mudou entre execuções. O "7" visto pelo dono é uma contagem intermediária (depende de build incremental — que não reimprime avisos de restore de projetos já restaurados — e do momento da consulta à base); a contagem auditável é a do build `--no-incremental`. Build completo (`--no-incremental`) da `main` (d94c907) em 2026-11-06: **10 linhas de aviso** (SQLite em API, Persistence e SmokeTests; `Pkcs` alta e moderada na API).

| Verificação (branch `chief/m1-platform-003`) | Resultado |
|---|---|
| `dotnet build ProjectVictory.sln --no-incremental` | **Verde, 0 erros, 0 avisos** |
| `dotnet list package --vulnerable --include-transitive` em API, Persistence, SmokeTests, Api.Tests (solução) e nas ferramentas fora do `.sln` `tools/PvDatabaseCheck` e `ItemCheck` | **Nenhum pacote vulnerável** |
| `dotnet test ProjectVictory.sln` | 0 falhas: 91 passam, 1 ignorado (`PV-REWARD-010`) |
| `has-pending-model-changes` com EF 10.0.14 | "No changes have been made to the model since the last migration." |
| Clone limpo (Development, banco temporário) | 14 migrations aplicadas; histórico com `ProductVersion` 10.0.14; cadastro e login 200 |

Pendência (revisão): versões desalinhadas entre famílias de pacotes (`JwtBearer` e `Mvc.Testing` em 10.0.0; xunit/Test.Sdk diferentes entre projetos de teste) — sem vulnerabilidade hoje; candidato a contrato de gestão central de versões (`Directory.Packages.props`) e `.config/dotnet-tools.json` fixando o `dotnet-ef`.

Observação: o `dotnet-ef` global da máquina está em 10.0.10; funciona com o runtime 10.0.14, mas convém atualizá-lo (`dotnet tool update -g dotnet-ef`) — fora do repositório, não feito.

## Baseline do cliente — `PV-QA-002`, 2026-11-06

Executado em `src/ProjectVictory.Client` (Node 24.18.0, npm 11.16.0). **Nenhum código do cliente foi alterado**; `package-lock.json` inalterado (mesmo hash antes e depois do `npm install`).

| Verificação | Resultado |
|---|---|
| `npm install` | OK — 67 pacotes auditados, "up to date". Aviso do npm: `esbuild@0.28.1` tem script de instalação ainda não aprovado (`allowScripts`) |
| `npm audit` | **6 vulnerabilidades (2 moderadas, 4 altas)**, todas em dependências de **build/dev**, todas com correção via `npm audit fix`: `vite` 7.0.0–7.3.6 (alta; `launch-editor` sem escape de `%` no Windows — GHSA-698v-4r3x-3vcv), `browserslist` ≤ 4.28.6 (alta; GHSA-c83g-rgw3-j3cx, GHSA-73wf-gq98-2v4g), `nanoid` < 3.3.18 (alta; GHSA-2v37-7h3g-55p8), `source-map-js` 1.0.0–1.2.1 (alta; GHSA-68fv-2mgg-jv7q), `postcss` ≤ 8.5.22 (moderada; GHSA-fxqj-rqcc-2cmp), `baseline-browser-mapping` 2.0.0–2.10.x (moderada; GHSA-w5vr-8v7q-w6rv) |
| `npm run build` (`vite build`) | **Verde** — 53 módulos, `dist/index.html` 0,33 kB, `dist/assets/index-*.js` 206,18 kB (64,15 kB gzip) |
| Lint | **Não existe** script de lint nem configuração de ESLint no `package.json` |
| `npx tsc --noEmit` (diagnóstico extra; o `build` **não** checa tipos) | **443 erros** |

Erros de TypeScript por causa:

| Código | Qtde | Causa |
|---|---|---|
| TS7026 (339), TS7016 (58), TS2503 (29) | 426 | **Faltam `@types/react` e `@types/react-dom`** nas `devDependencies` (React 19 não traz tipos próprios); com `strict: true`, todo JSX vira `any` implícito. Um único ajuste de dependências eliminaria quase todos |
| TS7006 | 13 | Parâmetros com `any` implícito (ex.: `CharacterCreationPage.tsx:186` `event`, `WorldScenePage.tsx:374` `drop`) |
| TS2305 | 2 | `pages/WorldScenePage.tsx:27-28` importa `attackEncounter` e `AttackEncounterResult` de `services/huntService.ts`, que **não existem** (o serviço só exporta `startHunt`). O endpoint existe no backend (`POST api/hunt/attack`, com verificação de dono): a correção certa é **recriar** `attackEncounter` no serviço, não apagar o uso |
| TS2307 | 1 | `components/Character/PlayerStatePanel.tsx:1` importa `../models/game` — o arquivo **existe** em `src/models/game.ts`; o erro é o **caminho relativo** (`../` em vez de `../../`). Não criar um `models/game` duplicado |
| TS2353 | 1 | `services/accountService.ts:100` monta `AccountLoginResult` com a propriedade `token`, que o tipo não declara |

**Código inalcançável** (não importado por nada a partir de `main.tsx`; por isso o `vite build` passa apesar dos imports quebrados): `pages/GamePage.tsx` (e o `WorldScenePage` que só ele usa), `components/Character/PlayerStatePanel.tsx`, `pages/CharacterCreationPage.tsx`, `pages/EntryExperiencePage.tsx`, `services/runtimeService.ts` (`createRuntime`) e o ramo `page === "reward"` do `App.tsx` (nada chama `setPage("reward")`). A criação de personagem funciona por `CharacterSelectionPage.tsx`. A navegação real usa Login, Register, CharacterSelection, PlayerInformation e Inventory.

**Autorização (conferido na revisão):** todas as chamadas a endpoints protegidos passam por `api/client.ts` (`apiRequest`), que envia o token; só cadastro e login usam `fetch` direto e são públicos. O cliente **não** quebra com o `PV-SEC-001`. Fora do escopo, para o `PV-UI-0xx`: não há tratamento de 401 (token expirado vira erro genérico, sem voltar ao login) e `accountService.ts` repete `API_URL` fixo.

Encaminhamento (sem corrigir agora, conforme o contrato): **`PV-PLATFORM-005`** (M1, novo — decisão do Arquiteto pela revisão: o equivalente do backend teve contrato no M1) para `npm audit fix` nas dependências de build, `@types/react*` e um script `typecheck` sem bloquear o build; **`PV-UI-0xx`** (M8) para o código: imports quebrados, código inalcançável, tipagem dos `any`, tratamento de 401.

## Ainda pendentes

- ~~Que o `ModelSnapshot` e as 14 migrations refletem o modelo atual do `DbContext`.~~ Verificado em `PV-PLATFORM-001` (tabela acima).
- Execução do fluxo no navegador (`COMO USAR O SMOKE TEST.txt`).

---

# Próximo Objetivo

Executar **M0** (decisões ~~R-001~~ (resolvida em 2026-11-06), R-005, R-006, R-008; baseline executável na máquina do dono) e em seguida **M1**. Ver `roadmap-to-playable.md`.

---

# Regras Permanentes (mantidas da v1.2)

- A documentação oficial é a única fonte de verdade, **subordinada à hierarquia código > contratos/ADRs > documentação**.
- Todo novo módulo consulta o Roadmap e este documento.
- Nenhum módulo LOCKED é alterado sem revisão arquitetural.
- Toda decisão permanente é documentada.
- Toda Sprint atualiza este documento.
