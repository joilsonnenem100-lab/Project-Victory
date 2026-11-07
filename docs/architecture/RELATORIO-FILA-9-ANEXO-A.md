# Anexo A — Fila 9: notas de superação e ressalvas

## Notas de superação (`DEC-20261107-02`, salvo indicação)
| Arquivo (L = LOCKED) | Trecho | PO |
|---|---|---|
| `knowledge-base/progression.md` | emenda de títulos: tier exige região | PO-09/55/52 |
| `knowledge-base/world.md` | Hunt area exige tier (2 pontos) | PO-09/55 |
| `knowledge-base/account.md` (L) | Character Slots 3→10 com VC Points; exclusão em 30 dias | PO-36/54; PO-39 |
| `knowledge-base/economy.md` | VC Points; sumidouros; taxa do Market | PO-19/54; PO-17/23/12; PO-18 |
| `knowledge-base/items.md` (L) | Raridade Normal→Unique; Trade | PO-21; PO-24 |
| `knowledge-base/equipment.md` (L) | Upgrade | PO-23 |
| `knowledge-base/combat.md` (L) | Critical; Cálculo Elemental | PO-04; PO-03 |
| `knowledge-base/spells.md` (L) | Critical Mágico; Elementos; Uso por turno (cura) | PO-04; PO-03; PO-53 |
| `knowledge-base/monsters.md` (L) | Monster Skills; Boss | PO-06/12 |
| `knowledge-base/training.md` (L) | Pendências (Free/Premium, obtenção do item) | PO-26/27 |
| `developer-guide/adr/ADR-026` | Ascension desbloqueia regiões; Legend | PO-09/55/34/52 |
| `developer-guide/domain/experience-distribution.md` | regra de grupo | PO-07 |
| `developer-guide/architecture/roadmap.md` | Fase 7 (elemental); Fase 10 (Bosses, Seasons) | PO-03/13; PO-12/50 |
| `developer-guide/architecture/roadmap-to-playable.md` | poções; Free × Premium (2 pontos) | PO-15/16/53; PO-26/27 |
| `developer-guide/architecture/decision-log.md` | DEC-02 (poções), DEC-03/04/22 (cura), DEC-30 (tier) | PO-15/16; PO-53; PO-55 |
| `docs/contracts/active/` | PV-SUPPLY-010/011 (cura); PV-PROG-013, PV-RUNTIME-013 (tier) | PO-53; PO-55 |
| `project-state.md` | R-041 reaberto como R-064; notas em R-043/045/050/057; novos R-064/065/066 | PO-55, 06, 07, 14, 53, 36 |
| Handbook V3 §10 | emenda (`ADR-059`, `DEC-20261107-03`) | — (arquitetura) |

Sem nota (coerentes com o PO): penalidade de morte (PO-08), Seasons no `ranking.md` (PO-50), Premium +10% de XP no `progression.md`. Pastas históricas/arquivadas: vale a regra 3 do arquivo do PO.

## Ressalvas da revisão (todas aplicadas)
(a) relatório criado; (b) `DEC-20261107-04` registra a remoção autorizada do `gameplay/action`; (c) R-064 cita o retorno automático (`InvalidHuntAreaException`); (d) ADR-059: Event Bus LOCKED, Hunt Loop em REVIEW; (e) notas aninhadas nas listas numeradas dos contratos.
