# PROJECT-VICTORY --- Core Gameplay Loop Contract Preparation

## Objetivo

Este documento transforma as decisões de produto do primeiro ciclo
jogável em uma preparação para contratos de implementação.

Fluxo alvo:

Login

↓

Character

↓

Tela Inicial

↓

Hunt

↓

Monster Encounter

↓

Turn Combat

↓

Reward

↓

Progression

------------------------------------------------------------------------

# 1. Escopo do MVP

O objetivo inicial é construir um ciclo jogável completo.

Incluído:

-   Login;
-   criação/seleção de personagem;
-   entrada no mundo;
-   Hunt;
-   encontro com monstros;
-   combate;
-   recompensa;
-   progressão.

Fora do escopo inicial:

-   PvP;
-   Crafting;
-   Economia avançada;
-   Affixes;
-   Legendary Items;
-   Talent Tree;
-   Status Effects avançados;
-   sistemas sociais complexos.

------------------------------------------------------------------------

# 2. Sistemas do Core Loop

## Account

Responsabilidade:

-   autenticação;
-   acesso do jogador.

Estado:

Fundação existente.

------------------------------------------------------------------------

## Character

Responsabilidade:

Representar a identidade permanente do jogador.

Dados:

-   Classe;
-   Level;
-   Experience;
-   HP;
-   Mana;
-   Skills.

Classes:

-   Knight;
-   Paladin;
-   Sorcerer;
-   Druid;
-   Monk.

------------------------------------------------------------------------

## Class System

Responsabilidade:

Definir:

-   crescimento de HP;
-   crescimento de Mana;
-   habilidades iniciais;
-   progressão.

------------------------------------------------------------------------

## Skill System MVP

Cada classe inicia com uma habilidade.

Knight:

Ground Slam

Paladin:

Exori Con

Sorcerer:

Exori Vis

Druid:

Exori Tera

Monk:

Power Punch

Regras:

-   Skills consomem Mana;
-   possuem cooldown;
-   desbloqueio por level;
-   dano fixo inicialmente.

------------------------------------------------------------------------

## Combat System MVP

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

Ações do jogador:

-   Ataque básico;
-   Poção de cura;
-   Skill de Heal;
-   Spell ofensivo.

Monstros:

Inicialmente:

-   Auto Attack.

------------------------------------------------------------------------

## Monster System

Primeiros monstros:

-   Rat;
-   Cave Rat.

Responsabilidades:

-   combate;
-   HP;
-   dano;
-   recompensa.

------------------------------------------------------------------------

## Reward System

Fluxo:

Monster Dead

↓

XP

↓

Gold

↓

Item Drop

Rat:

XP:

50

Gold:

1-5

Drop:

Nenhum.

------------------------------------------------------------------------

## Hunt System

Primeira Hunt:

Ascendancy Path

Configuração:

-   3 encontros;
-   composição fixa;
-   level recomendado 1.

Saída:

Exit Hunt:

-   espera 5 segundos;
-   retorna para tela inicial.

Reentrada:

-   espera inicial de 10 segundos;
-   pode aumentar futuramente.

------------------------------------------------------------------------

# 3. Ordem recomendada de contratos

## Contrato 1

Character Progression Foundation

Objetivo:

-   classes;
-   HP;
-   Mana;
-   Level;
-   XP;
-   Skills.

------------------------------------------------------------------------

## Contrato 2

Monster Encounter Foundation

Objetivo:

-   Hunt;
-   Encounter;
-   criação dos primeiros encontros.

------------------------------------------------------------------------

## Contrato 3

Combat MVP

Objetivo:

-   turnos;
-   ataque;
-   skills;
-   dano;
-   morte.

------------------------------------------------------------------------

## Contrato 4

Reward Integration

Objetivo:

-   XP;
-   Gold;
-   Loot.

------------------------------------------------------------------------

## Contrato 5

Item MVP

Objetivo:

-   Item Definition;
-   Item Instance;
-   Inventory básico;
-   equipamentos iniciais.

------------------------------------------------------------------------

## Contrato 6

Complete Gameplay Loop Validation

Validar:

Login

↓

Character

↓

Hunt

↓

Combat

↓

Reward

↓

Progression

------------------------------------------------------------------------

# 4. Diretriz arquitetural

O fluxo central deve ser:

Character State

↓

Combat

↓

Reward

O combate deve consumir estado consolidado do personagem.

Evitar dependência direta:

Combat consultando individualmente:

-   Item;
-   Skill;
-   Classe;
-   Equipamento.

------------------------------------------------------------------------

# 5. Critério de conclusão do Core Gameplay Loop

O MVP estará concluído quando o jogador conseguir:

1.  Entrar no jogo.
2.  Possuir um personagem.
3.  Entrar em uma Hunt.
4.  Lutar contra monstros.
5.  Derrotar monstros.
6.  Receber XP.
7.  Receber Gold.
8.  Evoluir personagem.
9.  Repetir o ciclo.

------------------------------------------------------------------------

# Próxima etapa

Criação dos contratos de implementação seguindo esta ordem.
