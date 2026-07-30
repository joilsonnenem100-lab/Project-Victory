# PV-WORLD-005

# Exploration Context


## Objetivo

Criar a fundação para representar o contexto de exploração do jogador dentro do mundo do Project-Victory.

Este contrato utiliza as estruturas criadas anteriormente:

World

↓

Region

↓

Location

↓

Runtime Location Binding


para preparar o primeiro nível de interação do jogador com o mundo.


## Contexto atual

Após os contratos anteriores:


PV-WORLD-001

World Definition Foundation


↓

PV-WORLD-002

Region Foundation


↓

PV-WORLD-003

Location Definition Foundation


↓

PV-WORLD-004

Player Location Binding


O sistema consegue representar:


Onde o jogador está.


Porém ainda não existe uma camada que represente:


O que o jogador pode fazer naquele local.


## Problema atual

Atualmente:


Runtime

↓

Location


apenas informa posição atual.


Ainda não existe contexto de exploração:


Location

↓

Possíveis ações


Exemplo:


DarkForest


Pode possuir:


- Explorar
- Iniciar Hunt
- Encontrar Evento
- Interagir com Conteúdo


## Objetivo arquitetural

Criar uma fundação para o contexto de exploração.


Modelo esperado:


ExplorationContext


- LocationId
- AvailableActions
- ExplorationState


## Responsabilidade

ExplorationContext representa:


A interação temporária do jogador com uma localização.


Ele deve permitir que sistemas futuros descubram:


Quais possibilidades existem no local atual.


## Separação arquitetural


LocationDefinition:


Representa o conteúdo estático do local.


Runtime:


Representa a sessão atual.


ExplorationContext:


Representa a experiência de exploração naquele local.


## Não faz parte deste contrato

Não implementar:


- Sistema de combate
- Sistema de Hunt
- Spawn de monstros
- Quests
- NPC
- Eventos completos
- Recompensas


Este contrato cria somente o contexto de exploração.


## Escopo de implementação

A Engenharia deve analisar:


src/ProjectVictory.Domain/

src/ProjectVictory.Application/

src/ProjectVictory.Persistence/

src/ProjectVictory.Application/Runtime/


Implementar somente os componentes necessários para representar Exploration Context.


## Critérios de aceitação


O contrato será considerado concluído quando:


Runtime

↓

Location válida

↓

ExplorationContext


permitir consulta do contexto de exploração atual.


Validar:


- jogador possui contexto de exploração;
- contexto está ligado à localização correta;
- estrutura permite evolução futura.


## Resultado esperado


Antes:


Runtime

↓

Location


Depois:


Runtime

↓

Location

↓

ExplorationContext

↓

Possíveis ações futuras


## Restrições


Não alterar:


- Character Aggregate
- Combat
- Hunt
- Inventory
- Reward Pipeline
- Runtime Recovery


Alterações devem permanecer dentro da evolução do sistema World.


## Status


PV-WORLD-005

Exploration Context


Aguardando implementação da Engenharia.
