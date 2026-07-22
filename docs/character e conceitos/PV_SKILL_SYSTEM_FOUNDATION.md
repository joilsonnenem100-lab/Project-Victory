# PROJECT-VICTORY --- Skill System Foundation

## Status

Documento de consolidação das decisões de Product Owner relacionadas ao
sistema base de Skills.

Este documento define a fundação do sistema e separa futuras expansões.

------------------------------------------------------------------------

# 1. Objetivo do Sistema de Skills

Skills representam a evolução das capacidades do personagem.

Elas conectam:

Character

↓

Progression

↓

Combat

↓

Gameplay Identity

------------------------------------------------------------------------

# 2. Modelo de Desbloqueio

Decisão:

Modelo híbrido.

Algumas habilidades básicas existem inicialmente.

Outras podem ser liberadas através da progressão.

Fluxo:

Character Creation

↓

Skills iniciais

↓

Level Progression

↓

Novas habilidades

------------------------------------------------------------------------

# 3. Treinamento de Skills

## Fase inicial

Skills evoluem somente através de uso real.

Exemplo:

Sword Fighting:

Combate usando espada

↓

Evolução da skill

------------------------------------------------------------------------

## Futuro

NPCs poderão fornecer treinamento.

Possibilidade futura:

NPC Trainer

↓

Treino

↓

Progressão da Skill

------------------------------------------------------------------------

# 4. Velocidade de Progressão

Decisão:

Cada classe possui velocidade própria de evolução.

A classe influencia:

-   velocidade de aprendizado;
-   especialização;
-   identidade de gameplay.

Exemplo:

Knight:

Maior evolução em skills físicas.

Sorcerer:

Maior evolução em Magic Level.

------------------------------------------------------------------------

# 5. Limite de Skill

Decisão:

Skills possuem escala livre inicialmente.

Não existe limite máximo definido na primeira versão.

O balanceamento será tratado posteriormente.

------------------------------------------------------------------------

# 6. Escolha de Skills no Level Up

Decisão:

Não existe escolha do jogador no Level Up.

A progressão é definida pela classe.

Fluxo:

Level Up

↓

Sistema identifica desbloqueios

↓

Aplica progressão

------------------------------------------------------------------------

# 7. Raridade de Skills

Decisão:

Não existe sistema de raridade inicialmente.

Todas as skills seguem progressão normal.

Sistemas futuros podem adicionar:

-   habilidades raras;
-   habilidades especiais;
-   habilidades lendárias.

------------------------------------------------------------------------

# 8. Skills Exclusivas de Classe

Decisão:

Modelo híbrido.

Existem:

-   habilidades compartilhadas;
-   habilidades exclusivas de classe.

Exemplo conceitual:

Knight:

habilidades defensivas e físicas.

Sorcerer:

habilidades mágicas ofensivas.

Druid:

habilidades de suporte.

------------------------------------------------------------------------

# 9. Skills Base do Sistema

## Implementação inicial

Entram:

-   Sword Fighting
-   Axe Fighting
-   Club Fighting
-   Distance Fighting
-   Shielding
-   Magic Level

------------------------------------------------------------------------

## Futuro

Não entram inicialmente:

-   Fist Fighting
-   Fishing

Fishing permanece como habilidade secundária futura.

------------------------------------------------------------------------

# 10. Relação com Classes

Classes definem:

-   quais skills possuem maior velocidade;
-   quais habilidades podem desbloquear;
-   identidade de combate.

------------------------------------------------------------------------

# 11. Relação com Character

Character possui:

-   Skills atuais;
-   progresso das skills;
-   evolução acumulada.

Skills não substituem o Character.

Elas fazem parte da progressão do personagem.

------------------------------------------------------------------------

# 12. Estado do Bloco

Definido:

-   modelo de desbloqueio;
-   treinamento inicial;
-   progressão por classe;
-   ausência de limite inicial;
-   ausência de escolha no level up;
-   ausência de raridade inicial;
-   habilidades exclusivas híbridas;
-   conjunto inicial de skills.

Próximos blocos:

-   Attribute System;
-   Combat Formula;
-   Advanced Skill Tree;
-   Equipment Interaction.
