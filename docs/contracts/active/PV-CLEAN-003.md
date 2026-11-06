# PV-CLEAN-003 — Remover os 5 handlers mortos do R-031

- **Status:** Implementado (Fila 4, branch `chief/q4-clean-003`, commit isolado `1a2889b`)
- **Origem:** R-031 (revisão do `PV-HUNT-010`: handlers que mudariam o estado da Hunt fora do pipeline único do ADR-012); autorização do PO no `DEC-20261106-25`; ordem "MODO FILA 4 - VERSÃO FINAL", item 6.

## Verificação antes da remoção

Grep de cada nome em `src` e `tests` (`*.cs`, `*.csproj`), fora do próprio arquivo: **zero referências** para os 5. Não há registro de handler por reflexão (busca por `GetTypes()`, `Scan(`, `EndsWith("Handler")`: nada). Nenhum handler teve de ser mantido.

## Removidos (`git rm`, listados no commit)

`CancelHuntHandler`, `ExecuteExistingHuntHandler`, `ExecuteNextHuntHandler`, `ProcessHuntExecutionHandler`, `ResolveHuntResultHandler` (`src/ProjectVictory.Application/Hunt/`).

## Não removido

Os Commands/Results desses handlers ficaram órfãos, mas a autorização nomeia só os handlers → **R-038** (decisão do PO).

## Resultado

Build com 0 avisos; **295 aprovados, 0 ignorados, 0 falhas**. Registro no CLEANUP-MANIFEST. R-031 resolvido. Reverter: `git revert 1a2889b`.
