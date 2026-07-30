# PV-WORLD-006

# Location Content Resolver


## Objetivo

Criar a fundação responsável por resolver quais conteúdos e possibilidades existem dentro de uma localização atual do jogador.

Este contrato utiliza as bases criadas anteriormente:

World

↓

Region

↓

Location

↓

Runtime World Context

↓

Exploration Context


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


↓

PV-WORLD-005

Exploration Context


O sistema consegue representar:

Onde o jogador está.

Porém ainda não existe uma camada responsável por descobrir:

O que existe neste local.


## Problema atual

Hoje:

Runtime

↓

Location


Informa apenas posição atual.


O jogo precisa evoluir para:

Location

↓

Conteúdo disponível


Exemplo:


DarkForest


Pode possuir:

- Hunts disponíveis
- Eventos
- Interações
- Pontos de interesse


## Objetivo arquitetural

Criar um resolvedor de conteúdo de localização.


Modelo esperado:


LocationContentResolver


Entrada:

LocationId


Saída:

AvailableLocationContent


## Responsabilidade

O resolver deve responder:


Quais possibilidades existem nesta localização?


Exemplo:


Location:

DarkForest Entrance


Resultado:


- Hunt Rat
- Explore Area
- Event Check


## Separação arquitetural


LocationDefinition:

Representa o local.


LocationContentResolver:

Resolve conteúdo disponível.


Runtime:

Mantém contexto atual do jogador.


## Não faz parte deste contrato

Não implementar:


- Sistema de combate
- Geração de monstros
- Quest System
- NPC System
- Reward System
- Economia
- Conteúdo final do jogo


Este contrato cria somente a camada de resolução de conteúdo.


## Escopo de implementação

A Engenharia deve analisar:


src/ProjectVictory.Domain/

src/ProjectVictory.Application/

src/ProjectVictory.Persistence/

src/ProjectVictory.Application/Runtime/


Implementar somente os componentes necessários para resolver conteúdo de localização.


## Critérios de aceitação


O contrato será considerado concluído quando:


Runtime

↓

Location válida

↓

LocationContentResolver


permitir consultar conteúdo disponível.


Validar:


- localização existente retorna contexto;
- conteúdo está associado ao local correto;
- estrutura permite expansão futura.


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

Conteúdo disponível


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


PV-WORLD-006

Location Content Resolver


Aguardando implementação da Engenharia.
