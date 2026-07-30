# PV-WORLD-001

# World Definition Foundation

## Objetivo

Criar a fundação para representar os mundos existentes dentro do Project-Victory.

## Contexto atual

O Runtime já possui:

Runtime
|
RuntimeWorldContext
|
WorldId

Porém o WorldId ainda representa apenas uma referência.

## Objetivo arquitetural

Criar a definição estática dos mundos existentes no jogo.

Modelo esperado:

WorldDefinition

- Id
- Name
- Description
- Status

## Responsabilidade

WorldDefinition representa a existência de um mundo no jogo.

## Separação arquitetural

WorldDefinition:

Conteúdo estático.

RuntimeWorldContext:

Contexto atual do jogador.

## Não faz parte deste contrato

Não implementar:

- Regiões
- Localizações
- Exploração
- Movimento
- NPC
- Quests
- Eventos
- Spawn de monstros

## Critérios de aceitação

Existe uma representação oficial de WorldDefinition.

Runtime pode referenciar um mundo válido.

## Restrições

Não alterar:

- Character Aggregate
- Runtime Context existente
- Combat
- Hunt
- Inventory
- Reward Pipeline

## Status

PV-WORLD-001

World Definition Foundation

Aguardando implementação da Engenharia.
