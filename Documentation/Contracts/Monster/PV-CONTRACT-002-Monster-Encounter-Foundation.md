# PV-CONTRACT-002 --- Monster Encounter Foundation

## Local recomendado

Project-Victory/Documentation/Contracts/Monster/

------------------------------------------------------------------------

# Objetivo

Implementar a fundação necessária para conectar:

Hunt

↓

Encounter

↓

Monster

↓

Combat

------------------------------------------------------------------------

# Decisões de Produto Confirmadas

## Primeira Hunt

Nome:

Ascendancy Path

------------------------------------------------------------------------

# Geração de Encounter

Os monstros disponíveis são utilizados de forma randômica.

A composição do encontro depende dos monstros disponíveis na Hunt.

------------------------------------------------------------------------

# Quantidade de monstros

Cada Encounter gera:

3 monstros simultaneamente.

------------------------------------------------------------------------

# Seleção de alvo

Quando existir um pack de monstros:

O alvo inicial é selecionado automaticamente.

Regra:

Primeiro monstro do pack gerado.

------------------------------------------------------------------------

# Sistema de Level de Monstro

Monstros não possuem level inicialmente.

A dificuldade é definida pelos seus atributos próprios.

------------------------------------------------------------------------

# Monstros MVP

## Rat

Stats:

HP:

20

Damage:

5

Experience:

50

Gold:

1-5

Drop:

Nenhum definido inicialmente.

------------------------------------------------------------------------

## Cave Rat

Stats:

HP:

50

Damage:

10

Experience:

100

Gold:

2-6

Drop:

Nenhum definido inicialmente.

------------------------------------------------------------------------

# Comportamento dos Monstros

Inicialmente:

Somente ataque básico.

Não implementar:

-   habilidades;
-   fuga;
-   comportamento avançado.

------------------------------------------------------------------------

# Informação visual

O jogador visualiza:

-   nome do monstro;
-   HP do monstro.

------------------------------------------------------------------------

# Respawn / Próximo Encounter

Após derrota:

Novo encontro pode ser gerado.

Fluxo:

Monster Defeated

↓

Reward

↓

Next Encounter

------------------------------------------------------------------------

# Remoção do Monstro

Após derrota:

O monstro desaparece imediatamente.

------------------------------------------------------------------------

# Critérios de Validação

## Teste 1

Iniciar Ascendancy Path.

Resultado:

Um Encounter com 3 monstros gerados.

## Teste 2

Gerar Rat.

Esperado:

HP 20

Damage 5

XP 50

Gold 1-5

## Teste 3

Gerar Cave Rat.

Esperado:

HP 50

Damage 10

XP 100

Gold 2-6

------------------------------------------------------------------------

# Fora de Escopo

Não implementar:

-   Monster Level;
-   AI avançada;
-   habilidades de monstros;
-   elites;
-   bosses;
-   loot complexo.

------------------------------------------------------------------------

# Próximo contrato

PV-CONTRACT-003 --- Combat MVP
