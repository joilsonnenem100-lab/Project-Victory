# PV-LOOT-001B — Correção Integração Reward → Inventory

Status:

APROVADO PARA IMPLEMENTAÇÃO


## Objetivo

Corrigir a integração entre Reward e Inventory.

O objetivo é garantir que itens presentes em RewardDrop sejam adicionados corretamente ao Inventory do Character.


## Problema atual identificado

O teste:

RewardInventoryIntegrationTest.ApplyReward_Should_Add_Item_To_Character_Inventory

falha com:

Assert.Contains() Failure

Item esperado:

"Smoke Test Item"

Inventory encontrado:

[]

Resultado:

Reward é processado, mas o item não chega ao Character.Inventory.


## Fluxo esperado

Atual:

Reward

↓

RewardDrop

↓

ApplyRewardHandler

↓

Character

❌ Inventory sem item


Esperado:

Reward

↓

RewardDrop

↓

ApplyRewardHandler

↓

Character.Inventory.AddItem()

↓

Inventory contém item


## Escopo

Corrigir somente a integração existente.


Implementar:

- processamento dos RewardDrop;
- entrega dos itens ao Inventory;
- manutenção do fluxo atual de Experience.


## Não implementar

Não criar:

- novo sistema de Loot;
- ItemInstance;
- Equipment;
- Slots;
- Stack;
- Raridade;
- Drop Tables.


Esses sistemas ficam fora deste contrato.


## Arquivos para análise

Consultar:

src/ProjectVictory.Application/Reward/

src/ProjectVictory.Domain/

tests/ProjectVictory.SmokeTests/RewardInventoryIntegrationTest.cs


## Regras

Não alterar:

- Smoke Test principal;
- arquitetura existente;
- contrato de Reward;
- Inventory existente.


Utilizar estruturas já existentes:

- RewardDrop;
- RewardResult;
- ApplyRewardHandler;
- Character.Inventory.


## Critérios de aceite

O teste:

RewardInventoryIntegrationTest.ApplyReward_Should_Add_Item_To_Character_Inventory

deve passar.


Resultado esperado:

```text
Total: 1
Falhas: 0
Sucesso: 1