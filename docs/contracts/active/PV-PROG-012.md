# PV-PROG-012 — Level sem teto e Ascension derivada do Level

Status: **IMPLEMENTADO** (branch `chief/q4-prog-012`)
Emitido por: Arquitetura, 2026-11-06 (ordem "MODO FILA 4", item 3).
Lastro:
- `DEC-20261106-19` (Ascension derivada do Level; Level sem teto; máximo e passo em configuração; conteúdo por dados) — supera o texto anterior do `ADR-026` e do `PV-PROG-011`;
- `DEC-20261106-10` (curva do Tibia);
- ADR-023, 025, 027 e 028.

## Escopo implementado

1. **`ExperienceCurve.LevelFor(experience)`**: o Level sai **da fórmula** (busca binária na curva do Tibia), **sem tabela fixa**.
   - `MaximumSupportedLevel` = **569.256**, o maior Level cuja Experience cabe em `long`.
   - Estouro → `OverflowException`, nunca valor errado.
2. **`ExperienceTable.Standard`** é a **curva sem teto** (`IsUnbounded`):
   - `GetLevel`, `GetMinimumExperience`, `GetMaximumExperience` e `GetExperienceToNextLevel` usam a fórmula;
   - as tabelas de **entradas explícitas** continuam (construtores e `FromCurve` mantidos);
   - o **limite de 500 entradas por tabela foi removido** (era o texto antigo do ADR-026);
   - "último nível válido" (`experience-table.md`) passa a ser o último da lista, numa tabela explícita, ou o maior Level representável, na curva.
3. **`AscensionRules`** (Domain, dados): `AscensionFor(level)` = mín(1 + Level / passo, máximo).
   - Com o padrão (passo 500, máximo 4): 1 até o Level 499; 2 no 500; 3 no 1000; 4 no 1500 em diante.
   - `UnlockLevel(ascension)` dá o Level em que cada Ascension é desbloqueada.
   - **Configuração do servidor:** `Ascension:LevelStep` e `Ascension:Maximum` (`appsettings.json`: 500 e 4), registrada no `AddProjectVictoryCore`.
4. **`Character`:**
   - `Level` vem da curva sem teto e **nunca volta a 1**;
   - `Ascension` é **derivada** do Level (regras padrão);
   - `AscensionUnder(rules)` aplica as regras configuradas.

   A Ascension não é persistida (é derivada).
5. **Conteúdo por Ascension (dados):**
   - `HuntAreaDefinition.RequiredAscension` (padrão 1);
   - `IHuntAreaCatalog.GetAll()` e `GetAvailable(ascension)`: áreas com Ascension mínima ≤ a do jogador, a nova e as antigas;
   - a Spider Cave é conteúdo da Ascension 1.

   **Escolher a área** pelo jogador entra junto com a "última Hunt escolhida" do M6 (`DEC-20261106-20`).
6. **Vida e Mana** seguem crescendo com o Level depois do 500 (curva do `PV-CHAR-011`, com `checked`).

## Testes substituídos (comportamento superado pelo `DEC-20261106-19`)

| Teste antigo | Novo |
|---|---|
| `ExperienceTableTests.Standard_HasExplicitEntriesForLevels1To500` | `Standard_IsTheUnboundedCurve_ExplicitTablesStillAvailable` |
| `ExperienceTableTests.GetLevel_NeverPassesLevel500_ExperienceKeepsCounting` | `GetLevel_HasNoCap_FollowsTheCurveAfter500` (+ `Curve_OverflowIsDetected_NeverWrong`) |
| `ExperienceTableTests.GetExperienceToNextLevel_ReturnsRemaining_AndNullAtLastLevel` | `…_AndNullOnlyAtLastRepresentableLevel` |
| `ExperienceTableReviewTests.MaximumExperience_IsDerived_AndOpenAtLastLevel` | `…_AndOpenOnlyAtLastRepresentableLevel` |
| `ExperienceTableReviewTests.Table_CannotExceedAscensionLimit` | `ExplicitTable_CanExceed500_ButNotTheSupportedLevel` |
| `ExperienceTableReviewTests.Entries_CannotBeMutatedByCast` | Mesmo teste, sobre uma tabela explícita (`FromCurve(500)`) |
| `DerivedLevelTests.Experience_AboveIntRange_IsSupported_AndLevelStopsAt500` | `…_AndLevelHasNoCap` (Level 575, Ascension 2) |
| `CharacterExperiencePersistenceTests.ExperienceAboveIntRange_RoundTrips_AndLevelIsDerived` | Level esperado de 500 → **566** (3 bilhões de XP) |

## Testes novos

- `Domain.Tests/AscensionTests.cs`:
  - limites 499/500/999/1000/1499/1500/2000/500.000;
  - regras configuráveis (máximo 6, passo 100);
  - valores inválidos;
  - personagem no Level 1000 = Ascension 3, com vida 15 × 1000 + 65;
  - ressalvas do `PV-CHAR-011`: fórmula do Tibia para Paladin, Druid e Sorcerer em Level alto; estouro e `VocationLevel` inválido; corte da vida atual no novo máximo.
- `SmokeTests/AscensionContentTests.cs`: Spider Cave = Ascension 1; uma área de Ascension 2 só aparece a partir da Ascension 2, junto com as antigas.

Resultado da solução: build com 0 avisos; **279 aprovados, 0 ignorados, 0 falhas**.

## PROVISÓRIO / pendências

- **R-034** (PO): "5 Ascensions / Legend" em `knowledge-base/character.md` × máximo 4.
- Conteúdo das Ascensions 2 a 4: ainda não existe (só a Spider Cave, Ascension 1).
