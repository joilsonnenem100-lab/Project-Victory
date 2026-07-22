# PROJECT-VICTORY --- Combat Turn Structure MVP Foundation

## Objetivo

Definir o modelo inicial de combate do Project-Victory para o primeiro
ciclo jogável.

Escopo:

Player

↓

Monster

↓

Turnos

↓

Morte

↓

Reward

Não faz parte deste bloco:

-   PvP;
-   resistências avançadas;
-   status effects;
-   balanceamento final;
-   inteligência avançada de monstros.

------------------------------------------------------------------------

# 1. Início do Combate

Decisão:

O jogador sempre inicia o combate.

Fluxo:

Encounter

↓

Player Turn

↓

Monster Turn

------------------------------------------------------------------------

# 2. Ações do Jogador

O turno do jogador possui ações principais:

## Ataque Básico

Ação padrão de combate.

------------------------------------------------------------------------

## Poção de Cura OU Skill de Heal

O jogador pode utilizar recuperação.

Regra:

Não utiliza simultaneamente:

-   poção de cura;
-   skill de heal.

------------------------------------------------------------------------

## Spell Ofensivo

O jogador pode utilizar magias ofensivas.

Exemplos:

-   Exori Vis;
-   Exori Tera.

------------------------------------------------------------------------

# 3. Ações dos Monstros

Inicialmente:

Monstros possuem apenas auto ataque.

Fluxo:

Monster Turn

↓

Basic Attack

------------------------------------------------------------------------

Spells de monstros ficam para uma evolução futura.

------------------------------------------------------------------------

# 4. Chance de Acerto

Decisão:

Existe chance de acerto inicialmente.

Ataques podem:

-   acertar;
-   errar.

------------------------------------------------------------------------

# 5. Critical System

Decisão:

Crítico não faz parte do MVP.

Sistema fica para evolução futura.

------------------------------------------------------------------------

# 6. Cálculo de Dano

Decisão:

O dano inicial será baseado em:

-   arma;
-   skill;
-   regras de combate.

Não será somente um valor fixo.

Fluxo:

Equipment / Skill

↓

Damage Calculation

↓

Combat Result

------------------------------------------------------------------------

# 7. Morte

Quando HP chega a zero:

O combate termina imediatamente.

Fluxo:

HP \<= 0

↓

Combat End

↓

Reward

------------------------------------------------------------------------

# 8. Fuga do Combate

Decisão:

O jogador pode sair da Hunt.

A ação:

Exit Hunt

retorna o jogador para a tela inicial.

------------------------------------------------------------------------

# 9. Após Derrotar Monstro

Fluxo:

Monster Dead

↓

Reward

↓

Next Encounter

------------------------------------------------------------------------

A continuidade da Hunt acontece automaticamente.

------------------------------------------------------------------------

# 10. Log de Combate

Existe visualização de combate.

Exemplo:

Knight attacked Rat

Damage: XX

Rat HP: XX/XX

------------------------------------------------------------------------

# Estado Atual

Definido:

-   jogador inicia combate;
-   ações de jogador;
-   auto ataque dos monstros;
-   chance de acerto;
-   ausência de crítico inicial;
-   dano baseado em arma/skill;
-   morte imediata;
-   Exit Hunt;
-   progressão automática para próximo encontro;
-   log de combate.

------------------------------------------------------------------------

# Próximo bloco

PV --- Hunt Loop Final Foundation
