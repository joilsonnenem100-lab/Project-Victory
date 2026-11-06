# Project-Victory — Roadmap de Execução até o Primeiro Teste Jogável

Versão: 1.0 · Data: 2026-11-05 · Status: **PROPOSTO — aguarda aprovação do dono (M0)**
Relação com outros roadmaps: `roadmap.md` continua sendo a **ordem oficial dos módulos** (Fases 0–10). Este documento é o roadmap de **execução**: ordena o trabalho de engenharia necessário para o jogo funcionar. Não altera nenhum módulo LOCKED; ele os **implementa** (resolve R-005).

## Definição de "ponto de funcionamento" (Primeiro Teste Jogável)

Fonte: `FIRST_FUN.md`, `CORE_GAMEPLAY_LOOP.md`, `phase-4-game-validation-roadmap.md` ("validar o Vertical Slice em ambiente executável… preparação do primeiro teste jogável"), `PV-MILESTONE-001-FIRST-COMPLETE-IDLE-HUNT-LOOP`.

Um jogador, pelo navegador, consegue: **cadastrar → logar → criar personagem → entrar no mundo → iniciar uma Hunt → o loop de hunt roda sozinho com combate por turnos contra monstros com IA → recebe XP e loot reais → sobe de nível pela Experience Table → pode sair e voltar sem perder o progresso** — tudo autenticado e validado no servidor, com build e testes verdes.

## Regras de condução

1. **Um marco por vez; sem pular dependência** (`roadmap.md`: "Nenhum módulo deverá ser iniciado fora da sequência").
2. Cada item vira **contrato** (`PV-<ÁREA>-NNN`) emitido pela Arquitetura *antes* da implementação.
3. Cada marco termina com: build verde, testes verdes, smoke do fluxo validado, `project-state.md` atualizado, revisão do `pv-architecture-reviewer`.
4. Regra Aditiva: nada validado é removido; stubs são **substituídos atrás da mesma interface**.
5. Marcos M1–M4 não dependem da decisão ADR-057. **M6 depende.**

---

## M0 — Baseline e Governança *(documentação; sem funcionalidade nova)*

| Item | Lastro |
|---|---|
| Aprovar `project-state.md` v2.0 e este roadmap | `roadmap.md` (Roadmap Rules 1–5) |
| Decidir **ADR-057** (Hunt Session × Runtime State/Checkpoint) — ✅ *ACCEPTED em 2026-11-06: B1 (Hunt termina ao desconectar) + Offline Training de skills (`DEC-20261106-05`)* | R-001; ADR-011/012; Handbook V3 §17–19 |
| Formalizar R-005, R-006, R-008 no `decision-log.md`; preencher/descartar ADR-026/044/048/055 — ✅ *ADR-026 (Ascension Model) ACCEPTED em `PV-GOV-006`.* ✅ *044/048 = NOT USED, 055 PROPOSED, R-006 e R-013 aplicadas em `PV-GOV-007` (`DEC-20261106-13`).* *2026-11-06: levantamento e propostas em `docs/architecture/PV-GOV-005-RELATORIO-2026-11-06.md` (026 = Ascension Model, 044/048 = número não utilizado, 055 = Event-Driven Quests; 5 linhas de R-006 com texto proposto); ~~aplicação pendente de decisão e contrato~~ (aplicada no `PV-GOV-007`)* | `ARCHITECTURE-GOVERNANCE` |
| **Dono roda `dotnet build` e `dotnet test`** na solução e informa o resultado (baseline) — ✅ **executado em 2026-11-05 (PV-GOV-002)**; resultado em `project-state.md`, Pendências de Validação | Regra Aditiva ("builds continuam passando") |
| Dono sobe API + client e percorre `COMO USAR O SMOKE TEST.txt`; informa onde quebra | `ENGINEERING-HANDOFF` (validação manual quando mais eficiente) |

**Saída:** nenhuma revisão R-00x sem dono/decisão; baseline numérico conhecido (build/test/smoke).

## M1 — Fundação executável e segura *(Fase 4: "integração, execução real, validação")*

| Contrato a emitir | Conteúdo | Lastro |
|---|---|---|
| `PV-PLATFORM-001` Startup | ✅ **Concluído em 2026-11-05** (branch `chief/m1-platform-001`): `MigrateAsync()` em Development antes do seed; snapshot sem divergência; clone limpo validado; `projectvictory.db` fora do versionamento (ver `project-state.md` e `CLEANUP-MANIFEST`). — Escopo original: aplicar as migrations existentes no startup de desenvolvimento (ou documentar `dotnet ef database update`); conferir `ModelSnapshot` × modelo (6 DbSets); **não renomear nem apagar `NomeDaMigration`** (já aplicada em bancos reais); depois disso, tirar `projectvictory.db` do versionamento | Análise §4.4 e §4.7 |
| `PV-PLATFORM-002` Configuração | ✅ **Concluído em 2026-11-05** (branch `chief/m1-platform-002`): `Jwt:Key` sem valor padrão em **nenhum** ambiente (ausente ou < 32 caracteres → startup falha com mensagem clara); `Issuer/Audience` emitidos e validados; connection string e CORS por configuração; log sensível do EF só em Development; chave de Development via user-secrets (guia `developer-guide/guides/configuracao-local.md`). O caminho do banco ficou configurável (`ConnectionStrings:ProjectVictory`). A criação do schema fora de Development foi **separada** para o `PV-PLATFORM-004` (decisão do Arquiteto: estratégia de migrations em produção é decisão própria). — Escopo original: chave JWT, connection string e CORS vindos de configuração/segredos; sem chave padrão fora de Development; validar `Issuer/Audience`; desligar `EnableSensitiveDataLogging`/`LogTo(Console)` fora de Development; **definir como o schema é criado/atualizado fora de Development** (hoje o `MigrateAsync()` roda só em Development — pendência herdada do `PV-PLATFORM-001`) e tornar o caminho do banco SQLite configurável (hoje relativo ao diretório de trabalho) | Handbook §24; Análise §4.3 |
| `PV-SEC-001` Autorização | ✅ **Implementado e revisado em 2026-11-06, aguardando merge** (branch `chief/m1-sec-001`; contrato `docs/contracts/active/PV-SEC-001.md`): `[Authorize]` nos 7 controllers (incluindo `Playable`); só `Account` é público; `PlayerOwnershipGuard` verifica posse de personagem/runtime/hunt session/encounter (outra conta → 403); 401/403/200 comprovados na API real. **Decisão D1 do Arquiteto:** a rota `api/inventory/{characterId}` foi **mantida** com verificação de dono, em vez de removida — a conta tem vários personagens e não há "personagem selecionado" no servidor; remover quebraria o cliente (Regra Aditiva). — Escopo original: `[Authorize]` em Hunt, HuntSession, Inventory, Runtime, Gameplay, Location; `characterId` derivado de `ICurrentPlayerContext` com verificação de dono; remover `characterId` livre da rota do Inventory (mantendo o contrato de resposta) | `PRINCIPLES`, Handbook §23 |
| `PV-PLATFORM-003` Dependências vulneráveis | ✅ **Concluído em 2026-11-06** (branch `chief/m1-platform-003`): EF Core 10.0.0 → 10.0.14 elimina o NU1903 do SQLite (CVE-2025-6965) e os avisos do `System.Security.Cryptography.Pkcs` (CVE-2026-84008/70162, via `.Design`); build da solução sem nenhum aviso; ferramentas `tools/PvDatabaseCheck` e `ItemCheck` também corrigidas. — Escopo original: Atualizar o pacote `SQLitePCLRaw` (`SQLitePCLRaw.lib.e_sqlite3` 2.1.11 → versão corrigida) para eliminar todos os avisos **NU1903** (vulnerabilidade alta) — 6 no build atual: 4 do baseline PV-GOV-002 + 2 do `SmokeTests` após `PV-QA-001` parte 1; build continua verde. **Atenção:** o pacote é dependência **transitiva** de `Microsoft.EntityFrameworkCore.Sqlite` 10.0.0 (`ProjectVictory.Persistence.csproj`), sem `PackageReference` direto — o contrato decide entre fixar a versão transitiva ou subir o EF Core. *Separado do `PV-QA-001` por ser dependência de plataforma, não teste (decisão do Arquiteto em PV-GOV-002, ADR-016: roadmap e organização cabem ao Arquiteto)* | Baseline PV-GOV-002 (`project-state.md`, Pendências de Validação) |
| `PV-QA-002` Baseline do cliente | ✅ **Concluído em 2026-11-06** (branch `chief/m1-qa-002`): `npm install`, `npm run build` (verde), lint inexistente, `tsc --noEmit` e `npm audit` registrados em `project-state.md`, sem alterar código do cliente | Ordem do PO (modo noite) |
| `PV-PLATFORM-005` Dependências do cliente | ✅ **Implementado em 2026-11-06** (branch `chief/q2-platform-005`; `docs/contracts/active/PV-PLATFORM-005.md`): `@types/react*` 19.3, `npm audit` 0 vulnerabilidades (diretas: patch; indiretas: patch/minor dentro das faixas), script `typecheck`; `tsc` 444 → 7 erros; build verde. — `npm audit fix` nas dependências de build (6 vulnerabilidades, 4 altas — inclui `vite`/`launch-editor` no Windows); adicionar `@types/react` e `@types/react-dom`; script `typecheck` (`tsc --noEmit`) sem bloquear o build. **Sem alterar código das telas** (isso é `PV-UI-0xx`) | Baseline `PV-QA-002`; revisão do `PV-QA-002` (R4) |
| `PV-PLATFORM-004` Schema fora de Development | ✅ **Implementado em 2026-11-06** (branch `chief/q2-platform-004`; `docs/contracts/active/PV-PLATFORM-004.md`): chave `Database:ApplyMigrationsOnStartup` (padrão desligada) fora de Development; guia `developer-guide/guides/deploy.md`. — Definir e implementar como o schema é criado/atualizado fora de Development (ex.: `dotnet ef database update`/bundle de migrations no deploy × `MigrateAsync()` controlado por configuração); hoje um banco vazio em Production falha no seed. Sem alterar migrations existentes | Pendência herdada do `PV-PLATFORM-001`, separada no `PV-PLATFORM-002`; Análise §4.4 |
| `PV-QA-001` Baseline de testes | ✅ **Parte 1 concluída em 2026-11-05** (branch `chief/m1-qa-001`): SmokeTests compila, está no `.sln` e o `dotnet test` não tem falhas; 1 teste com Skip aprovado pelo dono até o `PV-REWARD-010` (ver `project-state.md`). ✅ **Parte 2 concluída em 2026-11-06** (branch `chief/m1-qa-001b`): `Api.Tests` (HTTP 401/403, JWT), teste de DI do SmokeTests, `Domain.Tests` e `Runtime.Tests` com testes reais — 91 passam, 1 ignorado; convenção de `[Authorize]` por reflexão (ver `project-state.md`). ~~Ficou para depois: 403 por HTTP em runtime/hunt-session/encounter, token expirado e sem claim;~~ (feito no `PV-QA-003`, 2026-11-06) composição de DI compartilhada (R-014). *Escopo original da parte 2:* testes de Domain; teste de autorização por endpoint (HTTP, `WebApplicationFactory`: 401 sem token, 403 com outra conta, incluindo o mapeamento `OwnershipViolationException` → 403 do `PV-SEC-001`); **testes de `JwtSettings.Validate`** (chave ausente, chave < 32 caracteres, Issuer/Audience vazios, mensagem em Development × fora dele — ressalva R1 da revisão do `PV-PLATFORM-002`; hoje não há projeto de teste da API, o contrato decide onde criar). — Escopo original: **corrigir a compilação do `SmokeTests`** (CS1503 em `RewardInventoryIntegrationTest.cs` linha 70: `Assert.Contains` de `string` contra `Inventory.Items`, coleção de `ItemInstance` — ajustar a asserção, não o domínio); incluir `SmokeTests` no `.sln`; testes de Domain para entidades/VOs existentes; teste de autorização por endpoint | `TESTING-STRATEGY`, Análise §4.5 |
| `PV-CLEAN-001` Quarentena | ✅ **Concluído em 2026-11-06** (autorizado pelo PO; branch `chief/m1-clean-001`, commit isolado `38d1be7`; `CLEANUP-MANIFEST`). — Após build verde, remover `Application/Architecture|Playtest|Production` (R-007) | Regra Aditiva (exceção: decisão + contrato + validação) |

**Saída:** clone limpo → `dotnet build` e `dotnet test ProjectVictory.sln` verdes; API sobe com schema; nenhum endpoint de gameplay anônimo; fluxo atual do Vertical Slice intacto.

**Fila 2 (ordem do PO, 2026-11-06)** — fechamento do M1 e governança, branches `chief/q2-*` e `chief/m1-clean-001`: ✅ `PV-GOV-007` (ADRs 044/048/055, R-006, R-013; revisado) · ✅ `PV-GOV-008` (`CLAUDE.md`/`project-state.md`; revisado) · ✅ `PV-QA-003` (testes HTTP de posse e token; achado R-018) · ✅ `PV-ARCH-001` (R-014; projeto `ProjectVictory.Composition`) · ✅ `PV-PLATFORM-004` · ✅ `PV-PLATFORM-005` · ✅ `PV-CI-001` (`.github/workflows/ci.yml`) · ✅ `PV-CLEAN-001`. Status final em `docs/architecture/RELATORIO-FILA-2.md`.

## M2 — Conformidade com a arquitetura LOCKED *(corrigir divergências; sem novos sistemas)*

Decisões do PO que orientam o M2 (`PV-GOV-006`): curva de XP `XP(L) = 50/3 × (L³ − 6L² + 17L − 12)`, Experience `long`, Level derivado, Experience Table gerada pela fórmula (`DEC-20261106-10`); mesma curva em todas as Ascensions (`DEC-20261106-11`); XP rate 1x como configuração do servidor, sem endpoint (`DEC-20261106-12`); teto de Level 500 por Ascension (`ADR-026`); todo item com preço de venda ao NPC provisório (`DEC-20261106-08`).

**Ordem de execução (Arquiteto, ADR-016):** `PV-PROG-011` vem **antes** do `PV-PROG-010`, porque o Level derivado precisa da Experience Table para existir. Os demais seguem a ordem da tabela.

| Contrato | Conteúdo | Lastro |
|---|---|---|
| `PV-PROG-011` Experience Table | ✅ **Implementado em 2026-11-06** (branch `chief/m2-prog-011`; `docs/contracts/active/PV-PROG-011.md`). — Experience Table no Domain, **gerada pela fórmula** do `DEC-20261106-10`, com entradas explícitas para os Levels 1–500 (teto por Ascension, `ADR-026`) e validação (sem lacunas, crescente, contínua); `ProgressionProcessor` passa a interpretar a tabela (dados, não `if`s) atrás do mesmo `IProgressionProcessor`; sem teto fixo em 5 | ADR-025, ADR-026 (R-003); `DEC-20261106-10/11` |
| `PV-PROG-010` Level derivado | ✅ **Implementado em 2026-11-06** (branch `chief/m2-prog-010`; `docs/contracts/active/PV-PROG-010.md`). — Experience passa a `long` (Domain e persistência, **migration aditiva**); `Character.Level` calculado de Experience + Ascension (primeira) + Experience Table; o Level deixa de ser fonte de verdade. **A coluna `Level` do banco não é apagada** (apagar coluna é destrutivo): passa a ser só escrita com o valor derivado, nunca lida como verdade; removê-la exige decisão própria | ADR-023, 027, 028 (R-002); `DEC-20261106-10` |
| `PV-REWARD-010` Reward Pipeline real | ✅ **Implementado em 2026-11-06** (branch `chief/m2-reward-010`; `docs/contracts/active/PV-REWARD-010.md`). — `RewardProcessor` deixa de ser stub: XP e ouro vêm da definição de monstro; **XP × XP rate da configuração do servidor** (`Progression:ExperienceRate`, padrão 1x, sem endpoint); Loot via Loot Tables (chance de drop respeitada); Reward Pipeline é o único que muda Experience; **`ApplyRewardHandler` passa a aplicar drops e ouro ao personagem** (hoje só aplica XP — lacuna encontrada em `PV-QA-001` parte 1); o fluxo real do ataque (`RewardResolutionHandler`) passa a entregar XP; **preço de venda ao NPC** na definição de item (valores provisórios); aceite inclui remover o Skip de `ApplyReward_Should_Add_Item_To_Character_Inventory` e ligar a asserção ao `ItemDefinitionId` do drop | ADR-008, 024, 037, 038; `DEC-20261106-08/12` |
| `PV-STATE-001` Posse de estado | ✅ **Implementado em 2026-11-06** (branch `chief/m2-state-001`; `docs/contracts/active/PV-STATE-001.md`). — Cada store em memória classificado (temporário × semi-permanente × permanente); corrigir `HuntRepository`/`CombatContextRepository` (thread-safety) e `CombatResultStore` (escopo) | Handbook §17 (o §8 citado antes estava errado), `STATE-OWNERSHIP-MODEL` |

**Saída:** checklist de conformidade ADR-021…038 completo para o código tocado; testes de regressão do loop passando.

*Itens do M1 ainda abertos quando o M2 começou (2026-11-06): `PV-PLATFORM-004`, `PV-PLATFORM-005` e `PV-CLEAN-001`. O PO mandou executar o M2 (`PV-GOV-006`); nenhum deles é dependência técnica do M2. O `PV-CLEAN-001` apaga arquivos e por isso só roda com autorização explícita.*

## M3 — Combate real *(Fase 5: Combat Architecture LOCKED → implementação)*

| Contrato | Conteúdo | Lastro |
|---|---|---|
| `PV-COMBAT-010` Combat Orchestrator | ✅ **Implementado em 2026-11-06** (branch `chief/m3-combat-010`; `docs/contracts/active/PV-COMBAT-010.md`). — Orquestrador coordena turnos; **não** implementa dano, skills, IA, loot ou XP | ADR-051 |
| `PV-COMBAT-011` Turnos | ✅ **Implementado em 2026-11-06** (branch `chief/m3-combat-011`). — Combate por turnos, exatamente 1 participante ativo por turno, determinístico | ADR-052 |
| `PV-COMBAT-012` Regras de dano e ação | ✅ **Implementado em 2026-11-06** (branch `chief/m3-combat-012`; fórmula PROVISÓRIA atrás de `IDamageFormula`). — Dano do atacante e do inimigo; ação básica + secundária; vida do jogador; resultado (vitória/derrota) substitui `CombatResolver` stub atrás de `ICombatResolver` | ADR-006; Snapshot "Próximos Objetivos" |
| `PV-HUNT-010` Derrota | ✅ **Implementado em 2026-11-06** (branch `chief/m3-hunt-010`). — Morte encerra a Hunt pelo **mesmo** pipeline de encerramento; jogador volta à cidade e aguarda comando; **nenhuma perda de loot ou XP** | ADR-012; `DEC-20261106-04` |
| `PV-SUPPLY-010` Suprimentos (poções) | ✅ **Servidor implementado em 2026-11-06** (branch `chief/m3-supply-010`; endpoint `POST api/hunt/supplies`). **A "tela própria" do cliente está pendente** → `PV-UI-0xx` (M8). — Só **poção de vida** e **poção de mana**; uso configurado pelo jogador em tela própria, inclusive durante a Hunt; servidor valida e aplica **a partir do próximo turno**; **sem suprimento não há cura**; **fim de suprimento encerra a Hunt** pelo mesmo pipeline | `DEC-20261106-02/03/04`; ADR-012; ADR-052 |
| `PV-CONTENT-011` Spider Cave | ✅ **Implementado em 2026-11-06** (branch `chief/m3-content-011`). — Primeira Hunt: **Spider Cave** com 5 monstros — **Rat** e **Cave Rat** (já existem em `EncounterGenerator.cs`) + **Spider**, **Poison Spider** e **Giant Spider** (novos). Atributos entregues como **BALANCEAMENTO PROVISÓRIO** para o PO ajustar; definições por dados (base para o `PV-MONSTER-010`) | `DEC-20261106-09`; ADR-005/006 |

**Saída:** combate com resultado não trivial, determinístico e coberto por testes de Domain/Application; cliente continua consumindo o mesmo contrato de resultado.

**Fila 3 (ordem do PO, 2026-11-06)** — execução do M3 com pré-requisitos, branches `chief/m3-*` (nascem de `chief/m1-clean-001`; a `main` ainda não o contém): ✅ `PV-SEC-002` (R-018; revisado) · ✅ `PV-CHAR-010` (Vida/Mana derivadas, ADR-029; valores PROVISÓRIOS, R-021) · ✅ `PV-COMBAT-010` (orquestrador; integração no 011/012) · ✅ `PV-COMBAT-011` (semente por encounter; `Combat:FixedSeed` só para testes) · ✅ `PV-COMBAT-012` (combate real no ataque; fórmula e atributos PROVISÓRIOS) · ✅ `PV-HUNT-010` (pipeline único `EndHuntHandler`; cidade PROVISÓRIA) · ✅ `PV-SUPPLY-010` (poções; valores e interpretações PROVISÓRIOS) · ✅ `PV-CONTENT-011` (Spider Cave por dados; balanceamento PROVISÓRIO) · `PV-QA-004` · `PV-CLEAN-002` (só com "R-020: AUTORIZADO"). Status final em `docs/architecture/RELATORIO-FILA-3.md`.

## M4 — Gameplay Foundation (Fase 6) *(na ordem oficial: Monster AI → Experience Distribution → Spawn)*

| Contrato | Conteúdo | Lastro |
|---|---|---|
| `PV-MONSTER-010` Monster AI orientada a dados | Escolha de ação do monstro por definição de dados | ADR-005, 006 |
| `PV-XP-010` Experience Distribution | Distribuição de XP como responsabilidade própria | ADR-007 |
| `PV-SPAWN-010` Spawn System | Stateless, dirigido pela Hunt, com ciclo de vida de spawn | ADR-009, 010, 047 |

**Saída:** encontros gerados pelo Spawn, monstros agindo por dados, XP distribuída corretamente; Fase 6 → LOCKED no `roadmap.md` via revisão.

## M5 — Persistência definitiva e Save Strategy

| Contrato | Conteúdo | Lastro |
|---|---|---|
| `PV-PERSIST-010` | Substituir repositórios em memória que devam ser permanentes/semi-permanentes por EF Core, conforme classificação de `PV-STATE-001`; estado puramente temporário permanece em memória | Snapshot ("Persistência definitiva"); Handbook §17 |
| `PV-PERSIST-011` Save Strategy / Checkpoints | Salvar em início, conclusão, recompensa e logout; **não** a cada ação pequena | Handbook §19; `KNOWN-GAPS` |

**Saída:** reiniciar a API não perde Account, Character, Inventory, Runtime/checkpoints; testes de persistência após reinício (já exigido em `PV-VS-2.0-044`).

## M6 — Hunt Loop contínuo, encerramento ao desconectar e Offline Training *(ADR-057 ACCEPTED)*

Decisão do PO (`DEC-20261106-05`, ADR-057): **enquanto conectado**, a Hunt roda em loop; **ao desconectar**, termina pelo pipeline único (ADR-012); **offline**, só o Offline Training de skills progride. A Hunt nunca é recuperada.

| Contrato | Conteúdo | Lastro |
|---|---|---|
| `PV-RUNTIME-010` Game Session + Runtime Coordinator | Coordenação de experiência contínua **enquanto o jogador está conectado**, sem inflar handlers; detecta a desconexão (fim da sessão do jogador) | Handbook §9–10; `KNOWN-GAPS` |
| `PV-HUNT-011` Hunt Loop contínuo | *Checklist herdado do `PV-STATE-001` (D2/D3): limpar da memória Hunts/encounters/contextos encerrados; serializar comandos sobre o mesmo agregado (ex.: `ActiveEncounter._monsters`).* Hunt Definition → Session → Encounter Generator → Combat → Reward → Progression → novo loop; ciclo Starting→Active→Ending→Finished; **mede o tempo da Hunt** (início/fim) para o XP/hora do Analyzer | Handbook §11; ADR-012, 045, 046 |
| `PV-HUNT-012` Encerramento ao desconectar | Desconexão é **motivo de encerramento** da Hunt pelo **mesmo** pipeline de morte, fim de suprimento e saída manual; jogador volta à cidade; nenhuma perda de loot ou XP já obtidos | ADR-012; ADR-057; `DEC-20261106-04/05` |
| `PV-RUNTIME-011` Recovery do Runtime | Servidor inicia → carrega checkpoint do **personagem e do Runtime** → reconstrói contexto. **Não** recupera Hunt Session (ADR-057, Parte A simplificada) | Handbook §18; ADR-057; `PV-RUNTIME-002-A` |
| `PV-TRAIN-010` Offline Training de skills | Implementar o módulo LOCKED da Fase 2 conforme `knowledge-base/training.md`: 1 Training ativo por personagem; item de Training com **cargas** que definem a duração; durante o Training o personagem não caça, não recebe EXP nem combate; mesmo funcionamento online e offline (sem bônus); progresso calculado **no servidor** e aplicado a `Skills` (Fist, Club, Sword, Axe, Distance, Shielding, Magic Level); Training < Hunt em velocidade; cancelamento sem penalidade | `roadmap.md` Fase 2 (LOCKED); `knowledge-base/training.md` (LOCKED); `DEC-20261106-05` |

**Saída:** o loop roda sem cliques repetidos enquanto o jogador está conectado; desconectar encerra a Hunt pelo pipeline único; reiniciar o servidor recupera personagem e Runtime (sem Hunt); Offline Training progride skills offline.

### Dúvidas para o Product Owner antes do `PV-TRAIN-010` (não bloqueiam M1–M5)

Levantadas na leitura de `roadmap.md` (Fase 2), `knowledge-base/training.md` e `src/ProjectVictory.Domain/Entities/Skills.cs`; o próprio `training.md` lista "fórmula de progresso; consumo de cargas; tipos de Training Items; diferenças Free/Premium; interface visual; balanceamento" como pendências futuras.

1. **Como o jogador obtém o item de Training?** (compra em NPC, item inicial, drop) — compra em NPC depende de comércio, que está na Fase 7/8.
2. **Início do Offline Training:** o jogador precisa iniciar o Training antes de sair, ou ele começa sozinho ao desconectar? (Pelo `training.md`, Training e Hunt são exclusivos.)
3. **Skill do Monk:** o `training.md` lista "First Fighting"; o código tem `Fist`. Confirmar que o Monk treina **Fist Fighting** e se treina mais alguma skill.
4. **Velocidade e teto:** quanto mais lento que a Hunt (regra "Hunt > Training") e se existe limite de horas offline acumuladas.
5. **Free × Premium:** existe diferença no Training nesta fase, ou fica para depois?
6. **Tolerância de queda de conexão:** uma queda curta de rede já encerra a Hunt, ou há uma janela de reconexão (ex.: alguns segundos) antes do encerramento?

## M7 — Event System

| Contrato | Conteúdo | Lastro |
|---|---|---|
| `PV-EVENT-010` Typed Event Bus | Eventos derivam de `EngineEvent`; despacho por tipo, nunca por string | ADR-049 |
| `PV-EVENT-011` Domain Events | `CombatCompletedEvent → Reward → (futuro) Achievement/Quest` | ADR-050; `PRINCIPLES` ("Eventos futuros") |

**Saída:** Combat→Reward desacoplado por evento, sem alterar contratos públicos.

## M8 — Experiência do jogador e Primeiro Teste Jogável

| Contrato | Conteúdo | Lastro |
|---|---|---|
| `PV-FRONTEND-GAME-HUD-FOUNDATION-001` (ativo) | Refazer HUD aprovado (o anterior estava "NOT APPROVED") | contrato ativo |
| `PV-UI-0xx` Navegação | Router real no client no lugar de `useState` por string; tipar `any`; estados de loading/erro. **Entrada do baseline `PV-QA-002` (2026-11-06):** `vite build` verde, mas `tsc --noEmit` = 443 erros (426 por falta de `@types/react*`), 2 imports quebrados em `WorldScenePage`/`PlayerStatePanel`, `GamePage` órfão, sem lint, `npm audit` 6 vulnerabilidades de build (ver `project-state.md`) | Análise §4.5; contratos `PV-UI-001/002` (histórico git: "Page Composition Rules") |
| `PV-QA-010` Playtest | Roteiro de playtest, matriz de regressão, critérios de aceite | `ACCEPTANCE_CRITERIA.md`, `PV-VS-2.0-045` |
| `PV-CONTENT-010` Conteúdo mínimo | Monstros, itens e loot suficientes para ~30 min de jogo | `FIRST_FUN.md`, `PROJECT-VICTORY-CONTENT-PIPELINE-ARCHITECTURE.md` |
| `PV-HUD-ANALYZER-010` Hunt Analyzer | Aba "Analyzer" do Game HUD: XP obtido, XP/hora, débito, lucro e lucro líquido da Hunt, em tempo real durante a Hunt e resumo ao final, **calculados no servidor** e só exibidos pelo cliente | `DEC-20261106-01` (PO); ADR-002; `PV-FRONTEND-GAME-HUD-FOUNDATION-001` |
| `PV-ADMIN-010` Ferramenta de administração | Tela de ajuste do **XP rate** restrita a uma **conta administradora**: papel de administrador na Account, autorização por papel no servidor, endpoint protegido; jogador comum e cliente comum nunca alteram o valor | `DEC-20261106-12` (PO); Server Authority (`CLAUDE.md`) |
| `PV-HUD-BATTLE-VIEW-010` Representação visual | Animação central por local (ex.: Spider Cave), duelo automático decorativo, **sem efeito no resultado** | `DEC-20261106-01` (PO); ADR-002 |

### Dependências do Hunt Analyzer (`DEC-20261106-01`)

O Analyzer é **entregue em M8 (UI)** e **não bloqueia M1 a M4**. Ele consome dados que precisam existir antes, entregues pelos marcos que já os tocam:

| Dependência | Onde entra | Observação |
|---|---|---|
| **Preços de compra e venda NPC nas definições de item** (dado, não comércio) | O campo e os preços provisórios entram no `PV-REWARD-010` (M2); o `PV-CONTENT-010` (M8) completa os preços dos itens novos | Só o **dado** de preço. Comércio com NPC continua na Fase 7/8 (`roadmap.md`) — Guardião de Escopo |
| **Modelo de suprimentos (poções)** — o que é gasto e quando | `PV-SUPPLY-010` (M3) | **Decidido pelo PO em 2026-11-06:** só poção de vida e de mana, uso configurável, aplicado no próximo turno (`DEC-20261106-02/03`). O débito do Analyzer = poções gastas × preço de compra no NPC |
| **Valor do loot no lucro** | `PV-HUD-ANALYZER-010` (M8) | **Preço de venda ao NPC** — confirmado pelo PO em 2026-11-06 (`DEC-20261106-08`); preços iniciais provisórios entram no `PV-REWARD-010` (M2) |
| **Medição de tempo da Hunt** (início, pausas, fim) para XP/hora | `PV-HUNT-011` (M6, Hunt Loop contínuo) | Política decidida (`ADR-057` ACCEPTED): a Hunt só existe enquanto o jogador está conectado, então XP/hora usa apenas tempo online |
| XP e loot reais da Hunt | `PV-REWARD-010` (M2), `PV-XP-010` (M4) | Hoje o Reward é stub (XP 0); sem eles o Analyzer mostraria zeros |

**Saída = Primeiro Teste Jogável** (definição no topo).

## Depois do M8 — ordem oficial (`roadmap.md`)

Fase 7 World (NPCs, Cities, Maps, Areas, Regions — ADR-013…020, 053, 054) → Fase 8 Economy → Fase 9 Systems → Fase 10 Endgame. Cada uma exige seu próprio detalhamento e contratos na hora certa; **não** detalhar agora (Guardião de Escopo: "Economia não foi criada antes da fundação").

---

## Dependências resumidas

```
M0 ─► M1 ─► M2 ─► M3 ─► M4 ─► M5 ─► M6 ─► M7 ─► M8
              ▲                   ▲
              └── conformidade ───┴── M6 exige ADR-057 aprovado
```

## Riscos acompanhados

| Risco | Mitigação |
|---|---|
| Sem `dotnet` no ambiente de nuvem, build/test só rodam na máquina do dono | Dono executa e cola a saída; a equipe nunca declara "passou" sem evidência (`CLAUDE.md`) |
| Volume de 604 arquivos em Application dificulta localizar o que está vivo | M1 remove o código morto confirmado; contratos de cada marco listam arquivos tocados |
| Reverter ADR 011/012 sem decisão | Gate explícito em M0/M6 |
| Escopo cresce (Fase 7+) | Guardião de Escopo: nada fora do marco corrente |
