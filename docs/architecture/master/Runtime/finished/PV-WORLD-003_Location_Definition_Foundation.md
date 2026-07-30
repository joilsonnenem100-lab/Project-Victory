# PV-WORLD-003

# Location Definition Foundation


## Objetivo

Criar a fundação para representar localizações existentes dentro de uma região do mundo do Project-Victory.

Este contrato evolui a hierarquia criada pelos contratos anteriores:

World

↓

Region

↓

Location


## Contexto atual

Após:

PV-WORLD-001

World Definition Foundation


E:

PV-WORLD-002

Region Foundation


O sistema possui:


WorldDefinition

↓

RegionDefinition


Porém ainda não existe uma definição oficial de localização.


## Problema atual

O Runtime possui:

RuntimeWorldContext

↓

LocationId


Mas ainda não existe uma entidade de conteúdo representando esse local.


Exemplo:


LocationId:

forest_entrance


Precisa evoluir para:


LocationDefinition

Id:

forest_entrance


Name:

Forest Entrance


Region:

DarkForest


## Objetivo arquitetural

Criar a definição estática das localizações existentes no mundo.


Modelo esperado:


LocationDefinition

- Id
- RegionId
- Name
- Description
- Status


## Responsabilidade

LocationDefinition representa um ponto ou área existente dentro de uma região.


Exemplos:


DarkForest

↓

Forest Entrance


DarkForest

↓

Deep Forest


## Separação arquitetural


WorldDefinition:

Representa o mundo.


RegionDefinition:

Representa uma região do mundo.


LocationDefinition:

Representa um local dentro da região.


RuntimeWorldContext:

Representa onde o jogador está atualmente.


## Não faz parte deste contrato

Não implementar:


- Exploração
- Movimento
- Navegação
- Spawn
- Encontros
- NPC
- Quests
- Eventos
- Conteúdo jogável


Este contrato cria somente a definição de localização.


## Escopo de implementação

A Engenharia deve analisar:


src/ProjectVictory.Domain/

src/ProjectVictory.Application/

src/ProjectVictory.Persistence/


Implementar somente os componentes necessários para representar LocationDefinition.


## Critérios de aceitação


O contrato será considerado concluído quando:


WorldDefinition

↓

RegionDefinition

↓

LocationDefinition


estiverem representados corretamente.


Validar:


- Location possui identidade própria;
- Location pertence a uma Region válida;
- Estrutura permite uso futuro pelo Runtime.


## Resultado esperado


Antes:


Runtime

↓

LocationId sem definição


Depois:


Runtime

↓

LocationId

↓

LocationDefinition


## Restrições


Não alterar:


- Character Aggregate
- Runtime Context
- Combat
- Hunt
- Inventory
- Reward Pipeline


Alterações devem permanecer dentro da evolução do sistema World.


## Status


PV-WORLD-003

Location Definition Foundation


Aguardando implementação da Engenharia.
