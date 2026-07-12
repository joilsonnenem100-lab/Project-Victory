# Inventory System

Version: 1.0

Status: LOCKED

Last Sprint: SPRINT-008

---

# Objetivo

Definir o funcionamento oficial do sistema de inventário do Project-Victory.

O InventoryService é responsável pelo armazenamento, movimentação e segurança dos itens do personagem.

---

# Filosofia

No Project-Victory, o inventário representa segurança e exclusividade.

O inventário representa as conquistas obtidas pelo jogador.

Itens importantes devem possuir identidade própria e permanecer protegidos.

---

# Estrutura Base

Cada personagem possui:

```text
Character

↓

Inventory

↓

Slots
```

---

# Capacidade

O Project-Victory não possui:

- Capacity;
- Peso;
- Limitação por massa.

A limitação ocorre através de Slots.

---

# Slots

Inventário inicial:

```text
50 Slots
```

A quantidade poderá ser expandida futuramente através de sistemas aprovados.

---

# Containers

O Project-Victory não possui mochilas funcionais.

Não existe:

- Backpack dentro de Backpack;
- Containers internos;
- Árvore de inventário.

O personagem possui apenas seu inventário principal.

---

# Backpack Visual

Existe um slot visual de Backpack.

Função:

- aparência;
- personalização;
- cosméticos.

Não aumenta espaço do inventário.

---

# Organização

O inventário possui abas:

## Recent Loot

Exibe itens recebidos recentemente.

Características:

- ordem cronológica;
- não permite reorganização manual.

---

## All Container

Exibe todos os itens armazenados.

Possui organização automática por tipo.

---

# Stack

Itens empilháveis:

- Rune;
- Food;
- Material;
- Resource;
- Flechas;
- Poções.

Itens não empilháveis:

- Weapons;
- Shields;
- Equipamentos;
- Itens únicos.

---

# Limite de Stack

Valor padrão:

```text
666 unidades
```

O valor deve permanecer configurável.

---

# Item Architecture

O inventário trabalha com:

```text
Item Template

↓

Item Instance
```

---

# Item Template

Representa o item base.

Exemplo:

```text
Fire Sword
```

Contém:

- categoria;
- atributos base;
- regras do item.

---

# Item Instance

Representa uma existência específica do item.

Exemplo:

```text
Fire Sword

Rare

+14

Critical

Life Leech

3 Imbuement Slots

Durability 90%
```

Cada Item Instance pertence a um jogador até ser transferido.

---

# Movimentação

Toda movimentação deve passar pelo InventoryService.

Exemplos:

- Loot;
- Trade;
- Market;
- Equipamento;
- Venda NPC;
- Descarte.

---

# Salvamento

O inventário utiliza salvamento contínuo.

Eventos importantes devem ser persistidos:

- receber item;
- remover item;
- equipar;
- transferir;
- vender;
- descartar.

---

# Descarte

Fluxo:

```text
Selecionar item

↓

Arrastar para fora do inventário

↓

Confirmação

↓

Excluir item
```

---

# Segurança

O servidor deve validar:

- propriedade do item;
- quantidade;
- espaço disponível;
- bloqueios;
- Market;
- Trade.

Nenhuma alteração de item deve ser confiada ao cliente.

---

# Integrações

InventoryService se comunica com:

- LootService;
- ItemService;
- EquipmentService;
- MarketService;
- TradeService;
- EconomyService;
- CharacterService.

---

# Arquitetura

Modelo:

```text
InventoryService

↓

InventoryContainer

↓

Slot

↓

ItemInstance
```

---

# Pendências

Definir futuramente:

- Interface visual;
- Expansão de slots;
- Sistema de filtros;
- Histórico de movimentação;
- Proteção de itens favoritos.