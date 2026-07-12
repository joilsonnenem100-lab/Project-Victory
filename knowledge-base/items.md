# Item System

Version: 1.0

Status: LOCKED

Last Sprint: SPRINT-024

---

# Objetivo

Definir o funcionamento oficial dos itens no Project-Victory.

Os itens são uma das principais formas de progressão e economia do jogador.

---

# Filosofia

No Project-Victory, um item representa história.

Um item representa:

- conquista;
- evolução;
- investimento;
- valor econômico;
- memória da jornada.

---

# Estrutura Fundamental

Todo item possui duas camadas:

```
ItemTemplate

↓

ItemInstance
```

---

# ItemTemplate

Representa a definição base do item.

Exemplo:

```
Fire Sword
```

Contém:

- ID interno;
- nome;
- categoria;
- tipo;
- atributos base;
- requisitos;
- regras de geração.

---

# ItemInstance

Representa o item real existente no servidor.

Exemplo:

```
Fire Sword #58291

Rare

Upgrade +10

Affixes
```

Contém:

- Unique Item ID;
- dono atual;
- raridade;
- tier;
- upgrade;
- afixos;
- imbuements.

---

# Criação de Item

Itens são criados através do Loot.

Fluxo:

```
Monster Death

↓

Loot Calculation

↓

ItemInstance Factory

↓

Item criado

↓

Inventory
```

---

# Categorias

Categorias iniciais:

## Equipment

Equipamentos:

- armas;
- armaduras;
- capacetes;
- escudos;
- acessórios.

---

## Material

Exemplo:

```
Dragon Leather
```

Utilizado em:

- craft;
- collections;
- sistemas futuros.

---

## Consumable

Exemplo:

- poções;
- runas;
- itens temporários.

---

## Quest Item

Itens relacionados a sistemas específicos.

---

## Currency

Exemplo:

- Gold;
- VC Points.

---

# Raridade

Ordem:

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

# Tier

Tier representa a qualidade e potencial do item.

Influencia:

- geração de afixos;
- qualidade;
- potencial máximo.

---

# Afixos

Afixos existem em:

```
Magic

Rare
```

Não existem em:

```
Normal
```

---

# Craft Affix

Sistema especial de upgrade.

Após:

```
Upgrade +9
```

existe chance de desbloquear:

```
Craft Affix
```

Limite inicial:

```
2 Craft Affixes
```

---

# Upgrade

Upgrade aumenta:

```
Atributo Base
```

Não altera:

- afixos;
- propriedades existentes.

Fluxo:

```
+0

↓

+9

↓

Possibilidade especial

↓

Craft Affix
```

---

# Imbuement

Itens podem possuir:

```
Imbuement Slots
```

Características:

- podem nascer no drop;
- são permanentes;
- efeito possui duração;
- slot fica vazio após expirar.

---

# Unique Items

Unique possui tabela própria.

Exemplo:

```
Dragon Slayer

Chance controlada pela Unique Table
```

---

# Item Level

Não existe inicialmente.

O poder do item vem de:

```
Tier

+

Rarity

+

Upgrade

+

Affixes

+

Imbuements
```

---

# Proprietário

Item mantém:

```
Dono atual
```

Não existe inicialmente:

- histórico de donos;
- rastreamento de transferências.

---

# Trade

Todos os itens são negociáveis inicialmente.

Não existem:

- Soulbound;
- Account Bound;
- Character Bound.

---

# Stack

Itens stackáveis:

Exemplo:

```
Dragon Leather x666
```

Equipamentos:

```
Não stackam
```

---

# Segurança

Todo ItemInstance possui:

```
Unique Item ID
```

Regra:

Uma instância nunca pode existir duplicada.

---

# Durabilidade

Equipamentos possuem:

- durabilidade;
- repair.

---

# Arquitetura

```
ItemService

↓

ItemTemplateRepository

↓

ItemInstanceFactory

↓

ItemInstance
```

---

# Integrações

ItemService comunica com:

- LootService;
- InventoryService;
- EquipmentService;
- MarketService;
- TradeService;
- CraftService.

---

# Pendências futuras

Definir:

- tabela completa de raridade;
- atributos base;
- categorias completas;
- fórmula de afixos;
- fórmula de upgrade;
- sistema de craft;
- tabela Unique.