# Inventory System

Version: 1.0

Status: LOCKED

Last Sprint: SPRINT-023

---

# Objetivo

Definir o funcionamento oficial do sistema de Inventário do Project-Victory.

O inventário representa o patrimônio conquistado pelo jogador.

---

# Filosofia

No Project-Victory, um inventário representa exclusividade e riqueza.

Também representa:

- segurança;
- patrimônio;
- coleção;
- progresso.

---

# Papel do Inventário

O inventário conecta:

```
Monster

↓

Loot

↓

Item

↓

Inventory

↓

Equipment

↓

Market
```

---

# Arquitetura

O inventário é um sistema independente.

Estrutura:

```
Character

↓

InventoryService

↓

InventoryEntity
```

---

# Estrutura Expansível

O inventário possui:

```
Backpack inicial

↓

Expansões futuras
```

A quantidade de slots deve ser configurável.

---

# Interface Visual

Modelo:

```
Slot System
```

Exemplo:

```
[ ][ ][ ][ ]

[ ][ ][ ][ ]
```

---

# Organização

O jogador pode:

- mover itens;
- organizar;
- visualizar;
- filtrar;
- pesquisar.

---

# Recent Loot

Existe uma visualização de itens recebidos recentemente.

Objetivo:

Facilitar acompanhamento do Loot.

---

# Stack

Itens iguais podem ser agrupados.

Exemplo:

```
Dragon Leather x50
```

---

# Limite de Stack

Limite inicial:

```
666 unidades
```

por item stackável.

Valor configurável.

---

# Item Template

ItemTemplate representa a definição base.

Exemplo:

```
Fire Sword
```

Contém:

- nome;
- categoria;
- tipo;
- propriedades base.

---

# Item Instance

ItemInstance representa o item real do jogador.

Exemplo:

```
Fire Sword #001

Rare

Upgrade +10

Affixes
```

Cada instância possui identidade própria.

---

# Equipamentos

Equipamentos passam pelo inventário.

Fluxo:

```
Loot

↓

Inventory

↓

Equip

↓

Equipment Slot
```

---

# Negociação

Todos os itens são negociáveis inicialmente.

Não existem:

- itens vinculados à conta;
- itens vinculados ao personagem.

---

# Peso

Não existe sistema de peso inicialmente.

Motivo:

Priorizar:

- progressão;
- loot;
- economia;
- experiência idle.

---

# Segurança

Todo ItemInstance possui:

```
Unique Item ID
```

Regra:

Uma instância nunca pode existir duplicada.

---

# Exclusão

Itens destruídos não possuem histórico inicialmente.

Exemplo:

```
Fire Sword

↓

Destruída

↓

Removida
```

---

# Integrações

InventoryService comunica com:

```
LootService

↓

EquipmentService

↓

MarketService

↓

TradeService
```

---

# Fluxos principais

## Receber Item

```
Loot

↓

Inventory Validation

↓

Create ItemInstance

↓

Adicionar ao Inventário
```

---

## Equipar Item

```
Inventory

↓

Equipment Validation

↓

Equipment Slot
```

---

## Transferir Item

```
Trade

↓

Validation

↓

Novo proprietário
```

---

# Regras Futuras Preparadas

O sistema suporta futuramente:

- Soulbound;
- Account Bound;
- Character Bound;
- Peso;
- Expansões;
- Categorias avançadas.

---

# Pendências futuras

Definir:

- quantidade inicial de slots;
- filtros;
- interface;
- categorias;
- regras de organização;
- sistema de busca.