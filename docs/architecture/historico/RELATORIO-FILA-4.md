# Relatório — MODO FILA 4 (versão final), 2026-11-06

Ordem do PO: "MODO FILA 4 - VERSÃO FINAL". Branches empilhadas a partir da `main` (`5f2c3ab`, que contém a Fila 3), trabalho no worktree `C:/pv4`, push de cada branch, sem `push --force`, `main` intocada, nenhuma migration editada, nenhum segredo escrito.

## Branches (ordem de empilhamento — fazer o merge da última traz todas)

| # | Branch | Commit final | Conteúdo |
|---|---|---|---|
| 1 | `chief/q4-gov-009` | `1f8031f` | PV-GOV-009 (só `.md`) |
| 2 | `chief/q4-char-011` | `98e4b91` | PV-CHAR-011 + ressalvas do GOV-009 |
| 3 | `chief/q4-prog-012` | `dcb017c` | PV-PROG-012 |
| 4 | `chief/q4-supply-011` | `82e90c9` | PV-SUPPLY-011 |
| 5 | `chief/q4-combat-013` | `4ba16ba` | PV-COMBAT-013 + ressalvas do PROG-012 e SUPPLY-011 |
| 6 | `chief/q4-clean-002` | `a68d10d` | PV-CLEAN-002 (remoção isolada em `902fa1a`) |
| 7 | `chief/q4-clean-003` | `05abbbd` | PV-CLEAN-003 (remoção isolada em `1a2889b`) |
| 8 | `chief/q4-combat-013b` | (última; contém este relatório) | Correção da revisão REPROVADA do PV-COMBAT-013 + este relatório |

## Status por contrato

| Contrato | Status | Revisão |
|---|---|---|
| PV-GOV-009 — decisões a–k do PO (`DEC-20261106-15…25`) | ✅ | APROVADO COM RESSALVAS (aplicadas no CHAR-011) |
| PV-CHAR-011 — Vida/Mana pela tabela do Tibia | ✅ | APROVADO COM RESSALVAS (aplicadas/registradas) |
| PV-PROG-012 — Level sem teto; Ascension derivada (passo e máximo em configuração) | ✅ | APROVADO COM RESSALVAS (aplicadas no COMBAT-013; R-037) |
| PV-SUPPLY-011 — fim de suprimento não encerra; morte → "Victoria" com aviso | ✅ | APROVADO COM RESSALVAS (aplicadas no COMBAT-013; R-036) |
| PV-COMBAT-013 — 4 skills por classe em dados, seletor, dano extensível | ✅ (após correção) | **REPROVADO** (B1, B2) → corrigido em `chief/q4-combat-013b`; ver abaixo |
| PV-CLEAN-002 — remover `Domain/Playtest/` | ✅ | ver abaixo |
| PV-CLEAN-003 — remover os 5 handlers do R-031 | ✅ | ver abaixo |

Nada travou nem foi pulado.

## Build e testes

Todos executados no worktree, contrato a contrato (`dotnet build ProjectVictory.sln` + `dotnet test ProjectVictory.sln`):

| Momento | Build | Testes |
|---|---|---|
| Início (main, Fila 3) | 0 avisos | 256 |
| PV-CHAR-011 | 0 avisos | 259 |
| PV-PROG-012 | 0 avisos | 279 (8 testes substituídos, lista no contrato) |
| PV-SUPPLY-011 | 0 avisos | 279 (5 testes alterados, lista abaixo) |
| PV-COMBAT-013 | 0 avisos | 295 |
| PV-CLEAN-002 / PV-CLEAN-003 | 0 avisos | 295 / 295 |
| **Final** (correção do COMBAT-013 + ressalvas da revisão final) | **0 avisos, 0 erros** | **298 aprovados, 0 ignorados, 0 falhas** (Application 23, Domain 108, Runtime 17, Smoke 66, Api 84) |

## O que mudou no jogo

- **Vida e Mana do Tibia:** Level 1 = 150/55 para todos; +5/+5 até o Level 8; depois Knight +15/+5, Paladin +10/+15, Sorcerer e Druid +5/+30, Monk +12/+15.
- **Level sem teto:** a curva de XP do Tibia é calculada, sem tabela fixa (limite técnico: Level 821.009, o que cabe em `long`).
- **Ascension derivada do Level:** 1 + Level/500, máximo 4 (`Ascension:LevelStep` e `Ascension:Maximum` no `appsettings.json`). As Hunt areas têm `RequiredAscension`; a Spider Cave é Ascension 1.
- **Acabar o suprimento não encerra mais a Hunt.** Na morte, o jogador volta para **"Victoria"**, e a resposta traz `returnCity` e `deathNotice` (`reasonCode = "Death"`, `checks = ["Potions","Runes","HuntLevel"]`). O texto e a tela ficam com o cliente (M8).
- **Skills:** 4 por classe (2 em área, 2 em alvo único), desbloqueadas por Level. A skill em área atinge todos os inimigos vivos e gasta a mana uma vez. O seletor automático escolhe área com 2 ou mais inimigos e alvo único com 1. O ataque básico tem multiplicador por classe.
  - **Efeito no balanceamento (ressalva 1 da revisão):** o jogador causa mais dano do que na Fila 3. O balanceamento da Spider Cave precisa ser revisto.
- **Remoções autorizadas (`DEC-20261106-25`):** `Domain/Playtest/` (3 arquivos) e os 5 handlers mortos. Cada remoção está em um commit isolado (`git revert` desfaz) e registrada no `CLEANUP-MANIFEST-2026-11-05.md`.

## Testes alterados pelo PV-SUPPLY-011 (comportamento antigo → `DEC-20261106-22`)

| Teste antigo | Situação |
|---|---|
| `SmokeTests/SupplyTests.Potions_AreConsumedFromInventory_AndOutOfSuppliesEndsHuntThroughPipeline` | **Substituído** por `Potions_AreConsumedFromInventory_AndRunningOutDoesNotEndTheHunt` |
| `SmokeTests/SupplyTests.OutOfSupplies_WithPolicyAndNoPotions_EndsAfterFirstRound` | **Substituído** por `PolicyWithoutPotions_DoesNotEndTheHunt` (na revisão: exige `HuntEndReason` nulo, combate não finalizado, estado `Active`) |
| `Api.Tests/SpiderCaveEndToEndTests.Potion_PolicyAppliesFromNextTurn_ConsumesSupplies` | Desfecho esperado mudou de `OutOfSupplies` para **`Victory`** (3 poções usadas) |
| `Api.Tests/SpiderCaveEndToEndTests.OutOfSupplies_EndsHuntThroughSamePipeline_AndReturnsToCity` | **Substituído** por `NoSupplies_DoesNotEndTheHunt` (com as mesmas checagens fortes) |
| `Api.Tests/SpiderCaveEndToEndTests.Defeat_…` | **Ampliado**: confere `returnCity = "Victoria"` e o `deathNotice` |

Teste novo (revisão): `SmokeTests/SupplyTests.EndHunt_DeathNoticeOnlyOnDeath_AlwaysReturnsToVictoria`.

Outros testes alterados na fila:
- PV-PROG-012: 8 testes substituídos (tabela no contrato);
- PV-CHAR-011: valores de vida esperados (o E2E de derrota agora espera 150);
- PV-COMBAT-013: em `SupplyTests`, a asserção do seletor mudou de `ClassAbility` para `UseSkill`/`null`;
- revisão do PROG-012: o limite de Level esperado mudou de 569.256 para 821.009.

## Achados dos revisores

- **PV-GOV-009:** notas de superação nos documentos antigos; R-034 (5 Ascensions/Legend × máximo 4); logout no ADR-057 v1.2; Monk marcado como PROVISÓRIO. Tudo aplicado.
- **PV-CHAR-011:**
  - os valores ficam em tabela de dados no Domain (exige recompilar);
  - a vida de personagens antigos é cortada no novo máximo (teste);
  - testes de estouro;
  - R-035 (Monk).
- **PV-PROG-012:**
  - R-037: a configuração de Ascension só vale via `AscensionUnder`;
  - `AscensionRules` ficou só-leitura;
  - a conta divide antes de multiplicar (limite 821.009);
  - efeito colateral aceito: `GetRequiredExperience` acima de 500 não lança mais exceção;
  - comentário do `ProgressionProcessor` corrigido.
- **PV-SUPPLY-011:**
  - R-036: a vitória do encounter ainda encerra a Hunt; fica para o Hunt Loop do `PV-HUNT-011`, no M6;
  - a localização semeada passou a se chamar "Victoria";
  - testes mais fortes;
  - teste do `DeathNotice`;
  - nota de processo.
- **PV-COMBAT-013 — REPROVADO, corrigido:**
  - **B1:** com o resolver real, a skill em área parava no primeiro alvo quando a mana estava entre o custo e o dobro. Corrigido: a mana é conferida uma vez, antes de qualquer sorteio, e cobrada uma vez. Há teste com o resolver real e mana igual ao custo.
  - **B2:** a regra da área estava no orquestrador (ADR-051). Ela passou para `ICombatResolver.ResolveSkill`, em operação aditiva.
  - Ressalvas:
    - 1: balanceamento (acima);
    - 2: texto sobre `DamageType`;
    - 3: testes de integração movidos para o SmokeTests (o Application.Tests não referencia mais Infrastructure);
    - 4: R-039 (Magic Level ainda não entra na conta);
    - 5 e 6: comentários.
- **PV-CLEAN-002 / PV-CLEAN-003 e a correção do COMBAT-013: APROVADO COM RESSALVAS** (sem bloqueio).
  - O revisor refez o grep e confirmou: zero referências aos itens removidos, nenhum registro de handler por reflexão, cada remoção num commit isolado e reversível, manifesto correto.
  - B1 e B2 resolvidos; o determinismo foi conferido no código.
  - Ressalvas:
    1. faltava um teste direto do guard de mana do resolver — **acrescentado** (0 sorteios sem mana);
    2. o método padrão `ResolveSkill` devolve `NotExecuted` para resolvers que não o implementam — **registrado no contrato** (hoje só os fakes de teste);
    3. R-038 — pergunta 6 abaixo;
    4. BOM do csproj — sem ação.

## O que ficou PROVISÓRIO e onde trocar

| Item | Onde trocar |
|---|---|
| Nomes, multiplicadores, custos e Levels das 20 skills; multiplicador do ataque básico por classe | `src/ProjectVictory.Domain/Combat/Rules/ClassSkills.cs` (`ClassSkillTable`) |
| Política do seletor automático (área com 2 ou mais inimigos, senão alvo único, melhor skill que o mana paga) | `SkillSelectionPolicy` no mesmo arquivo (`MinimumEnemiesForArea`) |
| Tipo de dano (só `Physical`) e escala por Magic Level/Distance/Fist (ainda sem efeito, R-039) | `DamageType`/`SkillScaling` em `ClassSkills.cs`; fórmula em `DamageFormula.cs` |
| Fórmula de dano/defesa "Tibia-like" | `src/ProjectVictory.Domain/Combat/Rules/DamageFormula.cs` (`ProvisionalTibiaLikeDamageFormula`) |
| Valores do Monk do Level 1 ao 8 (R-035) | `src/ProjectVictory.Domain/Character/CharacterClassDefinition.cs` / `AttributeCurve.cs` (`AttributeCurveTable`) |
| Nome da cidade de retorno "Victoria" e as checagens do aviso de morte | `src/ProjectVictory.Application/World/HomeCity.cs` |
| Passo e máximo de Ascension (500 / 4) | `appsettings.json` → `Ascension:LevelStep`, `Ascension:Maximum` (valem via `AscensionUnder`; R-037) |
| Atributos/XP/drops da Spider Cave (já PROVISÓRIOS na Fila 3, agora com o jogador mais forte) | catálogo da Spider Cave (`SpiderCaveCatalog`) |

## Perguntas abertas ao PO

1. **R-034:** `character.md` fala em "5 Ascensions" e "Legend = Level 500 + Ascension 5"; o `DEC-20261106-19` fixou máximo 4, uma a cada 500 Levels. Qual vale? O que é "Legend" agora?
2. **R-035:** os valores do Monk antes do Level 8 (150/55, +5/+5) não estão na fonte usada. Confirma, ou quer outros?
3. **Skills:** aprova os nomes, os números e os Levels de desbloqueio da tabela provisória? Druid e Monk têm alguma regra própria (cura, suporte, combo) além do dano?
4. **Seletor automático:** área com 2 ou mais inimigos e alvo único com 1 está bom? O jogador vai poder configurar isso?
5. **R-036:** com o Hunt Loop contínuo (M6), a vitória num encounter gera o próximo automaticamente até morte, saída ou queda. Confirma?
6. **R-038:** autoriza remover os 9 Commands/Results órfãos dos handlers removidos?
7. **Balanceamento da Spider Cave**, agora com skills: refazer depois do M4 (Monster AI)?
8. **Treino (Free × Premium)** e demais pontos abertos do `DEC-20261106-21`.
9. **Pendentes de filas anteriores:**
   - fórmula oficial de dano/defesa e atributos iniciais;
   - quem começa o combate;
   - vitória só com todos os monstros mortos e recompensa por monstro;
   - cura e preço das poções;
   - como obter poções antes do comércio;
   - dados da Spider Cave;
   - R-011 (leitura "dono pelo token");
   - R-019 (install script do esbuild).

## Processos

Ao fim de cada contrato: `dotnet build-server shutdown` e checagem de que nenhum processo `dotnet` ou `ProjectVictory.API` ficou rodando.
