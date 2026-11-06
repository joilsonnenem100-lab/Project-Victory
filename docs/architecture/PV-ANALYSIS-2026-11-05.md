# Project-Victory — Análise Completa do Estado Atual

Data: 2026-11-05 · Autor: Desenvolvedor Chefe / Chefe de Arquitetura (Claude) · Base: `Project-Victory.zip` (6.238 entradas, 328 MB descompactados; git: 236 entradas de log, 12–30/jul/2026)

**Limites desta análise.** Leitura de documentação, estrutura, configuração e dos arquivos de código decisivos (Program.cs, controllers, DbContext, repositórios, serviços de Infrastructure, App.tsx, csproj/sln). **Não** li todos os 920 arquivos de `src/`, **não** compilei nem rodei testes (sem SDK .NET), **não** executei o client. Cada afirmação abaixo cita o arquivo de onde veio.

---

## 1. O que o projeto é

MMORPG Idle inspirado em Tibia, progressão automática por **Hunts**, com ênfase em escalabilidade, simplicidade e manutenibilidade (`README.md`, `PROJECT_CONTEXT.md`). Princípios LOCKED: Engine First, Domain First, Template→Factory→Instance, Clean Architecture, Zero Framework Domain, Event Driven, Server Authority (`architecture-rules.md`, 20 regras).

Stack: .NET `net10.0`, ASP.NET + JWT, EF Core 10 + SQLite, React 19 + Vite 7 + TypeScript.

## 2. Fotografia do repositório

| Área | Volume |
|---|---|
| `src/ProjectVictory.Application` | 604 `.cs` (≈ 9.500 linhas; **397 arquivos ≤ 15 linhas** — records de Command/Result) |
| `src/ProjectVictory.Domain` | 147 `.cs` (≈ 3.100 linhas; 98 ≤ 15 linhas) |
| `src/ProjectVictory.Persistence` | 45 `.cs` (31 em `Database/`) |
| `src/ProjectVictory.Infrastructure` | 6 `.cs` |
| `src/ProjectVictory.API` | 17 `.cs`, 11 controllers |
| `src/ProjectVictory.Runtime` / `Shared` | **0 `.cs`** (só `.csproj`) |
| Client | ≈ 4.000 linhas TS/TSX, 10 páginas, 3 usos de `any` |
| Testes | **3** testes no total (`Application.Tests` 1, `SmokeTests` 2, `Domain.Tests` 0, `Runtime.Tests` 0) |
| Documentação | `docs/` 656 arquivos; `developer-guide/` 132; 53 ADRs |

Atividade git: 229 das 236 entradas ocorrem entre 12 e 20/jul; depois disso, 7 entradas em 10 dias (última: 30/jul).

## 3. Arquitetura documentada (a "lei")

Hierarquia da fonte da verdade (`MASTER-ARCHITECTURE-MAP`): código oficial aprovado → contratos/decisões → documentação → mapa master.
Fluxo oficial: Arquitetura → Contrato → Engenharia → Implementação → Build → Teste → Validação → Aceite (`CONTEXT-HANDOFF`, `ENGINEERING-HANDOFF`).
Cadeia de domínio: Account → Authentication → Player Context → Character State → Runtime Environment → Gameplay Runtime → Persistence; e Combat → Reward → Progression → Character (Aggregate Ownership).
Roadmap oficial: Fases 0–5 LOCKED; Fase 6 (Gameplay Foundation) em andamento — Monsters LOCKED; Monster AI, Experience Distribution, Spawn System PLANNED; Fases 7–10 PLANNED (`roadmap.md`).

ADRs relevantes para o trabalho à frente (todos ACCEPTED): 005/006 (Monster AI, ação básica/secundária), 007 (distribuição de XP), 008/024 (Reward Pipeline), 009/010/047 (Spawn), 011/012 (Hunt Session e ciclo único), 021–031 (Character, Experience absoluta, Level derivado), 033–038 (Equipment, Inventory, Loot), 045/046 (Hunt Orchestrator, Context/Result), 049/050 (Event Bus tipado, Domain Events), 051/052 (Combat Orchestrator, combate por turnos).

## 4. Diagnóstico: onde o código diverge da lei

### 4.1 Stubs no coração do loop (o "jogo" ainda não existe)
- `CombatResolver.Resolve` ignora a requisição e retorna `new CombatResult(request.AttackerId, true)`: o atacante **sempre vence**. Não há turnos (ADR-052), dano, vida do jogador nem derrota.
- `RewardProcessor.Process` retorna XP `0`, ouro `0` e um drop fixo `"Smoke Test Item"` com `Guid.NewGuid()`. Está em `Infrastructure/Services`, registrado no DI de produção.
- `ProgressionProcessor` deriva nível por `if`s fixos (<100→1, <300→2, <600→3, <1000→4, senão 5). Teto de nível 5 e tabela embutida.
- Conclusão: o State Snapshot afirma "Combat Loop Complete / Validated"; o que foi validado é o **encadeamento** (pipeline de chamadas), não as regras. Isso é coerente com o próprio Snapshot, que lista como próximos objetivos "Turn system, Dano do inimigo, Vida do jogador, Resultado de combate, Persistência definitiva".

### 4.2 Conflitos com ADRs ACCEPTED
- **R-002 / ADR-027, 028 — Level derivado.** `PersistentCharacterState.Level` é coluna persistida e obrigatória (`DbContext`). ADR-027: "Level nunca poderá armazenar progresso"; Experience é a única fonte.
- **R-003 / ADR-025 — Experience Table por Ascension.** Tabela fixa dentro de `ProgressionProcessor`. Violação direta de "Experience Table é o interpretador".
- **R-001 / ADR-011, 012 — Hunt Session.** ADR-011: "Nunca será persistida"; ADR-012: "recuperação de Hunt Session" proibida, "nenhuma recuperação de Hunt após desconexão". O código tem `HuntSessionRepository` + `PersistentHuntSessionState` (SQLite) e os contratos `PV-RUNTIME-002/002-A` pedem persistência e recovery de Runtime. O Handbook V3 §17 classifica Hunt Session como "semi-permanente" e §18–19 definem Recovery e Checkpoint. **Há quatro fontes em conflito.**
  - *Análise do chefe de arquitetura:* a hierarquia dá precedência a ADR sobre Handbook (documentação). Porém ADR-012 também proíbe recuperar hunt após desconexão, o que colide com o núcleo declarado do produto (jogo **Idle** com progressão automática, e módulo **Offline Training LOCKED** na Fase 2). Isso torna provável que a intenção atual seja a do Handbook V3 e que os ADRs 011/012 estejam desatualizados nesse ponto.
  - *Por isso esta é a única decisão que devolvo ao dono:* **ADR-057 (PROPOSED)** — separar (a) *Hunt Session* como objeto efêmero de execução (preserva o essencial de ADR-011) de (b) *Runtime State/Checkpoint* persistível que permite retomar o loop (cumpre Handbook §18–19 e PV-RUNTIME-002). A parte arquitetural é minha (`decision-log.md`, ADR-016: Arquiteto responde por arquitetura e revisões), mas "a Hunt sobrevive à desconexão?" é regra de **mecânica de jogo**, que o mesmo ADR-016 atribui ao Product Owner; além disso a emenda reverte restrições de dois ADRs ACCEPTED.
- **R-004 / ADR-049, 050 — Event Bus tipado:** inexistente (listado em `KNOWN-GAPS` e `SYSTEM-MATURITY-MATRIX`).

### 4.3 Segurança e Server Authority (Handbook §23–24)
- `[Authorize]` presente apenas em `CharacterController`, `GameController.enter` e `ItemController`. **Sem** `[Authorize]`: `Hunt`, `HuntSession`, `Inventory`, `Runtime`, `Gameplay`, `Location`, `Playable`. Em `InventoryController`, `GET api/inventory/{characterId}` recebe o id na rota: sem autenticação nem verificação de dono, qualquer chamador lê o inventário de qualquer personagem.
- `Program.cs`: chave JWT com **valor padrão fixo no código** quando `Jwt:Key` não está configurada; `ValidateIssuer/Audience = false`.
- `ProjectVictoryDbContext.OnConfiguring` liga `EnableSensitiveDataLogging()` e `LogTo(Console)` justamente quando as opções **estão** configuradas (`if (!IsConfigured) return;`), isto é, na API real.
- Connection string SQLite e origem CORS fixas no código.

### 4.4 Inicialização e persistência
- `Program.cs` só executa `ItemDefinitionSeed`; **não chama `Migrate()`/`EnsureCreated()`**. Só o smoke test cria o schema (`EnsureCreatedAsync`). Um clone limpo roda a API sem tabelas, salvo passo manual não documentado.
- Existem **14 migrations** (de `InitialAccountPersistence` a `RuntimePersistenceFoundation`); uma delas, já aplicada nos bancos de desenvolvimento, tem nome-placeholder (`NomeDaMigration`) e **não deve ser renomeada**. Não verifiquei se o `ModelSnapshot` corresponde ao modelo atual. O banco de desenvolvimento `projectvictory.db` está **versionado no git** (7 contas de teste) e é o que faz um clone rodar hoje, apesar de o `Program.cs` não migrar.
- Em memória (perdem estado ao reiniciar): `HuntRepository` (`List<Hunt>` singleton, **não thread-safe**), `CombatContextRepository` (`List<>`), `EncounterStateStore`, `RuntimeExecutionStateStore` (todos confirmados no código). `CombatResultStore` é `AddScoped` mas guarda `ConcurrentDictionary`: **provável perda de resultados entre requests** (o próprio Snapshot registra FIX-001 com o mesmo padrão em `EncounterStateStore`). Marcado *a verificar na execução*.
- `DbSet<Monsters>` e `DbSet<Items>` existem no modelo; a persistência de monstros por `Persistent*State` está definida, porém o fluxo de monstros passa por `MonsterLootDefinitionProvider` e catálogos — **não confirmei** qual caminho é o ativo.

### 4.5 Estrutura e testabilidade
- `ProjectVictory.Runtime` e `ProjectVictory.Shared` são projetos vazios, enquanto `Domain/Runtime` (9 arquivos) e `Application/Runtime` + `Application/VerticalSlice` concentram o runtime. `Runtime.Tests` referencia o projeto vazio.
- `SmokeTests` fora do `.sln` → `dotnet test ProjectVictory.sln` não o executa.
- 42 arquivos em `Application/Architecture|Playtest|Production` (handlers que devolvem booleanos de "validação" de processo) **sem nenhuma referência** fora das próprias pastas e fora do DI. Parecem scaffolding de governança modelado como código.
- `Application/VerticalSlice` (96 arquivos) contém tipos de produção usados por Hunt/Runtime/World (ex.: `RuntimeRecoveryData`, `RuntimeExecutionStateStore`): nome de pasta temporário abrigando código permanente.
- Client: roteamento por `useState("login")` em `App.tsx` (sem router), 3 usos de `any`; `components/gameHUD` vazio aguardando contrato ativo `PV-FRONTEND-GAME-HUD-FOUNDATION-001`.

### 4.6 Governança documental
- Estado oficial = `project-state.md`, mas estava na v1.2 de 14/jul (dizia "Revisões pendentes: nenhuma"). Atualizado para v2.0 nesta análise.
- `CURRENT_CONTEXT.md` se declara histórico; `PROJECT_CONTEXT.md`, `AI_WORKFLOW.md`, `architecture-checklist.md` e `README_DEVELOPMENT.md` ainda o tratam como estado atual.
- **Duas séries de ADR com a mesma numeração** (R-009): `decision-log.md` tem `ADR-001…018` próprios; `developer-guide/adr/` tem `ADR-001…057`. Novas decisões usam prefixo `DEC-`.
- Duas raízes de documentação (`docs/` e `Documentation/`); duas famílias de roadmap (R-005); ADRs 026, 044, 048 ausentes, ADR-055 e `Sprint-054..056` vazios; `knowledge-base/Market.md` vazio.
- Volume de contratos: `docs/expansion/contracts` (161), `docs/vertical-slice/contracts` (58), `work-items` (46), `docs/contracts/*`: excelente rastreabilidade, mas sem índice único de "qual contrato está em vigor".

### 4.7 Higiene do repositório GitHub (verificada contra o clone)
- O `.gitignore` não cobria `node_modules/` nem `dist/`: **2.440 arquivos de `node_modules`/`dist`** (2.438 de `node_modules` e 2 de `dist`) estavam versionados no client (repositório com 4.266 arquivos rastreados contra ≈ 1.830 de conteúdo real).
- Binários e listagens versionados sem valor de fonte: `src.zip` (5 MB), `client-files.txt` (352 KB), `NOT-APROVED-gameHUD.zip`.
- `projectvictory.db` versionado (banco SQLite de desenvolvimento com 7 contas de teste, e-mails `@projectvictory.com`/`@test.com`). Não há dado pessoal aparente; o arquivo permanece no histórico do git. **Não foi removido nesta revisão**: sem migração no startup (M1, `PV-PLATFORM-001`), um clone novo ficaria sem banco.
- Conteúdo de código e documentação: GitHub e zip enviado são **idênticos** em 1.768 arquivos comparados (diferença apenas de fim de linha CRLF×LF).

## 5. O que está bom (e deve ser preservado)

Separação em camadas respeitada nas referências de projeto (Domain sem dependências; Application→Domain; Persistence/Infrastructure→Application/Domain; API não referencia Domain). Handlers pequenos e nomeados por caso de uso. Seed de definições de item. Fluxo de autenticação funcional com hash de senha. Documentação de arquitetura rara em qualidade e consistência de linguagem. `ICurrentPlayerContext` já abstrai identidade — a correção de autorização é incremental, não estrutural.

## 6. Limpeza executada

Ver `CLEANUP-MANIFEST-2026-11-05.md`. Em resumo: apagados 5 itens redundantes/lixo (Handbook V2 superado, 3 binários/listagens do client, 2 `deleteme.md` idênticos, 1 pasta vazia); **movidas** para arquivo/lugar correto as 4 peças de `DELETE/` com conteúdo único — inclusive as **20 regras LOCKED**, que existiam *apenas* ali. Nenhum C# tocado (sem build disponível).

## 7. Decisões que o chefe de arquitetura já tomou (com lastro)

| Decisão | Lastro |
|---|---|
| Código se conforma aos ADRs 024/025/027/028 (Level derivado, Experience Table, Reward Pipeline) | Hierarquia da fonte da verdade; `ARCHITECTURE-RULES` exige decisão formal para divergir e não há |
| Autenticação/autorização em todos os endpoints de gameplay; identidade só pelo token | `PRINCIPLES` ("O cliente solicita. O servidor decide"), Handbook §23–24 |
| Stubs de Combat/Reward/Progression são substituídos nos marcos M2–M3 sem mudar contratos públicos | Regra Aditiva: manter endpoints, DTOs e handlers; trocar a implementação por trás da interface |
| Pastas `Architecture/Playtest/Production` entram em quarentena e só saem após build verde | Regra Aditiva exige validar "builds continuam passando" |
| `roadmap-to-playable.md` é o roadmap de execução; `roadmap.md` continua a ordem de módulos | Resolve R-005 sem alterar documento LOCKED |

## 8. Decisão que exige o dono

**ADR-057** (R-001): ver §4.2. Recomendação do chefe de arquitetura: **aprovar a separação Hunt Session efêmera × Runtime State/Checkpoint persistível**. Até a decisão, M1–M4 não dependem dela; M6 depende.
