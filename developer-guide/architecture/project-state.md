# Project-Victory

# Project State

Versão do Documento: 2.0
Status: Oficial
Data da verificação: 2026-11-05 · **Atualizado em 2026-11-06** (M1 parcial, M2 completo, Fila 2 — `PV-GOV-008`)
Substitui: v1.2 (2026-07-14), preservada no histórico git.
Método: estado **verificado contra o código** (hierarquia da fonte da verdade: código > contratos/ADRs > documentação). Build e testes **não foram executados** (ambiente sem `dotnet`) nesta verificação; o baseline foi executado depois na máquina do dono (2026-11-05) e está registrado em Pendências de Validação.

---

# Resumo

O código está **à frente** da documentação de estado anterior: o Vertical Slice (conta → login → personagem → entrada no mundo → hunt → combate → recompensa → XP) existe ponta a ponta, porém **sobre implementações provisórias** (stubs) em Combate, Recompensa e Progressão, e com divergências em relação a ADRs ACCEPTED. O projeto **não está pronto para jogo**; está pronto para a fase de **conformidade e substituição de stubs**.

*Atualização 2026-11-06 (`PV-GOV-008`):* M1 quase completo (faltam os itens **do M1** `PV-PLATFORM-004`, `PV-PLATFORM-005` e `PV-CLEAN-001`, em andamento na Fila 2; os demais itens da Fila 2 — `PV-GOV-007/008`, `PV-QA-003`, `PV-ARCH-001`, `PV-CI-001` — são governança e qualidade, não itens do M1) e **M2 completo**: Experience Table pela curva do PO, Level derivado, Experience `long`, Reward Pipeline real (XP × XP rate do servidor, ouro, drops por chance), stores em memória seguros. O **combate** ainda é stub (dano fixo 5, sem vida do jogador): é o M3.

---

# Sprint Atual

Nome: **Fila 2** (ordem do PO de 2026-11-06): fechamento do M1 e governança — `PV-GOV-007/008`, `PV-QA-003`, `PV-ARCH-001`, `PV-PLATFORM-004/005`, `PV-CI-001`, `PV-CLEAN-001`. Depois: **M3 — Combate real**.
*Histórico:* M0 — Baseline e Governança (2026-11-05); M1 (2026-11-05/06); M2 (2026-11-06, `docs/architecture/RELATORIO-M2.md`).

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
| Integração contínua | **Ativa** desde o `PV-CI-001`: `.github/workflows/ci.yml` roda `dotnet build`/`dotnet test ProjectVictory.sln` (.NET 10, Release, Ubuntu) e o build do cliente (Node 22) em push e pull request; typecheck informativo (aviso visível). Primeiras execuções: **success** (42225918358, 42226163716) | `.github/workflows/ci.yml` |
| Projeto `ProjectVictory.Composition` | **Ativo** desde o `PV-ARCH-001`: raiz de composição de DI compartilhada pela API e pelos testes (`AddProjectVictoryCore`); referencia Domain, Application, Infrastructure e Persistence; não é camada de regra | `src/ProjectVictory.Composition` |
| Projetos `ProjectVictory.Runtime` e `ProjectVictory.Shared` | **Vazios** (somente `.csproj`) | 0 arquivos `.cs` |
| Client | Telas de login, cadastro, personagem, entrada, jogo, inventário, recompensa | `src/ProjectVictory.Client/src/pages` |

Testes automatizados (após `PV-SUPPLY-010`, 2026-11-06): **242** no `.sln` — `Application.Tests` 12; `Domain.Tests` 87; `Runtime.Tests` 17; `SmokeTests` 48; `Api.Tests` 78. **242 passam, 0 ignorados, 0 falhas.** *Após `PV-HUNT-010`:* 227. *Após `PV-COMBAT-012`:* 222. *Após `PV-COMBAT-011`:* 202. *Após `PV-COMBAT-010`:* 196. *Após `PV-CHAR-010`:* 191. *Após `PV-SEC-002`:* 179. *Após `PV-PLATFORM-004`…`PV-CLEAN-001`:* 173. *Após `PV-ARCH-001`:* 167. *Após `PV-QA-003`:* 166. *Após `PV-STATE-001`:* 161 (o Skip do `PV-QA-001` foi removido no `PV-REWARD-010`). *Após `PV-REWARD-010`:* 156. *Após `PV-PROG-010`:* 135 (134 passam, 1 ignorado). *Contagem anterior* (após `PV-QA-001` parte 2): **92** no `.sln` — `Application.Tests` 1; `Domain.Tests` 22; `Runtime.Tests` 17; `SmokeTests` 11 (1 com Skip aprovado pelo PO até o `PV-REWARD-010`); `Api.Tests` 41 (HTTP com `WebApplicationFactory`, `JwtSettings` e convenção de `[Authorize]` por reflexão). **91 passam, 1 ignorado, 0 falhas.** Detalhe em Pendências de Validação.

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
| R-002 | `Level` persistido × Level derivado | ADR-027, ADR-028 × `PersistentCharacterState.Level` | Código deve se conformar ao ADR (hierarquia: ADR > código não aprovado por contrato). Contrato `PV-PROG-010` (M2): Level derivado; a coluna `Level` **fica** como cache gravado com o valor derivado e nunca lido como verdade (`PV-GOV-006` D2; `level.md`). ✅ **Resolvida em `PV-PROG-010`** (2026-11-06): `Character.Level` derivado de Experience (`long`) + Ascension (primeira) + Experience Table; migration `CharacterExperienceLong` (sem mudança de schema no SQLite) | Nenhum — ADR prevalece |
| R-003 | Tabela de nível fixa (máx. 5) × Experience Table por Ascension | ADR-025 × `ProgressionProcessor` | Idem R-002. Curva decidida pelo PO em 2026-11-06 (`DEC-20261106-10/11`); teto de 500 por Ascension (`ADR-026`). ✅ **Resolvida em `PV-PROG-011`** (2026-11-06): Experience Table gerada pela curva, Levels 1–500, usada por `ProgressionProcessor`, `LevelCalculator` e `GrowthProcessor` | Nenhum |
| R-004 | Event Bus tipado exigido × inexistente | ADR-049/050 × Known Gaps | Implementar em M7 (já planejado) | Nenhum |
| R-005 | Duas famílias de roadmap (`developer-guide/architecture/roadmap.md` Fases 0–10 × `docs/roadmap/phase-3..5`, ambos "Aguardando validação") | docs | `roadmap-to-playable.md` torna-se o roadmap de **execução**; `roadmap.md` permanece a ordem de **módulos**; `phase-3..5` ficam subordinados. Registrar em decision-log | Nenhum |
| R-006 | Estado oficial declarado em `project-state.md`, mas 4 documentos ainda apontam `CURRENT_CONTEXT.md` (histórico) | `PROJECT_CONTEXT.md`, `AI_WORKFLOW.md`, `architecture-checklist.md`, `README_DEVELOPMENT.md` | Ordem de leitura fixada no `CLAUDE.md`. Edição dos documentos LOCKED aguarda revisão formal (M0) ✅ **Resolvida em `PV-GOV-007` (2026-11-06, `DEC-20261106-13`):** as 5 linhas foram trocadas por `project-state.md`, com o texto antigo tachado e nota de emenda nos LOCKED. | Nenhum (resolvida; antes: Nenhum  · Levantamento e proposta: `PV-GOV-005-RELATORIO-2026-11-06.md`.) |
| R-007 | Pastas `Application/Architecture`, `Playtest`, `Production` (42 arquivos) modelam processos de governança como handlers que ecoam booleanos; **sem referência** em nenhum outro ponto | Código × Clean Architecture (Application = casos de uso do jogo) | Remoção **somente após `dotnet build` verde**; contrato de limpeza em M1 ✅ **Resolvida em `PV-CLEAN-001` (2026-11-06):** 42 arquivos removidos no commit isolado `38d1be7` (autorizado pelo PO), sem referências; build e testes inalterados (173). | Nenhum (resolvida; antes: Build) |
| R-009 | Duas séries de ADR com a mesma numeração (`decision-log.md` ADR-001…018 × `developer-guide/adr/` ADR-001…057) | decision-log × pasta adr | Novas decisões usam prefixo `DEC-`; unificação aguarda decisão | Dono/Arquiteto |
| R-010 | Higiene do repositório: `node_modules`/`dist` (2.440 arquivos; corrigido em PV-GOV-002), 3 binários/listagens e `projectvictory.db` versionados; `.gitignore` incompleto | Clone do GitHub | `node_modules/` e `dist/` ignorados e removidos do controle; binários removidos (ver `CLEANUP-MANIFEST`). `projectvictory.db` sai **depois** do `PV-PLATFORM-001` (migração no startup). ✅ **Concluído em `PV-PLATFORM-001`:** `projectvictory.db` fora do controle e no `.gitignore` (aviso de backup no `CLEANUP-MANIFEST`) | Nenhum |
| R-008 | Numeração de ADR: faltam 026, 044, 048; ADR-055 existe vazio; `docs/history/Sprint-054..056.md` vazios | `developer-guide/adr/` | Preencher ou declarar formalmente como reservados/descartados ✅ **ADR-026 preenchido** (Ascension Model, ACCEPTED, `PV-GOV-006`); faltam 044, 048 e 055 ✅ **Resolvida em `PV-GOV-007`:** ADR-044 e ADR-048 = NOT USED (reservados); ADR-055 preenchido como PROPOSED. Os `docs/history/Sprint-054..056.md` vazios continuam (não são ADR; fora do escopo). | Nenhum (resolvida; antes: Dono  · Levantamento e proposta: `PV-GOV-005-RELATORIO-2026-11-06.md`.) |
| R-011 | Ordem do PO: "verificação de dono do personagem pelo token, nunca pelo corpo ou rota" × `PV-SEC-001` D1/D2, que mantêm `characterId` no corpo/rota como **seletor de recurso conferido contra a conta do token** | Ordem do PO (2026-11-06) × `docs/contracts/active/PV-SEC-001.md` | Leitura do Arquiteto: o dono vem sempre do token; o id só escolhe qual personagem da conta. A leitura literal exigiria um conceito de "personagem selecionado" no servidor (contrato novo) e mudaria o cliente | **PO confirma** a leitura ou pede o contrato novo |
| R-012 | `ExecuteGameplayLoopCommand` (`src/ProjectVictory.Application/Game/ExecuteGameplayLoopCommand.cs`) recebe `AccountId` como parâmetro; nenhum controller o expõe hoje | Server Authority (`CLAUDE.md`) | Se for exposto, a conta deve vir de `ICurrentPlayerContext`, nunca do corpo (risco latente, revisão do `PV-SEC-001`) | Nenhum enquanto não exposto |
| R-013 | `ADR-002` ("suporte ao modo Hunt Offline") e `knowledge-base/loot.md` (LOCKED, "regras de Hunt Offline previamente definidas") × decisão do PO de não haver Hunt Offline (`DEC-20261106-05`, ADR-057) | ADR-002, `loot.md` × ADR-057 | ADR-057 prevalece (decisão mais recente, com aceite do PO). Notas nos documentos só por revisão documentada (`loot.md` é LOCKED) ✅ **Resolvida em `PV-GOV-007`:** nota de emenda no ADR-002 e no `knowledge-base/loot.md` (LOCKED), apontando para o ADR-057, sem apagar texto. | Nenhum (resolvida; antes: Revisão documental futura) |
| R-014 | Composição de DI duplicada: `Program.cs` × `tests/ProjectVictory.SmokeTests/TestServiceCollectionExtensions.cs`. O `DependencyInjectionTests` valida só a cópia de teste; ~16 serviços do `Program.cs` não estão no DI de teste | Revisão do `PV-QA-001` parte 2 | Contrato futuro: extrair um método de composição compartilhado em `src` (ex.: `AddProjectVictoryApplication()`) usado pela API e pelos testes. Até lá, a validação do container real vem do `Api.Tests` (Development + `ValidateOnBuild`) ✅ **Resolvida em `PV-ARCH-001` (2026-11-06):** método único `AddProjectVictoryCore` no projeto novo `src/ProjectVictory.Composition`, usado pelo `Program.cs` e pelo `TestServiceCollectionExtensions`; o `DependencyInjectionTests` passou a validar a composição real. | Nenhum (resolvida; antes: Contrato próprio (M1/M2)) |
| R-015 | ADR-036 (toda movimentação de Resources passa por um **Inventory Service**) × código: não existe Inventory Service; `ApplyLootToCharacterInventoryHandler` e `ApplyRewardHandler` chamam `character.Inventory.AddItem/AddGold` direto (padrão anterior, estendido ao ouro no `PV-REWARD-010`) | ADR-036 × `src/ProjectVictory.Application/Loot`, `Reward` | Aceito no M2 (revisão do `PV-REWARD-010`, ressalva 1). Criar o Inventory Service por contrato próprio antes do comércio (Fase 7/8) ou do consumo de suprimentos (`PV-SUPPLY-010`, M3) | Contrato futuro |
| R-016 | Dois caminhos de recompensa: ataque real (`RewardResolutionHandler` + `LootResolutionHandler`) × `IRewardProcessor` + `ApplyRewardHandler` (sem DI de produção). A regra é a mesma (Domain `MonsterLootRoll`, `ExperienceRate`), mas o ADR-024 fala em **um** Reward Pipeline | ADR-024 × código | Escolher um caminho e aposentar o outro por contrato (candidato: Combat Orchestrator, `PV-COMBAT-010`, M3, ou Event System, M7) | Contrato futuro |
| R-017 | Recompensa gravada em dois `UpdateAsync` separados (loot e ouro em `ApplyLootToCharacterInventoryHandler`, XP em `ApplyResolvedRewardToCharacterHandler`): uma falha no meio entrega metade da recompensa | Revisão do `PV-REWARD-010` (ressalva 3) × Handbook §19 (salvar na recompensa) | Gravação única da recompensa na Save Strategy (`PV-PERSIST-011`, M5). Fora do `PV-STATE-001`, que trata posse e concorrência do estado **em memória** | `PV-PERSIST-011` |
| R-018 | Token JWT **válido sem a claim da conta** (`NameIdentifier`) passa pelo `[Authorize]`; nos endpoints que usam `ICurrentPlayerContext`, `HttpCurrentPlayerContext` lança `InvalidOperationException("Player identity unavailable.")` → **500**, não 401. Nunca devolve dados; endpoints que não usam a identidade (ex.: `playable/status`) respondem 200 | `PV-QA-003` (`OwnershipAndTokenHttpTests.TokenWithoutAccountClaim_NeverReachesPlayerData`) × Handbook §23 | Contrato futuro em `src/`: mapear identidade ausente para 401 (exceção própria no `ApiExceptionHandler`) e/ou exigir a claim numa policy de autorização. Ao corrigir, trocar a asserção do teste para 401. Agravante (revisão do `PV-QA-003`): cada requisição assim gera um log de nível Error ("Unhandled exception"), ruído que pode ser usado para inflar o log. O teste de token expirado depende da tolerância padrão de 5 min do JwtBearer (rever se o `ClockSkew` mudar). ✅ **Resolvida em `PV-SEC-002` (2026-11-06):** `OnTokenValidated` recusa token sem claim de conta válida (401 em todo `[Authorize]`); `HttpCurrentPlayerContext` lança `PlayerIdentityUnavailableException` → 401 com log de aviso. | Nenhum (resolvida; antes: Contrato futuro (API)) |
| R-019 | npm 11 avisa que o install script do `esbuild` não está liberado (`npm approve-scripts`). O build do cliente funciona (binário do pacote opcional da plataforma), mas a liberação, ou não, de scripts de instalação é decisão de segurança da cadeia de dependências | `PV-PLATFORM-005` (revisão, ressalva 4) | Decidir se libera o script do `esbuild` (`npm approve-scripts esbuild`) ou mantém bloqueado e documentado | Decisão do Arquiteto/PO (não bloqueia) |
| R-020 | `src/ProjectVictory.Domain/Playtest/` (3 arquivos: `PlaytestFeedback`, `PlaytestIssueClassification`, `TechnicalIssueAnalysis`) ficou **sem nenhuma referência** depois do `PV-CLEAN-001`: só os handlers removidos de `Application/Playtest` a usavam | Revisão do `PV-CLEAN-001` | **Não removida** (fora da autorização do PO). Decidir em contrato próprio: remover (com autorização) ou manter | Decisão do PO (remoção) |
| R-021 | Vida/Mana por classe — **documento × documento**: `knowledge-base/progression.md` (HP inicial 100 para todas; Knight +0.75/Level, Mana Knight 50 +0.25…) × `docs/character e conceitos/PV_CHARACTER_FOUNDATION_SPECIFICATION.md`, `PV_CHARACTER_CLASS_PROGRESSION_MASTER.md`, `PV_CHARACTER_IDENTITY_AND_PROGRESSION_MASTER_v2.md` e o código `CharacterClassDefinition` (Knight 185 HP +15/Level, 55 Mana +5…; só os valores iniciais estavam em uso) | `progression.md` × código | `PV-CHAR-010` usa os valores do código como **PROVISÓRIOS** (Level 1 inalterado), atrás da `AttributeCurveTable`; a curva aceita ganho fracionário. Trocar = mudar dados | **PO decide os valores** (e o significado de "+0.75") |
| R-022 | Ascension × Vida/Mana máximas: `progression.md`/ADR-026 dizem que na Ascension "o Level retorna para 1" e "os atributos permanentes são mantidos"; a Attribute Curve (`PV-CHAR-010`) deriva o máximo só do Level, que voltaria ao inicial | ADR-026 × ADR-029 × `AttributeCurve` | Sem efeito hoje (Ascension fixa em 1). Definir no contrato de Ascension (ex.: curva por Ascension ou base acumulada) | Contrato futuro (Ascension) + PO |
| R-023 | Bug antigo: `AttackEncounterHandler` preenchia `AttackEncounterResult.PlayerMaximumHealth` com a **Mana** atual (argumentos posicionais) | Revisão do `PV-CHAR-010` | Corrigido na integração do combate (`PV-COMBAT-011`/`012`) | Nenhum (corrigido no `PV-COMBAT-012`) |
| R-024 | Loot e recompensa usam o `SystemRandomSource` global (sem semente), não o gerador do combate: o **combate** é reproduzível, os **drops/ouro** não | Revisão do `PV-COMBAT-011` | Decidir se "combate reproduzível" inclui loot; se sim, passar o gerador da sessão ao Reward/Loot por contrato | Decisão do Arquiteto (PO se afetar jogo) |
| R-025 | `developer-guide/domain/combat-orchestrator.md` (DRAFT) × implementação do `PV-COMBAT-010`: estados Created/Preparing/Running/Finishing/Finished × Ongoing/Victory/Defeat; `StartedAt/FinishedAt`, Combat Context imutável e Engine Events inexistentes; o orquestrador conhece `CombatSide.Player`. O ADR-006 fala em "Combat Engine" (= orquestrador + resolver) | Revisão do `PV-COMBAT-010` | Atualizar o DRAFT ou a implementação no Event System (M7) | Revisão documental/contrato futuro |
| R-026 | Dois caminhos de combate: o real (`POST api/hunt/attack` → orquestrador) × o antigo `ICombatResolver.Resolve` (fluxo `gameplay/action`, "atacante vence"), mantido pela Regra Aditiva | `PV-COMBAT-012` | Aposentar o fluxo antigo por contrato quando o cliente usar só o combate real | Contrato futuro |
| R-027 | Hunt Session persistida (`api/hunt-session/start|stop`) × Hunt em memória do combate: o `stop` manual não passa pelo pipeline único (`EndHuntHandler`) e o pipeline não fecha a Hunt Session | ADR-012 × `PV-HUNT-010` | Unificar no `PV-HUNT-011` (Hunt Loop, M6), com o motivo `Manual`, e no `PV-HUNT-012` (`Disconnect`) | `PV-HUNT-011`/`012` |
| R-028 | Regras de ação no `CombatResolver` da **Infrastructure** (custo e checagem de mana da ClassAbility, efeito das poções), em vez do Domain (Domain First) | Revisão do `PV-COMBAT-012` | Mover para o Domain (ex.: `CombatActionRules`) quando as regras forem definitivas | Contrato futuro |
| R-029 | Concorrência: a `CombatSession` vive no `EncounterStateStore` (singleton) e é alterada sem trava; dois ataques simultâneos no mesmo encounter podem disputar o estado | Revisão do `PV-COMBAT-012` (e `PV-STATE-001` D3) | Serializar comandos por encounter no Hunt Loop/Runtime Coordinator (M6) | `PV-HUNT-011`/`PV-RUNTIME-010` |
| R-030 | O `EndHuntHandler` grava `RuntimeActivity.Idle` direto no store, sem passar por `RuntimeActivityTransitionRules` (Domain), que não tem transição Hunting/Combat/Reward → Idle | Revisão do `PV-HUNT-010` | Acrescentar no Domain a transição "fim de Hunt → Idle" por contrato do Runtime (`PV-RUNTIME-010`) | `PV-RUNTIME-010` |
| R-031 | Handlers mortos que mudam o estado da Hunt fora do pipeline único: `CancelHuntHandler`, `ExecuteExistingHuntHandler`, `ExecuteNextHuntHandler`, `ProcessHuntExecutionHandler`, `ResolveHuntResultHandler` (sem DI nem referência). Religá-los violaria o ADR-012 | Revisão do `PV-HUNT-010` | Remover (com autorização do PO) ou adaptar ao `EndHuntHandler` | PO (remoção) |

---

# Dívida técnica e risco verificados

Detalhe e evidências em `docs/architecture/PV-ANALYSIS-2026-11-05.md`. Itens críticos:
- **Segurança (Handbook §23–24):** ~~só `Character`, `Game/enter` e `Item` exigem `[Authorize]`; `Hunt`, `HuntSession`, `Inventory`, `Runtime`, `Gameplay` e `Location` não exigem.~~ **Resolvido em `PV-SEC-001`:** todos esses controllers exigem token; `PlayerOwnershipGuard` (Application) garante que personagem, runtime, hunt session e encounter pertencem à conta do token (outra conta → 403). Público por desenho: só cadastro/login (`Account`); `Playable/status` também passou a exigir token. Contrato: `docs/contracts/active/PV-SEC-001.md`. ~~Chave JWT padrão fixa no código; SQLite com caminho fixo; log com dados sensíveis habilitado.~~ **Resolvido em `PV-PLATFORM-002`:** chave JWT sem valor padrão (user-secrets em Development, `Jwt__Key` fora dele; ausente ou < 32 caracteres derruba o startup), `Issuer/Audience` validados, connection string e CORS por configuração, log sensível do EF só em Development. Guia: `developer-guide/guides/configuracao-local.md`. *A chave antiga continua no histórico do git (repositório público) e não deve ser reutilizada em nenhum ambiente.*
- **Inicialização:** ~~o `Program.cs` não aplica migrations; o banco `projectvictory.db` versionado é o que faz um clone rodar hoje.~~ **Resolvido em `PV-PLATFORM-001`:** em Development, o `Program.cs` aplica as 14 migrations (`Database.MigrateAsync()`) antes do seed; `projectvictory.db` não é mais versionado. `NomeDaMigration` (nome-placeholder, já aplicada) continua intocada — não renomear. ~~**Restante:** fora de Development nada cria o schema (um banco vazio em Production falha no seed) → **`PV-PLATFORM-004`**.~~ ✅ **Resolvido em `PV-PLATFORM-004`:** fora de Development, `Database:ApplyMigrationsOnStartup=true` aplica as migrations no startup (padrão desligado; com migrations pendentes e a chave desligada, aviso no log); guia `developer-guide/guides/deploy.md`. ~~O caminho do banco é relativo ao diretório de trabalho~~ → configurável via `ConnectionStrings:ProjectVictory` desde o `PV-PLATFORM-002` (o valor padrão continua relativo).
- ✅ **Resolvido em `PV-CHAR-010`:** Vida e Mana máximas derivadas de Class + Level pela Attribute Curve (ADR-029), com valores PROVISÓRIOS (R-021). ~~**Vida/Mana máximas não crescem com o Level** (efeito do `PV-PROG-010`): o crescimento por Level ficava em `Character.ApplyLevel`, que nenhum endpoint chamava. Com o Level derivado, `ApplyLevel` só confirma o Level atual. O crescimento passa a ser **atributo derivado** (ADR-029; `knowledge-base/progression.md`, HP/Mana por Level) — contrato futuro (candidato: junto do combate real no M3, que usa a vida do jogador).~~
- **Recompensa por monstro sem definição** (achado do `PV-REWARD-010`; revisão, ressalvas 4 e 6 — o **Rat Tail** existe só no `MonsterLootDefinitionProvider`, não na tabela `Items`, e `LootDropDefinition.Chance` não valida 0–1; ambos para o `PV-CONTENT-011`): só o **Rat** tem definição de loot/XP (`MonsterLootDefinitionProvider`); o **Cave Rat** do `EncounterGenerator` não dá XP, ouro nem drop. Conteúdo da Spider Cave é o `PV-CONTENT-011` (M3). O ouro do Inventory é persistido mas **não aparece** em nenhuma resposta HTTP ainda (exibição: `PV-HUD-ANALYZER-010`/UI, M8).
- ✅ **Resolvido em `PV-STATE-001` (2026-11-06):** `HuntRepository` e `CombatContextRepository` passaram a `ConcurrentDictionary` (recusam id repetido); `CombatResultStore` continua *scoped* por decisão (estado temporário de uma requisição). Classificação de todos os stores em `docs/contracts/active/PV-STATE-001.md`. ~~**Concorrência/estado:** `HuntRepository` é `List<>` singleton (não thread-safe); `CombatContextRepository` idem; `CombatResultStore` está registrado como *scoped* guardando estado (padrão do FIX-001).~~
- ✅ **Resolvido em `PV-REWARD-010` (2026-11-06):** o `ApplyRewardHandler` aplica drops, ouro e XP × XP rate; o ataque real entrega XP da definição do monstro; Skip removido. ~~**Reward não chega ao inventário**~~ (encontrado em `PV-QA-001` parte 1): `ApplyRewardHandler` (`src/ProjectVictory.Application/Reward/ApplyRewardHandler.cs`) aplica só a experiência e ignora `RewardResult.Drops` e `RewardResult.Gold`; nenhum item ou ouro chega ao personagem. O teste `RewardInventoryIntegrationTest.ApplyReward_Should_Add_Item_To_Character_Inventory` documenta o comportamento esperado e está com Skip aprovado pelo dono até a correção. Resolução: **`PV-REWARD-010`** (M2, Reward Pipeline real; ADR-008, 024, 037, 038). Remover o Skip faz parte do aceite desse contrato.
- ✅ **Resolvido em `PV-STATE-001`:** `CreateRuntimeExecutionHandler` recusa `RuntimeId` já em uso antes de gravar (outro personagem → 403). **Limite:** a verificação não é atômica e só enxerga a memória — `create` simultâneo ou depois de reiniciar a API cai na chave primária do banco (500, sem sobrescrita). Solução definitiva: id gerado no servidor ou `PV-RUNTIME-011`. ~~**`RuntimeId` escolhido pelo cliente** em `POST api/runtime/create` (achado da revisão do `PV-SEC-001`): a proteção contra sobrescrever o runtime de outra conta no `RuntimeExecutionStateStore` depende de a gravação no banco (`RuntimeRepository.AddAsync`, chave primária) falhar antes do `Store`. Resolução futura: gerar o id no servidor ou recusar id existente — candidato ao `PV-STATE-001` (M2).~~
- **Checagem de posse duplicada** em chamadas internas (`GameplayActionExecution` → `ExecuteHunt`; `EnterGame` → `CreateRuntimeExecution`): uma consulta extra ao banco, sem defeito funcional. Só tratar se virar gargalo.
- ✅ **Resolvido em `PV-ARCH-001`:** ~~**DI de teste mantido à mão:** `tests/ProjectVictory.SmokeTests/TestServiceCollectionExtensions.cs` replica registros do `Program.cs` e já ficou defasado uma vez (faltava `IRuntimeRepository`; corrigido em `PV-QA-001` parte 1). Risco de nova defasagem quando handlers ganharem dependências.~~

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

> **Atualização `PV-PLATFORM-005` (2026-11-06):** `@types/react` e `@types/react-dom` 19.3 instalados; `npm audit fix` sem `--force` → **0 vulnerabilidades** (dependências diretas e as citadas nos alertas: vite 7.3.7, postcss 8.5.32, source-map-js 1.2.2, patch; outras indiretas mudaram em patch/minor dentro das faixas — lista no contrato); script `typecheck` (`tsc --noEmit`, fora do build); `tsc` **444 → 7 erros** (código das telas, lista no contrato `PV-PLATFORM-005`, destino `PV-UI-0xx`); `npm run build` verde. Os números abaixo são o baseline anterior.

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

Concluir a **Fila 2** e iniciar o **M3** (Combate real: `PV-COMBAT-010/011/012`, `PV-HUNT-010`, `PV-SUPPLY-010`, `PV-CONTENT-011`). Revisões ainda abertas: R-005 (duas famílias de roadmap), R-011 (confirmação do PO), R-012, R-014 (`PV-ARCH-001`), R-015…R-017. Ver `roadmap-to-playable.md`.

*Histórico:* ~~Executar **M0** (decisões ~~R-001~~ (resolvida em 2026-11-06), R-005, R-006, R-008; baseline executável na máquina do dono) e em seguida **M1**.~~ R-006 e R-008 resolvidas no `PV-GOV-007`.

---

# Regras Permanentes (mantidas da v1.2)

- A documentação oficial é a única fonte de verdade, **subordinada à hierarquia código > contratos/ADRs > documentação**.
- Todo novo módulo consulta o Roadmap e este documento.
- Nenhum módulo LOCKED é alterado sem revisão arquitetural.
- Toda decisão permanente é documentada.
- Toda Sprint atualiza este documento.
