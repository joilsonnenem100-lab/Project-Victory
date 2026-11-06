# PV-CI-001 — Integração contínua (GitHub Actions)

Status: **IMPLEMENTADO** (branch `chief/q2-ci-001`)
Emitido por: Arquitetura, 2026-11-06 (ordem "MODO FILA 2", item 7): ".github/workflows/ci.yml rodando dotnet build e dotnet test ProjectVictory.sln em push e pull request, com .NET 10 e o build do cliente. Se o push for rejeitado por falta de permissão para workflows, registre no relatório e NÃO tente contornar."
Lastro: Definition of Done (`CLAUDE.md`: build e testes verdes); `TESTING-STRATEGY`.

## Workflow `.github/workflows/ci.yml`

- **Gatilhos:** `push` (qualquer branch) e `pull_request`. `permissions: contents: read`. Execuções antigas da mesma ref são canceladas.
- **Job `dotnet`** (Ubuntu, .NET `10.0.x` via `actions/setup-dotnet@v4`), executado em ordem:
  1. `dotnet restore`;
  2. `dotnet build ProjectVictory.sln -c Release`;
  3. `dotnet test ProjectVictory.sln -c Release`;
  4. upload dos `.trx` como artefato.

  Nenhum segredo é necessário: o `Api.Tests` gera a própria chave JWT aleatória.
- **Job `client`** (Ubuntu, Node 22 com cache npm), executado em ordem:
  1. `npm ci`;
  2. `npm run build`;
  3. `npm run typecheck` em modo **informativo** (`continue-on-error`), porque ainda há 7 erros de tipo conhecidos (`PV-PLATFORM-005` → `PV-UI-0xx`).

## Validação

- **Local (Windows):**
  - `dotnet build -c Release`: 0 avisos;
  - `dotnet test -c Release`: 173 aprovados, 0 ignorados;
  - `npm run build`: verde (`PV-PLATFORM-005`).
- **No GitHub:** só depois do push.
  - O resultado da primeira execução está no `RELATORIO-FILA-2.md`.
  - **Riscos de Linux não testados localmente:** diferença de maiúsculas e minúsculas em nomes de arquivo (ex.: `character/` × `Character/` no cliente, hoje só em telas inalcançáveis) e caminhos temporários do SQLite.

## Fora do escopo

Publicação/deploy, análise de cobertura, lint e proteção de branch (configuração do repositório no GitHub, do PO).
