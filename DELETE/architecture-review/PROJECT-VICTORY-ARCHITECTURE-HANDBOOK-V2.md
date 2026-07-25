# PROJECT-VICTORY --- ARCHITECTURE HANDBOOK v2.0

## Documento Master de Continuidade Arquitetural

Versão: 2.0

Objetivo:

Este documento representa uma consolidação do conhecimento arquitetural
acumulado do Project-Victory.

Ele existe para permitir que novas equipes, ferramentas ou sessões de
trabalho compreendam:

-   o propósito do projeto;
-   o estado atual;
-   decisões arquiteturais;
-   sistemas existentes;
-   sistemas futuros;
-   limites entre domínios;
-   evolução planejada.

------------------------------------------------------------------------

# 1. Introdução ao Project-Victory

O Project-Victory é concebido como um projeto de jogo com foco em
progressão persistente, ciclos contínuos de gameplay e evolução de
personagem.

A arquitetura deve permitir evolução gradual:

Fundação funcional

↓

Gameplay Foundation

↓

Runtime Engine

↓

Persistent World

------------------------------------------------------------------------

# 2. Princípios Fundamentais

## Separação de Responsabilidades

Cada sistema possui uma responsabilidade clara.

Character representa identidade.

Runtime representa execução.

Combat representa resolução.

Reward representa distribuição.

Progression representa evolução.

------------------------------------------------------------------------

## Estado Permanente vs Estado Temporário

Um dos princípios mais importantes:

Quem o personagem é:

Character State

O que o personagem está fazendo:

Runtime State

Esses conceitos não devem ser misturados.

------------------------------------------------------------------------

# 3. Fonte da Verdade

A hierarquia de confiança:

1.  Código oficial.
2.  Contratos aprovados.
3.  Documentação arquitetural oficial.
4.  Conhecimento auxiliar.

Documentos conceituais não devem substituir decisões oficiais.

------------------------------------------------------------------------

# 4. Estado Atual do Projeto

## Implementado

-   Account Foundation
-   Authentication
-   Character Foundation
-   Persistence
-   Hunt Foundation
-   Monster Foundation
-   Combat Foundation
-   Reward Foundation
-   Progression Foundation

------------------------------------------------------------------------

## Em evolução

-   Hunt Loop
-   Runtime Architecture
-   Session Management

------------------------------------------------------------------------

## Visão futura

-   World State
-   Economy
-   Events
-   AI/NPC
-   Multiplayer evolution

------------------------------------------------------------------------

# 5. Arquitetura Macro

Fluxo principal:

Account

↓

Authentication

↓

Player Context

↓

Character

↓

Runtime Environment

↓

Gameplay Systems

↓

Persistence

------------------------------------------------------------------------

# 6. Account Domain

Responsabilidade:

Controlar identidade externa do jogador.

Não deve controlar:

-   personagem;
-   combate;
-   progressão.

------------------------------------------------------------------------

# 7. Authentication Domain

Responsabilidade:

Validar acesso.

Fluxo:

Credencial

↓

Authentication

↓

Contexto do jogador

------------------------------------------------------------------------

# 8. Character Domain

Character representa:

"Quem o jogador é dentro do mundo."

Inclui:

-   identidade;
-   experiência;
-   nível;
-   atributos;
-   skills futuras;
-   equipamentos futuros.

Não inclui:

-   monstro atual;
-   combate atual;
-   hunt ativa.

------------------------------------------------------------------------

# 9. Runtime Architecture

Runtime representa:

"O que está acontecendo agora."

Ele existe para evitar que sistemas temporários contaminem entidades
permanentes.

Exemplo:

Character:

Knight nível 10.

Runtime:

Knight lutando contra Orc na Dark Forest.

------------------------------------------------------------------------

# 10. Runtime State

Conceito:

Estado de execução temporário ou semi-persistente.

Modelo conceitual:

RuntimeState

-   Id
-   CharacterId
-   Type
-   Status
-   ReferenceId
-   UpdatedAt

------------------------------------------------------------------------

# 11. Runtime Coordinator

Responsabilidade:

Orquestrar ciclos.

Não possui regras profundas.

Ele coordena:

Hunt

↓

Encounter

↓

Combat

↓

Reward

↓

Progression

------------------------------------------------------------------------

# 12. Hunt Architecture

A Hunt evolui de uma ação isolada para um ciclo contínuo.

Modelo:

Hunt Definition

↓

Hunt Session

↓

Encounter Generator

↓

Encounter

↓

Combat

↓

Reward

↓

Progression

↓

Novo Encounter

------------------------------------------------------------------------

# 13. Hunt Definition

Representa conteúdo.

Exemplo:

Dark Forest.

Possui:

-   nome;
-   dificuldade;
-   encontros possíveis;
-   recompensas.

Não representa execução.

------------------------------------------------------------------------

# 14. Hunt Session

Representa a execução.

Exemplo:

Jogador iniciou Dark Forest.

Possui:

-   CharacterId;
-   HuntId;
-   status;
-   tempo;
-   encontro atual.

------------------------------------------------------------------------

# 15. Encounter System

Separação:

Encounter Definition:

Configuração.

Encounter Instance:

Evento real durante gameplay.

------------------------------------------------------------------------

# 16. Monster Architecture

Separação:

Monster Definition:

Modelo.

Monster Instance:

Criatura existente em execução.

------------------------------------------------------------------------

# 17. Combat Architecture

Combat é responsável por:

-   ações;
-   resolução;
-   resultado.

Combat não deve:

-   entregar recompensa;
-   alterar personagem diretamente.

------------------------------------------------------------------------

# 18. Reward Architecture

Combat gera resultado.

Reward transforma resultado em recompensas:

-   XP;
-   gold;
-   itens.

------------------------------------------------------------------------

# 19. Progression Architecture

Progression altera Character.

Exemplo:

XP recebida

↓

Level Check

↓

Character Update

------------------------------------------------------------------------

# 20. Persistence Strategy

Estados permanentes:

-   Account
-   Character
-   Inventory

Estados temporários:

-   cálculos;
-   ações;
-   animações.

Estados intermediários:

-   Hunt Session;
-   Runtime State.

------------------------------------------------------------------------

# 21. Recovery

Objetivo:

Permitir continuidade.

Fluxo:

Servidor inicia

↓

Carrega Runtime

↓

Reconstrói contexto

↓

Continua execução

------------------------------------------------------------------------

# 22. Checkpoints

Salvar em momentos importantes:

-   início de atividade;
-   fim de combate;
-   recompensa;
-   logout.

------------------------------------------------------------------------

# 23. Domain Ownership

Regra:

Um sistema não altera internamente outro sistema.

Fluxo correto:

Combat

↓

Combat Result

↓

Reward

↓

Progression

↓

Character

------------------------------------------------------------------------

# 24. Event Driven Evolution

Futuro:

CombatCompletedEvent

↓

Reward

↓

Achievement

↓

Analytics

------------------------------------------------------------------------

# 25. Content Pipeline

Separação:

Conteúdo

↓

Definition

↓

Factory

↓

Runtime Instance

↓

Persistence

------------------------------------------------------------------------

# 26. Server Authority

Regra:

O cliente solicita.

O servidor decide.

Exemplo:

Cliente:

"ataquei monstro"

Servidor:

"validar e resolver."

------------------------------------------------------------------------

# 27. Segurança

Áreas:

-   autenticação;
-   autorização;
-   validação;
-   integridade do estado.

------------------------------------------------------------------------

# 28. Testes

Tipos:

## Unitários

Regras de domínio.

## Integração

API até persistência.

## Gameplay

Fluxos completos.

------------------------------------------------------------------------

# 29. Governança

Mudanças arquiteturais devem possuir:

-   contexto;
-   impacto;
-   decisão;
-   validação.

------------------------------------------------------------------------

# 30. Vertical Slice

Vertical Slice entrega experiência completa.

Não apenas arquivos.

Inclui:

-   implementação;
-   integração;
-   validação.

------------------------------------------------------------------------

# 31. Débitos Conhecidos

Principais lacunas:

-   Runtime formal;
-   Hunt Loop;
-   Session;
-   World State;
-   Economy.

------------------------------------------------------------------------

# 32. Evolução Futura

Roadmap arquitetural:

Foundation

↓

Runtime

↓

Persistent Gameplay

↓

World Systems

↓

Social Systems

------------------------------------------------------------------------

# 33. Glossário

Aggregate:

Unidade de domínio.

Runtime:

Execução atual.

State:

Condição atual.

Definition:

Configuração.

Instance:

Objeto criado em execução.

------------------------------------------------------------------------

# 34. Diretrizes para Novas IAs

Antes de responder:

-   consultar fonte da verdade;
-   separar oficial de futuro;
-   não assumir decisões;
-   não implementar sem contrato.

------------------------------------------------------------------------

# 35. Conclusão

O objetivo do Project-Victory é evoluir de uma aplicação funcional para
um mundo persistente.

A arquitetura deve permitir:

-   crescimento;
-   manutenção;
-   evolução;
-   novos sistemas;
-   continuidade do jogador.

# Architecture Review Notes

Este capítulo registra contexto arquitetural para futuras análises. Não
representa uma decisão implementada. Deve ser validado por contratos
oficiais quando houver implementação.

# Future Runtime Considerations

Este capítulo registra contexto arquitetural para futuras análises. Não
representa uma decisão implementada. Deve ser validado por contratos
oficiais quando houver implementação.

# Gameplay Evolution Considerations

Este capítulo registra contexto arquitetural para futuras análises. Não
representa uma decisão implementada. Deve ser validado por contratos
oficiais quando houver implementação.

# Persistence Considerations

Este capítulo registra contexto arquitetural para futuras análises. Não
representa uma decisão implementada. Deve ser validado por contratos
oficiais quando houver implementação.

# System Interaction Reference

Este capítulo registra contexto arquitetural para futuras análises. Não
representa uma decisão implementada. Deve ser validado por contratos
oficiais quando houver implementação.

# Engineering Review Checklist

Este capítulo registra contexto arquitetural para futuras análises. Não
representa uma decisão implementada. Deve ser validado por contratos
oficiais quando houver implementação.

# Architecture Validation Checklist

Este capítulo registra contexto arquitetural para futuras análises. Não
representa uma decisão implementada. Deve ser validado por contratos
oficiais quando houver implementação.

# Future Multiplayer Considerations

Este capítulo registra contexto arquitetural para futuras análises. Não
representa uma decisão implementada. Deve ser validado por contratos
oficiais quando houver implementação.

# World Evolution Considerations

Este capítulo registra contexto arquitetural para futuras análises. Não
representa uma decisão implementada. Deve ser validado por contratos
oficiais quando houver implementação.

# Product Vision Notes

Este capítulo registra contexto arquitetural para futuras análises. Não
representa uma decisão implementada. Deve ser validado por contratos
oficiais quando houver implementação.
