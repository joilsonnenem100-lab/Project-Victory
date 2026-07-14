# Items

# Objetivo

Items representam objetos pertencentes ao domínio do Project-Victory.

Todo Item é construído seguindo a arquitetura Template → Factory → Instance.

Essa separação garante escalabilidade, reutilização, geração de loot, raridades e evolução futura do sistema sem duplicação de regras.

---

# Arquitetura

Todo Item é composto por três conceitos independentes:

- Item Template
- Item Factory
- Item Instance

Cada um possui responsabilidades específicas.

---

# Item Template

Item Template representa a definição oficial e permanente de um Item.

Um Template descreve apenas as características base do Item.

Exemplos:

- Nome
- Sprite
- Categoria
- Tipo
- Tier
- Attack
- Defense
- Requirements
- Valor base
- Peso

Um Template nunca pertence a um jogador.

Um Template nunca possui Affixes.

Um Template nunca possui Rarity.

Um Template nunca sofre alterações durante sua existência.

---

# Item Factory

Item Factory é responsável por gerar uma nova Item Instance.

Toda aleatoriedade do sistema ocorre exclusivamente durante esse processo.

A Factory é responsável por:

- definir a Rarity;
- determinar a quantidade de Affixes;
- selecionar os Affixes válidos;
- gerar os valores dos Affixes;
- construir a Item Instance final.

---

# Item Instance

Item Instance representa um Item existente dentro do mundo.

Cada Item Instance possui identidade própria.

Uma Instance referencia obrigatoriamente um Item Template.

Cada Instance pode possuir características diferentes das demais.

Exemplos:

- UUID
- Template
- Rarity
- Affixes
- Estado futuro do Item

Duas Instances originadas do mesmo Template podem possuir valores completamente diferentes.

---

# Tier

Todo Item Template pertence a um Tier.

Tier representa o potencial máximo de evolução daquele Item.

Tier influencia diretamente:

- intervalo dos valores dos Affixes;
- qualidade dos Affixes;
- futuras expansões do sistema.

Tier não representa Rarity.

---

# Rarity

Rarity representa a qualidade da Instance gerada.

A Rarity nunca pertence ao Template.

Categorias oficiais:

## Normal

Não recebe Affixes.

## Magic

Recebe exatamente um Affix.

## Rare

Recebe entre dois e três Affixes.

A quantidade é determinada durante a geração da Instance.

---

# Unique Items

Unique Items pertencem a um sistema próprio.

Eles não utilizam geração aleatória de Affixes.

Suas características são definidas manualmente.

A arquitetura para Unique Items será documentada futuramente.

---

# Relação entre Tier e Rarity

Tier e Rarity possuem responsabilidades diferentes.

Tier define o potencial do Item.

Rarity define quantos Affixes serão gerados.

Esses conceitos são independentes.

---

# Relação com Loot

O sistema de Loot nunca gera diretamente um Item Instance.

O Loot apenas referencia um Item Template.

A geração da Instance ocorre posteriormente através da Item Factory.

---

# Relação com Equipamentos

Weapons, Armors, Shields, Spellbooks, Accessories e demais equipamentos são especializações do sistema de Items.

Todos seguem exatamente a mesma arquitetura baseada em Template, Factory e Instance.

---

# Princípios Arquiteturais

- Todo Item possui um Template.
- Toda Instance referencia exatamente um Template.
- Toda geração ocorre através da Factory.
- Tier e Rarity são conceitos independentes.
- Rarity nunca modifica um Template.
- Templates são imutáveis.
- Toda aleatoriedade ocorre exclusivamente durante a criação da Instance.