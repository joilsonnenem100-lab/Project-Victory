# PV-RUNTIME-004

# Runtime World Context Foundation


## Objetivo

Criar a fundação para representar o contexto mundial atual do jogador dentro do Runtime.

O Runtime deve evoluir de um simples controle de sessão para um contexto capaz de representar onde o jogador está inserido no mundo do jogo.


## Contexto atual

Estado atual:


Character

↓

Runtime

↓

Gameplay Context

↓

Hunt / Gameplay


O Runtime já possui:


- RuntimeId
- CharacterId
- CurrentActivity
- CurrentHunt


Porém ainda não representa:


- World
- Region
- Location


## Objetivo arquitetural

Criar uma estrutura de contexto de mundo associada ao Runtime.


Modelo esperado:


Runtime

↓

RuntimeWorldContext


Dados iniciais:


- WorldId
- RegionId
- LocationId


## Responsabilidade

Após este contrato, o Runtime deve responder:


Onde o jogador está atualmente?


Exemplo:


Character

↓

Runtime

↓

WorldContext


World:

FirstWorld


Region:

DarkForest


Location:

ForestEntrance


## Regras arquiteturais


O Runtime representa:


Estado temporário da sessão


O World Context representa:


Contexto atual do jogador dentro do mundo


O Runtime não substitui:


Character Aggregate


## Regra de extensibilidade


O RuntimeWorldContext deve ser criado permitindo evolução futura.


A estrutura não deve impedir a adição de novos dados de contexto.


Exemplos de possíveis extensões futuras:


- WorldInstanceId
- AreaId
- LocationState
- WorldEventContext
- EnvironmentContext


Essas extensões não fazem parte deste contrato.


O objetivo atual é criar somente a fundação.


## Não faz parte deste contrato


Não implementar:


- Sistema de mapa
- Navegação
- Movimento do personagem
- Teleporte
- NPC
- Quest System
- Conteúdo de mundo


Este contrato cria somente a representação do contexto mundial.


## Escopo de implementação


A Engenharia deve analisar:


src/ProjectVictory.Application/Runtime/


src/ProjectVictory.Domain/


src/ProjectVictory.Persistence/Runtime/


Implementar somente os componentes necessários para representar o World Context.


## Critérios de aceitação


O contrato será considerado concluído quando:


Runtime

↓

possuir World Context associado


E permitir consulta do contexto atual do jogador.


Validar:


- Runtime possui WorldId;
- Runtime possui RegionId;
- Runtime possui LocationId;
- Estrutura permite evolução futura sem quebra arquitetural.


## Resultado esperado


Antes:


Runtime

=

Sessão

+

Atividade

+

Hunt atual


Depois:


Runtime

=

Sessão

+

Atividade

+

Hunt atual

+

Contexto de Mundo


## Restrições


Não alterar:


- Combat
- Hunt
- Inventory
- Reward Pipeline
- Character Aggregate


Alterações devem permanecer dentro da evolução do Runtime.


## Status


PV-RUNTIME-004

Runtime World Context Foundation


Aguardando implementação da Engenharia.