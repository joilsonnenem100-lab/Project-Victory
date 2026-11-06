# PV-GOV-006 — Decisões do PO (manhã de 2026-11-06) e preparação do M2

Status: **CONCLUÍDO** — revisado pelo `pv-architecture-reviewer` (APROVADO COM RESSALVAS; ressalvas aplicadas no branch `chief/m2-prog-011`). Branch `chief/m2-gov-006`; só arquivos `.md`
Emitido por: Arquitetura (sessão Chefe), 2026-11-06, por ordem do PO.
Lastro: ADR-016 deste projeto (`decision-log.md`: PO decide Game Design; Arquiteto decide arquitetura, roadmap e documentação); `knowledge-base/progression.md`; ADR-023, 024, 025, 027, 028.

## Escopo

Somente documentação (`.md`). Nenhum código, migration ou configuração.

## Decisões registradas

| # | Decisão do PO | Onde ficou registrada |
|---|---|---|
| 1 | O valor do loot no lucro do Analyzer é o **preço de venda ao NPC**; todo item terá preço de venda **provisório** definido pelo Arquiteto, ajustável depois | `DEC-20261106-08` → **Accepted (PO)** |
| 2 | **Spider**, **Poison Spider** e **Giant Spider** confirmados (Spider Cave) | `DEC-20261106-09` → confirmada |
| 3 | O Analyzer mostra o **lucro líquido em tempo real durante** a Hunt e o **resumo ao final** | `DEC-20261106-01` → texto corrigido, com nota de correção |
| 4 | **ADR-026 Ascension Model**, a partir de `knowledge-base/progression.md`, ACCEPTED, sem inventar nada além do documentado | `developer-guide/adr/ADR-026-ascension-model.md` (itens não documentados ficaram na seção "Em aberto") |
| 5 | Curva de XP: `XP(L) = 50/3 × (L³ − 6L² + 17L − 12)`; Experience `long` (migration aditiva); Level derivado; Experience Table gerada pela fórmula; XP rate como configuração (1x) | `DEC-20261106-10` |
| 6 | (complemento) **Mesma curva em todas as Ascensions**; só muda o conteúdo desbloqueado | `DEC-20261106-11`; `ADR-026` |
| 7 | (complemento) XP rate = **configuração do servidor**, nunca do cliente nem de jogador comum; **tela de administração** restrita a conta administradora no **M8**; no M2 só a configuração e o uso no Reward Pipeline, **sem endpoint** | `DEC-20261106-12`; roadmap M8 `PV-ADMIN-010` |

## Decisões do Arquiteto (ADR-016: roadmap e organização)

- **D1 — Ordem dentro do M2:** `PV-PROG-011` (Experience Table) antes do `PV-PROG-010` (Level derivado), porque o Level derivado depende da tabela. Depois `PV-REWARD-010` e `PV-STATE-001`.
- **D2 — Coluna `Level` não é apagada.** O roadmap dizia "remover `Level` persistido". Apagar coluna é destrutivo e a ordem do PO é "migration aditiva". A coluna fica, gravada com o valor derivado e **nunca lida como verdade** (ADR-028: "informações derivadas nunca poderão tornar-se fonte oficial da verdade"; `developer-guide/domain/level.md`: Level armazenado "para otimização" é "apenas um cache"). Removê-la exige decisão e contrato próprios.
- *(D3 e D4 superados em 2026-11-06 por `DEC-20261106-19`: não há teto de Level e a Ascension é derivada do Level.)*
- **D3 — Teto de Level 500 já no M2.** A entrada do Level 500 tem Experience máxima **aberta** (sem limite), porque a Experience continua acumulando (ressalva 4 da revisão; `experience-table.md`, "Estrutura"). Está documentado (`progression.md`, `ADR-026`) e cumpre a regra "último nível válido" de `developer-guide/domain/experience-table.md`. A Experience continua acumulando depois do teto (ADR-023).
- **D4 — Só a primeira Ascension no M2.** Como o Level volta a 1 sem a Experience reiniciar é item em aberto do `ADR-026`; nenhum personagem muda de Ascension antes de um contrato próprio.
- **D5 — Itens do M1 em aberto** (`PV-PLATFORM-004`, `PV-PLATFORM-005`, `PV-CLEAN-001`) não são dependência técnica do M2 e ficam registrados no roadmap. O `PV-CLEAN-001` apaga arquivos e só roda com autorização explícita.

## Arquivos alterados

`developer-guide/adr/ADR-026-ascension-model.md` (novo), `developer-guide/architecture/decision-log.md`, `developer-guide/architecture/roadmap-to-playable.md`, `developer-guide/architecture/project-state.md` (R-003, R-008), `knowledge-base/progression.md` (pendências decididas), `CLAUDE.md` (próximo ADR livre = 058; ADR-026 preenchido), este contrato.
