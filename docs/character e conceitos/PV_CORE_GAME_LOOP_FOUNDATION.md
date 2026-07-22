# PROJECT-VICTORY --- Core Game Loop Foundation

## Documento de consolidação das decisões do primeiro ciclo jogável

## Objetivo

Fechar o primeiro loop funcional do Project-Victory:

Login → Character → World → Hunt → Combat → Reward → Progression

Este documento consolida somente decisões já definidas.

------------------------------------------------------------------------

# Character System

O personagem representa a identidade permanente do jogador.

Informações principais:

-   Level
-   Experience
-   HP
-   Mana
-   Skills

Skills:

-   Magic Level
-   Sword Fighting
-   Axe Fighting
-   Club Fighting
-   Distance Fighting

Informações avançadas:

-   Armour
-   Resistances
-   Elemental Damages

------------------------------------------------------------------------

# Attribute System

O Project-Victory não utiliza:

-   Strength
-   Vitality
-   Intelligence
-   Dexterity

O modelo é baseado em:

-   HP
-   Mana
-   Armour
-   Resistances
-   Elemental Damages
-   Skills
-   Equipment Bonuses

------------------------------------------------------------------------

# Crescimento

Não existe distribuição manual de pontos.

Level Up:

Classe define crescimento.

Equipamentos e Skills alteram o estado do personagem.

HP e Mana:

-   crescimento automático por classe.

Regeneração:

-   não existe inicialmente.

------------------------------------------------------------------------

# Morte

Fluxo:

Alive

↓

Dead

↓

Respawn

Sem perda inicial de:

-   XP
-   itens
-   progresso

------------------------------------------------------------------------

# Combat MVP

Modelo:

Combate por turnos.

Fluxo:

Player Action

↓

Damage Resolution

↓

Monster Response

↓

Next Turn

------------------------------------------------------------------------

Ações iniciais:

-   Attack
-   Skill
-   Item

Skills existem desde o início.

Dano considera:

-   Classe
-   Level
-   Skills
-   Equipment
-   Estado do personagem

------------------------------------------------------------------------

# Monster MVP

Tipos:

-   Normal
-   Elite
-   Boss

Inicialmente sem evolução dinâmica.

Monstros iniciais:

-   Rat
-   Cave Rat

------------------------------------------------------------------------

# Item MVP

Primeiro fluxo:

Monster

↓

Combat

↓

Reward

↓

Item Drop

↓

Equipment

------------------------------------------------------------------------

Primeiro loot:

Somente equipamentos.

Não entram inicialmente:

-   Consumíveis
-   Materiais
-   Crafting

------------------------------------------------------------------------

Equipamentos iniciais:

Knight:

-   Sword
-   Shield
-   Armor

Paladin:

-   Bow
-   Shield
-   Armor

Mage:

-   Staff
-   Robe

------------------------------------------------------------------------

Raridade:

Inicialmente todos os itens são normais.

Sem:

-   Rare
-   Epic
-   Legendary

------------------------------------------------------------------------

# Loop básico definido

LOGIN

↓

CHARACTER

↓

ENTER WORLD

↓

HUNT

↓

MONSTER ENCOUNTER

↓

TURN COMBAT

↓

REWARD

↓

XP + ITEM

↓

CHARACTER UPDATE

↓

NEXT HUNT

------------------------------------------------------------------------

# Pendências antes dos contratos

Reward System MVP:

-   XP por monstro
-   Level progression
-   Gold
-   Chance de item

Skill System MVP:

-   skills iniciais
-   custo
-   efeito
-   desbloqueio

Combat Turn Structure:

-   iniciativa
-   acerto
-   crítico
-   ordem dos turnos

Hunt Loop:

-   quantidade de monstros
-   encerramento
-   retorno
-   recompensas

------------------------------------------------------------------------

# Objetivo da fase

Transformar o Project-Victory em um ciclo jogável:

Login → Hunt → Combat → Loot → Progression
