# PV-COMBAT-015 — Cura igual para todas as vocações (Fila 10, item 3)

Status: IMPLEMENTADO (2026-11-07) · Lastro: `PO-DECISOES-BASE.md` **PO-59** (`DEC-20261107-05`), PO-53; responde à pergunta 1 do `PV-COMBAT-014`.

## O que muda
- `ClassHealingSpellTable` (Domain): sai `DruidBands` (+5 pontos por faixa); a spell do Druid usa `StandardBands`, como as outras vocações. A cura continua % da **vida máxima** (confirmado pelo PO-59). Remoção autorizada pelo PO-59 ("o +5 do Druid sai").
- Valores (10/15/20/25 % por faixa; 20 de Mana) seguem **PROVISÓRIOS** — trocar em `ClassHealingSpellTable.StandardBands` / `DefaultManaCost`.

## Testes (justificativa)
- `Druid_HealsMorePerBand_PO01` → substituído por `AllClasses_HealTheSamePercentPerBand_DruidIncluded_PO59`: o teste antigo fixava a inferência do PO-01 que o PO-59 superou; o novo prova a mesma % e a mesma cura bruta para as 5 vocações.

## Resultado
Build 0 avisos; 483 testes verdes. Sem migration.
