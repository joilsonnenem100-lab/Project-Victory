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
- Removidos do **controle** do git (os arquivos continuam no disco de quem já os tem; rode `npm install` no client após o merge, pois o git remove `node_modules` da árvore de trabalho ao aplicar esta remoção): 2.438 arquivos de `node_modules` e 113 de `dist` em `src/ProjectVictory.Client/`.
- **Mantido de propósito:** `src/ProjectVictory.API/projectvictory.db` (ver Análise §4.7). Sai depois do `PV-PLATFORM-001`.
- Os mesmos 5 itens apagados e 4 movidos descritos acima foram aplicados com `git rm` / `git mv` (histórico dos arquivos movidos preservado).
