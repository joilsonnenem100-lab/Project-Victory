# PV-CONTRACT-001 --- Character Progression Foundation

## Objetivo

Implementar a fundação do sistema de personagem necessária para o
primeiro ciclo jogável do Project-Victory.

Este contrato prepara:

Character

↓

Class

↓

Level

↓

Experience

↓

HP / Mana

↓

Skills

------------------------------------------------------------------------

# Decisões de Produto Confirmadas

## Level Inicial

Todos os personagens começam em:

Level 1

------------------------------------------------------------------------

## HP e Mana Inicial

Valores base confirmados:

  Classe        HP   Mana
  ---------- ----- ------
  Knight       185     55
  Paladin      155     95
  Sorcerer     125    140
  Druid        125    140
  Monk         150     80

------------------------------------------------------------------------

## Skills Iniciais

As skills iniciais começam desbloqueadas no Level 1.

------------------------------------------------------------------------

# Classes

Classes suportadas:

-   Knight
-   Paladin
-   Sorcerer
-   Druid
-   Monk

------------------------------------------------------------------------

# Crescimento por Level

O crescimento automático segue a identidade da classe.

## Knight

Por level:

-   +15 HP
-   +5 Mana

## Paladin

Por level:

-   +10 HP
-   +15 Mana

## Sorcerer

Por level:

-   +5 HP
-   +30 Mana

## Druid

Por level:

-   +5 HP
-   +30 Mana

## Monk

Por level:

-   +10 HP
-   +10 Mana

------------------------------------------------------------------------

# Skills Iniciais

## Knight

Ground Slam

------------------------------------------------------------------------

## Paladin

Exori Con

------------------------------------------------------------------------

## Sorcerer

Exori Vis

------------------------------------------------------------------------

## Druid

Exori Tera

------------------------------------------------------------------------

## Monk

Power Punch

------------------------------------------------------------------------

# Character State Necessário

O Character deve suportar:

-   Class
-   Level
-   Experience
-   Current HP
-   Maximum HP
-   Current Mana
-   Maximum Mana
-   Skills

------------------------------------------------------------------------

# Regras de Progressão

Implementar:

-   ganho de experiência;
-   cálculo de level;
-   crescimento automático;
-   atualização de HP/Mana.

------------------------------------------------------------------------

# Fora de Escopo

Não implementar:

-   Strength;
-   Vitality;
-   Intelligence;
-   Dexterity;
-   distribuição manual de pontos;
-   árvore de skills;
-   equipamentos;
-   resistências avançadas;
-   combate.

------------------------------------------------------------------------

# Critérios de Validação

## Teste 1 --- Knight

Criar Knight.

Esperado:

-   Level 1;
-   HP 185;
-   Mana 55;
-   Ground Slam disponível.

## Teste 2 --- Sorcerer

Criar Sorcerer.

Esperado:

-   Level 1;
-   HP 125;
-   Mana 140;
-   Exori Vis disponível.

## Teste 3 --- Level Up

Aplicar experiência suficiente.

Esperado:

-   aumento de Level;
-   crescimento conforme classe.

------------------------------------------------------------------------

# Resultado Esperado

Após conclusão:

Character possui identidade inicial completa para participar do Core
Gameplay Loop.

Próximo contrato:

PV-CONTRACT-002 --- Monster Encounter Foundation
