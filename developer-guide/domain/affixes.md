# Affixes

# Objetivo

Affixes representam modificadores permanentes gerados durante a criação de uma Item Instance.

Eles tornam cada Item potencialmente único, aumentam a variedade do loot e estimulam a economia do jogo.

Affixes pertencem exclusivamente à Item Instance.

Templates nunca possuem Affixes.

---

# Geração

Affixes são gerados exclusivamente durante a criação da Item Instance.

Após a criação do Item, seus Affixes permanecem permanentes.

O sistema de Affixes não realiza modificações posteriores na Instance.

---

# Relação com Rarity

A quantidade de Affixes é determinada exclusivamente pela Rarity da Item Instance.

Categorias oficiais:

## Normal

Não recebe Affixes.

## Magic

Recebe exatamente um Affix.

## Rare

Recebe entre dois e três Affixes.

A quantidade é determinada aleatoriamente durante a geração da Instance.

Unique Items não utilizam este sistema.

---

# Affix Pool

Todo Item Template define seu próprio Affix Pool.

O Affix Pool determina quais Affixes podem ser sorteados para aquele Template.

Cada Template possui sua própria lista de Affixes válidos.

Nem todo Affix pode aparecer em qualquer Item.

---

# Affixes Duplicados

Uma Item Instance nunca poderá possuir o mesmo Affix mais de uma vez.

Cada Affix pode existir apenas uma única vez na mesma Instance.

---

# Valores

Todo Affix possui um intervalo de valores.

Os valores nunca são fixos.

O valor definitivo é determinado durante a geração da Item Instance.

---

# Tier

O Tier do Item determina o potencial máximo dos valores que um Affix poderá gerar.

Quanto maior o Tier:

- maior poderá ser o intervalo dos valores;
- maior poderá ser a qualidade dos Affixes;
- novos Affixes poderão tornar-se disponíveis.

Tier não altera a quantidade de Affixes.

---

# Rarity

Rarity determina exclusivamente quantos Affixes serão gerados.

Rarity não determina os valores dos Affixes.

Rarity não altera o Tier do Item.

Tier e Rarity são conceitos independentes.

---

# Tipos de Affixes

Affixes podem modificar qualquer atributo permitido pela arquitetura do jogo.

Exemplos:

- Skills
- Health
- Mana
- Attack
- Defense
- Resistências
- Critical Chance
- Critical Damage
- Life Leech
- Mana Leech
- Healing Power
- Outros modificadores definidos futuramente.

---

# Affixes Exclusivos

A arquitetura permite a existência de Affixes exclusivos.

Esses Affixes poderão possuir regras próprias de geração.

Sua implementação será definida futuramente.

---

# Relação com Imbuements

Affixes e Imbuements são sistemas completamente independentes.

## Affix

- permanente;
- nasce junto com a Item Instance;
- não pode ser removido;
- faz parte da identidade permanente do Item.

## Imbuement

- temporário;
- aplicado posteriormente pelo jogador;
- pode expirar;
- pode ser substituído;
- pertence a um sistema arquitetural próprio.

Imbuements serão documentados separadamente.

---

# Princípios Arquiteturais

- Affixes pertencem exclusivamente à Item Instance.
- Templates nunca possuem Affixes.
- Rarity determina apenas a quantidade de Affixes.
- Tier determina o potencial dos Affixes.
- Um mesmo Affix nunca pode ser repetido na mesma Instance.
- Todo Template possui seu próprio Affix Pool.
- Affixes e Imbuements são sistemas independentes.