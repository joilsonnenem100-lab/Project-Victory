# PROJECT-VICTORY ITEM SYSTEM DESIGN

## Documento de Visão --- Items, Equipment, Slots, Affixes e Progressão

Status: - Visão de produto - Arquitetura futura - Conceitos
complementares ao Vertical Slice 2.0 - Não representa implementação
concluída

------------------------------------------------------------------------

# 1. Filosofia do Sistema de Itens

No Project-Victory, itens não devem ser apenas valores numéricos.

O sistema deve permitir:

-   identidade do personagem;
-   criação de builds;
-   escolhas estratégicas;
-   progressão;
-   economia;
-   interação social.

O item deve responder:

"Como este jogador escolheu evoluir?"

------------------------------------------------------------------------

# 2. Separação Fundamental

## Item Definition

Representa o modelo do item.

Exemplo:

Iron Sword

Contém:

-   Id
-   Nome
-   Categoria
-   Tipo
-   Características base
-   Regras permitidas

Não pertence a nenhum jogador.

------------------------------------------------------------------------

## Item Instance

Representa uma cópia real existente no mundo.

Exemplo:

Iron Sword #839291

Possui:

-   InstanceId
-   DefinitionId
-   Owner
-   Qualidade
-   Durabilidade
-   Afixos
-   Modificadores gerados

------------------------------------------------------------------------

Fluxo:

Item Definition

↓

Item Generator

↓

Item Instance

↓

Inventory

↓

Equipment

------------------------------------------------------------------------

# 3. Item Categories

Categorias futuras:

-   Weapon
-   Armor
-   Accessory
-   Consumable
-   Material
-   Quest Item
-   Crafting Resource

Cada categoria possui regras próprias.

------------------------------------------------------------------------

# 4. Equipment System

Equipamento representa itens ativos no personagem.

Fluxo:

Character

↓

Equipment

↓

Equipped Items

↓

Character Stats

↓

Combat

------------------------------------------------------------------------

# 5. Equipment Slots

Slots representam locais onde equipamentos podem ser utilizados.

Modelo conceitual:

-   Head
-   Chest
-   Legs
-   Feet
-   Main Hand
-   Off Hand
-   Ring
-   Necklace
-   Accessory

------------------------------------------------------------------------

# 6. Inventory Slots

Inventory é diferente de Equipment.

Inventory:

"onde guardo"

Equipment:

"onde utilizo"

Exemplo:

Inventory Slot 15:

Iron Sword

↓

Equipar

↓

Main Hand Slot

------------------------------------------------------------------------

# 7. Two Handed Rules

Itens podem possuir regras:

Exemplo:

Great Sword

Requer:

-   Main Hand
-   Off Hand

O sistema deve validar compatibilidade antes de equipar.

------------------------------------------------------------------------

# 8. Affix System

Affixes são modificadores que tornam itens únicos.

Modelo:

Item Base

-   

Affixes

=

Item Final

------------------------------------------------------------------------

Exemplo:

Sword

-   

Strong Prefix

-   

Fire Damage Suffix

=

Flaming Sword of Strength

------------------------------------------------------------------------

# 9. Affix Definition

Um afixo possui:

-   Id
-   Nome
-   Tipo de atributo
-   Valor mínimo
-   Valor máximo
-   Tier
-   Restrições

------------------------------------------------------------------------

Exemplo:

Strong III

Strength:

10-20

------------------------------------------------------------------------

# 10. Prefix e Suffix

Prefix:

Antes do nome.

Exemplo:

Flaming Sword

Suffix:

Depois do nome.

Exemplo:

Sword of Power

------------------------------------------------------------------------

# 11. Affix Tier System

Afixos podem possuir níveis.

Exemplo:

Strength I

+5

Strength II

+10

Strength III

+20

------------------------------------------------------------------------

Isso permite progressão e raridade.

------------------------------------------------------------------------

# 12. Quality System

Qualidade controla potencial.

Exemplo:

Common

0 afixos

Magic

1 afixo

Rare

2-3 afixos

Epic

3-4 afixos

Legendary

Regras especiais

------------------------------------------------------------------------

# 13. Legendary Items

Itens lendários devem possuir identidade.

Não apenas números maiores.

Exemplo:

Dragon Fang Sword

Regra:

Ataques críticos aplicam queimadura.

------------------------------------------------------------------------

# 14. Item Stat Pipeline

Processamento:

Item Definition

↓

Base Stats

↓

Quality

↓

Affix Generation

↓

Sockets

↓

Enhancements

↓

Final Stats

------------------------------------------------------------------------

# 15. Loot Generation

Futuro:

Monster

↓

Combat Result

↓

Reward

↓

Loot Generator

↓

Item Instance

↓

Inventory

------------------------------------------------------------------------

# 16. Bind Rules

Possíveis regras:

-   Tradable
-   Bind on Equip
-   Bind on Pickup
-   Account Bound

------------------------------------------------------------------------

# 17. Durability

Sistema futuro.

Item:

Durability 100/100

Uso:

↓

Desgaste

↓

Repair

------------------------------------------------------------------------

# 18. Upgrade System

Exemplo:

Sword +1

↓

Sword +2

↓

Sword +3

Pode envolver:

-   materiais;
-   ouro;
-   chance;
-   risco.

------------------------------------------------------------------------

# 19. Socket e Gem System

Itens podem possuir espaços.

Exemplo:

Helmet

Socket:

-   

Ruby

Resultado:

+Health

------------------------------------------------------------------------

# 20. Crafting Integration

Futuro:

Materials

↓

Crafting

↓

Item Definition

↓

Item Instance

------------------------------------------------------------------------

# 21. Item Economy

Itens influenciam:

-   mercado;
-   comércio;
-   valor;
-   raridade;
-   comunidade.

------------------------------------------------------------------------

# 22. Relação com Character Build

Item deve criar identidade.

Exemplo:

Tank:

-   Health
-   Defense
-   Block

Damage:

-   Critical
-   Attack
-   Speed

Magic:

-   Mana
-   Spell Power

------------------------------------------------------------------------

# 23. Relação com Skills

Fluxo:

Equipment

↓

Stats

↓

Skills

↓

Combat

------------------------------------------------------------------------

# 24. Relação com Combat

Fluxo futuro:

Combat

↓

Reward

↓

Loot

↓

Equipment

↓

Character Build

↓

Novo Combat

------------------------------------------------------------------------

# 25. Persistência

Possível separação:

Item Definition

(Content)

↓

Item Instance

(Player Data)

↓

Inventory

↓

Equipment

------------------------------------------------------------------------

# 26. Arquitetura Macro

Monster

↓

Combat

↓

Reward

↓

Loot

↓

Item Instance

↓

Inventory

↓

Equipment

↓

Character Build

↓

Progression

------------------------------------------------------------------------

# 27. Estado Atual Project-Victory

Confirmado:

Item Definition

Item Characteristics

Persistência inicial

------------------------------------------------------------------------

Ainda futuro:

Item Instance

Inventory completo

Equipment completo

Slots oficiais

Affix Generation

Loot System

Crafting

Economy

------------------------------------------------------------------------

# 28. Direção Final

O sistema de itens deve ser uma ferramenta de expressão do jogador.

O objetivo não é apenas aumentar números.

O objetivo é permitir:

-   escolhas;
-   especializações;
-   builds;
-   identidade;
-   interação social.
