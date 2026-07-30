# PV-WORLD-002

# Region Foundation


## Objetivo

Criar a fundação para representar regiões existentes dentro de um mundo do Project-Victory.

Este contrato evolui a estrutura de mundo criada no PV-WORLD-001, adicionando uma camada intermediária entre World e Location.


## Contexto atual

Após PV-WORLD-001:

WorldDefinition

↓

World


Porém um mundo ainda não possui uma organização interna.


## Problema atual

O sistema precisa representar:


World

↓

Regiões

↓

Localizações


Exemplo:


FirstWorld

├── Village

├── DarkForest

└── MountainPass


## Objetivo arquitetural

Criar uma definição oficial de região.


Modelo esperado:


RegionDefinition

- Id
- WorldId
- Name
- Description
- Status


## Responsabilidade

RegionDefinition representa uma área lógica pertencente a um mundo.


Exemplo:


World:

FirstWorld


Region:

DarkForest


## Separação arquitetural


WorldDefinition:

Representa o mundo.


RegionDefinition:

Representa uma divisão dentro do mundo.


RuntimeWorldContext:

Representa onde o jogador está atualmente.


## Não faz parte deste contrato

Não implementar:


- Localizações
- Exploração
- Movimento
- Spawn
- NPC
- Quest
- Eventos
- Conteúdo jogável


Este contrato cria somente a fundação de regiões.


## Escopo de implementação

A Engenharia deve analisar:


src/ProjectVictory.Domain/

src/ProjectVictory.Application/

src/ProjectVictory.Persistence/


Implementar somente os componentes necessários para representar RegionDefinition.


## Critérios de aceitação


O contrato será considerado concluído quando:


WorldDefinition

↓

possuir Regions associadas


E:


- Region possuir identidade própria;
- Region estiver vinculada a um World válido;
- Estrutura permitir evolução futura para Locations.


## Resultado esperado


Antes:


World

=

Apenas definição do mundo


Depois:


World

↓

Region


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


PV-WORLD-002

Region Foundation


Aguardando implementação da Engenharia.
