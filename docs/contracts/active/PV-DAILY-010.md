# PV-DAILY-010 — Recompensa diária por conta (Fila 10, item 4)

Status: IMPLEMENTADO (2026-11-07) · Lastro: `PO-DECISOES-BASE.md` **PO-60** e PO-16 (`DEC-20261107-05`); fecha **R-068**. Regras: Server Authority (conta do token, `ICurrentPlayerContext`), Aggregate Ownership (itens e ouro pelos métodos do agregado `Character`), ADR-024 intacto (nenhuma Experience). Escopo: só servidor.

## Regra
- Por **conta**: no máximo **uma** entrega por **ciclo diário**. O ciclo começa no horário de reset (`DailyReward:ResetTimeUtc`, UTC) e vai até o próximo reset.
- Entrega na **primeira entrada no mundo** (`POST api/game/enter`) do ciclo, para o personagem que entra. A resposta do enter ganha o campo opcional `dailyReward` (`items[] { itemDefinitionId, name, quantity }`, `gold`, `cycleStart`), ou `null` quando não há entrega (aditivo).
- Itens e ouro entram no `Inventory` pelo mesmo caminho do kit inicial e do Reward Pipeline (`Inventory.AddItem`/`AddGold` + `ICharacterRepository.UpdateAsync`).
- Sem itens e sem ouro configurados, a recompensa fica desligada (padrão da composição, testes antigos).
- Concorrência: uma trava por conta (`DailyRewardGate`) e a chave primária `(AccountId, CycleStartUnixMilliseconds)`. O registro é gravado **antes** do inventário. Uma falha entre os dois faz a recompensa do ciclo se perder, mas nunca a duplica.

## PROVISÓRIOS (onde trocar: `src/ProjectVictory.API/appsettings.json`, seção `DailyReward`)
- `ResetTimeUtc` = `"09:00"`. É o horário de referência do server save do Tibia, 10:00 CET, levado para UTC no horário de inverno.
- `Gold` = 100; `Items` = `HealthPotion` 5, `ManaPotion` 5 (chaves do `PotionCatalog`; só poções nesta fase, como o kit inicial).

## Persistência
Migration **nova e aditiva** `20261107093707_DailyRewardClaims`: só faz `CreateTable DailyRewardClaims` (AccountId, CycleStartUnixMilliseconds, ClaimedAtUnixMilliseconds, CharacterId). Nenhuma tabela existente foi alterada.

## Testes
- `SmokeTests/DailyRewardTests` (relógio controlado, reset às 09:00):
  - primeira entrada após o reset entrega, e no mesmo dia não entrega de novo;
  - no dia seguinte às 08:59 não entrega, e às 09:00 entrega;
  - cada conta tem o seu ciclo;
  - 5 entradas simultâneas entregam uma vez só;
  - sem configuração não há entrega;
  - início do ciclo calculado;
  - configuração inválida é recusada.
- `Api.Tests/DailyRewardHttpTests`: o appsettings tem a configuração; o primeiro enter traz a recompensa e o inventário cresce; o segundo enter traz `null`.
- **Ajuste com justificativa:** `ApiFactory.WithoutStarterKit()` passa a zerar também `DailyReward:Items:*` e `DailyReward:Gold`. Os cenários antigos que contam itens exatos do inventário (`RewardFlowHttpTests`, `SpiderCaveEndToEndTests`) foram escritos antes da recompensa. É o mesmo precedente do kit inicial (`PV-CONTENT-012`), e nenhuma asserção mudou.

## Resultado
Build sem avisos; **494 testes verdes** (Runtime 17, Application 34, Domain 136, Smoke 179, Api 128).
