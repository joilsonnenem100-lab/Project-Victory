# Cleanup Manifest — 2026-11-06 (Fila 5)

Autorização: PO, `DEC-20261106-27` (remover atributos e stats que não existem no Tibia) e `DEC-20261106-28` (exceção formal à Regra Aditiva), além do `DEC-20261106-31` (R-038). Inventário completo: `docs/architecture/PV-GOV-010-INVENTARIO-ATRIBUTOS.md`.

Regras: cada remoção listada aqui; nenhum arquivo inteiro apagado sem estar listado; nenhuma migration existente editada.

## PV-GOV-010 — documentação (texto tachado, não apagado)

| Documento | O que saiu | Por quê |
|---|---|---|
| `developer-guide/domain/entities.md` | `AttackSpeed`, `Capacity` (lista de value objects) | `DEC-20261106-27`; sem Capacidade (`DEC-20261106-17/26`) |
| `developer-guide/domain/value-objects.md` | seções `AttackSpeed` e `Capacity` | idem |
| `docs/character e conceitos/PROJECT-VICTORY-ITEM-SYSTEM-DESIGN.md` | exemplos de afixo com `Strength` ("of Strength", "Strong III / Strength", "Strength I/II/III") | `DEC-20261106-27` |

Emendas (sem remoção): `ADR-026` v1.2, `knowledge-base/character.md`, `schema/character.md`, `knowledge-base/progression.md`, `developer-guide/domain/attributes.md`, `PV_ATTRIBUTE_SYSTEM_FOUNDATION.md`.
