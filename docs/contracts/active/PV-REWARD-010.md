# PV-REWARD-010 — Reward Pipeline real

Status: **IMPLEMENTADO** (branch `chief/m2-reward-010`)
Emitido por: Arquitetura (sessão Chefe), 2026-11-06, conforme `roadmap-to-playable.md` (M2) e `PV-GOV-006`.
Lastro: ADR-024 (Reward Pipeline é o único que muda Experience e progressões permanentes), ADR-035 (Inventory gerencia Resources), ADR-037 (Loot gera coleções; a entrega é do Reward Pipeline), ADR-038 (Loot Tables), `DEC-20261106-08` (preço de venda ao NPC), `DEC-20261106-12` (XP rate do servidor, sem endpoint). Resolve a lacuna do `PV-QA-001` parte 1 (Skip removido).

## Problema

- `RewardProcessor` era stub: XP 0, ouro 0 e um item fictício.
- `ApplyRewardHandler` só aplicava XP e ignorava drops e ouro.
- O fluxo real do ataque (`AttackEncounter` → `RewardResolutionHandler`) passava XP `null`: derrotar um monstro **nunca** dava XP.
- Os drops ignoravam a chance da Loot Table.
- Não existia preço de venda nos itens.

## Escopo implementado

1. **XP rate (Domain + configuração do servidor):**
   - VO `Domain/Reward/ExperienceRate` (maior que zero; `Apply` = base × rate, **arredondado para baixo**; com 1x o valor é exato).
   - `Application/Reward/RewardSettings` registrado no `Program.cs` a partir de `Progression:ExperienceRate` (padrão **1x** em `appsettings.json`). Valor ≤ 0 ou inválido derruba o startup.
   - **Nenhum endpoint** lê ou altera o valor (teste por rota). A tela de administração é o `PV-ADMIN-010` (M8).
2. **Aleatoriedade injetável:** `Domain/Primitives/IRandomSource`, implementado por `Infrastructure/Services/SystemRandomSource` (`Random.Shared`, thread-safe), singleton.
3. **Sorteio de loot (Domain):** `Domain/Loot/MonsterLootRoll`:
   - drops por chance (`NextDouble() < Chance`: 1.0 sempre cai, 0 nunca cai);
   - ouro entre o mínimo e o máximo da definição do monstro.
4. **Ouro como Resource do Inventory (ADR-035):** `Inventory.Gold` (`long`) e `AddGold` (só valores ≥ 0). Comércio e gasto ficam para a Fase 7/8.
5. **Fluxo real do ataque:**
   - `LootResolutionHandler` aplica a chance e sorteia o ouro;
   - `ApplyLootToCharacterInventoryHandler` entrega itens e ouro;
   - `RewardResolutionHandler` entrega **XP da definição do monstro × XP rate** pelo `ApplyResolvedRewardToCharacterHandler`. Derrota ou monstro sem definição → nenhuma XP.
   - Tipos de XP do fluxo (`ResolvedReward`, command e result) passaram a `long`.
6. **Pipeline `IRewardProcessor`:**
   - `RewardProcessor` deixa de ser stub. Para uma vitória (`SourceResult` = `CombatResult`), XP base, ouro e drops vêm da definição do monstro; qualquer outra origem gera recompensa vazia.
   - `ApplyRewardHandler` aplica XP × rate, drops (instância nova com o `ItemDefinitionId` do drop) e ouro.
7. **Preço de venda ao NPC (dado, `DEC-20261106-08`):** `ItemDefinition.NpcSellPrice`, num construtor novo; o construtor antigo continua e usa 0. Valores **PROVISÓRIOS** do Arquiteto, ajustáveis pelo PO:

   | Item | Preço de venda |
   |---|---|
   | Rat Tail | 2 |
   | Training Sword | 10 |

8. **Persistência:**
   - migration **nova** `InventoryGoldAndItemSellPrice`: `Characters.Gold` (INTEGER, padrão 0) e `Items.NpcSellPrice` (INTEGER, padrão 0), mais um `UPDATE` do preço do `Training Sword` em bancos já semeados;
   - nenhuma migration existente foi editada;
   - repositórios mapeiam ouro e preço; o seed grava o preço.

## Decisões do Arquiteto

- **D1 — Ouro é saldo no Inventory**, não item: ADR-035 ("o Inventory gerencia Resources") e ADR-028 (Inventory é estado permanente). Persistido em coluna do personagem, que é dono do Inventory (`OwnsMany`).
- **D2 — XP rate aplicado só no Reward Pipeline** (`RewardResolutionHandler`/`ApplyRewardHandler`), nunca no `RewardProcessor` nem no Loot (ADR-024). Arredondamento para baixo: o jogador nunca recebe XP fracionária.
- **D3 — A configuração tem padrão 1x** se a chave faltar (`DEC-20261106-12`: "começando em 1x"). Diferente da chave JWT, um valor padrão aqui não é risco de segurança.

## Fora do escopo

- Definições de loot e XP do Cave Rat e da Spider Cave (`PV-CONTENT-011`, M3).
- Mostrar o ouro na API ou na UI (M8).
- Comércio com NPC (Fase 7/8).
- Tela de administração do XP rate (`PV-ADMIN-010`, M8).

## Testes

- `Domain.Tests/RewardDomainTests.cs`: rate, ouro, sorteio por chance e preço de venda.
- `SmokeTests/RewardPipelineTests.cs`:
  - ataque com vitória dá 10 XP com rate 1x e 20 XP com rate 2x, mais ouro e Rat Tail;
  - derrota ou monstro desconhecido não dão nada;
  - `RewardProcessor` real;
  - preço do Rat Tail.
- `SmokeTests/RewardInventoryIntegrationTest.cs`: **sem Skip**, confere o `ItemDefinitionId` do drop, o ouro e a XP.
- `SmokeTests/CharacterExperiencePersistenceTests.cs`: ouro e preço de venda em SQLite real.
- `Api.Tests/ExperienceRateConfigurationTests.cs`:
  - padrão 1x;
  - configuração 2x;
  - 0 e −1 derrubam o startup;
  - nenhuma rota de rate, progression ou admin.
- `Api.Tests/RewardFlowHttpTests.cs`: pela API real, derrotar um Rat deixa o personagem com 10 de XP e 1 item.

Resultado da solução: build com 0 avisos; **156 aprovados, 0 ignorados, 0 falhas**.
