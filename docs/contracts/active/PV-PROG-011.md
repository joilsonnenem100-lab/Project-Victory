# PV-PROG-011 — Experience Table gerada pela curva

Status: **IMPLEMENTADO** (branch `chief/m2-prog-011`)
Emitido por: Arquitetura (sessão Chefe), 2026-11-06, conforme `roadmap-to-playable.md` (M2) e `PV-GOV-006`.
Lastro: ADR-025 (Experience Table interpreta a Experience), ADR-026 (Level 500 por Ascension), `DEC-20261106-10/11`, `developer-guide/domain/experience-table.md`. Resolve **R-003**.

## Problema

`ProgressionProcessor` resolvia o Level com `if`s fixos e teto 5; `LevelCalculator` usava outra regra (`Experience/100 + 1`); `GrowthProcessor` ignorava a tabela recebida; a única `ExperienceTable` existente tinha um Level só.

## Escopo implementado

1. `Domain/Progression/ExperienceCurve` — `XP(L) = 50/3 × (L³ − 6L² + 17L − 12)` em `long`, divisão exata (testada para 1..500).
2. `Domain/Progression/ExperienceTable` (classe existente, estendida):
   - entradas explícitas `ExperienceTableEntry(Level, MinimumExperience)`, validadas na construção: começa no Level 1 com 0, níveis contínuos, Experience estritamente crescente;
   - `Standard` = Levels 1–500 gerados pela curva; a entrada do Level 500 tem máximo **aberto** (a Experience continua acumulando — ADR-023);
   - `GetLevel(long)`, `GetMinimumExperience(int)`, `GetExperienceToNextLevel(long)` (null no último Level);
   - `ForAscension(int)` devolve a mesma tabela para toda Ascension (`DEC-20261106-11`);
   - construtor por dicionário e `GetRequiredExperience(int)` mantidos (Regra Aditiva); o construtor antigo agora também é validado.
3. `ProgressionProcessor` (atrás do mesmo `IProgressionProcessor`), `LevelCalculator` e `GrowthProcessor` passam a interpretar a tabela; `ProcessExperienceRewardHandler` usa `ExperienceTable.Standard` no lugar da tabela de um Level só.

## Mudança de comportamento (aprovada pelo PO no `DEC-20261106-10`)

Os limiares de Level mudam: antes 100/300/600/1000 com teto 5; agora 100/200/400/800/1500… com teto 500. Nenhum desses componentes é chamado pelos endpoints atuais (o fluxo real do ataque não aplica Level); nenhum contrato HTTP muda.

## Fora do escopo

Level derivado no `Character` e Experience `long` (`PV-PROG-010`); XP rate (`PV-REWARD-010`); troca de Ascension (`ADR-026`, em aberto).

## Testes

`tests/ProjectVictory.Domain.Tests/ExperienceTableTests.cs` (curva, divisão exata, 500 entradas, interpretação, teto, validação, construtor antigo, `LevelCalculator`) e `tests/ProjectVictory.SmokeTests/ProgressionProcessorTests.cs`. Solução: build com 0 avisos; 122 testes aprovados, 1 ignorado (Skip do `PV-REWARD-010`), 0 falhas.
