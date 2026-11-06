# Cleanup Manifest — 2026-11-05

Executado na cópia de trabalho. O `.zip` original enviado permanece intacto e é a referência de rollback.
Regra aplicada: *Additive Development Principle* (ARCHITECTURE-RULES) — só remove o que é comprovadamente redundante; conteúdo único é movido, nunca descartado.
Nenhum arquivo C# foi alterado: o ambiente de análise não tem `dotnet`, logo não é possível provar que o build continua passando (exigência da mesma regra).

## Apagados (5 itens + 2 duplicatas + 1 pasta vazia)
| Item | Evidência |
|---|---|
| `DELETE/architecture-review/PROJECT-VICTORY-ARCHITECTURE-HANDBOOK-V2.md` | Superado pelo Handbook V3 (v3.0 "Master"); capítulos de V2 existem em V3 com numeração diferente; estava em pasta rotulada DELETE |
| `src/ProjectVictory.Client/src.zip` (5 MB) | Snapshot binário do próprio `src/` do client; o histórico git (236 commits) já versiona o código |
| `src/ProjectVictory.Client/client-files.txt` (352 KB) | Listagem gerada, sem valor de fonte |
| `src/ProjectVictory.Client/src/components/NOT-APROVED-gameHUD.zip` | Rotulado "NOT APPROVED" pelo próprio dono; contrato `PV-FRONTEND-GAME-HUD-FOUNDATION-001` continua ativo para refazer o HUD |
| `tests/deleteme.md`, `tools/deleteme.md` | Idênticos entre si (hash) e cópia de `README_DEVELOPMENT.md`; auto-rotulados deleteme |
| `src/ProjectVictory.Domain/Nova pasta/` | Pasta vazia |
| `DELETE/` | Esvaziada após as movimentações abaixo |

## Movidos (conteúdo único preservado)
| De | Para | Motivo |
|---|---|---|
| `DELETE/architecture-review/architecture-rules.md` | `developer-guide/architecture/architecture-rules.md` | **Única** lista das 20 regras LOCKED (Engine First, Domain First, Clean Architecture, Zero Framework Domain…). Não existe em outro lugar |
| `DELETE/bootstrap-review/bootstrap.md` | `vc-lab/implementation/bootstrap.md` | Bootstrap LOCKED da Sprint 050; pertence ao vc-lab |
| `DELETE/developer-guide-review/adrs/ADR-001..004` | `docs/architecture/archive/adr-drafts-with-context/` | Versões mais ricas (têm seção *Contexto*) que as oficiais enxutas; hashes diferentes |
| `DELETE/developer-guide-review/contracts/HC-001..004` | `docs/contracts/archive/hunt-contracts-HC/` | Contratos técnicos sem par em nenhum outro lugar |

## NÃO tocados — decisão pendente (ver relatório)
- C# inteiro (stubs, pastas `Architecture/`, `Playtest/`, `Production/`): candidatos a remoção **depois** de um `dotnet build` verde.
- `Documentation/` (raiz paralela a `docs/`): contratos citam o próprio caminho; mover quebraria a rastreabilidade.
- `docs/CURRENT_CONTEXT.md`: auto-declarado histórico, mas citado por 4 documentos como "estado atual".
- Vazios a preencher (não apagar): `ADR-055`, `docs/history/Sprint-054..056.md`, `knowledge-base/Market.md`, `developer-guide/releases/account-foundation-ready.md`.
- `Patches/PV-TOOLS-002.patch` e `tests/.../TESTE/PV-TOOLS-002.patch` (hashes diferentes), `ItemCheck/`, `tools/ProjectVictory.PatchEngine`: ferramental de contrato ativo (`PV-TOOLS-002`).

## Higiene do repositório GitHub (R-010) — aplicada na branch `chief/m0-baseline-cleanup`
- `.gitignore`: adicionados `node_modules/`, `dist/` e `*.swp`.
- Removidos do **controle** do git (os arquivos continuam no disco de quem já os tem; rode `npm install` no client após o merge, pois o git remove `node_modules` da árvore de trabalho ao aplicar esta remoção): **2.440 arquivos** em `src/ProjectVictory.Client/` (2.438 de `node_modules` e 2 de `dist`). *Corrigido em PV-GOV-002: a contagem anterior ("113 de `dist`", total 2.551) contava em duplicidade 111 arquivos de pastas `dist/` internas a `node_modules`; conferido no commit `ec1a1e0` — 2.441 arquivos alterados = 2.440 remoções + `.gitignore`.*
- **Mantido de propósito:** `src/ProjectVictory.API/projectvictory.db` (ver Análise §4.7). Sai depois do `PV-PLATFORM-001`. *Atualização: removido do controle em `PV-PLATFORM-001` — ver seção abaixo.*
- Os mesmos 5 itens apagados e 4 movidos descritos acima foram aplicados com `git rm` / `git mv` (histórico dos arquivos movidos preservado).

## `projectvictory.db` fora do versionamento (R-010) — `PV-PLATFORM-001`, branch `chief/m1-platform-001`
- `git rm --cached src/ProjectVictory.API/projectvictory.db` (o arquivo **continua no disco** de quem fez a remoção) e `.gitignore` com `projectvictory.db`, `*.db-shm` e `*.db-wal`.
- O schema passa a ser criado pelo `Database.Migrate()` no startup em **Development** (`Program.cs`). Um clone limpo sobe a API com as 14 migrations aplicadas e o seed de itens.
- O arquivo continua no histórico do git (commits anteriores); não houve reescrita de histórico.

> ⚠️ **AVISO — clones já existentes perdem o banco local no `git pull`/merge.** Quando um commit deixa de rastrear um arquivo, o git **apaga** esse arquivo da árvore de trabalho de quem recebe o commit. Em qualquer máquina que já tenha o repositório, **antes** do `git pull` ou do merge desta mudança, copie o banco para **fora do repositório**:
>
> ```
> cp src/ProjectVictory.API/projectvictory.db ~/projectvictory.db.backup
> ```
>
> Depois do pull, para manter as contas de teste, copie de volta para `src/ProjectVictory.API/projectvictory.db` (agora ignorado pelo git). Sem a cópia, a API recria um banco **vazio** no próximo startup em Development (contas e personagens de teste se perdem).

### Como rodar a API usando o banco de `src/ProjectVictory.API`
- A connection string é relativa (`Data Source=projectvictory.db`): o banco fica no **diretório de trabalho** do processo.
- **Forma recomendada** (a partir da raiz do repositório):
  ```
  dotnet run --project src/ProjectVictory.API
  ```
  O `dotnet run` usa a pasta do projeto como diretório de trabalho (verificado em 2026-11-05: content root `src/ProjectVictory.API`), e o perfil `http` do `launchSettings.json` define `ASPNETCORE_ENVIRONMENT=Development` (porta 5250). Resultado: banco em `src/ProjectVictory.API/projectvictory.db`, migrations aplicadas.
- **Evite** executar a DLL diretamente (`dotnet ProjectVictory.API.dll`) de outra pasta: o banco seria criado na pasta corrente. Com `--no-launch-profile` o ambiente vira Production e o `Migrate()` **não** roda.
- Tornar o caminho do banco configurável é escopo do `PV-PLATFORM-002`.
