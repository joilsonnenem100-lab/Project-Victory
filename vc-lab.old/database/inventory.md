# VC-LAB Database — Inventory

Status: Draft

Sprint: 031

## Objetivo

Definir a implementação do domínio de Inventário.

## Filosofia

Inventory é um Container.

Não armazena ItemInstances diretamente.

## Estrutura

Character

↓

Inventory

↓

InventorySlot

↓

ItemInstance

↓

ItemTemplate

## Entidades

### Inventory

- Id
- CharacterId
- ExpansionLevel
- CreatedAt
- UpdatedAt

### InventorySlot

- Id
- InventoryId
- SlotIndex
- ItemInstanceId (nullable)
- CreatedAt

## Decisões

- Inventory é um Container.
- Slot é uma entidade.
- Todo ItemInstance possui exatamente uma localização.
- Item pertence ao Container e não ao Character.