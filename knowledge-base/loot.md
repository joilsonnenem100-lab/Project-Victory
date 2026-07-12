# Loot System

Version: 1.0

Status: LOCKED

Last Sprint: SPRINT-026

---

# Objetivo

Definir o funcionamento oficial do sistema de Loot do Project-Victory.

O Loot representa a principal fonte de progressão, economia e recompensa do jogador.

---

# Filosofia

No Project-Victory, um Loot representa alegria.

Também representa:

- surpresa;
- expectativa;
- recompensa;
- descoberta.

---

# Papel do Loot

O Loot conecta:

```
Monster

↓

Death Event

↓

Loot Calculation

↓

Inventory

↓

Equipment

↓

Economia
```

---

# Fluxo Oficial

Quando um MonsterInstance morre:

```
Monster Death

↓

EXP Calculation

↓

Gold Calculation

↓

Item Drop Calculation

↓

Collection Check

↓

Achievement Check
```

Esta ordem é oficial.

---

# Loot Individual

Cada MonsterInstance calcula seu Loot individualmente.

Exemplo:

```
Pack

Dragon

Dragon

Dragon Elite
```

Resultado possível:

```
Dragon #1

↓

Dragon Leather


Dragon #2

↓

Gold


Dragon Elite

↓

Fire Sword Rare
```

Os monstros não compartilham Loot.

---

# Loot Table

Cada MonsterTemplate possui sua própria Loot Table.

Exemplo:

```
Dragon

↓

LootTable

↓

Dragon Leather

Dragon Ham

Fire Sword

Gold
```

---

# Gold

Todo monstro gera Gold.

Exemplo:

```
Orc

↓

1~5 Gold
```

Distribuição configurável.

Exemplo inicial:

```
1~3

80%

4~5

20%
```

---

# Materiais

Materiais possuem quantidade própria.

Exemplo:

```
Dragon Leather

Quantidade máxima:

1 unidade
```

A quantidade é definida pela Loot Table.

---

# Equipamentos

Quando um equipamento é sorteado:

```
ItemTemplate

↓

ItemInstanceFactory

↓

ItemInstance

↓

Inventory
```

Cada equipamento é criado como uma instância única.

---

# Raridade

A raridade é definida durante a criação da ItemInstance.

Possíveis resultados:

```
Normal

↓

Magic

↓

Rare

↓

Unique
```

---

# Elite Monster

Elite utiliza a mesma Loot Table.

Diferenças:

- maior chance de itens;
- maior chance de raridade;
- melhor qualidade média.

---

# Boss Loot

Bosses utilizam Loot Table própria.

Isso facilita:

- balanceamento;
- criação de novos Bosses;
- eventos.

---

# Jogador Offline

Jogadores Offline:

- não recebem Loot;
- continuam seguindo as regras de Hunt Offline previamente definidas.

---

# Inventário Cheio

Caso o inventário não possua espaço:

```
Item Gerado

↓

Item Perdido
```

O Gold continua sendo creditado normalmente na Wallet da Account.

---

# Auto Loot

Todo Loot é coletado automaticamente.

Fluxo:

```
Monster Death

↓

Loot

↓

Inventory
```

Nenhuma ação manual é necessária.

---

# Loot Raro

Itens raros podem gerar:

- mensagem para o jogador;
- mensagem global.

A visibilidade poderá ser configurada futuramente.

---

# Recent Loot

Todo item obtido é registrado na interface Recent Loot.

Informações:

- item;
- quantidade;
- horário.

---

# Eventos

Eventos podem alterar:

- chances;
- materiais;
- equipamentos;
- Gold.

Sem modificar permanentemente a Loot Table original.

---

# Arquitetura

```
LootService

↓

LootTable

↓

DropCalculator

↓

ItemInstanceFactory

↓

InventoryService
```

---

# Integrações

LootService comunica com:

- MonsterService;
- ItemService;
- InventoryService;
- CollectionService;
- AchievementService;
- WalletService.

---

# Pendências futuras

Definir:

- fórmulas completas de Drop Rate;
- Lucky Find;
- Founder Rarity;
- modificadores de evento;
- tabelas completas de Loot;
- balanceamento de probabilidades.