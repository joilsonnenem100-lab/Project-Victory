# PV-PROG-010 — Level derivado e Experience `long`

Status: **IMPLEMENTADO** (branch `chief/m2-prog-010`)
Emitido por: Arquitetura (sessão Chefe), 2026-11-06, conforme `roadmap-to-playable.md` (M2) e `PV-GOV-006`.
Lastro: ADR-023 (Experience absoluta), ADR-027 (Level derivado), ADR-028 (estado permanente × derivado), ADR-026 (Ascension), `DEC-20261106-10`, `developer-guide/domain/level.md`. Resolve **R-002**. Depende do `PV-PROG-011`.

## Escopo implementado

1. **Experience `long`:** `Domain/ValueObjects/Experience.Value` e `PersistentCharacterState.Experience` passam a `long`; `GainExperience` soma com `checked`. DTOs `GetCharactersResult.Experience` e `EnterGameResult.Experience` passam a `long` (JSON continua número; o cliente não muda).
2. **Level derivado:** `Character.Level` é calculado de `Experience` pela `ExperienceTable.ForAscension(Ascension)`; o agregado não guarda Level. `Character.Ascension` = 1 (primeira), não persistida (`PV-GOV-006` D4).
3. **Construtores do `Character`** perdem o parâmetro `level` (os 2 chamadores — `CreateCharacterHandler` e `CharacterRepository` — foram ajustados). Lastro: ADR-027 + este contrato.
4. **`ApplyLevel`** não define mais o Level: só aceita o Level atual e lança `InvalidOperationException` para qualquer outro. O crescimento de Vida/Mana máximas que ele fazia **sai** (nenhum endpoint o chamava; vira atributo derivado, ADR-029 — registrado em `project-state.md`).
5. **Persistência:** coluna `Level` **mantida** como cache, gravada com o valor derivado e **nunca lida** (`PV-GOV-006` D2). Migration **nova** `CharacterExperienceLong`: no SQLite `INTEGER` já é 64 bits, então o `Up/Down` é vazio e só o snapshot muda (`int` → `long`); nenhuma migration existente foi editada; `has-pending-model-changes` = sem mudanças.

## Ressalvas do `PV-PROG-011` aplicadas aqui

`ExperienceTable.FirstAscension`; `LevelCalculator`, `ProgressionProcessor` e `ProcessExperienceRewardHandler` selecionam a tabela por `ForAscension` (não `Standard` direto); validação do teto de 500 Levels por tabela; `GetMaximumExperience` (máxima derivada, aberta no último Level); `Entries` somente leitura; comentário e teste do `OverflowException` de `GetRequiredExperience`.

## Testes

`Domain.Tests/DerivedLevelTests.cs` (Level segue a Experience, Experience acima de `int`, teto 500, `ApplyLevel`); `SmokeTests/CharacterExperiencePersistenceTests.cs` (SQLite real com todas as migrations: 3 bilhões de XP fazem ida e volta; cache `Level` divergente é ignorado na leitura); `Domain.Tests/ExperienceTableTests.cs` (ressalvas). Solução: build com 0 avisos; **134 aprovados, 1 ignorado, 0 falhas**.

## Fora do escopo

Apagar a coluna `Level` (destrutivo; decisão própria); troca de Ascension (`ADR-026`, em aberto); Vida/Mana derivadas por Level (ADR-029).
