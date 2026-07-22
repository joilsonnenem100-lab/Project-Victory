# PV-CONTRACT-007 --- Core Gameplay Loop Implementation Order

## Local recomendado

Project-Victory/Documentation/Contracts/Execution/

------------------------------------------------------------------------

# Objetivo

Definir a ordem oficial de implementação do primeiro Vertical Slice
jogável do Project-Victory.

Este contrato organiza a execução dos contratos de gameplay já
aprovados.

------------------------------------------------------------------------

# Ordem oficial de implementação

A implementação seguirá:

Character

↓

Monster

↓

Combat

↓

Reward

↓

Item

↓

Validation

------------------------------------------------------------------------

# FASE 1 --- Character Foundation

Contrato:

PV-CONTRACT-001 --- Character Progression Foundation

Objetivo:

Criar personagem funcional com:

-   Classe;
-   Level;
-   Experience;
-   HP;
-   Mana;
-   Skills.

Resultado esperado:

Character pronto para participar do gameplay loop.

------------------------------------------------------------------------

# FASE 2 --- Monster Encounter

Contrato:

PV-CONTRACT-002 --- Monster Encounter Foundation

Objetivo:

Criar:

-   Hunt;
-   Encounter;
-   Monster Pack.

Monstros iniciais:

-   Rat;
-   Cave Rat.

Resultado esperado:

O jogo consegue gerar encontros.

------------------------------------------------------------------------

# FASE 3 --- Combat

Contrato:

PV-CONTRACT-003 --- Combat MVP

Objetivo:

Implementar:

-   início de combate;
-   turnos;
-   ataque;
-   skills;
-   dano;
-   morte.

Resultado esperado:

Jogador consegue derrotar monstros.

------------------------------------------------------------------------

# FASE 4 --- Reward

Contrato:

PV-CONTRACT-004 --- Reward Integration

Objetivo:

Implementar:

-   XP;
-   Gold;
-   Reward Flow.

Resultado esperado:

Monstros derrotados geram progressão.

------------------------------------------------------------------------

# FASE 5 --- Item

Contrato:

PV-CONTRACT-005 --- Item MVP

Objetivo:

Implementar:

-   Item Definition;
-   Item Instance;
-   Inventory;
-   Equipment.

Resultado esperado:

Jogador recebe e equipa itens.

------------------------------------------------------------------------

# FASE 6 --- Validation

Contrato:

PV-CONTRACT-006 --- Complete Gameplay Loop Validation

Objetivo:

Validar integração completa.

Fluxo:

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

Loot

↓

Progression

------------------------------------------------------------------------

# Regras de Execução

Não implementar antecipadamente:

-   PvP;
-   Crafting;
-   Affixes;
-   Raridade;
-   Boss System;
-   Economy avançada;
-   Talent Tree.

------------------------------------------------------------------------

# Critério de aceite do Vertical Slice

O ciclo será considerado funcional quando o jogador conseguir:

1.  Entrar no jogo.
2.  Criar personagem.
3.  Escolher Hunt.
4.  Encontrar monstros.
5.  Combater.
6.  Visualizar HP/Mana.
7.  Derrotar monstros.
8.  Receber XP.
9.  Receber Gold.
10. Receber item.
11. Equipar item.
12. Evoluir personagem.
13. Repetir Hunt.

------------------------------------------------------------------------

# Aprovação

Ordem aprovada pelo Product Owner:

Character

↓

Monster

↓

Combat

↓

Reward

↓

Item

↓

Validation
