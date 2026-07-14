# Loot

# Objetivo

O sistema de Loot é responsável por determinar quais Item Templates serão gerados quando uma fonte de recompensa produzir itens.

O sistema de Loot nunca gera diretamente Item Instances.

Sua responsabilidade termina na seleção dos Item Templates.

A criação das Item Instances pertence exclusivamente à Item Factory.

---

# Arquitetura

O sistema de Loot é composto pelos seguintes conceitos:

- Loot Source
- Loot Table
- Loot Entry

Cada componente possui responsabilidade única.

---

# Loot Source

Loot Source representa qualquer origem capaz de produzir recompensas.

Exemplos:

- Monsters
- Bosses
- Chests
- Quests
- Events
- Crafting
- Outras fontes futuras

Toda Loot Source referencia exatamente uma Loot Table.

---

# Loot Table

Loot Table representa a coleção oficial de possíveis recompensas de uma Loot Source.

Uma Loot Table é composta por múltiplos Loot Entries.

A Loot Table nunca conhece jogadores, vocações ou inventários.

Sua responsabilidade é apenas definir quais Item Templates podem ser selecionados.

---

# Loot Entry

Loot Entry representa uma possível recompensa.

Cada Loot Entry define apenas:

- Item Template
- Chance de obtenção
- Quantidade mínima
- Quantidade máxima

Loot Entry nunca conhece:

- Rarity
- Tier
- Affixes
- Item Instance

Esses conceitos pertencem a outros sistemas.

---

# Loot Pipeline

O fluxo oficial de geração de loot é:

Loot Source

↓

Loot Table

↓

Loot Entry

↓

Item Template

↓

Item Factory

↓

Item Instance

↓

Inventário do jogador

Toda geração aleatória de Rarity e Affixes ocorre exclusivamente durante a criação da Item Instance.

---

# Relação com Items

Loot nunca gera diretamente um Item.

Loot seleciona apenas um Item Template.

A transformação do Template em Item Instance pertence exclusivamente à Item Factory.

---

# Relação com Rarity

Loot não conhece Rarity.

A definição da Rarity ocorre posteriormente durante a geração da Item Instance.

---

# Relação com Affixes

Loot nunca gera Affixes.

Affixes pertencem exclusivamente ao processo de criação da Item Instance.

---

# Relação com Tier

Loot não altera o Tier de um Item.

O Tier pertence permanentemente ao Item Template.

---

# Relação com Character

O sistema de Loot não depende do Character.

Loot não conhece:

- Level
- Skills
- Vocação
- Equipamentos
- Progressão

Toda seleção de recompensas ocorre de forma independente.

---

# Princípios Arquiteturais

- Loot seleciona apenas Item Templates.
- Loot nunca cria Item Instances.
- Loot nunca gera Affixes.
- Loot nunca altera Tier.
- Loot não depende do Character.
- Toda criação de itens pertence exclusivamente à Item Factory.