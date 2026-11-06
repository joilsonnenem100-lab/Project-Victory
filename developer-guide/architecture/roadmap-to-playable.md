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
| Decidir **ADR-057** (Hunt Session × Runtime State/Checkpoint) — *2026-11-06: Parte A aprovada pelo Arquiteto; Parte B = B3 sem XP offline; escopo do ganho offline **aguardando PO** (`DEC-20261106-05`)* | R-001; ADR-011/012; Handbook V3 §17–19 |
| Formalizar R-005, R-006, R-008 no `decision-log.md`; preencher/descartar ADR-026/044/048/055 | `ARCHITECTURE-GOVERNANCE` |
| **Dono roda `dotnet build` e `dotnet test`** na solução e informa o resultado (baseline) — ✅ **executado em 2026-11-05 (PV-GOV-002)**; resultado em `project-state.md`, Pendências de Validação | Regra Aditiva ("builds continuam passando") |
| Dono sobe API + client e percorre `COMO USAR O SMOKE TEST.txt`; informa onde quebra | `ENGINEERING-HANDOFF` (validação manual quando mais eficiente) |

**Saída:** nenhuma revisão R-00x sem dono/decisão; baseline numérico conhecido (build/test/smoke).

## M1 — Fundação executável e segura *(Fase 4: "integração, execução real, validação")*

| Contrato a emitir | Conteúdo | Lastro |
|---|---|---|
| `PV-PLATFORM-001` Startup | ✅ **Concluído em 2026-11-05** (branch `chief/m1-platform-001`): `MigrateAsync()` em Development antes do seed; snapshot sem divergência; clone limpo validado; `projectvictory.db` fora do versionamento (ver `project-state.md` e `CLEANUP-MANIFEST`). — Escopo original: aplicar as migrations existentes no startup de desenvolvimento (ou documentar `dotnet ef database update`); conferir `ModelSnapshot` × modelo (6 DbSets); **não renomear nem apagar `NomeDaMigration`** (já aplicada em bancos reais); depois disso, tirar `projectvictory.db` do versionamento | Análise §4.4 e §4.7 |
| `PV-PLATFORM-002` Configuração | ✅ **Concluído em 2026-11-05** (branch `chief/m1-platform-002`): `Jwt:Key` sem valor padrão em **nenhum** ambiente (ausente ou < 32 caracteres → startup falha com mensagem clara); `Issuer/Audience` emitidos e validados; connection string e CORS por configuração; log sensível do EF só em Development; chave de Development via user-secrets (guia `developer-guide/guides/configuracao-local.md`). O caminho do banco ficou configurável (`ConnectionStrings:ProjectVictory`). A criação do schema fora de Development foi **separada** para o `PV-PLATFORM-004` (decisão do Arquiteto: estratégia de migrations em produção é decisão própria). — Escopo original: chave JWT, connection string e CORS vindos de configuração/segredos; sem chave padrão fora de Development; validar `Issuer/Audience`; desligar `EnableSensitiveDataLogging`/`LogTo(Console)` fora de Development; **definir como o schema é criado/atualizado fora de Development** (hoje o `MigrateAsync()` roda só em Development — pendência herdada do `PV-PLATFORM-001`) e tornar o caminho do banco SQLite configurável (hoje relativo ao diretório de trabalho) | Handbook §24; Análise §4.3 |
| `PV-SEC-001` Autorização | ✅ **Implementado e revisado em 2026-11-06, aguardando merge** (branch `chief/m1-sec-001`; contrato `docs/contracts/active/PV-SEC-001.md`): `[Authorize]` nos 7 controllers (incluindo `Playable`); só `Account` é público; `PlayerOwnershipGuard` verifica posse de personagem/runtime/hunt session/encounter (outra conta → 403); 401/403/200 comprovados na API real. **Decisão D1 do Arquiteto:** a rota `api/inventory/{characterId}` foi **mantida** com verificação de dono, em vez de removida — a conta tem vários personagens e não há "personagem selecionado" no servidor; remover quebraria o cliente (Regra Aditiva). — Escopo original: `[Authorize]` em Hunt, HuntSession, Inventory, Runtime, Gameplay, Location; `characterId` derivado de `ICurrentPlayerContext` com verificação de dono; remover `characterId` livre da rota do Inventory (mantendo o contrato de resposta) | `PRINCIPLES`, Handbook §23 |
| `PV-PLATFORM-003` Dependências vulneráveis | Atualizar o pacote `SQLitePCLRaw` (`SQLitePCLRaw.lib.e_sqlite3` 2.1.11 → versão corrigida) para eliminar todos os avisos **NU1903** (vulnerabilidade alta) — 6 no build atual: 4 do baseline PV-GOV-002 + 2 do `SmokeTests` após `PV-QA-001` parte 1; build continua verde. **Atenção:** o pacote é dependência **transitiva** de `Microsoft.EntityFrameworkCore.Sqlite` 10.0.0 (`ProjectVictory.Persistence.csproj`), sem `PackageReference` direto — o contrato decide entre fixar a versão transitiva ou subir o EF Core. *Separado do `PV-QA-001` por ser dependência de plataforma, não teste (decisão do Arquiteto em PV-GOV-002, ADR-016: roadmap e organização cabem ao Arquiteto)* | Baseline PV-GOV-002 (`project-state.md`, Pendências de Validação) |
| `PV-PLATFORM-004` Schema fora de Development | Definir e implementar como o schema é criado/atualizado fora de Development (ex.: `dotnet ef database update`/bundle de migrations no deploy × `MigrateAsync()` controlado por configuração); hoje um banco vazio em Production falha no seed. Sem alterar migrations existentes | Pendência herdada do `PV-PLATFORM-001`, separada no `PV-PLATFORM-002`; Análise §4.4 |
| `PV-QA-001` Baseline de testes | ✅ **Parte 1 concluída em 2026-11-05** (branch `chief/m1-qa-001`): SmokeTests compila, está no `.sln` e o `dotnet test` não tem falhas; 1 teste com Skip aprovado pelo dono até o `PV-REWARD-010` (ver `project-state.md`). **Pendente (parte 2):** testes de Domain; teste de autorização por endpoint (HTTP, `WebApplicationFactory`: 401 sem token, 403 com outra conta, incluindo o mapeamento `OwnershipViolationException` → 403 do `PV-SEC-001`); **testes de `JwtSettings.Validate`** (chave ausente, chave < 32 caracteres, Issuer/Audience vazios, mensagem em Development × fora dele — ressalva R1 da revisão do `PV-PLATFORM-002`; hoje não há projeto de teste da API, o contrato decide onde criar). — Escopo original: **corrigir a compilação do `SmokeTests`** (CS1503 em `RewardInventoryIntegrationTest.cs` linha 70: `Assert.Contains` de `string` contra `Inventory.Items`, coleção de `ItemInstance` — ajustar a asserção, não o domínio); incluir `SmokeTests` no `.sln`; testes de Domain para entidades/VOs existentes; teste de autorização por endpoint | `TESTING-STRATEGY`, Análise §4.5 |
| `PV-CLEAN-001` Quarentena | Após build verde, remover `Application/Architecture|Playtest|Production` (R-007) | Regra Aditiva (exceção: decisão + contrato + validação) |

**Saída:** clone limpo → `dotnet build` e `dotnet test ProjectVictory.sln` verdes; API sobe com schema; nenhum endpoint de gameplay anônimo; fluxo atual do Vertical Slice intacto.

## M2 — Conformidade com a arquitetura LOCKED *(corrigir divergências; sem novos sistemas)*

| Contrato | Conteúdo | Lastro |
|---|---|---|
| `PV-PROG-010` Level derivado | Remover `Level` persistido; Level calculado de Experience + Ascension + Experience Table; migration | ADR-023, 027, 028 (R-002) |
| `PV-PROG-011` Experience Table | Substituir `ProgressionProcessor` por intérprete de Experience Table (dados, não `if`s); sem teto fixo em 5 | ADR-025 (R-003) |
| `PV-REWARD-010` Reward Pipeline real | `RewardProcessor` deixa de ser stub: XP e ouro vêm da definição de monstro; Loot via Loot Tables; Reward Pipeline é o único que muda Experience; **`ApplyRewardHandler` passa a aplicar drops e ouro ao personagem** (hoje só aplica XP — lacuna encontrada em `PV-QA-001` parte 1); aceite inclui remover o Skip de `ApplyReward_Should_Add_Item_To_Character_Inventory` e ligar a asserção ao `ItemDefinitionId` do drop (hoje estrutural: um item com Ids não vazios) | ADR-008, 024, 037, 038 |
| `PV-STATE-001` Posse de estado | Cada store em memória classificado (temporário × semi-permanente × permanente); corrigir `HuntRepository`/`CombatContextRepository` (thread-safety) e `CombatResultStore` (escopo) | Handbook §8, `STATE-OWNERSHIP-MODEL` |

**Saída:** checklist de conformidade ADR-021…038 completo para o código tocado; testes de regressão do loop passando.

## M3 — Combate real *(Fase 5: Combat Architecture LOCKED → implementação)*

| Contrato | Conteúdo | Lastro |
|---|---|---|
| `PV-COMBAT-010` Combat Orchestrator | Orquestrador coordena turnos; **não** implementa dano, skills, IA, loot ou XP | ADR-051 |
| `PV-COMBAT-011` Turnos | Combate por turnos, exatamente 1 participante ativo por turno, determinístico | ADR-052 |
| `PV-COMBAT-012` Regras de dano e ação | Dano do atacante e do inimigo; ação básica + secundária; vida do jogador; resultado (vitória/derrota) substitui `CombatResolver` stub atrás de `ICombatResolver` | ADR-006; Snapshot "Próximos Objetivos" |
| `PV-HUNT-010` Derrota | Morte encerra a Hunt pelo **mesmo** pipeline de encerramento; jogador volta à cidade e aguarda comando; **nenhuma perda de loot ou XP** | ADR-012; `DEC-20261106-04` |
| `PV-SUPPLY-010` Suprimentos (poções) | Só **poção de vida** e **poção de mana**; uso configurado pelo jogador em tela própria, inclusive durante a Hunt; servidor valida e aplica **a partir do próximo turno**; **sem suprimento não há cura**; **fim de suprimento encerra a Hunt** pelo mesmo pipeline | `DEC-20261106-02/03/04`; ADR-012; ADR-052 |
| `PV-CONTENT-011` Spider Cave | Primeira Hunt: **Spider Cave** com 5 monstros — **Rat** e **Cave Rat** (já existem em `EncounterGenerator.cs`) + **Spider**, **Poison Spider** e **Giant Spider** (novos). Atributos entregues como **BALANCEAMENTO PROVISÓRIO** para o PO ajustar; definições por dados (base para o `PV-MONSTER-010`) | `DEC-20261106-09`; ADR-005/006 |

**Saída:** combate com resultado não trivial, determinístico e coberto por testes de Domain/Application; cliente continua consumindo o mesmo contrato de resultado.

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

## M6 — Runtime contínuo e Hunt Loop *(depende de ADR-057 — bloqueado pela pergunta aberta do PO)*

> **Bloqueio:** o PO escolheu B3 (Hunt continua offline) **sem ganho offline de XP**, mas ainda não respondeu o que a Hunt rende offline: a) loot e lucro sem XP; b) Hunt termina ao desconectar; c) só treino offline de skills (`DEC-20261106-05`). Nenhum contrato do M6 é emitido antes da resposta e de ADR-057 ACCEPTED. M1–M5 seguem normalmente.

| Contrato | Conteúdo | Lastro |
|---|---|---|
| `PV-RUNTIME-010` Game Session + Runtime Coordinator | Coordenação de experiência contínua sem inflar handlers | Handbook §9–10; `KNOWN-GAPS` |
| `PV-HUNT-011` Hunt Loop contínuo | Hunt Definition → Session → Encounter Generator → Combat → Reward → Progression → novo loop; ciclo Starting→Active→Ending→Finished | Handbook §11; ADR-012, 045, 046 |
| `PV-RUNTIME-011` Recovery | Servidor inicia → carrega estado → reconstrói contexto → continua | Handbook §18; ADR-057; `PV-RUNTIME-002-A` |
| `PV-PROG-012` Offline progression | Aplicar progresso ocioso ao reconectar, pelo Reward Pipeline | Roadmap Fase 2 (Offline Training LOCKED) |

**Saída:** o loop roda sem cliques repetidos e sobrevive a logout/reinício conforme ADR-057.

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
| `PV-UI-0xx` Navegação | Router real no client no lugar de `useState` por string; tipar `any`; estados de loading/erro | Análise §4.5; contratos `PV-UI-001/002` (histórico git: "Page Composition Rules") |
| `PV-QA-010` Playtest | Roteiro de playtest, matriz de regressão, critérios de aceite | `ACCEPTANCE_CRITERIA.md`, `PV-VS-2.0-045` |
| `PV-CONTENT-010` Conteúdo mínimo | Monstros, itens e loot suficientes para ~30 min de jogo | `FIRST_FUN.md`, `PROJECT-VICTORY-CONTENT-PIPELINE-ARCHITECTURE.md` |
| `PV-HUD-ANALYZER-010` Hunt Analyzer | Aba "Analyzer" do Game HUD: XP obtido, XP/hora, débito, lucro e lucro líquido da Hunt, em tempo real, **calculados no servidor** e só exibidos pelo cliente | `DEC-20261106-01` (PO); ADR-002; `PV-FRONTEND-GAME-HUD-FOUNDATION-001` |
| `PV-HUD-BATTLE-VIEW-010` Representação visual | Animação central por local (ex.: Spider Cave), duelo automático decorativo, **sem efeito no resultado** | `DEC-20261106-01` (PO); ADR-002 |

### Dependências do Hunt Analyzer (`DEC-20261106-01`)

O Analyzer é **entregue em M8 (UI)** e **não bloqueia M1 a M4**. Ele consome dados que precisam existir antes, entregues pelos marcos que já os tocam:

| Dependência | Onde entra | Observação |
|---|---|---|
| **Preços de compra e venda NPC nas definições de item** (dado, não comércio) | `PV-CONTENT-010` (M8), sobre as definições de item; o campo pode ser acrescentado já no `PV-REWARD-010` (M2) quando o Reward passar a usar definições reais | Só o **dado** de preço. Comércio com NPC continua na Fase 7/8 (`roadmap.md`) — Guardião de Escopo |
| **Modelo de suprimentos (poções)** — o que é gasto e quando | `PV-SUPPLY-010` (M3) | **Decidido pelo PO em 2026-11-06:** só poção de vida e de mana, uso configurável, aplicado no próximo turno (`DEC-20261106-02/03`). O débito do Analyzer = poções gastas × preço de compra no NPC |
| **Valor do loot no lucro** | `PV-HUD-ANALYZER-010` (M8) | **Preço de venda ao NPC — PROVISÓRIO** (recomendação do Arquiteto, aguardando confirmação do PO: `DEC-20261106-08`) |
| **Medição de tempo da Hunt** (início, pausas, fim) para XP/hora | `PV-HUNT-011` (M6, Hunt Loop contínuo) | Depende da política de desconexão (`ADR-057`, decisão do PO): o tempo offline conta ou não para XP/hora |
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
