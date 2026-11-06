# PV-CLEAN-001 — Remoção do código em quarentena (R-007)

Status: **CONCLUÍDO** — revisado pelo `pv-architecture-reviewer` (APROVADO COM RESSALVAS). Branch `chief/m1-clean-001`, a última da Fila 2
Autorização: **Product Owner, 2026-11-06** — "Remova com git rm os 42 arquivos de src/ProjectVictory.Application/Architecture, Playtest e Production, em UM commit separado e fácil de reverter."
Lastro: R-007 (`project-state.md`); `DEC-20261105-04` (remoção só após build verde, em contrato de limpeza); Regra Aditiva (exceção com decisão + contrato + validação de impacto).

## Verificação antes da remoção

`git grep` em todo o repositório, **fora** das três pastas:

| Busca | Resultado |
|---|---|
| Namespaces `ProjectVictory.Application.Architecture`, `.Playtest` e `.Production` (`using`, nome qualificado) | **0** |
| Os 42 nomes de tipo (palavra inteira): DI, controllers, testes, outros handlers | **0** |
| Caminhos `Application/Architecture`, `Playtest` e `Production` em `.csproj` e código | **0** |
| Caminhos em documentos | 5 citações de **planejamento** da própria remoção (`decision-log.md`, `project-state.md`, `roadmap-to-playable.md`, `PV-ANALYSIS-2026-11-05.md`, `RELATORIO-NOITE-2026-11-06.md`). Nenhuma depende dos arquivos |

Nenhum arquivo estava referenciado, então **todos os 42 foram removidos**.

## Remoção

- Commit **isolado** `38d1be7`, só com `git rm` das três pastas: 42 arquivos, 504 linhas.
  - Architecture: 9 arquivos.
  - Playtest: 24 arquivos.
  - Production: 9 arquivos.
- Para recuperar: `git revert 38d1be7` (tudo) ou `git checkout 38d1be7~1 -- <caminho>` (um arquivo).
- Lista completa dos caminhos: nova seção datada do `docs/architecture/CLEANUP-MANIFEST-2026-11-05.md`.

## Validação

- Antes da remoção, em `38d1be7~1` = `9c91392` (último commit do `PV-CI-001`): build com 0 avisos; **173 aprovados, 0 ignorados, 0 falhas**.
- Depois da remoção: build com **0 avisos e 0 erros**; **173 aprovados, 0 ignorados, 0 falhas**. O número de testes é o mesmo.

## Documentação

- `CLEANUP-MANIFEST` (seção nova);
- `decision-log.md` (nota na `DEC-20261105-04`);
- `project-state.md` (R-007 resolvida);
- `roadmap-to-playable.md` (M1).

Neste branch também entraram as ressalvas da revisão do `PV-PLATFORM-005` (precisão das versões e R-019).

## Revisão (`pv-architecture-reviewer`, 2026-11-06): APROVADO COM RESSALVAS

O revisor refez, de forma independente, o `git grep` no estado anterior (0 referências) e rodou build e testes antes e depois: 173 = 173. Ressalvas:
1. **`src/ProjectVictory.Domain/Playtest/`** (3 arquivos) ficou sem nenhuma referência, porque só os handlers removidos a usavam. **Não foi removida**: está fora da autorização do PO. Registrada como **R-020**.
2. Hash do estado "antes" (`9c91392`) citado acima.
3. Existem outras citações de R-007/`PV-CLEAN-001` em documentos (ex.: o relatório da Fila 2). Não mudam o resultado.
4. O branch está no `origin` (`60488c9`); o merge é do PO.
