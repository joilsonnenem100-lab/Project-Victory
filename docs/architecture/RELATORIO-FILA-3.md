# Relatório da Fila 3 — M3 Combate real (2026-11-06)

Ordem: "MODO FILA 3" (Product Owner e Arquiteto-Chefe). Valeram as regras do modo noite e da Fila 2:
- branches empilhadas;
- push de cada branch;
- sem `push --force` e sem alterar a `main`;
- sem apagar arquivos não autorizados;
- migrations só novas e aditivas;
- sem segredos.

Regra para o que depende do PO: **não parar**. O que depende dele foi implementado com valor **PROVISÓRIO** atrás de interface ou de dados, marcado no código, no contrato e aqui.

**Base:** a ordem dizia para nascer da `main` se ela contivesse `chief/m1-clean-001`. `git merge-base --is-ancestor` mostrou que **não contém** (`origin/main` = `8b7caab`). Por isso a fila nasceu de **`chief/m1-clean-001`**, num worktree separado (`C:\pv3`).

**Resultado:** **9 contratos concluídos** e enviados ao `origin`, e **1 pulado por regra** (`PV-CLEAN-002`). Basta **um merge**: `chief/m3-qa-004` na `main`, porque contém todos os anteriores (Fila 2, M2 e esta fila).

## Contratos

| # | Contrato | Branch | Revisão | Testes ao fim |
|---|---|---|---|---|
| 1 | `PV-SEC-002` — token sem claim da conta → 401 (R-018) | `chief/m3-sec-002` | APROVADO COM RESSALVAS (aplicadas) | 179 |
| 2 | `PV-CHAR-010` — Vida/Mana máximas derivadas do Level (ADR-029) | `chief/m3-char-010` | APROVADO COM RESSALVAS (aplicadas) | 191 |
| 3 | `PV-COMBAT-010` — Combat Orchestrator (ADR-051) | `chief/m3-combat-010` | APROVADO COM RESSALVAS (aplicadas) | 196 |
| 4 | `PV-COMBAT-011` — turnos determinísticos, gerador com semente (ADR-052) | `chief/m3-combat-011` | APROVADO COM RESSALVAS (aplicadas) | 202 |
| 5 | `PV-COMBAT-012` — regras de dano e ação atrás de `ICombatResolver`/`IDamageFormula`; combate real no ataque | `chief/m3-combat-012` | APROVADO COM RESSALVAS (aplicadas) | 222 |
| 6 | `PV-HUNT-010` — derrota; pipeline único de encerramento (ADR-012) | `chief/m3-hunt-010` | APROVADO COM RESSALVAS (aplicadas) | 227 |
| 7 | `PV-SUPPLY-010` — poções (política do jogador a partir do próximo turno; fim de suprimento) | `chief/m3-supply-010` | APROVADO COM RESSALVAS (aplicadas) | 242 |
| 8 | `PV-CONTENT-011` — Spider Cave por dados + pendências do M2 | `chief/m3-content-011` | APROVADO COM RESSALVAS (aplicadas) | 251 |
| 9 | `PV-QA-004` — ponta a ponta pela API real | `chief/m3-qa-004` | APROVADO COM RESSALVAS (aplicadas) | **256** |
| 10 | `PV-CLEAN-002` — remover `Domain/Playtest` | — | **PULADO** | — |

Por que o `PV-CLEAN-002` foi pulado: a ordem exigia a linha "R-020: AUTORIZADO" abaixo da lista, e ela **não constava**. Nada foi apagado; o R-020 continua aberto.

**Estado final** (`chief/m3-qa-004`):
- `dotnet build ProjectVictory.sln`: **0 avisos, 0 erros**.
- `dotnet test ProjectVictory.sln`: **256 aprovados, 0 ignorados, 0 falhas** (Application 12, Domain 87, Runtime 17, Smoke 56, Api 84).
- Em todos os contratos de código, os revisores rodaram build e testes por conta própria.
- Não sobrou nenhum processo `dotnet` ao fim de cada contrato.

**Banco:**
- 1 migration **nova** e aditiva, `SpiderCaveItems` (coluna `Items.NpcBuyPrice` + 6 itens); nenhuma migration existente editada.
- `has-pending-model-changes` = sem mudanças.
- Validada numa **cópia** do banco local do PO: dados antigos preservados. O MD5 do original não mudou (`107170ad…`).

## O que mudou no jogo

1. **O ataque é um combate por turnos de verdade.** Cada `POST api/hunt/attack` executa o turno do jogador e o de cada monstro vivo, até a vez do jogador voltar.
   - Os monstros ferem o jogador.
   - Há Basic Attack mais ação secundária: a habilidade da classe, ou poção.
   - A resposta traz o log `turns`.
2. **Vitória** = todos os monstros do encounter mortos. Cada monstro morto rende XP, ouro e loot **na hora**, pelo Reward Pipeline.
3. **Morte, vitória e fim de suprimento** passam pelo **mesmo** `EndHuntHandler`:
   - o jogador volta à cidade e aguarda comando;
   - na morte, volta com HP e Mana cheios;
   - nada é perdido.
4. **Poções:** o jogador configura "usar abaixo de X%" pelo endpoint `POST api/hunt/supplies`, que vale a partir do próximo turno. As poções saem do inventário. Sem poção não há cura.
5. **Spider Cave:** 5 monstros e 5 composições de encounter sorteadas pela semente. O Cave Rat agora dá XP e loot; os itens estão na tabela `Items`; todo drop tem preço de venda.
6. **Vida e Mana máximas** crescem com o Level.
7. **Combate reproduzível:** a mesma semente gera o mesmo combate. `Combat:FixedSeed` serve só para testes e é ignorada fora de Development.
8. **Segurança:** um token sem a claim da conta recebe 401, e não 500.

## O que ficou PROVISÓRIO (e onde trocar)

| Item | Valor provisório | Onde trocar |
|---|---|---|
| Vida/Mana por classe e por Level (R-021) | Knight 185/55 +15/+5 etc. (código e documentos de Character) × `progression.md` (100 HP, +0.75…) | `CharacterClassDefinition` (dados que a `AttributeCurveTable` lê) |
| Fórmula de dano e defesa | `floor(0,085 × skill × arma + Level/5)`; armadura [arm/2, arm−1]; defesa [0, (def+Level/10)/2] | Registrar outra `IDamageFormula` (`AddProjectVictoryCore`) |
| Atributos de combate por classe | arma, armadura, defesa, custo e multiplicador da habilidade | `ClassCombatProfileTable` |
| Quem começa o combate | sempre o jogador | `SequentialTurnScheduler` / `CombatSession` |
| Política da ação secundária do jogador | usa a habilidade sempre que tem mana | `DefaultCombatActionSelector` |
| Alvo | primeiro monstro vivo; poção no próprio jogador | `CombatSession.DefaultTargetFor` / orquestrador |
| Cidade | "Starting Location" faz o papel de Cidade Principal | `EndHuntHandler.MainCity` |
| Poções | +75 HP / +50 Mana; compra 50, venda 10 | `ProvisionalSupplyEffects`, `SupplyCatalog`, migration nova |
| "Fim de suprimento" | uma poção que a política manda usar chegou a 0 | `CombatSupplyContext.IsOutOfSupplies` |
| Como obter poções | configuração do servidor `Supplies:Starter…` (padrão 0) | `SupplySettings` / `CreateCharacterHandler` |
| Monstros da Spider Cave | vida, Level, perfil de combate, XP, ouro e chances; itens novos (Cheese, Spider Fang, Spider Silk); preços de venda | `SpiderCaveCatalog` (+ migration nova para a tabela `Items`) |
| Composição dos encounters | 5 grupos, sorteados pela semente | `SpiderCaveCatalog.EncounterTemplates` |
| Mecânica do encounter | vitória com todos mortos; recompensa por monstro | `AttackEncounterHandler` |

## Principais achados dos revisores (aplicados ou registrados)

- **Calibração:** com a primeira calibração, o Knight era **imune** aos Rats (armadura 8 + defesa 10 contra dano máximo 4). O teste pela API mostrou isso e os valores provisórios foram reduzidos.
- **Bug antigo corrigido (R-023):** `playerMaximumHealth` na resposta do ataque recebia a Mana atual.
- **Conflito documento × documento (R-021):** `progression.md` × `docs/character e conceitos/`. O código segue os documentos de Character.
- **API sem tipo do Domain:** a resposta usa o DTO `CombatTurnDto`.
- **Semente fixa barrada fora de Development**, com teste.
- **Limite de 1000 turnos por rodada sinalizado** (`TurnLimitReached`), com teste.
- **Pendências novas registradas no `project-state.md`:**
  - **R-022:** Ascension × Vida/Mana derivadas.
  - **R-024:** loot sem a semente do combate.
  - **R-025:** DRAFT `combat-orchestrator.md` × implementação.
  - **R-026:** fluxo antigo `gameplay/action`, em que "o atacante vence".
  - **R-027:** Hunt Session × pipeline.
  - **R-028:** regras de ação na Infrastructure.
  - **R-029:** concorrência no mesmo encounter.
  - **R-030:** transição de atividade fora do Domain.
  - **R-031:** 5 handlers mortos que encerram a Hunt por fora.
  - **R-032:** dois tipos de definição de monstro.
  - **R-033:** sementes correlacionadas.
  - **R-015** atualizado: o consumo de poção mexe direto no inventário, aceito como débito.
- **Ainda não entregue:** a "tela própria" de poções do cliente fica para o `PV-UI-0xx` (M8). O servidor está pronto.

## O que travou

Nada travou. Apenas o `PV-CLEAN-002` foi **pulado por regra**, porque faltou a linha de autorização do R-020.

## Perguntas consolidadas ao PO

1. **R-021 — Vida e Mana por classe:** valem os números de `progression.md` (100 HP para todos, Knight +0.75/Level…) ou os dos documentos de Character e do jogo atual (Knight 185 HP, +15/Level…)? Se valer o `progression.md`, "+0.75" significa 0,75 HP por Level?
2. **Fórmula de dano e defesa** oficial (o próprio documento diz "pendente"), e os atributos de combate de cada classe (arma, armadura, defesa e habilidade iniciais).
3. **Quem começa o combate** e a política da ação secundária do jogador (hoje usa a habilidade sempre que pode).
4. **Mecânica do encounter:** vitória só com **todos** os monstros mortos e recompensa **por monstro** na hora. Confirma?
5. **Poções:** cura (+75 HP / +50 Mana) e preços (compra 50 / venda 10).
6. **"Fim de suprimento":** é "acabou uma poção configurada" (implementado; com 0 poções e a política ligada, a Hunt termina na 1ª rodada) ou outra regra?
7. **Como o jogador obtém poções** antes do comércio (kit inicial? drop?).
8. **Spider Cave:** atributos, XP, ouro e chances dos 5 monstros; itens de drop (nomes e quem dá); composição dos encounters.
9. **Cidade Principal:** qual é a cidade de retorno (hoje "Starting Location")?
10. **Ascension (R-022):** o que acontece com Vida/Mana máximas quando o Level volta a 1?
11. **R-020:** autoriza remover `src/ProjectVictory.Domain/Playtest/` (3 arquivos órfãos)? E os 5 handlers mortos do R-031?
12. **Pendentes de filas anteriores:**
    - R-011 (leitura "dono pelo token");
    - R-019 (install script do esbuild);
    - itens em aberto do ADR-026;
    - dúvidas do Offline Training;
    - janela de reconexão.
