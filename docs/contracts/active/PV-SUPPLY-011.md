# PV-SUPPLY-011 — Fim de suprimento não encerra a Hunt; morte vai à tela inicial

Status: **IMPLEMENTADO** (branch `chief/q4-supply-011`)
Emitido por: Arquitetura, 2026-11-06 (ordem "MODO FILA 4", item 4).
Lastro:
- `DEC-20261106-22`: fim de suprimento não encerra; sem poção, segue sem cura até morrer; toda morte vai à tela inicial com mensagem. **Supera** o trecho correspondente do `DEC-20261106-04` e do `PV-SUPPLY-010`;
- `DEC-20261106-24`: cidade provisória "Victoria";
- ADR-012: pipeline único.

**Substituição de comportamento documentada** por decisão do PO.

## Escopo implementado

1. **`AttackEncounterHandler`:** **removido** o encerramento por fim de suprimento (`Ongoing when IsOutOfSupplies` → `OutOfSupplies`).
   - As poções continuam sendo usadas pela política e saindo do inventário.
   - Sem poção, o personagem segue **sem cura** até a morte (não existe outra cura).
   - `CombatSupplyContext.IsOutOfSupplies` continua só como informação.
   - O valor `HuntEndReason.OutOfSupplies` foi mantido no enum (Regra Aditiva), mas nunca mais é produzido.
2. **A morte continua encerrando a Hunt** pelo pipeline único (`EndHuntHandler`) e volta à **tela inicial**:
   - **`HomeCity`** (dados, **PROVISÓRIO**): nome **"Victoria"** e localização (a única do mundo semeado);
   - `EndHuntHandler.MainCity` passou a ler daqui.
3. **Dados da mensagem de morte** (`DeathNotice`):
   - `reasonCode = "Death"`;
   - `returnCity = "Victoria"`;
   - `checks = ["Potions", "Runes", "HuntLevel"]` (conferir poções, runas e o level da Hunt).

   Os dois vêm em `EndHuntResult` e na resposta de `POST api/hunt/attack` (`returnCity`, `deathNotice`; **aditivos**). O **texto e a tela** são do cliente (M8).

## Testes alterados (comportamento antigo → `DEC-20261106-22`)

| Teste antigo | Situação |
|---|---|
| `SmokeTests/SupplyTests.Potions_AreConsumedFromInventory_AndOutOfSuppliesEndsHuntThroughPipeline` | **Substituído** por `Potions_AreConsumedFromInventory_AndRunningOutDoesNotEndTheHunt`: 2 poções usadas, inventário zerado, nenhuma rodada encerra por fim de suprimento, fim por Victory ou Death |
| `SmokeTests/SupplyTests.OutOfSupplies_WithPolicyAndNoPotions_EndsAfterFirstRound` | **Substituído** por `PolicyWithoutPotions_DoesNotEndTheHunt` |
| `Api.Tests/SpiderCaveEndToEndTests.Potion_PolicyAppliesFromNextTurn_ConsumesSupplies` | **Desfecho esperado** de `OutOfSupplies` (3 poções) para **`Victory`** (3 poções usadas, depois a luta segue sem cura) |
| `Api.Tests/SpiderCaveEndToEndTests.OutOfSupplies_EndsHuntThroughSamePipeline_AndReturnsToCity` | **Substituído** por `NoSupplies_DoesNotEndTheHunt` |
| `Api.Tests/SpiderCaveEndToEndTests.Defeat_…` | **Ampliado:** confere `returnCity = "Victoria"` e o `deathNotice` (código e checagens) |

Resultado da solução: build com 0 avisos; **279 aprovados, 0 ignorados, 0 falhas**.

## Pendência registrada

**R-036:** pelo `DEC-20261106-22`, a Hunt só termina por morte, saída manual ou queda de conexão. Hoje a **vitória de um encounter** (todos os monstros mortos) ainda encerra a Hunt (`HuntEndReason.Victory`), porque 1 Hunt = 1 encounter até o **Hunt Loop contínuo** (`PV-HUNT-011`, M6), que vai gerar o próximo encounter em vez de encerrar.
