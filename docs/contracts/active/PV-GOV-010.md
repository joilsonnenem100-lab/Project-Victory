# PV-GOV-010 — Decisões da Fila 5, inventário de atributos e revisão dos docs LOCKED

- **Status:** Implementado (Fila 5, branch `chief/q5-gov-010`; só `.md`)
- **Origem:** ordem "MODO FILA 5" (PO e Arquiteto-Chefe, 2026-11-06), item 1, e decisões a–g.
- **Lastro:** ADR-016 (o PO decide Game Design); `roadmap.md`/`CLAUDE.md` (LOCKED só muda por revisão documentada).

## Decisões registradas (`decision-log.md`)

| Item | Entrada |
|---|---|
| a) atributos = só os do Tibia | `DEC-20261106-26` |
| b) remover o que não existe no Tibia | `DEC-20261106-27` |
| c) exceção formal à Regra Aditiva | `DEC-20261106-28` |
| d) identidade das classes (descritiva) | `DEC-20261106-29` |
| e) títulos por Level (resolve R-034) | `DEC-20261106-30` (+ emenda no `DEC-20261106-19`) |
| f) R-038 autorizado | `DEC-20261106-31` |
| g) skills de classe PROVISÓRIAS aprovadas | `DEC-20261106-32` |

## Inventário

`docs/architecture/PV-GOV-010-INVENTARIO-ATRIBUTOS.md`, com a tabela item / onde / existe no Tibia? / ação.

## Revisão formal dos docs LOCKED e emendas (texto antigo mantido por histórico)

- `ADR-026` → **v1.2** (títulos por Level).
- `knowledge-base/character.md` e `schema/character.md` (ambos **LOCKED**): seção Ascension (R-034) e Sistema de Poder.
- `knowledge-base/progression.md`: Level/Ascension.
- `developer-guide/domain/attributes.md` e `PV_ATTRIBUTE_SYSTEM_FOUNDATION.md`: conjunto oficial de atributos.
- Tachados, com lista no `CLEANUP-MANIFEST-2026-11-06.md`: `AttackSpeed` e `Capacity` em `entities.md` e `value-objects.md`; afixo "Strength" no design de itens.

## Pendências

- **R-034 resolvido.**
- **R-040 aberto:** Magic Level inicial 10 × 0 no Tibia.
- **Perguntas ao PO:**
  - Critical e Dodge entram como efeitos de item?
  - `BasicCombatAttributes` está sem uso: remover?

## Revisão (`pv-architecture-reviewer`): APROVADO COM RESSALVAS — aplicadas na branch `chief/q5-prog-013`

1. Conflito com `knowledge-base/world.md` ("a Hunt não possui Tier"): **emendado** — a Hunt area exige **tier de título**, distinto do Tier de item; no código o nome é `TitleTier`/`RequiredTitleTier` (sem colisão com o Tier de item).
2. `knowledge-base/skills.md` (LOCKED) sem Fishing e com "First Fighting": **emendado** (revisão formal).
3. Inventário completado: `affixes.md` (Healing Power → pergunta ao PO) e `combat.md` (Attack Speed/Cast Speed não existem).
4. `entities.md` e `value-objects.md` (LOCKED): nota de emenda formal no topo; lista de LOCKED corrigida acima.
5. A substituição da Ascension da Fila 4 é explicitada no contrato `PV-PROG-013` e no manifesto; R-037 encerrado.
6. R-040 com redação mais precisa; o nome "Arcane Lance" (Sorcerer = energia e fogo) vai para o balanceamento (`DEC-20261106-32`), citado no relatório da Fila 5.
