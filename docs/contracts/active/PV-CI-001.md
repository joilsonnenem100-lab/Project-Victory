# PV-CI-001 — Integração contínua (GitHub Actions)

Status: **IMPLEMENTADO** — revisado pelo `pv-architecture-reviewer` (APROVADO COM RESSALVAS; ressalvas aplicadas no branch `chief/m1-clean-001`). Branch `chief/q2-ci-001`
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
  - O resultado da primeira execução está no `docs/architecture/historico/RELATORIO-FILA-2.md`.
  - **Riscos de Linux não testados localmente:** diferença de maiúsculas e minúsculas em nomes de arquivo (ex.: `character/` × `Character/` no cliente, hoje só em telas inalcançáveis) e caminhos temporários do SQLite.

## Fora do escopo

Publicação/deploy, análise de cobertura, lint e proteção de branch (configuração do repositório no GitHub, do PO).

## Resultado no GitHub

- O push com o workflow foi **aceito**: não houve bloqueio por permissão de workflows.
- Execução **42225918358** (`chief/q2-ci-001`, `9c91392`): **success** nos dois jobs (.NET: restore, build Release, test; cliente: `npm ci`, build). https://github.com/joilsonnenem100-lab/Project-Victory/actions/runs/42225918358
- Execução **42226163716** (`chief/m1-clean-001`, `60488c9`): **success**.
- A **contagem de testes no Linux não foi conferida**: o log e o `.trx` exigem autenticação. Localmente são 173.

## Revisão (`pv-architecture-reviewer`, 2026-11-06): APROVADO COM RESSALVAS

1. e 2. Resultado da execução registrado acima. A contagem no Linux fica como pendência de leitura do artefato pelo PO.
3. O passo de typecheck agora publica um `::warning` visível quando há erros, em vez de `continue-on-error` silencioso (aplicado no branch `chief/m1-clean-001`).
4. Anotações do runner: as actions `@v4` usam Node 20, que está obsoleto, e `ubuntu-latest` vai migrar. Atualizar ou fixar versões fica para um contrato futuro.
5. Um PR de branch do próprio repositório roda o CI duas vezes (`push` + `pull_request`). Aceito, conforme a ordem.
6. O import com caixa errada (`PlayerIdentityArea.tsx`) quebra o CI no Linux no dia em que a tela ficar alcançável → `PV-UI-0xx`.
