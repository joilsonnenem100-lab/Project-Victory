# PROJECT-VICTORY --- MASTER ARCHITECTURE MAP

## Purpose

Este documento representa o mapa arquitetural macro do Project-Victory.

Objetivo:

-   preservar o entendimento global do produto;
-   permitir que novas sessões, equipes ou ferramentas compreendam o
    contexto;
-   conectar sistemas existentes, conceitos arquiteturais e visão
    futura.

Este documento NÃO substitui:

-   código fonte oficial;
-   contratos aprovados;
-   documentação oficial;
-   decisões da Arquitetura.

Hierarquia da fonte da verdade:

1.  Código oficial aprovado
2.  Contratos e decisões arquiteturais
3.  Documentação oficial
4.  Master Architecture Map (contexto)

------------------------------------------------------------------------

# Visão Geral

O Project-Victory evolui em três grandes dimensões:

## Player Flow

Representa a jornada do jogador.

Account

↓

Authentication

↓

Game Session

↓

Character

↓

Gameplay

## World Flow

Representa o mundo persistente.

World State

↓

Content

↓

Runtime Instances

↓

Persistence

## System Flow

Representa a execução dos sistemas.

Commands

↓

Domain

↓

Events

↓

Rewards

↓

Progression

------------------------------------------------------------------------

# Evolução do Produto

## Estado atual

Vertical Slice criou:

-   identidade do jogador;
-   autenticação;
-   personagem persistente;
-   entrada no jogo;
-   fundação de Hunt;
-   monstros;
-   combate;
-   recompensa;
-   progressão.

## Próxima evolução

Transformar:

"Executar ações"

em:

"Manter uma experiência contínua"

Com:

-   Runtime State;
-   Game Session;
-   Hunt Loop;
-   Recovery;
-   Checkpoints;
-   Sistemas persistentes.

------------------------------------------------------------------------

# Mapa Macro

ACCOUNT

↓

AUTHENTICATION

↓

PLAYER SESSION

↓

CHARACTER STATE

↓

RUNTIME ENVIRONMENT

↓

RUNTIME STATE

| 

-   Hunt Runtime

-   Quest Runtime

-   Event Runtime

-   Dungeon Runtime

↓

COMBAT

↓

REWARD

↓

PROGRESSION

↓

CHARACTER UPDATE

↓

PERSISTENCE
